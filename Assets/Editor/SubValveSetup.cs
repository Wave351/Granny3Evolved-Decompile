using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using C = ChainCutterSetup;

// Tools > Evolved Restoration > Set Up Submarine Valve (Slientlo Never Clear with Extras): a green copy of the balloon's
// valve knob goes on the submarine hatch and has to be turned before the handwheel opens it. The valve gets a spot in each
// preset and its own seed spots (a required escape item in the Extras seed system). Running it again keeps the spots.
public static class SubValveSetup
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string Folder = "Assets/SubValve";
	const string ItemName = SubValveItem.ItemName;
	const string PresetRoot = "Objects/MainItemSelection/ItemSpawnsAcrossMap (Preset Mode)";
	const string SeedSpotsName = "SubValveSeedSpots";
	const string Door = "EntireMansion/Property/Design/Granny_OmGi-Stuff/MiniSubmarine/SubmarineDoorAnimated/DoorSubmarine";
	static readonly Color Green = new Color(.35f, 1f, .4f);

	[MenuItem("Tools/Evolved Restoration/Set Up Submarine Valve")]
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
		Debug.Log("SUBVALVE done and saved");
	}

	static Material GreenCopy(Material source)
	{
		if (!source) return null;
		string path = $"{Folder}/Green_{source.name}.mat";
		var m = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!m) { m = new Material(source); AssetDatabase.CreateAsset(m, path); }
		m.CopyPropertiesFromMaterial(source);
		m.color = source.HasProperty("_Color") ? source.color * Green : Green;
		EditorUtility.SetDirty(m);
		return m;
	}

	static void TintGreen(GameObject g, bool glow)
	{
		foreach (var r in g.GetComponentsInChildren<Renderer>(true))
		{
			var mats = r.sharedMaterials.Select(GreenCopy).ToArray();
			if (glow) foreach (var m in mats.Where(m => m)) SwitchBoxLock.Glow(m, m.mainTexture, m.color * SwitchBoxLock.GlowStrength);
			r.sharedMaterials = mats;
		}
	}

	static Vector3 ThinAxis(Mesh mesh)
	{
		var s = mesh.bounds.size;
		return s.x <= s.y && s.x <= s.z ? Vector3.right : s.y <= s.z ? Vector3.up : Vector3.forward;
	}

	static void Build()
	{
		if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets", "SubValve");
		RemoveOld();
		var valveWorld = C.Find("Objects/MainItemSelection/MapItems/GasValve");
		var valveHand = C.Find("PlayerStuff/HandHoldObjects/GasValveHand");
		var valveOnBalloon = C.Find("EntireMansion/Property/ImportantInteractables/HotAirBalloon/GasValve_InPlace");
		var door = C.Find(Door);
		var wheelSlot = C.Find(Door + "/ColliderPlaceWheel");
		var inventory = C.All<Inventory>().FirstOrDefault();
		if (!valveWorld || !valveHand || !valveOnBalloon || !door || !wheelSlot || !inventory) { Debug.LogError("SUBVALVE something wasn't found"); return; }
		var valveDef = inventory.GetItemDefByName("Valve");

		// Dropped copy.
		string prefabPath = Folder + "/SubValve.prefab";
		if (!AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath)) AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(valveDef.dropPrefabObject), prefabPath);
		var root = PrefabUtility.LoadPrefabContents(prefabPath);
		root.name = "SubValve";
		TintGreen(root, false);
		var data = root.GetComponent<ItemSeedData>();
		if (data) { data.itemName = ItemName; data.category = 1; }
		foreach (var preset in root.GetComponents<ItemPresetSetup>()) Object.DestroyImmediate(preset);
		PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
		PrefabUtility.UnloadPrefabContents(root);
		var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

		// Lying in the map, and in the hand.
		var world = Object.Instantiate(valveWorld.gameObject, valveWorld.parent);
		world.name = "SubValve";
		world.SetActive(true);
		TintGreen(world, false);
		var worldData = world.GetComponent<ItemSeedData>();
		worldData.itemName = ItemName; worldData.category = 1; worldData.containedItems = new List<string>();
		world.AddComponent<SubValveItem>();
		var hand = Object.Instantiate(valveHand.gameObject, valveHand.parent);
		hand.name = "SubValveHand";
		hand.SetActive(false);
		TintGreen(hand, false);
		inventory.ItemDefs.Add(new ItemDefs
		{
			itemName = ItemName, handObject = hand, dropPoint = valveDef.dropPoint, textHighlighted = "Green valve knob",
			dropPrefabObject = prefab, pickAudioClip = valveDef.pickAudioClip,
		});
		EditorUtility.SetDirty(inventory);

		BuildOnHatch(door, wheelSlot, valveOnBalloon, hand);
		Physics.SyncTransforms();
		SetUpPresets(world, valveWorld);
		SetUpSeedSystem(world, valveWorld);
		Debug.Log("SUBVALVE built");
	}

	static void RemoveOld()
	{
		foreach (var item in C.All<SubValveItem>().ToList()) C.Kill(item);
		foreach (var l in C.All<SubValveLock>().ToList()) { if (l.Placed) C.Kill(l.Placed); C.Kill(l); }
		C.Kill(C.All<Transform>().FirstOrDefault(t => t.name == "SubValveHand" && t.parent && t.parent.name == "HandHoldObjects"));
		foreach (var inv in C.All<Inventory>()) { inv.ItemDefs.RemoveAll(d => d == null || d.itemName == ItemName); EditorUtility.SetDirty(inv); }
		foreach (var sm in C.All<SeedManager>())
		{
			sm.allItems?.RemoveAll(i => !i);
			sm.requiredEscapeItemNames?.RemoveAll(n => n == ItemName);
			if (sm.spawnAreas != null) sm.spawnAreas = sm.spawnAreas.Where(a => a != null && a.areaName != SeedSpotsName).ToArray();
			EditorUtility.SetDirty(sm);
		}
	}

	// The green valve on the hatch door, above the handwheel, standing out from the door, and the click spot on it.
	// Both are part of the door, so they swing with it when the hatch opens.
	static void BuildOnHatch(Transform door, Transform wheelSlot, Transform valveOnBalloon, GameObject hand)
	{
		var wheel = wheelSlot.Find("HandwheelSubmarinePlaced");
		var wm = wheel.GetComponent<MeshFilter>().sharedMesh;
		var wheelCentre = wheel.TransformPoint(wm.bounds.center);
		var outward = wheel.TransformDirection(ThinAxis(wm)).normalized;
		if (Vector3.Dot(outward, wheelCentre - door.GetComponent<Renderer>().bounds.center) < 0) outward = -outward;
		var up = Vector3.ProjectOnPlane(Vector3.up, outward).normalized;
		// The door's surface above the wheel, found with a ray against a temporary copy of the door's shape.
		var temp = new GameObject("TempDoorShape");
		temp.transform.SetPositionAndRotation(door.position, door.rotation);
		temp.transform.localScale = door.lossyScale;
		var tempCol = temp.AddComponent<MeshCollider>();
		tempCol.sharedMesh = door.GetComponent<MeshFilter>().sharedMesh;
		Physics.SyncTransforms();
		var guess = wheelCentre + up * .95f;
		var surface = tempCol.Raycast(new Ray(guess + outward * 2f, -outward), out RaycastHit hit, 4f) ? hit.point : guess;
		Object.DestroyImmediate(temp);

		var placed = Object.Instantiate(valveOnBalloon.gameObject, door);
		placed.name = "SubValvePlaced";
		foreach (var anim in placed.GetComponentsInChildren<Animation>(true)) Object.DestroyImmediate(anim);
		foreach (var col in placed.GetComponentsInChildren<Collider>(true)) Object.DestroyImmediate(col);
		var pm = placed.GetComponent<MeshFilter>().sharedMesh;
		var stemLocal = ThinAxis(pm);
		// Turn the knob so its stem points out of the door.
		var stemWorld = valveOnBalloon.TransformDirection(stemLocal).normalized;
		placed.transform.rotation = Quaternion.FromToRotation(stemWorld, outward) * valveOnBalloon.rotation;
		var w = Vector3.Scale(pm.bounds.size, valveOnBalloon.lossyScale);
		float stand = Vector3.Dot(new Vector3(Mathf.Abs(w.x), Mathf.Abs(w.y), Mathf.Abs(w.z)), new Vector3(Mathf.Abs(stemLocal.x), Mathf.Abs(stemLocal.y), Mathf.Abs(stemLocal.z))) * .5f;
		// Keep its real size under the door.
		var ls = valveOnBalloon.lossyScale; var ps = door.lossyScale;
		placed.transform.localScale = new Vector3(ls.x / ps.x, ls.y / ps.y, ls.z / ps.z);
		placed.transform.position = surface + outward * stand - (placed.transform.TransformPoint(pm.bounds.center) - placed.transform.position);
		placed.AddComponent<SubValveAxis>().LocalAxis = stemLocal;
		TintGreen(placed, true);
		var sound = placed.GetComponent<AudioSource>();
		if (!sound)
		{
			sound = placed.AddComponent<AudioSource>();
			sound.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AudioClip/MetalDrawer.ogg");
			sound.spatialBlend = 1f; sound.playOnAwake = false;
		}
		placed.SetActive(false);

		var spot = new GameObject("SubValvePlaceCollider");
		spot.transform.SetParent(door, false);
		spot.transform.SetPositionAndRotation(surface + outward * .12f, Quaternion.LookRotation(outward, up));
		spot.transform.localScale = new Vector3(1f / ps.x, 1f / ps.y, 1f / ps.z);
		var box = spot.AddComponent<BoxCollider>();
		box.size = new Vector3(.4f, .4f, .26f);
		spot.layer = wheelSlot.gameObject.layer;
		var puzzle = spot.AddComponent<RegPuzzle>();
		puzzle.requiredItem = hand;
		puzzle.performedEvent = new UnityEngine.Events.UnityEvent();
		puzzle.timedEvent = new UnityEngine.Events.UnityEvent();
		puzzle.performedEventNo = new UnityEngine.Events.UnityEvent();
		var lockScript = spot.AddComponent<SubValveLock>();
		lockScript.Hand = hand; lockScript.Placed = placed; lockScript.Puzzle = puzzle;
		lockScript.Pick = C.All<PickRay>().FirstOrDefault();
		lockScript.WheelPuzzle = wheelSlot.GetComponent<RegPuzzle>();
		lockScript.TurnSound = sound;
		UnityEventTools.AddPersistentListener(puzzle.performedEvent, lockScript.Use);
		ShowSubValveSpot.FixUp(lockScript); // knob fully in front of the door, and following the click spot
		Debug.Log($"SUBVALVE on the hatch at {surface} (hatch faces {outward}); valve stands {stand:0.000} m out; turn sound {(sound.clip ? sound.clip.name : "-")}");
	}

	// ---------- spots ----------

	static float Lift(Transform item)
	{
		var r = item.GetComponent<Renderer>();
		return r ? item.position.y - r.bounds.min.y : .03f;
	}

	static Transform MakeSpot(string name, Transform parent, Vector3 at, float yaw, Transform item)
	{
		var spot = new GameObject(name).transform;
		spot.SetParent(parent, false);
		var p = C.OnSurface(at) - Vector3.up * .05f + Vector3.up * (Lift(item) + .005f);
		spot.SetPositionAndRotation(p, Quaternion.Euler(0f, yaw, 0f) * Quaternion.Euler(0f, -item.eulerAngles.y, 0f) * item.rotation);
		return spot;
	}

	static bool Clashes(Vector3 p, IEnumerable<Vector3> others) =>
		others.Any(o => new Vector2(o.x - p.x, o.z - p.z).sqrMagnitude < 1f && Mathf.Abs(o.y - p.y) < 2f);

	static IEnumerable<Vector3> OtherItemSpots() =>
		C.All<Transform>().Where(t => t.name.StartsWith("- ChainCutterPreset") || t.name.StartsWith("ChainCutterSeedSpot ")
			|| t.name.StartsWith("- BlueFusePreset") || t.name.StartsWith("BlueFuseSeedSpot ")).Select(t => t.position);

	static void SetUpPresets(GameObject world, Transform item)
	{
		var presetRoot = C.Find(PresetRoot);
		var seedSpots = C.All<SeedManager>().SelectMany(s => s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null)
			.SelectMany(a => a.freeSpots).Where(t => t).Distinct().ToList();
		var setup = world.GetComponent<ItemPresetSetup>();
		var mains = new Transform[6];
		// Only items in the same preset are out at the same time (the other items' preset spots are in each preset's folder).
		var used = new List<Vector3>();
		for (int i = 0; i < 6; i++)
		{
			var preset = presetRoot.Cast<Transform>().FirstOrDefault(c => c.name == "Preset_" + (i + 1));
			if (!preset) continue;
			string name = $"- SubValvePreset{i + 1} / Main+Extras";
			var kept = preset.Cast<Transform>().FirstOrDefault(c => c.name == name);
			if (kept) { mains[i] = kept; used.Add(kept.position); continue; }
			var taken = preset.GetComponentsInChildren<Transform>(true).Where(t => t != preset).Select(t => t.position).Concat(used).ToList();
			var pick = C.Pick(seedSpots.Where(s => s.position.y < 24f && C.Reachable(s.position) && !Clashes(s.position, taken)).ToList(), taken, 1).FirstOrDefault();   // not up on the chimneys
			if (!pick) { Debug.LogWarning("SUBVALVE no spot for preset " + (i + 1)); continue; }
			mains[i] = MakeSpot(name, preset, pick.position, pick.eulerAngles.y, item);
			used.Add(mains[i].position);
			Debug.Log($"SUBVALVE preset {i + 1} spot {mains[i].position} (from {C.PathOf(pick)})");
		}
		// The valve is only used with Extras on, so the same spot serves both.
		setup.PositionMain_P1 = setup.PositionExtra_P1 = mains[0]; setup.PositionMain_P2 = setup.PositionExtra_P2 = mains[1];
		setup.PositionMain_P3 = setup.PositionExtra_P3 = mains[2]; setup.PositionMain_P4 = setup.PositionExtra_P4 = mains[3];
		setup.PositionMain_P5 = setup.PositionExtra_P5 = mains[4]; setup.PositionMain_P6 = setup.PositionExtra_P6 = mains[5];
		if (mains[0]) world.transform.SetPositionAndRotation(mains[0].position, mains[0].rotation);
		EditorUtility.SetDirty(setup);
	}

	// The submarine is only in the Extras version, so only the Extras seed system gets the valve.
	static void SetUpSeedSystem(GameObject world, Transform item)
	{
		var all = C.All<SeedManager>().ToList();
		var managers = all.Where(s => s.gameObject.name.EndsWith("Extras")).ToList();
		var seedTaken = all.SelectMany(s => (s.spawnAreas ?? new SpawnArea[0]).Where(a => a?.freeSpots != null).SelectMany(a => a.freeSpots)
			.Concat((s.puzzleDefs ?? new List<PuzzleDef>()).Select(p => p?.spawnPoint))).Where(t => t).Select(t => t.position)
			.Concat(OtherItemSpots()).ToList();
		var presetRoot = C.Find(PresetRoot);
		var holder = C.All<Transform>().FirstOrDefault(t => t.name == SeedSpotsName);
		Transform[] spots;
		if (holder && holder.childCount > 0) spots = holder.Cast<Transform>().ToArray();
		else
		{
			if (!holder) { holder = new GameObject(SeedSpotsName).transform; holder.SetParent(all[0].transform.parent, false); }
			var candidates = presetRoot.GetComponentsInChildren<Transform>(true)
				.Where(t => t.parent && t.parent.parent == presetRoot && !t.name.StartsWith("- ChainCutter") && !t.name.StartsWith("- BlueFuse") && !t.name.StartsWith("- SubValve"))
				.Where(t => C.Reachable(t.position) && !C.NearWall(t.position) && !Clashes(t.position, seedTaken)).ToList();
			spots = C.Pick(candidates, seedTaken, 8).Select((p, i) => MakeSpot("SubValveSeedSpot " + (i + 1), holder, p.position, p.eulerAngles.y, item)).ToArray();
		}
		foreach (var s in spots) Debug.Log($"SUBVALVE seed spot {s.position}");
		foreach (var sm in managers)
		{
			if (sm.allItems == null) sm.allItems = new List<GameObject>();
			if (!sm.allItems.Contains(world)) sm.allItems.Add(world);
			if (sm.requiredEscapeItemNames == null) sm.requiredEscapeItemNames = new List<string>();
			sm.requiredEscapeItemNames.Add(ItemName);
			sm.spawnAreas = (sm.spawnAreas ?? new SpawnArea[0]).Concat(new[] { new SpawnArea { areaName = SeedSpotsName, freeSpots = spots, maxItems = 1, allowedItemNames = new List<string> { ItemName } } }).ToArray();
			EditorUtility.SetDirty(sm);
			Debug.Log($"SUBVALVE added to {sm.gameObject.name} ({spots.Length} own spots)");
		}
	}

	// Picks the valve's preset spots again, then rebuilds and saves. -executeMethod SubValveSetup.RepickPresetsBatch
	public static void RepickPresetsBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		foreach (var t in C.All<Transform>().Where(t => t.name.StartsWith("- SubValvePreset")).ToList()) C.Kill(t);
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		foreach (var s in C.All<Transform>().Where(t => t.name.StartsWith("- SubValvePreset") || t.name.StartsWith("SubValveSeedSpot ")).OrderBy(t => t.name))
			Debug.Log($"VALVESPOT {s.name} at {s.position} on {(Physics.Raycast(s.position + Vector3.up * .2f, Vector3.down, out RaycastHit h, 1f) ? C.PathOf(h.collider.transform) : "nothing")}");
	}

	// Command line: pictures of the hatch (empty and with the valve on) and the valve at preset 1 (nothing is saved).
	public static void Preview()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var l = C.All<SubValveLock>().First();
		var c = l.transform.position;
		var n = l.transform.forward;
		var side = Vector3.Cross(n, Vector3.up).normalized;
		l.PlaceKnob();
		l.BuildStem();
		Shot(dir + "/subvalve_hatch_empty.png", c + n * 2.6f + side * .6f + Vector3.down * .3f, c + Vector3.down * .4f);
		Shot(dir + "/subvalve_hatch_empty_close.png", c + n * 1f + side * .5f + Vector3.up * .2f, c);
		Shot(dir + "/subvalve_hatch_empty_angle.png", c + n * .8f + side * .9f + Vector3.up * .1f, c);
		l.Placed.SetActive(true);
		Shot(dir + "/subvalve_hatch_valve.png", c + n * 2.6f + side * .6f + Vector3.down * .3f, c + Vector3.down * .4f);
		Shot(dir + "/subvalve_hatch_close.png", c + n * 1f + side * .5f + Vector3.up * .2f, c);
		var item = C.All<SubValveItem>().First().transform;
		Shot(dir + "/subvalve_item.png", item.position + new Vector3(.5f, .45f, .5f), item.position);
		Debug.Log("SUBVALVE pictures done");
	}

	static void Shot(string file, Vector3 from, Vector3 lookAt)
	{
		var go = new GameObject("PreviewCam");
		var cam = go.AddComponent<Camera>();
		cam.transform.position = from; cam.transform.LookAt(lookAt);
		cam.fieldOfView = 55f; cam.nearClipPlane = .02f;
		var light = new GameObject("PreviewLight").AddComponent<Light>();
		light.type = LightType.Directional; light.intensity = 1f; light.transform.rotation = Quaternion.Euler(40f, 30f, 0f);
		var rt = new RenderTexture(900, 600, 24);
		cam.targetTexture = rt; cam.Render();
		RenderTexture.active = rt;
		var tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, 900, 600), 0, 0);
		System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
		RenderTexture.active = null;
		Object.DestroyImmediate(go); Object.DestroyImmediate(light.gameObject); Object.DestroyImmediate(rt);
	}
}
