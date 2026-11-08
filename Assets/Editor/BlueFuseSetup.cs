using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using C = ChainCutterSetup;

// Tools > Evolved Restoration > Set Up Blue Fuse (Slientlo Never Clear only): a blue copy of the gate fuse, its socket on
// the wall by the gate controls (Add Blue Fuse Socket), and the gate button needing it as well as the generator.
// The fuse gets a spot in each preset and its own seed spots (a required escape item in every seed system).
// Running it again rebuilds everything but keeps the spots.
public static class BlueFuseSetup
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string Folder = "Assets/GateFuse";
	const string ItemName = BlueFuse.ItemName;
	const string PresetRoot = "Objects/MainItemSelection/ItemSpawnsAcrossMap (Preset Mode)";
	const string SeedSpotsName = "BlueFuseSeedSpots";
	const string GateControl = "EntireMansion/Property/ImportantInteractables/GateControl";
	static readonly Color Blue = new Color(.4f, .62f, 1f);

	[MenuItem("Tools/Evolved Restoration/Set Up Blue Fuse")]
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
		Debug.Log("BLUEFUSE done and saved");
	}

	// ---------- materials ----------

	// A blue copy of a material (kept next to the others in Assets/GateFuse), with the faint glow added objects need.
	static Material BlueCopy(Material source)
	{
		if (!source) return null;
		string path = $"{Folder}/Blue_{source.name}.mat";
		var m = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!m) { m = new Material(source); AssetDatabase.CreateAsset(m, path); }
		m.CopyPropertiesFromMaterial(source);
		m.color = source.HasProperty("_Color") ? source.color * Blue : Blue;
		EditorUtility.SetDirty(m);
		return m;
	}

	static void TintBlue(GameObject g, bool glow)
	{
		foreach (var r in g.GetComponentsInChildren<Renderer>(true))
		{
			var mats = r.sharedMaterials.Select(BlueCopy).ToArray();
			if (glow) foreach (var m in mats.Where(m => m)) SwitchBoxLock.Glow(m, m.mainTexture, m.color * SwitchBoxLock.GlowStrength);
			r.sharedMaterials = mats;
		}
	}

	// ---------- build ----------

	static void Build()
	{
		if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets", "GateFuse");
		RemoveOld();
		var fuseWorld = C.Find("Objects/MainItemSelection/MapItems/GateFuse / 1");
		var fuseHand = C.Find("PlayerStuff/HandHoldObjects/CamMonitorFuseHand");
		var inventory = C.All<Inventory>().FirstOrDefault();
		var socket = C.Find(GateControl + "/BlueFuseSocket");
		if (!fuseWorld || !fuseHand || !inventory) { Debug.LogError("BLUEFUSE gate fuse, hand or inventory not found"); return; }
		if (!socket) { SwitchBoxLock.AddBlueSocket(); socket = C.Find(GateControl + "/BlueFuseSocket"); }
		var fuseDef = inventory.GetItemDefByName("Fuse");

		// Dropped copy.
		string prefabPath = Folder + "/BlueFuse.prefab";
		if (!AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath)) AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(fuseDef.dropPrefabObject), prefabPath);
		var root = PrefabUtility.LoadPrefabContents(prefabPath);
		root.name = "BlueFuse";
		TintBlue(root, false);
		var data = root.GetComponent<ItemSeedData>();
		if (data) { data.itemName = ItemName; data.category = 1; }
		foreach (var preset in root.GetComponents<ItemPresetSetup>()) Object.DestroyImmediate(preset);
		PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
		PrefabUtility.UnloadPrefabContents(root);
		var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

		// Lying in the map.
		var world = Object.Instantiate(fuseWorld.gameObject, fuseWorld.parent);
		world.name = "BlueFuse";
		world.SetActive(true);
		TintBlue(world, false);
		var worldData = world.GetComponent<ItemSeedData>();
		worldData.itemName = ItemName; worldData.category = 1; worldData.containedItems = new List<string>();
		world.AddComponent<BlueFuseItem>();

		// In the hand.
		var hand = Object.Instantiate(fuseHand.gameObject, fuseHand.parent);
		hand.name = "BlueFuseHand";
		hand.SetActive(false);
		TintBlue(hand, false);

		inventory.ItemDefs.Add(new ItemDefs
		{
			itemName = ItemName, handObject = hand, dropPoint = fuseDef.dropPoint, textHighlighted = "Blue gate fuse",
			dropPrefabObject = prefab, pickAudioClip = fuseDef.pickAudioClip,
		});
		EditorUtility.SetDirty(inventory);

		var slot = BuildSlot(socket, hand);
		var button = C.Find(GateControl + "/CamMonitorButton");
		var gate = button.gameObject.AddComponent<BlueFuseGate>();
		gate.Slot = slot;
		EditorUtility.SetDirty(button.gameObject);

		AddNightmareTextures();
		Physics.SyncTransforms();
		SetUpPresets(world, fuseWorld);
		SetUpSeedSystem(world, fuseWorld);
		Debug.Log("BLUEFUSE built");
	}

	static void RemoveOld()
	{
		foreach (var item in C.All<BlueFuseItem>().ToList()) C.Kill(item);
		foreach (var slot in C.All<BlueFuseSlot>().ToList()) { if (slot.Placed) C.Kill(slot.Placed); C.Kill(slot); }
		foreach (var gate in C.All<BlueFuseGate>().ToList()) Object.DestroyImmediate(gate);
		C.Kill(C.All<Transform>().FirstOrDefault(t => t.name == "BlueFuseHand" && t.parent && t.parent.name == "HandHoldObjects"));
		foreach (var inv in C.All<Inventory>()) { inv.ItemDefs.RemoveAll(d => d == null || d.itemName == ItemName); EditorUtility.SetDirty(inv); }
		foreach (var sm in C.All<SeedManager>())
		{
			sm.allItems?.RemoveAll(i => !i);
			sm.requiredEscapeItemNames?.RemoveAll(n => n == ItemName);
			if (sm.spawnAreas != null) sm.spawnAreas = sm.spawnAreas.Where(a => a != null && a.areaName != SeedSpotsName).ToArray();
			EditorUtility.SetDirty(sm);
		}
		foreach (var tc in C.All<Texture_Changing>().Where(t => t.Material && t.Material.name.StartsWith("Blue_")).ToList()) Object.DestroyImmediate(tc);
	}

	// The click spot in the socket's hole, and the blue fuse that appears there once it's put in.
	static BlueFuseSlot BuildSlot(Transform socket, GameObject hand)
	{
		var original = C.Find(GateControl + "/FusePlaceCollider");
		var originalFuse = C.Find(GateControl + "/CamMonitorFusePlaced");
		var socketMesh = socket.GetComponent<MeshFilter>().sharedMesh;
		var ring = SwitchBoxLock.ExtractFuseSocket(out Vector3 originalCentre);
		float ringFront = ring.vertices.Min(v => v.z);   // how far the original fuse sticks out in front of its socket
		var front = socket.TransformPoint(new Vector3(0f, 0f, socketMesh.bounds.min.z));

		var placed = Object.Instantiate(originalFuse.gameObject, originalFuse.parent);
		placed.name = "BlueFusePlaced";
		// The socket's wall faces the same way as the original's when it isn't turned, so turn the copies with it.
		placed.transform.SetPositionAndRotation(front - socket.forward * ringFront, socket.rotation * originalFuse.rotation);
		placed.SetActive(false);
		TintBlue(placed, true);

		var spot = Object.Instantiate(original.gameObject, original.parent);
		spot.name = "BlueFusePlaceCollider";
		spot.transform.SetPositionAndRotation(front - socket.forward * .02f, socket.rotation * original.rotation);
		spot.SetActive(true);
		var puzzle = spot.GetComponent<RegPuzzle>();
		puzzle.requiredItem = hand;
		puzzle.performedAction = false;
		var slot = spot.AddComponent<BlueFuseSlot>();
		slot.Hand = hand; slot.Placed = placed; slot.Puzzle = puzzle; slot.Pick = C.All<PickRay>().FirstOrDefault();
		puzzle.performedEvent = new UnityEngine.Events.UnityEvent();
		UnityEventTools.AddPersistentListener(puzzle.performedEvent, slot.Place);
		// Part of the socket, so they move with it when the socket is moved in the editor.
		placed.transform.SetParent(socket, true);
		spot.transform.SetParent(socket, true);
		Debug.Log($"BLUEFUSE socket click spot at {spot.transform.position}, fuse shows at {placed.transform.position}");
		return slot;
	}

	// Nightmare mode swaps the fuse's texture; the blue copies follow it.
	static void AddNightmareTextures()
	{
		var changers = C.All<Texture_Changing>().ToList();
		if (changers.Count == 0) return;
		var host = changers[0].gameObject;
		foreach (var tc in changers.Where(t => t.Material && !t.Material.name.StartsWith("Blue_")).ToList())
		{
			var blue = AssetDatabase.LoadAssetAtPath<Material>($"{Folder}/Blue_{tc.Material.name}.mat");
			if (!blue) continue;
			var copy = host.AddComponent<Texture_Changing>();
			copy.Material = blue; copy.Default = tc.Default; copy.DefaultNM = tc.DefaultNM;
		}
		EditorUtility.SetDirty(host);
	}

	// ---------- spots ----------

	static float Lift(Transform fuse)
	{
		var r = fuse.GetComponent<Renderer>();
		return r ? fuse.position.y - r.bounds.min.y : .03f;
	}

	static Transform MakeSpot(string name, Transform parent, Vector3 at, float yaw, Transform fuse)
	{
		var spot = new GameObject(name).transform;
		spot.SetParent(parent, false);
		var p = C.OnSurface(at) - Vector3.up * .05f + Vector3.up * (Lift(fuse) + .005f);
		spot.SetPositionAndRotation(p, Quaternion.Euler(0f, yaw, 0f) * Quaternion.Euler(0f, -fuse.eulerAngles.y, 0f) * fuse.rotation);
		return spot;
	}

	// The same place as another spot: close sideways and not on a different floor (spots are compared before they're
	// settled onto the surface below, so height alone can't tell).
	static bool Clashes(Vector3 p, IEnumerable<Vector3> others) =>
		others.Any(o => new Vector2(o.x - p.x, o.z - p.z).sqrMagnitude < 1f && Mathf.Abs(o.y - p.y) < 2f);

	static IEnumerable<Vector3> OtherItemSpots() =>
		C.All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset") || t.name.StartsWith("ChainCutterSeedSpot ")).Select(t => t.position);

	static void SetUpPresets(GameObject world, Transform fuse)
	{
		var presetRoot = C.Find(PresetRoot);
		var seedSpots = C.All<SeedManager>().SelectMany(s => s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null)
			.SelectMany(a => a.freeSpots).Where(t => t && !t.name.Contains("Extras")).Distinct().ToList();
		var setup = world.GetComponent<ItemPresetSetup>();
		var mains = new Transform[6];
		var used = new List<Vector3>(OtherItemSpots());
		for (int i = 0; i < 6; i++)
		{
			var preset = presetRoot.Cast<Transform>().FirstOrDefault(c => c.name == "Preset_" + (i + 1));
			if (!preset) continue;
			string name = $"- BlueFusePreset{i + 1} / Main+Extras";
			var kept = preset.Cast<Transform>().FirstOrDefault(c => c.name == name);
			if (kept) { mains[i] = kept; used.Add(kept.position); continue; }
			var taken = preset.GetComponentsInChildren<Transform>(true).Where(t => t != preset).Select(t => t.position).Concat(used).ToList();
			var pick = C.Pick(seedSpots.Where(s => C.Reachable(s.position) && !Clashes(s.position, taken)).ToList(), taken, 1).FirstOrDefault();
			if (!pick) { Debug.LogWarning("BLUEFUSE no spot for preset " + (i + 1)); continue; }
			mains[i] = MakeSpot(name, preset, pick.position, pick.eulerAngles.y, fuse);
			used.Add(mains[i].position);
			Debug.Log($"BLUEFUSE preset {i + 1} spot {mains[i].position} (from {C.PathOf(pick)})");
		}
		setup.PositionMain_P1 = mains[0]; setup.PositionMain_P2 = mains[1]; setup.PositionMain_P3 = mains[2];
		setup.PositionMain_P4 = mains[3]; setup.PositionMain_P5 = mains[4]; setup.PositionMain_P6 = mains[5];
		setup.PositionExtra_P1 = setup.PositionExtra_P2 = setup.PositionExtra_P3 = setup.PositionExtra_P4 = setup.PositionExtra_P5 = setup.PositionExtra_P6 = null;
		if (mains[0]) world.transform.SetPositionAndRotation(mains[0].position, mains[0].rotation);
		EditorUtility.SetDirty(setup);
	}

	// The gate is in every game version, so the blue fuse is a required item in all four seed systems.
	static void SetUpSeedSystem(GameObject world, Transform fuse)
	{
		var managers = C.All<SeedManager>().ToList();
		var seedTaken = managers.SelectMany(s => (s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null).SelectMany(a => a.freeSpots)
			.Concat((s.puzzleDefs ?? new List<PuzzleDef>()).Select(p => p?.spawnPoint))).Where(t => t).Select(t => t.position)
			.Concat(OtherItemSpots()).ToList();
		var presetRoot = C.Find(PresetRoot);
		var holder = C.All<Transform>().FirstOrDefault(t => t.name == SeedSpotsName);
		Transform[] spots;
		if (holder && holder.childCount > 0) spots = holder.Cast<Transform>().ToArray();
		else
		{
			if (!holder) { holder = new GameObject(SeedSpotsName).transform; holder.SetParent(managers[0].transform.parent, false); }
			var candidates = presetRoot.GetComponentsInChildren<Transform>(true)
				.Where(t => t.parent && t.parent.parent == presetRoot && !t.name.StartsWith("- ChainCutter") && !t.name.StartsWith("- BlueFuse") && !t.name.Contains("Extras"))
				.Where(t => C.Reachable(t.position) && !C.NearWall(t.position) && !Clashes(t.position, seedTaken)).ToList();
			spots = C.Pick(candidates, seedTaken, 8).Select((p, i) => MakeSpot("BlueFuseSeedSpot " + (i + 1), holder, p.position, p.eulerAngles.y, fuse)).ToArray();
		}
		foreach (var s in spots) Debug.Log($"BLUEFUSE seed spot {s.position}");
		foreach (var sm in managers)
		{
			if (sm.allItems == null) sm.allItems = new List<GameObject>();
			if (!sm.allItems.Contains(world)) sm.allItems.Add(world);
			if (sm.requiredEscapeItemNames == null) sm.requiredEscapeItemNames = new List<string>();
			sm.requiredEscapeItemNames.Add(ItemName);
			bool extras = sm.gameObject.name.EndsWith("Extras");
			var own = spots.Where(s => extras || !s.name.Contains("(Extras only)")).ToArray();
			sm.spawnAreas = (sm.spawnAreas ?? new SpawnArea[0]).Concat(new[] { new SpawnArea { areaName = SeedSpotsName, freeSpots = own, maxItems = 1, allowedItemNames = new List<string> { ItemName } } }).ToArray();
			EditorUtility.SetDirty(sm);
			Debug.Log($"BLUEFUSE added to {sm.gameObject.name} ({own.Length} own spots)");
		}
	}

	// Picks all the blue fuse's spots again (presets and seed), then rebuilds and saves. -executeMethod BlueFuseSetup.RepickBatch
	public static void RepickBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		foreach (var t in C.All<Transform>().Where(t => t.name.StartsWith("- BlueFusePreset") || t.name.StartsWith("BlueFuseSeedSpot ")).ToList()) C.Kill(t);
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		var mine = C.All<Transform>().Where(t => t.name.StartsWith("- BlueFusePreset") || t.name.StartsWith("BlueFuseSeedSpot ")).ToList();
		var others = OtherItemSpots().ToList();
		foreach (var s in mine.OrderBy(s => s.name))
			Debug.Log($"BLUESPOT {s.name} at {s.position} on {(Physics.Raycast(s.position + Vector3.up * .2f, Vector3.down, out RaycastHit h, 1f) ? C.PathOf(h.collider.transform) : "nothing")}{(Clashes(s.position, others) ? "  CLASHES with a chain cutter spot" : "")}");
	}

	// Command line: pictures of the socket (empty and with the blue fuse in) and the fuse at preset 1 (nothing is saved).
	public static void Preview()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var socket = C.Find(GateControl + "/BlueFuseSocket");
		var slot = C.All<BlueFuseSlot>().First();
		var c = socket.position;
		Shot(dir + "/bluefuse_socket_wide.png", c + new Vector3(.4f, .15f, -1.8f), c + new Vector3(.4f, 0f, 0f), false);
		Shot(dir + "/bluefuse_socket_close.png", c + new Vector3(-.25f, .12f, -.8f), c, false);
		slot.Placed.SetActive(true);
		Shot(dir + "/bluefuse_socket_filled.png", c + new Vector3(-.25f, .12f, -.8f), c, false);
		var item = C.All<BlueFuseItem>().First().transform;
		Shot(dir + "/bluefuse_item.png", item.position + new Vector3(.5f, .45f, .5f), item.position, true);
		Debug.Log("BLUEFUSE pictures done");
	}

	// Without an extra light (as in the game, where only baked light and the glow light things), unless asked for.
	static void Shot(string file, Vector3 from, Vector3 lookAt, bool light)
	{
		var go = new GameObject("PreviewCam");
		var cam = go.AddComponent<Camera>();
		cam.transform.position = from; cam.transform.LookAt(lookAt);
		cam.fieldOfView = 55f; cam.nearClipPlane = .02f;
		Light l = null;
		if (light) { l = new GameObject("PreviewLight").AddComponent<Light>(); l.type = LightType.Directional; l.intensity = 1f; l.transform.rotation = Quaternion.Euler(40f, 30f, 0f); }
		var rt = new RenderTexture(900, 600, 24);
		cam.targetTexture = rt; cam.Render();
		RenderTexture.active = rt;
		var tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, 900, 600), 0, 0);
		System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
		RenderTexture.active = null;
		Object.DestroyImmediate(go); if (l) Object.DestroyImmediate(l.gameObject); Object.DestroyImmediate(rt);
	}
}
