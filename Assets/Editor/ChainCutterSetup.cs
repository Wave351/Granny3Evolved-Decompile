using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Tools > Evolved Restoration > Set Up Chain Cutter: adds Granny Legacy's chain cutter to the map for Slientlo Never Clear.
//  * The item (lying in the map, in the hand, and dropped), built from the valve knob with Legacy's cutter model.
//  * A spot in each of the 6 item presets, and its own seed spots in the 1.2 and Extras seed systems (it's a required item there).
//  * Chains holding the hot air balloon down, cut by clicking them with the cutter in hand.
// Running it again rebuilds everything. Command line (project closed): -executeMethod ChainCutterSetup.RunBatch
public static class ChainCutterSetup
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string Folder = "Assets/ChainCutter";
	const string ItemName = ChainCutterItem.ItemName;
	const string PresetRoot = "Objects/MainItemSelection/ItemSpawnsAcrossMap (Preset Mode)";
	const string SeedSpotsName = "ChainCutterSeedSpots";
	const string ExtrasOnly = "(Extras only)";
	static readonly string[] SeedManagersWithBalloon = { "SeedSystemManager_Normal", "SeedSystemManager_Extras" };

	// Legacy's cutter: world size and collider (in model space), and its pose in the hand adapted to Evolved's hand rig.
	const float WorldScale = 16.405596f;
	static readonly Vector3 ColliderSize = new Vector3(.005f, .0343523f, .058460537f);
	static readonly Vector3 ColliderCentre = new Vector3(.000004f, 0f, -.012537678f);
	static readonly Vector3 HandPosition = new Vector3(.11f, -.02f, .14f);
	static readonly Quaternion HandRotation = new Quaternion(-.47932208f, .048722237f, -.090274096f, .8716233f);
	const float HandScale = 3.3f;

	// ---------- for the Hazard Placer's "Chain cutter spots" section ----------

	public const string ExtrasOnlyTag = ExtrasOnly;
	public const float CutterWorldScale = WorldScale;
	public static Mesh CutterMesh => AssetDatabase.LoadAssetAtPath<Mesh>(Folder + "/ChainCutterMesh.asset");
	public static Material CutterMaterial => AssetDatabase.LoadAssetAtPath<Material>(Folder + "/ChainCutter.mat");
	public static bool IsSetUp => All<ChainCutterItem>().Any();

	// The six preset spots, in order (null where a preset has none).
	public static Transform[] PresetSpots()
	{
		var root = All<Transform>().FirstOrDefault(t => PathOf(t) == PresetRoot);
		var spots = new Transform[6];
		if (!root) return spots;
		for (int i = 0; i < 6; i++)
		{
			var preset = root.Cast<Transform>().FirstOrDefault(c => c.name == "Preset_" + (i + 1));
			if (preset) spots[i] = preset.Cast<Transform>().FirstOrDefault(c => c.name.StartsWith("- ChainCutterPreset"));
		}
		return spots;
	}

	public static Transform SeedSpotHolder() => All<Transform>().FirstOrDefault(t => t.name == SeedSpotsName);

	public static List<Transform> SeedSpots()
	{
		var holder = SeedSpotHolder();
		return holder ? holder.Cast<Transform>().ToList() : new List<Transform>();
	}

	// A new seed spot at a position, settled onto the surface below and lying flat.
	public static Transform AddSeedSpot(Vector3 at)
	{
		var holder = SeedSpotHolder();
		if (!holder) return null;
		int n = 1;
		while (holder.Cast<Transform>().Any(t => t.name.StartsWith("ChainCutterSeedSpot " + n + " ") || t.name == "ChainCutterSeedSpot " + n)) n++;
		var spot = new GameObject("ChainCutterSeedSpot " + n).transform;
		spot.SetParent(holder, false);
		spot.SetPositionAndRotation(OnSurface(at), Quaternion.Euler(0f, 0f, 90f));
		return spot;
	}

	// Settles a spot onto whatever is below it and keeps it lying flat (only its turn around the vertical is kept).
	public static void Drop(Transform spot)
	{
		Physics.SyncTransforms();
		spot.SetPositionAndRotation(OnSurface(spot.position), LyingFlat(spot.rotation));
	}

	// Puts the spots as they are now into the presets and both seed systems (the cutter is rebuilt around them).
	public static void SaveSpots()
	{
		Build();
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
	}

	[MenuItem("Tools/Evolved Restoration/Set Up Chain Cutter")]
	public static void Run()
	{
		if (EditorSceneManager.GetActiveScene().path != SceneFile)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
			EditorSceneManager.OpenScene(SceneFile);
		}
		Build();
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		Debug.Log("CHAINCUTTER: set up. Save the map to keep it.");
	}

	// Command line: renders the chained balloon and the cutter at preset 1 to PNGs (nothing is saved to the map).
	// -executeMethod ChainCutterSetup.Preview -previewDir <folder>
	public static void Preview()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		var balloon = Find("EntireMansion/Property/ImportantInteractables/HotAirBalloon/HotAirBalloonPlaced");
		for (var t = balloon; t; t = t.parent) t.gameObject.SetActive(true);
		RenderSettings.fog = false;
		var chains = All<BalloonChains>().First();
		var box = new Bounds(chains.Chains[0].transform.position, Vector3.zero);
		foreach (var c in chains.Chains) box.Encapsulate(c.GetComponent<Renderer>().bounds);
		Shot(dir + "/preview_balloon_wide.png", box.center + new Vector3(4.5f, 2.2f, 4.5f), box.center);
		// Close-up of where each chain meets the basket, from outside and to the side.
		for (int i = 0; i < chains.Chains.Length; i++)
		{
			var c = chains.Chains[i].transform;
			var away = Vector3.ProjectOnPlane(c.forward, Vector3.up).normalized;
			var across = Vector3.Cross(Vector3.up, away);
			Shot(dir + $"/preview_chain_tie{i + 1}.png", c.position + away * 1.3f + across * .9f + Vector3.up * .5f, c.position);
		}
		var cutter = All<ChainCutterItem>().First().transform;
		Shot(dir + "/preview_cutter.png", cutter.position + new Vector3(.9f, .8f, .9f), cutter.position);
		Debug.Log("CHAINCUTTER: previews in " + dir);
	}

	// A camera position around the target with nothing solid between it and the target (tries angles, heights, distances).
	static Vector3 ClearView(Vector3 target, Transform item, float[] distances)
	{
		var aim = target + Vector3.up * .03f;
		foreach (float d in distances)
			foreach (float h in new[] { .55f, 1f, .25f, 1.6f })
				for (int a = 0; a < 360; a += 30)
				{
					var from = aim + Quaternion.Euler(0f, a, 0f) * Vector3.forward * d + Vector3.up * h;
					if (Physics.CheckSphere(from, .15f, ~0, QueryTriggerInteraction.Ignore)) continue;   // camera inside something
					if (!Physics.Linecast(from, aim, out RaycastHit hit, ~0, QueryTriggerInteraction.Ignore) || hit.transform == item) return from;
				}
		Debug.LogWarning($"CHAINSPOT no clear view of {target}");
		return target + new Vector3(.9f, .7f, .9f);
	}

	static void Shot(string file, Vector3 from, Vector3 at)
	{
		var go = new GameObject("PreviewCam");
		var cam = go.AddComponent<Camera>();
		cam.transform.position = from;
		cam.transform.LookAt(at);
		cam.fieldOfView = 55f; cam.nearClipPlane = .02f;
		var light = new GameObject("PreviewLight").AddComponent<Light>();
		light.type = LightType.Directional; light.intensity = 1.1f;
		light.transform.rotation = Quaternion.Euler(45f, 30f, 0f);
		var rt = new RenderTexture(900, 600, 24);
		cam.targetTexture = rt;
		cam.Render();
		RenderTexture.active = rt;
		var tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, 900, 600), 0, 0);
		System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
		RenderTexture.active = null;
		Object.DestroyImmediate(go); Object.DestroyImmediate(light.gameObject); Object.DestroyImmediate(rt);
	}

	// Command line: a close and a wide picture of the chain cutter at each named spot (nothing is saved to the map).
	// -executeMethod ChainCutterSetup.PreviewSpots -previewDir <folder> -spots "- ChainCutterPreset2|ChainCutterSeedSpot 7"
	public static void PreviewSpots()
	{
		var args = System.Environment.GetCommandLineArgs();
		string Arg(string key) { int i = System.Array.IndexOf(args, key); return i >= 0 && i + 1 < args.Length ? args[i + 1] : null; }
		string dir = Arg("-previewDir") ?? System.IO.Path.GetTempPath();
		var wanted = (Arg("-spots") ?? "").Split('|');
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var cutter = All<ChainCutterItem>().First().transform;
		foreach (var spot in All<Transform>().Where(t => wanted.Any(w => w.Length > 0 && t.name.StartsWith(w))).OrderBy(t => t.name))
		{
			cutter.SetPositionAndRotation(spot.position, spot.rotation);
			string file = spot.name.Replace("/", "").Replace(" ", "").Replace("-", "");
			Physics.SyncTransforms();
			Shot($"{dir}/spot_{file}_close.png", ClearView(spot.position, cutter, new[] { 1f, 1.4f, 1.9f }), spot.position);
			Shot($"{dir}/spot_{file}_wide.png", ClearView(spot.position, cutter, new[] { 3.2f, 2.5f, 1.9f }), spot.position);
			Debug.Log($"CHAINSPOT picture {spot.name} at {spot.position}");
		}
	}

	// One-off fixes asked for on 2026-11-07: preset 2 was floating (picked again, now with a surface check), and seed spot 7
	// sits in the Extras-only submarine area (moved to the middle of its workbench and used by the Extras seed system only).
	// -executeMethod ChainCutterSetup.FixSpots
	public static void FixSpots()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Physics.SyncTransforms();
		foreach (var t in All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset2 ")).ToList()) Kill(t);
		var seven = All<Transform>().FirstOrDefault(t => t.name.StartsWith("ChainCutterSeedSpot 7"));
		if (seven)
		{
			var below = Physics.RaycastAll(seven.position + Vector3.up * .4f, Vector3.down, 1.5f, ~0, QueryTriggerInteraction.Ignore)
				.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).FirstOrDefault();
			if (below.collider)
			{
				var b = below.collider.bounds;
				var middle = new Vector3(b.center.x, b.max.y + .5f, b.center.z);
				var top = Physics.RaycastAll(middle, Vector3.down, 2f, ~0, QueryTriggerInteraction.Ignore)
					.Where(h => h.collider == below.collider).OrderBy(h => h.distance).FirstOrDefault();
				float y = top.collider ? top.point.y : b.max.y;
				// Lengthways along the bench.
				float yaw = b.size.x > b.size.z ? 90f : 0f;
				seven.SetPositionAndRotation(new Vector3(b.center.x, y + .05f, b.center.z), Quaternion.Euler(0f, yaw, 90f));
				Debug.Log($"CHAINSPOT seed spot 7 moved to the middle of {PathOf(below.collider.transform)}: {seven.position}");
			}
			seven.name = "ChainCutterSeedSpot 7 " + ExtrasOnly;
		}
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		foreach (var t in All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset")).OrderBy(t => t.name))
			Debug.Log($"CHAINSPOT {t.name} at {t.position}");
		Debug.Log("CHAINSPOT fixed and saved");
	}

	// Repairs the first FixSpots run: removes the preset spots it picked by mistake (keeping each preset's original spot,
	// and preset 2's new one), puts seed spot 7 back, then centres it on the flat top it sits on.
	// -executeMethod ChainCutterSetup.RepairSpots
	public static void RepairSpots()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var presetRoot = Find(PresetRoot);
		for (int i = 1; i <= 6; i++)
		{
			var preset = presetRoot.Find("Preset_" + i);
			var mine = preset.Cast<Transform>().Where(c => c.name.StartsWith("- ChainCutterPreset")).OrderBy(c => c.GetSiblingIndex()).ToList();
			foreach (var extra in mine.Skip(1)) { Debug.Log($"CHAINSPOT removed extra preset {i} spot at {extra.position}"); Kill(extra); }
		}
		var seven = All<Transform>().FirstOrDefault(t => t.name.StartsWith("ChainCutterSeedSpot 7"));
		if (seven)
		{
			seven.position = new Vector3(-8.54f, -17.02f, 41.26f);   // where it was before the first fix
			Physics.SyncTransforms();
			CentreOnTop(seven);
		}
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		foreach (var t in All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset") || t.name.StartsWith("ChainCutterSeedSpot ")).OrderBy(t => t.name))
			Debug.Log($"CHAINSPOT {t.name} at {t.position}");
		Debug.Log("CHAINSPOT repaired and saved");
	}

	// Picks a new place for one of the cutter's seed spots, then rebuilds and saves.
	// -executeMethod ChainCutterSetup.RedoSeedSpot -spot 8
	public static void RedoSeedSpot()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-spot");
		string number = at >= 0 && at + 1 < args.Length ? args[at + 1] : "8";
		EditorSceneManager.OpenScene(SceneFile);
		Physics.SyncTransforms();
		var holder = All<Transform>().First(t => t.name == SeedSpotsName);
		var old = holder.Cast<Transform>().FirstOrDefault(t => t.name.StartsWith("ChainCutterSeedSpot " + number));
		int index = old ? old.GetSiblingIndex() : holder.childCount;
		var avoid = new List<Vector3>();
		if (old) { avoid.Add(old.position); Kill(old); }
		avoid.Add(new Vector3(17.69f, 22.39f, 22.87f));   // the roof beam above the third-floor hallway: out of reach
		// Away from every seed spot and puzzle spot, and from the cutter's other spots.
		avoid.AddRange(All<SeedManager>().SelectMany(s => (s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null).SelectMany(a => a.freeSpots)
			.Concat((s.puzzleDefs ?? new List<PuzzleDef>()).Select(p => p?.spawnPoint))).Where(t => t).Select(t => t.position));
		avoid.AddRange(holder.Cast<Transform>().Select(t => t.position));
		var presetRoot = Find(PresetRoot);
		var candidates = presetRoot.GetComponentsInChildren<Transform>(true)
			.Where(t => t.parent && t.parent.parent == presetRoot && !t.name.StartsWith("- ChainCutter")).ToList();
		// Only spots under a real floor or surface, and not hard against a wall.
		// Not "/ Extras" positions: those can be in areas that are only open with Extras.
		candidates = candidates.Where(c => !c.name.Contains("Extras") && HasSurface(c.position) && FarFrom(c.position, avoid, 3f) && Reachable(c.position)).ToList();
		var pick = Pick(candidates, avoid, candidates.Count).FirstOrDefault(c => !NearWall(c.position));
		if (!pick) { Debug.LogError("CHAINSPOT no new spot found"); return; }
		var spot = MakeSpot("ChainCutterSeedSpot " + number, holder, pick);
		spot.SetSiblingIndex(index);
		CentreOnTop(spot);
		Debug.Log($"CHAINSPOT new seed spot {number} at {spot.position} (from {PathOf(pick)})");
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log("CHAINSPOT redone and saved");
	}

	// Within a player's reach: lying on a wide floor, or on something (shelf, table) with a floor less than 1.8 m below it.
	// A narrow beam high above the floor fails.
	internal static bool Reachable(Vector3 p)
	{
		var hits = Physics.RaycastAll(p + Vector3.up * .3f, Vector3.down, 4f, ~0, QueryTriggerInteraction.Ignore)
			.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).ToList();
		if (hits.Count == 0) return false;
		float restY = hits[0].point.y;
		if (hits.Skip(1).Any(h => restY - h.point.y > .15f && restY - h.point.y < 1.8f)) return true;   // shelf or table over a floor
		// Otherwise it must be lying on the floor itself: a wide flat area around it.
		int flat = 0, total = 0;
		for (float x = -1f; x <= 1f; x += .1f)
			for (float z = -1f; z <= 1f; z += .1f)
			{
				total++;
				var down = Physics.RaycastAll(new Vector3(p.x + x, restY + .3f, p.z + z), Vector3.down, .6f, ~0, QueryTriggerInteraction.Ignore)
					.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).FirstOrDefault();
				if (down.collider && Mathf.Abs(down.point.y - restY) < .03f) flat++;
			}
		return flat > total * .7f;
	}

	internal static bool NearWall(Vector3 p)
	{
		var from = p + Vector3.up * .1f;
		for (int a = 0; a < 360; a += 20)
			if (Physics.RaycastAll(from, Quaternion.Euler(0f, a, 0f) * Vector3.forward, .8f, ~0, QueryTriggerInteraction.Ignore)
				.Any(h => !h.collider.GetComponent<ItemSeedData>())) return true;
		return false;
	}

	// Centres the named spots on the flat surface they lie on, then rebuilds and saves.
	// -executeMethod ChainCutterSetup.CentreSpots -spots "- ChainCutterPreset2|ChainCutterSeedSpot 8"
	public static void CentreSpots()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-spots");
		var wanted = at >= 0 && at + 1 < args.Length ? args[at + 1].Split('|') : new string[0];
		EditorSceneManager.OpenScene(SceneFile);
		Physics.SyncTransforms();
		foreach (var spot in All<Transform>().Where(t => wanted.Any(w => w.Length > 0 && t.name.StartsWith(w))).ToList()) CentreOnTop(spot);
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log("CHAINSPOT centred and saved");
	}

	// For a spot with no solid surface to map: moves it out from any wall closer than the cutter's half length,
	// and turns it to lie along the wall.
	static void PushOffWalls(Transform spot)
	{
		const float clearance = .9f;   // the handles reach about 0.7 m from the pivot
		var p = spot.position + Vector3.up * .06f;
		RaycastHit nearest = default;
		for (int a = 0; a < 360; a += 15)
		{
			var dir = Quaternion.Euler(0f, a, 0f) * Vector3.forward;
			if (Physics.Raycast(p, dir, out RaycastHit hit, clearance, ~0, QueryTriggerInteraction.Ignore) && !hit.collider.GetComponent<ItemSeedData>()
				&& (!nearest.collider || hit.distance < nearest.distance)) nearest = hit;
		}
		if (!nearest.collider) { Debug.Log($"CHAINSPOT {spot.name}: no wall near it, left as is"); return; }
		var away = Vector3.ProjectOnPlane(nearest.normal, Vector3.up).normalized;
		spot.position += away * (clearance - nearest.distance);
		var along = Vector3.Cross(Vector3.up, away);
		spot.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(along).eulerAngles.y, 90f);
		Debug.Log($"CHAINSPOT {spot.name}: moved {clearance - nearest.distance:0.00} m out from {PathOf(nearest.collider.transform)} to {spot.position}");
	}

	// Maps the flat top under a spot by probing straight down around it, then moves the spot to its middle,
	// lengthways along it.
	internal static void CentreOnTop(Transform spot)
	{
		var down = Physics.RaycastAll(spot.position + Vector3.up * .3f, Vector3.down, 1f, ~0, QueryTriggerInteraction.Ignore)
			.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).FirstOrDefault();
		if (!down.collider) { PushOffWalls(spot); return; }
		float topY = down.point.y;
		var points = new List<Vector3>();
		for (float x = -1f; x <= 1f; x += .04f)
			for (float z = -1f; z <= 1f; z += .04f)
			{
				var from = new Vector3(spot.position.x + x, topY + .3f, spot.position.z + z);
				var hit = Physics.RaycastAll(from, Vector3.down, .6f, ~0, QueryTriggerInteraction.Ignore)
					.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).FirstOrDefault();
				if (hit.collider && Mathf.Abs(hit.point.y - topY) < .02f) points.Add(hit.point);
			}
		// Keep only the part of the flat area connected to the spot (not another surface at the same height nearby).
		var area = new List<Vector3> { down.point };
		var queue = new Queue<Vector3>(area);
		var left = new List<Vector3>(points);
		while (queue.Count > 0)
		{
			var p = queue.Dequeue();
			var next = left.Where(q => (q - p).sqrMagnitude < .06f * .06f).ToList();
			foreach (var q in next) { left.Remove(q); area.Add(q); queue.Enqueue(q); }
		}
		var centre = area.Aggregate(Vector3.zero, (s, q) => s + q) / area.Count;
		float xx = 0f, zz = 0f, xz = 0f;
		foreach (var q in area) { float dx = q.x - centre.x, dz = q.z - centre.z; xx += dx * dx; zz += dz * dz; xz += dx * dz; }
		float yaw = .5f * Mathf.Atan2(2f * xz, xx - zz) * Mathf.Rad2Deg;   // direction the top is longest in, from +X
		spot.SetPositionAndRotation(new Vector3(centre.x, topY + .05f, centre.z), Quaternion.Euler(0f, 90f - yaw, 90f));
		Debug.Log($"CHAINSPOT {spot.name}: flat top of {area.Count * .0016f:0.00} mÂ² under it, centred at {spot.position}");
	}

	// Command line: drops both chains as if cut and simulates 3 seconds of physics, logging where they end up
	// (nothing is saved). -executeMethod ChainCutterSetup.FallTest
	public static void FallTest()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var balloon = Find("EntireMansion/Property/ImportantInteractables/HotAirBalloon/HotAirBalloonPlaced");
		for (var t = balloon; t; t = t.parent) t.gameObject.SetActive(true);
		var chains = All<BalloonChains>().First();
		Physics.simulationMode = SimulationMode.Script;
		var bodies = new List<Rigidbody>();
		foreach (var chain in chains.Chains)
		{
			chain.transform.SetParent(null, true);
			var body = chain.AddComponent<Rigidbody>();
			body.mass = 4f; body.linearDamping = .3f; body.angularDamping = .6f;
			body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			var away = Vector3.ProjectOnPlane(chain.transform.position - chains.transform.position, Vector3.up).normalized;
			body.AddForce(away * 1.5f + Vector3.down, ForceMode.VelocityChange);
			body.AddTorque(Vector3.Cross(Vector3.up, away) * 2f, ForceMode.VelocityChange);
			bodies.Add(body);
			Debug.Log($"FALLTEST start {chain.GetComponent<Renderer>().bounds.min.y:0.00} (bottom of chain)");
		}
		Physics.SyncTransforms();
		for (int i = 0; i < 150; i++) Physics.Simulate(.02f);
		foreach (var body in bodies)
			Debug.Log($"FALLTEST after 3 s: bottom at y {body.GetComponent<Renderer>().bounds.min.y:0.00}, speed {body.linearVelocity.magnitude:0.00}, sleeping {body.IsSleeping()}");
		Physics.simulationMode = SimulationMode.FixedUpdate;
	}

	// Command line: checks the cutter model's faces and renders it from all round (nothing is saved).
	// -executeMethod ChainCutterSetup.CutterAngles -previewDir <folder>
	public static void CutterAngles()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		var mesh = CutterMesh;
		var v = mesh.vertices; var n = mesh.normals; var tri = mesh.triangles;
		int agree = 0, total = tri.Length / 3;
		var centre = mesh.bounds.center;
		int outward = 0;
		for (int i = 0; i < tri.Length; i += 3)
		{
			var face = Vector3.Cross(v[tri[i + 1]] - v[tri[i]], v[tri[i + 2]] - v[tri[i]]);
			if (Vector3.Dot(face, n[tri[i]] + n[tri[i + 1]] + n[tri[i + 2]]) > 0) agree++;
			var mid = (v[tri[i]] + v[tri[i + 1]] + v[tri[i + 2]]) / 3f;
			if (Vector3.Dot(face, mid - centre) > 0) outward++;
		}
		Debug.Log($"CUTTERTEST {total} triangles: {agree} wound the same way as their normals, {outward} facing outward from the middle; submeshes {mesh.subMeshCount}");
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var cutter = All<ChainCutterItem>().First().transform;
		var target = cutter.position;
		var look = cutter.GetComponent<MeshRenderer>().bounds.center;
		int k = 0;
		foreach (float h in new[] { .12f, .6f })
			for (int a = 0; a < 360; a += 90)
			{
				var from = look + Quaternion.Euler(0f, a, 0f) * Vector3.forward * .9f + Vector3.up * h;
				Shot($"{dir}/cutter_angle_{k++}.png", from, look);
			}
		Debug.Log("CUTTERTEST pictures done");
	}

	// Command line: logs what each chain cutter spot is lying on, to describe where it is.
	public static void Describe()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Physics.SyncTransforms();
		var spots = All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset") || t.name.StartsWith("ChainCutterSeedSpot"))
			.OrderBy(t => t.name).ToList();
		foreach (var s in spots)
		{
			var hit = Physics.RaycastAll(s.position + Vector3.up * .3f, Vector3.down, 2f, ~0, QueryTriggerInteraction.Ignore)
				.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).FirstOrDefault();
			string on = hit.collider ? PathOf(hit.collider.transform) : "nothing below";
			Debug.Log($"CHAINSPOT {s.name} at {s.position} on: {on}");
		}
	}

	public static void RunBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log("CHAINCUTTER: done and saved");
	}

	// ---------- helpers ----------

	internal static string PathOf(Transform t)
	{
		string path = t.name;
		for (t = t.parent; t; t = t.parent) path = t.name + "/" + path;
		return path;
	}

	internal static Transform Find(string path)
	{
		foreach (var t in Resources.FindObjectsOfTypeAll<Transform>())
			if (t.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(t) && PathOf(t) == path) return t;
		Debug.LogError("CHAINCUTTER: not found: " + path);
		return null;
	}

	internal static IEnumerable<T> All<T>() where T : Component =>
		Resources.FindObjectsOfTypeAll<T>().Where(c => c.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(c));

	internal static void Kill(Object o) { if (o) Object.DestroyImmediate(o is Component c ? c.gameObject : o); }

	static Material MakeMaterial(string name, System.Action<Material> setUp)
	{
		string path = Folder + "/" + name + ".mat";
		var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!mat) { mat = new Material(Shader.Find("Standard")); AssetDatabase.CreateAsset(mat, path); }
		setUp(mat);
		EditorUtility.SetDirty(mat);
		return mat;
	}

	static void SetLook(GameObject g, Mesh mesh, Material mat)
	{
		g.GetComponent<MeshFilter>().sharedMesh = mesh;
		g.GetComponent<MeshRenderer>().sharedMaterials = new[] { mat };
	}

	// ---------- build ----------

	static void Build()
	{
		var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(Folder + "/ChainCutterMesh.asset");
		var normalTex = AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/CuttingPlier.png");
		var nightmareTex = AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/CuttingPlierNM.png");
		var cutClip = AssetDatabase.LoadAssetAtPath<AudioClip>(Folder + "/cutChain.ogg");
		if (!mesh || !normalTex || !cutClip) { Debug.LogError("CHAINCUTTER: assets missing in " + Folder); return; }
		var mat = MakeMaterial("ChainCutter", m => { m.mainTexture = normalTex; m.SetFloat("_Metallic", .35f); m.SetFloat("_Glossiness", .45f); });
		var metal = MakeMaterial("ChainMetal", m => { m.color = new Color(.42f, .42f, .44f); m.SetFloat("_Metallic", .85f); m.SetFloat("_Glossiness", .5f); });

		RemoveOld();

		var valveWorld = Find("Objects/MainItemSelection/MapItems/GasValve");
		var valveHand = Find("PlayerStuff/HandHoldObjects/GasValveHand");
		var inventory = All<Inventory>().FirstOrDefault();
		if (!valveWorld || !valveHand || !inventory) return;
		var valveDef = inventory.GetItemDefByName("Valve");

		GameObject prefab = BuildDropPrefab(mesh, mat);
		GameObject world = BuildWorldItem(valveWorld, mesh, mat);
		GameObject hand = BuildHand(valveHand, mesh, mat);

		inventory.ItemDefs.Add(new ItemDefs
		{
			itemName = ItemName, handObject = hand, dropPoint = valveDef?.dropPoint, textHighlighted = "Chain cutter",
			dropPrefabObject = prefab, pickAudioClip = valveDef?.pickAudioClip,
		});
		EditorUtility.SetDirty(inventory);

		Physics.SyncTransforms();
		SetUpPresets(world);
		SetUpSeedSystem(world);
		BuildChains(metal, cutClip);
		AddNightmareTexture(mat, normalTex, nightmareTex);
	}

	static void RemoveOld()
	{
		foreach (var item in All<ChainCutterItem>().ToList()) Kill(item);
		foreach (var chains in All<BalloonChains>().ToList()) Kill(chains);
		var oldHand = All<Transform>().FirstOrDefault(t => t.name == "ChainCutterHand" && t.parent && t.parent.name == "HandHoldObjects");
		Kill(oldHand);
		// Spots are kept, so fixes made to them survive a rebuild (delete a spot to have it picked again).
		foreach (var inv in All<Inventory>()) { inv.ItemDefs.RemoveAll(d => d == null || d.itemName == ItemName); EditorUtility.SetDirty(inv); }
		foreach (var sm in All<SeedManager>())
		{
			sm.allItems?.RemoveAll(i => !i);
			sm.requiredEscapeItemNames?.RemoveAll(n => n == ItemName);
			if (sm.spawnAreas != null) sm.spawnAreas = sm.spawnAreas.Where(a => a != null && a.areaName != SeedSpotsName).ToArray();
			EditorUtility.SetDirty(sm);
		}
		var mat = AssetDatabase.LoadAssetAtPath<Material>(Folder + "/ChainCutter.mat");
		foreach (var tc in All<Texture_Changing>().Where(t => t.Material == mat || !t.Material).ToList())
			if (tc.Material == mat) Object.DestroyImmediate(tc);
	}

	static GameObject BuildDropPrefab(Mesh mesh, Material mat)
	{
		string path = Folder + "/ChainCutter.prefab";
		if (!AssetDatabase.LoadAssetAtPath<GameObject>(path)) AssetDatabase.CopyAsset("Assets/GameObject/GasValve.prefab", path);
		var root = PrefabUtility.LoadPrefabContents(path);
		MakeCutter(root, mesh, mat);
		foreach (var preset in root.GetComponents<ItemPresetSetup>()) Object.DestroyImmediate(preset);
		PrefabUtility.SaveAsPrefabAsset(root, path);
		PrefabUtility.UnloadPrefabContents(root);
		return AssetDatabase.LoadAssetAtPath<GameObject>(path);
	}

	// Turns a valve copy into the chain cutter: model, size, collider, item name.
	static void MakeCutter(GameObject g, Mesh mesh, Material mat)
	{
		g.name = "ChainCutter";
		SetLook(g, mesh, mat);
		g.transform.localScale = Vector3.one * WorldScale;
		var box = g.GetComponent<BoxCollider>();
		box.size = ColliderSize;
		box.center = ColliderCentre;
		// Its landing sound: the game's dropped-crowbar clank (the copy came with the valve knob's).
		var landClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClip/DropCrowbar.ogg");
		foreach (var trigger in g.GetComponentsInChildren<SoundTrigger>(true)) if (landClip) trigger.Sounds = new[] { landClip };
		var data = g.GetComponent<ItemSeedData>();
		data.itemName = ItemName;
		data.category = 1;   // escape item
		data.containedItems = new List<string>();
	}

	static GameObject BuildWorldItem(Transform valve, Mesh mesh, Material mat)
	{
		var g = Object.Instantiate(valve.gameObject, valve.parent);
		MakeCutter(g, mesh, mat);
		g.SetActive(true);
		g.AddComponent<ChainCutterItem>();
		return g;
	}

	static GameObject BuildHand(Transform valveHand, Mesh mesh, Material mat)
	{
		var g = Object.Instantiate(valveHand.gameObject, valveHand.parent);
		g.name = "ChainCutterHand";
		SetLook(g, mesh, mat);
		g.transform.localPosition = HandPosition;
		g.transform.localRotation = HandRotation;
		g.transform.localScale = Vector3.one * HandScale;
		g.SetActive(false);
		return g;
	}

	// ---------- spots ----------

	// Lying flat on whatever is below the spot (the cutter's thin side down).
	internal static Quaternion LyingFlat(Quaternion spot) => Quaternion.Euler(0f, spot.eulerAngles.y, 90f);

	internal static Vector3 OnSurface(Vector3 p)
	{
		var hits = Physics.RaycastAll(p + Vector3.up * .4f, Vector3.down, 2f, ~0, QueryTriggerInteraction.Ignore)
			.Where(h => !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).ToArray();
		return hits.Length > 0 ? hits[0].point + Vector3.up * .05f : p;
	}

	// Whether there's something solid just under this point (not floating in the air).
	internal static bool HasSurface(Vector3 p) =>
		Physics.RaycastAll(p + Vector3.up * .4f, Vector3.down, 1.1f, ~0, QueryTriggerInteraction.Ignore).Any(h => !h.collider.GetComponent<ItemSeedData>());

	static Transform MakeSpot(string name, Transform parent, Transform from)
	{
		var spot = new GameObject(name).transform;
		spot.SetParent(parent, false);
		spot.SetPositionAndRotation(OnSurface(from.position), LyingFlat(from.rotation));
		return spot;
	}

	internal static bool FarFrom(Vector3 p, IEnumerable<Vector3> others, float distance) => others.All(o => (o - p).sqrMagnitude > distance * distance);

	// Picks spread-out spots from the candidates, away from the given taken positions and from each other.
	internal static List<Transform> Pick(List<Transform> candidates, List<Vector3> taken, int count)
	{
		var rng = new System.Random(351);
		var pool = candidates.Where(c => c && FarFrom(c.position, taken, .8f) && HasSurface(c.position)).OrderBy(_ => rng.Next()).ToList();
		var chosen = new List<Transform>();
		while (chosen.Count < count && pool.Count > 0)
		{
			// The candidate furthest from everything chosen so far.
			var best = chosen.Count == 0 ? pool[0] : pool.OrderByDescending(c => chosen.Min(x => (x.position - c.position).sqrMagnitude)).First();
			chosen.Add(best);
			pool.Remove(best);
		}
		return chosen;
	}

	// Seed mode's spots aren't used in preset mode, so each preset gets one of those, away from its own items.
	static void SetUpPresets(GameObject world)
	{
		var presetRoot = Find(PresetRoot);
		var seedSpots = All<SeedManager>().SelectMany(s => s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null)
			.SelectMany(a => a.freeSpots).Where(t => t).Distinct().ToList();
		var setup = world.GetComponent<ItemPresetSetup>();
		var mains = new Transform[6];
		var used = new List<Vector3>();
		for (int i = 0; i < 6; i++)
		{
			var preset = presetRoot ? presetRoot.Find("Preset_" + (i + 1)) : null;
			if (!preset) { Debug.LogWarning("CHAINCUTTER: no Preset_" + (i + 1)); continue; }
			string spotName = $"- ChainCutterPreset{i + 1} / Main+Extras";
			var kept = preset.Cast<Transform>().FirstOrDefault(c => c.name == spotName);   // Find() would read the "/" as a path
			if (kept) { mains[i] = kept; used.Add(kept.position); continue; }
			var taken = preset.GetComponentsInChildren<Transform>(true).Where(t => t != preset).Select(t => t.position)
				.Concat(used).Concat(All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset")).Select(t => t.position)).ToList();
			var pick = Pick(seedSpots, taken, 1).FirstOrDefault();
			if (!pick) { Debug.LogWarning("CHAINCUTTER: no free spot for preset " + (i + 1)); continue; }
			mains[i] = MakeSpot(spotName, preset, pick);
			used.Add(mains[i].position);
			Debug.Log($"CHAINCUTTER: preset {i + 1} spot {mains[i].position} (from {PathOf(pick)})");
		}
		setup.PositionMain_P1 = mains[0]; setup.PositionMain_P2 = mains[1]; setup.PositionMain_P3 = mains[2];
		setup.PositionMain_P4 = mains[3]; setup.PositionMain_P5 = mains[4]; setup.PositionMain_P6 = mains[5];
		setup.PositionExtra_P1 = setup.PositionExtra_P2 = setup.PositionExtra_P3 = setup.PositionExtra_P4 = setup.PositionExtra_P5 = setup.PositionExtra_P6 = null;
		if (mains[0]) world.transform.SetPositionAndRotation(mains[0].position, mains[0].rotation);
		EditorUtility.SetDirty(setup);
	}

	// Preset mode's spots aren't used in seed mode, so the cutter's own seed spots come from those, away from seed spots.
	static void SetUpSeedSystem(GameObject world)
	{
		var managers = All<SeedManager>().Where(s => SeedManagersWithBalloon.Contains(s.gameObject.name)).ToList();
		if (managers.Count == 0) { Debug.LogError("CHAINCUTTER: balloon seed managers not found"); return; }
		var seedTaken = All<SeedManager>().SelectMany(s => (s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null).SelectMany(a => a.freeSpots)
			.Concat((s.puzzleDefs ?? new List<PuzzleDef>()).Select(p => p?.spawnPoint))).Where(t => t).Select(t => t.position).ToList();
		var presetRoot = Find(PresetRoot);
		var presetSpots = presetRoot.GetComponentsInChildren<Transform>(true)
			.Where(t => t.parent && t.parent.parent == presetRoot && !t.name.StartsWith("- ChainCutter")).ToList();
		var holder = All<Transform>().FirstOrDefault(t => t.name == SeedSpotsName);
		Transform[] spots;
		if (holder && holder.childCount > 0) spots = holder.Cast<Transform>().ToArray();
		else
		{
			if (!holder) { holder = new GameObject(SeedSpotsName).transform; holder.SetParent(managers[0].transform.parent, false); }
			spots = Pick(presetSpots, seedTaken, 8).Select((p, i) => MakeSpot("ChainCutterSeedSpot " + (i + 1), holder, p)).ToArray();
		}
		foreach (var s in spots) Debug.Log($"CHAINCUTTER: seed spot {s.position}");
		foreach (var sm in managers)
		{
			if (sm.allItems == null) sm.allItems = new List<GameObject>();
			if (!sm.allItems.Contains(world)) sm.allItems.Add(world);
			if (sm.requiredEscapeItemNames == null) sm.requiredEscapeItemNames = new List<string>();
			sm.requiredEscapeItemNames.Add(ItemName);
			// Spots marked "(Extras only)" are in areas only open with Extras, so only the Extras seed system uses them.
			bool extras = sm.gameObject.name.EndsWith("Extras");
			var own = spots.Where(s => extras || !s.name.Contains(ExtrasOnly)).ToArray();
			var area = new SpawnArea { areaName = SeedSpotsName, freeSpots = own, maxItems = 1, allowedItemNames = new List<string> { ItemName } };
			sm.spawnAreas = (sm.spawnAreas ?? new SpawnArea[0]).Concat(new[] { area }).ToArray();
			EditorUtility.SetDirty(sm);
			Debug.Log($"CHAINCUTTER: added to {sm.gameObject.name} ({sm.allItems.Count} items, {own.Length} own spots)");
		}
	}

	// ---------- chains ----------

	// A layer the pickup ray sees but the player walks through, so the chains can be clicked without blocking the way.
	static int ChainLayer()
	{
		int player = LayerMask.NameToLayer("Player");
		var pick = All<PickRay>().FirstOrDefault();
		int mask = pick ? pick.LayersIncluded.value : 1;
		for (int l = 0; l < 32; l++)
			if ((mask & (1 << l)) != 0 && LayerMask.LayerToName(l) != "Item" && Physics.GetIgnoreLayerCollision(player, l))
			{ Debug.Log("CHAINCUTTER: chain layer " + LayerMask.LayerToName(l)); return l; }
		Debug.LogWarning("CHAINCUTTER: no pass-through layer found; chains use Door");
		return LayerMask.NameToLayer("Door");
	}

	static Mesh linkMesh;

	// One oval chain link (a torus stretched along Z), about 17 cm long.
	static Mesh LinkMesh()
	{
		if (linkMesh) return linkMesh;
		const int around = 20, tube = 8;
		const float halfStraight = .035f, radius = .04f, thick = .013f;
		var verts = new List<Vector3>(); var normals = new List<Vector3>(); var tris = new List<int>();
		for (int i = 0; i < around; i++)
		{
			float a = i / (float)around * Mathf.PI * 2f;
			var dir = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
			var centre = dir * radius + new Vector3(0f, 0f, Mathf.Sign(dir.z) * halfStraight);
			for (int j = 0; j < tube; j++)
			{
				float b = j / (float)tube * Mathf.PI * 2f;
				var n = dir * Mathf.Cos(b) + Vector3.up * Mathf.Sin(b);
				verts.Add(centre + n * thick); normals.Add(n);
			}
		}
		for (int i = 0; i < around; i++)
			for (int j = 0; j < tube; j++)
			{
				int a = i * tube + j, b = ((i + 1) % around) * tube + j, c = ((i + 1) % around) * tube + (j + 1) % tube, d = i * tube + (j + 1) % tube;
				tris.AddRange(new[] { a, d, b, b, d, c });
			}
		linkMesh = new Mesh { name = "ChainLink" };
		linkMesh.SetVertices(verts); linkMesh.SetNormals(normals); linkMesh.SetTriangles(tris, 0);
		linkMesh.RecalculateBounds();
		return linkMesh;
	}

	// A chain of links from a to b, as one saved mesh, with a thin collider along it.
	static GameObject MakeChain(string name, Transform parent, Vector3 a, Vector3 b, Material metal, int layer, bool collider)
	{
		var g = new GameObject(name);
		g.transform.SetParent(parent, true);
		g.transform.position = a;
		g.transform.rotation = Quaternion.LookRotation(b - a);
		Vector3 parentScale = parent.lossyScale;
		g.transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);
		float length = Vector3.Distance(a, b), step = .115f;
		int count = Mathf.Max(1, Mathf.CeilToInt(length / step));
		var parts = new CombineInstance[count];
		for (int i = 0; i < count; i++)
			parts[i] = new CombineInstance
			{
				mesh = LinkMesh(),
				transform = Matrix4x4.TRS(new Vector3(0f, 0f, (i + .5f) * length / count), Quaternion.Euler(0f, 0f, i % 2 == 0 ? 0f : 90f), Vector3.one),
			};
		var mesh = new Mesh { name = name };
		mesh.CombineMeshes(parts, true, true);
		mesh.RecalculateBounds();
		string path = Folder + "/" + name.Replace(' ', '_') + ".asset";
		AssetDatabase.DeleteAsset(path);
		AssetDatabase.CreateAsset(mesh, path);
		g.AddComponent<MeshFilter>().sharedMesh = mesh;
		var r = g.AddComponent<MeshRenderer>();
		r.sharedMaterial = metal;
		g.layer = layer;
		if (collider)
		{
			var c = g.AddComponent<CapsuleCollider>();
			c.direction = 2;
			c.radius = .1f;
			c.height = length;
			c.center = new Vector3(0f, 0f, length / 2f);
		}
		return g;
	}

	static void BuildChains(Material metal, AudioClip cutClip)
	{
		var balloon = Find("EntireMansion/Property/ImportantInteractables/HotAirBalloon/HotAirBalloonPlaced");
		if (!balloon) return;
		// The basket's world box, from its own model (the object is off in the editor, so renderer bounds are empty).
		var filter = balloon.GetComponent<MeshFilter>();
		if (!filter || !filter.sharedMesh) { Debug.LogError("CHAINCUTTER: balloon has no model"); return; }
		var lb = filter.sharedMesh.bounds;
		var corners = new List<Vector3>();
		for (int i = 0; i < 8; i++)
			corners.Add(balloon.TransformPoint(new Vector3(i % 2 == 0 ? lb.min.x : lb.max.x, (i / 2) % 2 == 0 ? lb.min.y : lb.max.y, i / 4 == 0 ? lb.min.z : lb.max.z)));
		var box = new Bounds(corners[0], Vector3.zero);
		foreach (var c in corners) box.Encapsulate(c);
		Debug.Log($"CHAINCUTTER: balloon basket box {box.center} size {box.size}");

		var root = new GameObject("BalloonChains").transform;
		root.SetParent(balloon, false);
		var chains = root.gameObject.AddComponent<BalloonChains>();
		chains.gameObject.AddComponent<HazardDifficulty>();
		var sound = root.gameObject.AddComponent<AudioSource>();
		sound.clip = cutClip; sound.playOnAwake = false; sound.spatialBlend = 1f; sound.minDistance = 2f; sound.maxDistance = 40f;
		var creak = All<TriggerCreakNoise>().FirstOrDefault(c => c.NoiseObj);
		chains.NoiseObj = creak ? creak.NoiseObj : null;

		int layer = ChainLayer();
		var stakeMat = metal;
		var chainList = new List<GameObject>(); var ends = new List<GameObject>();
		// Two chains on opposite sides of the basket, tied at its middle band. Each starts right on the basket's wall: a ray
		// against a temporary copy of the basket's shape finds the wall (the box corners stick out past the real basket).
		float tieHeight = box.min.y + Mathf.Min(box.size.y * .17f, 1.1f);
		var wall = new GameObject("TempBasketShape");
		wall.transform.SetPositionAndRotation(balloon.position, balloon.rotation);
		wall.transform.localScale = balloon.lossyScale;
		var wallCollider = wall.AddComponent<MeshCollider>();
		wallCollider.sharedMesh = filter.sharedMesh;
		Physics.SyncTransforms();
		// Along the basket's own sideways axis, so the chains come straight off two facing walls.
		var side = Vector3.ProjectOnPlane(balloon.right, Vector3.up).normalized;
		if (side.sqrMagnitude < .01f) side = Vector3.right;
		int n = 0;
		foreach (var outward in new[] { side, -side })
		{
			n++;
			var centre = new Vector3(box.center.x, tieHeight, box.center.z);
			Vector3 tie;
			var from = centre + outward * (box.extents.magnitude + 1f);
			if (wallCollider.Raycast(new Ray(from, -outward), out RaycastHit wallHit, 10f))
				tie = wallHit.point - outward * .04f;   // a little into the wicker, so the first link sits on it
			else
			{
				tie = centre + outward * Mathf.Min(box.extents.x, box.extents.z);
				Debug.LogWarning("CHAINCUTTER: basket wall not found; chain " + n + " tied at the box edge");
			}
			Debug.Log($"CHAINCUTTER: chain {n} tied at {tie}");
			var groundGuess = tie + outward * 1.8f;
			var groundHit = Physics.RaycastAll(groundGuess + Vector3.up * 3f, Vector3.down, 12f, ~0, QueryTriggerInteraction.Ignore)
				.Where(h => !h.collider.transform.IsChildOf(balloon.parent) && h.collider != wallCollider).OrderBy(h => h.distance).FirstOrDefault();
			var ground = groundHit.collider ? groundHit.point : new Vector3(groundGuess.x, box.min.y, groundGuess.z);
			if (groundHit.collider)
				Debug.Log($"CHAINCUTTER: ground under chain {n} is {groundHit.collider.name} on layer {LayerMask.LayerToName(groundHit.collider.gameObject.layer)}; " +
					$"a fallen chain {(Physics.GetIgnoreLayerCollision(layer, groundHit.collider.gameObject.layer) ? "FALLS THROUGH it" : "lands on it")}");
			// Ground stake.
			var stake = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			stake.name = "Chain Stake " + n;
			Object.DestroyImmediate(stake.GetComponent<Collider>());
			stake.transform.SetParent(root, true);
			stake.transform.position = ground + Vector3.up * .2f;
			Vector3 ps = root.lossyScale;
			stake.transform.localScale = new Vector3(.1f / ps.x, .2f / ps.y, .1f / ps.z);
			stake.GetComponent<Renderer>().sharedMaterial = stakeMat;
			var top = ground + Vector3.up * .36f;
			var chain = MakeChain("Balloon Chain " + n, root, tie, top, metal, layer, true);
			chain.name = "BalloonChain";   // PickRay matches this name
			chainList.Add(chain);
			// The bit left on the stake after cutting.
			var end = MakeChain("Balloon Chain End " + n, root, top, top + (tie - top).normalized * .3f, metal, layer, false);
			ends.Add(end);
		}
		Object.DestroyImmediate(wall);
		chains.Chains = chainList.ToArray();
		chains.CutEnds = ends.ToArray();
		chains.CutSound = sound;
		chains.LandClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClip/DropMetalObj.ogg");
		EditorUtility.SetDirty(chains);
		AssetDatabase.SaveAssets();
	}

	static void AddNightmareTexture(Material mat, Texture2D normal, Texture2D nightmare)
	{
		var existing = All<Texture_Changing>().FirstOrDefault();
		if (!existing || !nightmare) return;
		var tc = existing.gameObject.AddComponent<Texture_Changing>();
		tc.Material = mat; tc.Default = normal; tc.DefaultNM = nightmare;
		EditorUtility.SetDirty(existing.gameObject);
	}
}
