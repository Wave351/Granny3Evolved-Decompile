using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using C = ChainCutterSetup;

// Tools > Evolved Restoration > Set Up Plank Crossing: lets Granny and Grandpa cross the 3rd floor plank once it's placed.
// The area past the plank never had any nav mesh, so a nav mesh surface limited to that area is baked, and a link over the
// plank is put inside PlankPlaced, so it only exists while the plank is there. Run it again after moving things over there.
public static class PlankCrossingSetup
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string Folder = "Assets/PlankCrossing";
	const string RootName = "== Added Hazards ==";
	const string SurfaceName = "Plank Far Side NavMesh";
	const string LinkName = "Plank NavMesh Link";

	// The area past the plank (world space): from just past the hallway's end to the far wall, floor 3 only.
	static readonly Vector3 AreaMin = new Vector3(10f, 12.9f, 18.6f);
	static readonly Vector3 AreaMax = new Vector3(25f, 17.5f, 41f);

	[MenuItem("Tools/Evolved Restoration/Set Up Plank Crossing")]
	public static void Run()
	{
		if (EditorSceneManager.GetActiveScene().path != SceneFile)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
			EditorSceneManager.OpenScene(SceneFile);
		}
		Build();
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	public static void RunBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log("PLANKNAV done and saved");
	}

	static void Build()
	{
		var plank = C.All<Transform>().FirstOrDefault(t => t.name == "PlankPlaced");
		if (!plank) { Debug.LogError("PLANKNAV PlankPlaced not found"); return; }
		if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets", "PlankCrossing");

		var root = C.All<Transform>().FirstOrDefault(t => t.name == RootName && t.parent == null);
		if (!root) root = new GameObject(RootName).transform;
		var holder = root.Cast<Transform>().FirstOrDefault(t => t.name == SurfaceName);
		if (!holder) { holder = new GameObject(SurfaceName).transform; holder.SetParent(root, false); }
		holder.SetPositionAndRotation((AreaMin + AreaMax) / 2f, Quaternion.identity);
		holder.localScale = Vector3.one;

		if (!holder.TryGetComponent(out NavMeshSurface surface)) surface = holder.gameObject.AddComponent<NavMeshSurface>();
		surface.agentTypeID = 0;
		surface.collectObjects = CollectObjects.Volume;
		surface.center = Vector3.zero;
		surface.size = AreaMax - AreaMin;
		surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
		surface.layerMask = ~PlayerOnlyWall.Mask & ~(1 << 2); // not player-only walls or Ignore Raycast
		surface.defaultArea = 0;
		surface.ignoreNavMeshAgent = true;
		surface.ignoreNavMeshObstacle = true;

		// The plank must not be baked in (the link does the crossing, and only while the plank is there).
		bool plankWasActive = plank.gameObject.activeSelf;
		plank.gameObject.SetActive(false);
		surface.RemoveData();
		surface.navMeshData = null;
		// Baked by hand so invisible trigger zones (which the surface would count as solid) can be left out.
		var size = AreaMax - AreaMin;
		var sources = new System.Collections.Generic.List<NavMeshBuildSource>();
		UnityEngine.AI.NavMeshBuilder.CollectSources(new Bounds(holder.position, size), surface.layerMask, NavMeshCollectGeometry.PhysicsColliders, 0, new System.Collections.Generic.List<NavMeshBuildMarkup>(), sources);
		var triggers = sources.Where(s => s.component is Collider c && c.isTrigger).Select(s => s.component.name).ToList();
		// Also the invisible item-catching wall down the middle of the room, which isn't a wall for anyone walking.
		sources.RemoveAll(s => s.component is Collider c && (c.isTrigger || c.name == "CollisionPreventItemFailsafe"));
		foreach (var src in sources)
			if (src.component is Collider sc) Debug.Log($"PLANKNAV shape {sc.name} ({sc.GetType().Name}, layer {LayerMask.LayerToName(sc.gameObject.layer)}) {sc.bounds.min} .. {sc.bounds.max}");
		Debug.Log($"PLANKNAV {sources.Count} solid shapes; left out {triggers.Count} trigger zones: {string.Join(", ", triggers.Distinct().Take(30))}");
		// Baked for a slimmer agent than Granny's 0.3 m, so the narrow ledge past the chimney (which the player can squeeze
		// along) stays walkable for her too.
		var settings = NavMesh.GetSettingsByID(0);
		settings.agentRadius = .12f;
		var data = UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(settings, sources, new Bounds(Vector3.zero, size), holder.position, Quaternion.identity);
		data.name = "NavMesh-PlankFarSide";
		surface.navMeshData = data;
		plank.gameObject.SetActive(plankWasActive);

		string dataPath = $"{Folder}/NavMesh-PlankFarSide.asset";
		if (AssetDatabase.LoadAssetAtPath<NavMeshData>(dataPath)) AssetDatabase.DeleteAsset(dataPath);
		AssetDatabase.CreateAsset(surface.navMeshData, dataPath);
		surface.navMeshData = AssetDatabase.LoadAssetAtPath<NavMeshData>(dataPath);
		surface.AddData();
		EditorUtility.SetDirty(surface);

		// The link over the plank, from the hallway end to the far side, kept upright whatever the plank's rotation.
		var pb = plank.GetComponent<Renderer>().bounds;
		var along = pb.size.x > pb.size.z ? Vector3.right : Vector3.forward;
		float half = Mathf.Max(pb.size.x, pb.size.z) / 2f;
		var top = new Vector3(pb.center.x, pb.max.y, pb.center.z);
		var a = Snap(top - along * (half + .3f), top - along * half);
		var b = Snap(top + along * (half + .3f), top + along * half);

		var old = plank.Cast<Transform>().FirstOrDefault(t => t.name == LinkName);
		if (old) Object.DestroyImmediate(old.gameObject);
		var linkGo = new GameObject(LinkName);
		linkGo.transform.SetParent(plank, false);
		var link = linkGo.AddComponent<PlankNavLink>();
		link.Start = a;
		link.End = b;
		EditorUtility.SetDirty(link);

		var tri = surface.navMeshData ? NavMesh.CalculateTriangulation() : default;
		Debug.Log($"PLANKNAV surface baked ({tri.vertices?.Length ?? 0} nav verts in total now), link {a} -> {b}");
	}

	// The nearest nav mesh point to where a plank end meets the floor, or the plank end itself.
	static Vector3 Snap(Vector3 beyond, Vector3 end)
	{
		if (NavMesh.SamplePosition(beyond, out NavMeshHit h, 1.2f, NavMesh.AllAreas)) { Debug.Log($"PLANKNAV end {end} snapped to {h.position} ({h.distance:F2} away)"); return h.position; }
		Debug.LogWarning($"PLANKNAV no nav mesh near the plank end at {end}");
		return end;
	}

	// Command line check: from the hallway, can Granny reach the far side (escape lever 2's spot, if it's over there) with the
	// plank placed, and not without it? Also how much of the far side is reachable from the landing.
	public static void Check()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var surface = C.All<NavMeshSurface>().First(s => s.name == SurfaceName);
		surface.AddData();
		var link = C.All<PlankNavLink>().First();
		NavMesh.SamplePosition(new Vector3(13f, 13.83f, 12f), out NavMeshHit fh, 2f, NavMesh.AllAreas);
		var from = fh.position;
		var lever = C.All<EscapeLevers>().FirstOrDefault();
		var target = lever && lever.Spots[1] ? lever.Spots[1].position - lever.Spots[1].right * .5f : new Vector3(15f, 13.82f, 35.5f);
		NavMesh.SamplePosition(target, out NavMeshHit th, 2f, NavMesh.AllAreas);
		Debug.Log($"PLANKNAV target {th.position}");
		foreach (bool on in new[] { true, false })
		{
			// (The link's own OnEnable only runs in the game, so it's added here the same way.)
			var added = on ? NavMesh.AddLink(new NavMeshLinkData { startPosition = link.Start, endPosition = link.End, width = link.Width, bidirectional = true, costModifier = -1f, agentTypeID = 0 }) : default;
			var path = new NavMeshPath();
			NavMesh.CalculatePath(from, th.position, NavMesh.AllAreas, path);
			Debug.Log($"PLANKNAV plank {(on ? "placed" : "missing")}: {path.status}, ends at {(path.corners.Length > 0 ? path.corners.Last().ToString() : "-")}");
			if (added.valid) NavMesh.RemoveLink(added);
		}
		int ok = 0;
		var farAll = NavMesh.CalculateTriangulation().vertices.Where(v => v.y > 13.3f && v.y < 14.4f && v.z > 18.6f && v.z < 36.3f).Distinct().ToArray();
		foreach (var v in farAll)
		{
			var p = new NavMeshPath();
			if (NavMesh.CalculatePath(link.End, v, NavMesh.AllAreas, p) && p.status == NavMeshPathStatus.PathComplete) ok++;
		}
		Debug.Log($"PLANKNAV from the landing {ok}/{farAll.Length} points in the two rooms reachable");
	}
}
