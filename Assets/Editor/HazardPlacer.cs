using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Tools > Evolved Restoration > Hazard Placer: creaking floors and lasers for Slientlo Never Clear, like Legacy's placer.
// Everything added here is used on Slientlo Never Clear only. Slientlo's creaking floors replace the game's own there;
// the set starts as a copy of the game's floors that Slientlo uses (the Impossible ones).
// Hazards live under "== Added Hazards ==" in the map: save the map (Ctrl+S) to keep them.
public class HazardPlacer : EditorWindow
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string RootName = "== Added Hazards ==";
	const string CreakGroupName = "Creak Floors - Slientlo Never Clear";
	const string LaserGroupName = "Lasers - Slientlo Never Clear";
	const string MaterialFolder = "Assets/Hazards";
	static readonly Color CreakColour = new Color(1f, .55f, .1f, .35f);

	static float cyanLength = 3f;
	static bool newRotating;

	[MenuItem("Tools/Evolved Restoration/Hazard Placer")]
	static void Open() => GetWindow<HazardPlacer>("Hazard Placer");

	void OnSelectionChange() => Repaint();

	// ---------- chain cutter spots ----------

	bool showCutterSpots = true;
	Vector2 cutterScroll;

	void OnEnable()
	{
		showCutterSpots = EditorPrefs.GetBool("HazardPlacer.ShowCutterSpots", true);
		SceneView.duringSceneGui += DrawCutterSpots;
	}

	void OnDisable() => SceneView.duringSceneGui -= DrawCutterSpots;

	static string SpotLabel(Transform spot, bool preset, int number) =>
		(preset ? "Preset " : "Seed ") + number + (spot.name.Contains(ChainCutterSetup.ExtrasOnlyTag) ? " (Extras only)" : "");

	// A see-through chain cutter at every spot, with its name, so you can see how it will lie.
	void DrawCutterSpots(SceneView view)
	{
		if (!showCutterSpots || Event.current.type != EventType.Repaint || !ChainCutterSetup.IsSetUp) return;
		var mesh = ChainCutterSetup.CutterMesh;
		var mat = ChainCutterSetup.CutterMaterial;
		if (!mesh || !mat) return;
		var presets = ChainCutterSetup.PresetSpots();
		var seeds = ChainCutterSetup.SeedSpots();
		var all = new List<(Transform spot, string label, Color colour)>();
		for (int i = 0; i < presets.Length; i++) if (presets[i]) all.Add((presets[i], SpotLabel(presets[i], true, i + 1), new Color(1f, .45f, .85f)));
		for (int i = 0; i < seeds.Count; i++) all.Add((seeds[i], SpotLabel(seeds[i], false, i + 1), new Color(.35f, .9f, 1f)));
		// The real cutter already stands at one spot (preset 1); every other spot gets a copy drawn by the Scene view
		// camera with normal lighting (drawing it straight from the GUI skipped the lighting and garbled it).
		var real = Object.FindObjectsByType<ChainCutterItem>(FindObjectsInactive.Include);
		foreach (var (spot, label, colour) in all)
		{
			bool hasReal = System.Array.Exists(real, r => (r.transform.position - spot.position).sqrMagnitude < .0001f);
			if (!hasReal)
				Graphics.DrawMesh(mesh, Matrix4x4.TRS(spot.position, spot.rotation, Vector3.one * ChainCutterSetup.CutterWorldScale), mat, 0, view.camera);
			Handles.color = colour;
			Handles.DrawWireDisc(spot.position, Vector3.up, .5f);
			Handles.Label(spot.position + Vector3.up * .25f, label, EditorStyles.whiteBoldLabel);
		}
		Handles.color = Color.white;
	}

	void CutterSpotsSection()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Chain cutter spots", EditorStyles.boldLabel);
		if (!ChainCutterSetup.IsSetUp) { EditorGUILayout.HelpBox("The chain cutter isn't set up yet: Tools > Evolved Restoration > Set Up Chain Cutter.", MessageType.Info); return; }
		EditorGUI.BeginChangeCheck();
		showCutterSpots = EditorGUILayout.ToggleLeft("Show them in the Scene view (pink = preset, blue = seed)", showCutterSpots);
		if (EditorGUI.EndChangeCheck()) { EditorPrefs.SetBool("HazardPlacer.ShowCutterSpots", showCutterSpots); SceneView.RepaintAll(); }

		cutterScroll = EditorGUILayout.BeginScrollView(cutterScroll, GUILayout.MaxHeight(260));
		var presets = ChainCutterSetup.PresetSpots();
		for (int i = 0; i < presets.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(presets[i] ? SpotLabel(presets[i], true, i + 1) : $"Preset {i + 1}: no spot (Save picks one)", GUILayout.MinWidth(120));
			GUI.enabled = presets[i];
			if (GUILayout.Button("Select", GUILayout.Width(60))) SelectSpot(presets[i]);
			if (GUILayout.Button("Drop", GUILayout.Width(50))) DropSpot(presets[i]);
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
		}
		var seeds = ChainCutterSetup.SeedSpots();
		for (int i = 0; i < seeds.Count; i++)
		{
			var spot = seeds[i];
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(SpotLabel(spot, false, i + 1), GUILayout.MinWidth(120));
			if (GUILayout.Button("Select", GUILayout.Width(60))) SelectSpot(spot);
			if (GUILayout.Button("Drop", GUILayout.Width(50))) DropSpot(spot);
			bool extras = spot.name.Contains(ChainCutterSetup.ExtrasOnlyTag);
			bool want = GUILayout.Toggle(extras, "Extras only", "Button", GUILayout.Width(80));
			if (want != extras)
			{
				Undo.RecordObject(spot.gameObject, "Extras only");
				spot.name = want ? spot.name + " " + ChainCutterSetup.ExtrasOnlyTag : spot.name.Replace(" " + ChainCutterSetup.ExtrasOnlyTag, "");
				EditorSceneManager.MarkSceneDirty(spot.gameObject.scene);
			}
			GUI.enabled = seeds.Count > 1;
			if (GUILayout.Button("Remove", GUILayout.Width(60))) { Undo.DestroyObjectImmediate(spot.gameObject); GUIUtility.ExitGUI(); }
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add seed spot", GUILayout.Height(28)))
		{
			var spot = ChainCutterSetup.AddSeedSpot(SceneView.lastActiveSceneView ? SceneView.lastActiveSceneView.pivot : Vector3.zero);
			if (spot) { Undo.RegisterCreatedObjectUndo(spot.gameObject, "Add seed spot"); SelectSpot(spot); }
		}
		if (GUILayout.Button("Save cutter spots", GUILayout.Height(28))) ChainCutterSetup.SaveSpots();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox(
			"Each preset puts the cutter at its own spot; in seed mode the seed picks one of the seed spots (it's a required item, so it's always placed). " +
			"Select a spot and move it with W, turn it with E, then Drop it onto the surface below. Mark seed spots in areas only open with Extras as \"Extras only\". " +
			"Press Save cutter spots when done (it also saves the map), then build the game.", MessageType.None);
	}

	static void SelectSpot(Transform spot)
	{
		Selection.activeTransform = spot;
		if (SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.Frame(new Bounds(spot.position, Vector3.one * 2f), false);
	}

	static void DropSpot(Transform spot)
	{
		Undo.RecordObject(spot, "Drop chain cutter spot");
		ChainCutterSetup.Drop(spot);
		EditorSceneManager.MarkSceneDirty(spot.gameObject.scene);
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		if (GUILayout.Button("Open map and load the hazards", GUILayout.Height(30))) OpenMap();
		if (EditorSceneManager.GetActiveScene().path != SceneFile) { EditorGUILayout.HelpBox("Open the map first.", MessageType.Warning); return; }
		if (Event.current.type == EventType.Layout) UpdateHousings();
		EditorGUILayout.HelpBox("Everything here is used on Slientlo Never Clear only.", MessageType.None);

		// Creaking floors
		EditorGUILayout.Space();
		Transform creaks = CreakGroup(false);
		EditorGUILayout.LabelField($"Creaking floors on Slientlo Never Clear: {(creaks ? creaks.childCount : 0)}", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add creaking floor", GUILayout.Height(30))) AddCreak();
		if (GUILayout.Button("Reset to the game's", GUILayout.Height(30))) ResetCreaks();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox("The orange boxes: stepping into one creaks and alerts Granny. On Slientlo these replace the game's own creaking floors. W moves, E turns, R sizes.", MessageType.None);

		// Lasers
		EditorGUILayout.Space();
		Transform lasers = LaserGroup(false);
		EditorGUILayout.LabelField($"Lasers on Slientlo Never Clear: {(lasers ? lasers.childCount : 0)}", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		foreach (LaserHazard.LaserType type in System.Enum.GetValues(typeof(LaserHazard.LaserType)))
		{
			var old = GUI.backgroundColor;
			GUI.backgroundColor = LaserHazard.ColourOf(type);
			if (GUILayout.Button(type.ToString(), GUILayout.Height(40))) AddLaser(type);
			GUI.backgroundColor = old;
		}
		EditorGUILayout.EndHorizontal();
		cyanLength = EditorGUILayout.FloatField("New cyan laser length (m)", cyanLength);
		newRotating = EditorGUILayout.ToggleLeft("New lasers rotate (60° side to side; change it once placed)", newRotating);
		EditorGUILayout.HelpBox(
			"Red: on until it hits a wall.  Pink: grows out to the wall, stays out, shrinks back, waits (1 second each unless you change it).\nPurple: flickers on and off (1 second each unless you change it).  Cyan: only as long as its length, still stops at walls.\n" +
			"A laser fires along its blue arrow and plays its animation in the Scene view. W moves, E turns. Ctrl+D copies an item, Delete removes it.", MessageType.None);
		SelectedLasers();

		CutterSpotsSection();

		// Show / hide
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Show on the map", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		ShowToggle(creaks, "Creaking floors");
		ShowToggle(lasers, "Lasers");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		if (GUILayout.Button("Save the map", GUILayout.Height(30))) EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		EditorGUILayout.HelpBox("Save the map to keep your changes, then build the game (Tools > Evolved Restoration > Build Windows Game).", MessageType.None);
	}

	static void ShowToggle(Transform group, string label)
	{
		if (!group) return;
		bool on = group.gameObject.activeSelf;
		bool want = GUILayout.Toggle(on, $"{label} ({group.childCount})");
		if (want == on) return;
		Undo.RecordObject(group.gameObject, "Show " + label);
		group.gameObject.SetActive(want);
	}

	static void OpenMap()
	{
		if (EditorSceneManager.GetActiveScene().path != SceneFile)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
			EditorSceneManager.OpenScene(SceneFile);
		}
		// First time: Slientlo's floors start as the game's.
		if (!CreakGroup(false)) { CreakGroup(true); SeedCreaks(); }
	}

	// ---- groups ----

	static Transform Root(bool create)
	{
		var root = GameObject.Find(RootName);
		if (!root)
			foreach (var t in Resources.FindObjectsOfTypeAll<Transform>())   // also finds it while hidden
				if (t.name == RootName && t.parent == null && t.gameObject.scene.IsValid()) { root = t.gameObject; break; }
		if (!root && create) { root = new GameObject(RootName); Undo.RegisterCreatedObjectUndo(root, "Add hazards"); }
		return root ? root.transform : null;
	}

	static Transform Group(string name, bool create)
	{
		Transform root = Root(create);
		if (!root) return null;
		Transform group = root.Find(name);
		if (!group && create)
		{
			group = new GameObject(name).transform;
			group.SetParent(root, false);
			Undo.RegisterCreatedObjectUndo(group.gameObject, "Add " + name);
		}
		return group;
	}

	static Transform CreakGroup(bool create)
	{
		Transform group = Group(CreakGroupName, create);
		if (group && !group.GetComponent<SlientloCreakSet>()) group.gameObject.AddComponent<SlientloCreakSet>();
		return group;
	}

	static Transform LaserGroup(bool create) => Group(LaserGroupName, create);

	static Vector3 SpawnPoint()
	{
		var view = SceneView.lastActiveSceneView;
		return view ? view.pivot : Vector3.zero;
	}

	static void Finish(GameObject g, string undo)
	{
		Undo.RegisterCreatedObjectUndo(g, undo);
		Selection.activeGameObject = g;
		if (SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.FrameSelected();
		EditorSceneManager.MarkSceneDirty(g.scene);
	}

	// ---- creaking floors ----

	// The game's own creaking floors (not ones added here).
	static List<TriggerCreakNoise> GameCreaks()
	{
		var list = new List<TriggerCreakNoise>();
		foreach (var c in Resources.FindObjectsOfTypeAll<TriggerCreakNoise>())
			if (c.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(c) && !c.GetComponent<HazardDifficulty>()) list.Add(c);
		return list;
	}

	// The game's floors Slientlo would use: it plays as Impossible, where the Normal+ floors are switched off.
	static List<TriggerCreakNoise> GameCreaksOnSlientlo()
	{
		var days = Resources.FindObjectsOfTypeAll<Days>();
		GameObject normalOnly = null;
		foreach (var d in days) if (d.gameObject.scene.IsValid() && d.NormalFeature) { normalOnly = d.NormalFeature; break; }
		var list = new List<TriggerCreakNoise>();
		foreach (var c in GameCreaks())
			if (!normalOnly || !c.transform.IsChildOf(normalOnly.transform)) list.Add(c);
		return list;
	}

	// A see-through orange box that creaks: the box is the trigger, at its size. Sound and noise come from the game's floor.
	static GameObject CreateCreak(TriggerCreakNoise source, Vector3 position, Quaternion rotation, Vector3 size)
	{
		GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);   // comes with a 1x1x1 BoxCollider
		floor.name = "Creak";
		floor.transform.SetParent(CreakGroup(true), false);
		floor.transform.SetPositionAndRotation(position, rotation);
		Vector3 under = floor.transform.parent.lossyScale;
		floor.transform.localScale = new Vector3(size.x / under.x, size.y / under.y, size.z / under.z);
		floor.GetComponent<BoxCollider>().isTrigger = true;
		var look = floor.GetComponent<MeshRenderer>();
		look.sharedMaterial = Mat("CreakBox", CreakColour);
		look.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		look.receiveShadows = false;
		if (source)
		{
			floor.tag = source.tag;
			floor.layer = source.gameObject.layer;
			var sourceAudio = source.GetComponent<AudioSource>();
			if (sourceAudio && UnityEditorInternal.ComponentUtility.CopyComponent(sourceAudio)) UnityEditorInternal.ComponentUtility.PasteComponentAsNew(floor);
			var creak = floor.AddComponent<TriggerCreakNoise>();
			creak.NoiseObj = source.NoiseObj;
			creak.Number = source.Number;
			creak.CanCreak = true;
		}
		else floor.AddComponent<TriggerCreakNoise>().CanCreak = true;
		floor.AddComponent<HazardDifficulty>();
		return floor;
	}

	// World-space box of a game floor's trigger.
	static void BoxOf(TriggerCreakNoise c, out Vector3 centre, out Quaternion rotation, out Vector3 size)
	{
		rotation = c.transform.rotation;
		if (c.TryGetComponent(out BoxCollider box))
		{
			centre = c.transform.TransformPoint(box.center);
			Vector3 s = c.transform.lossyScale;
			size = new Vector3(Mathf.Abs(box.size.x * s.x), Mathf.Abs(box.size.y * s.y), Mathf.Abs(box.size.z * s.z));
		}
		else if (c.TryGetComponent(out Collider col)) { centre = col.bounds.center; size = col.bounds.size; rotation = Quaternion.identity; }
		else { centre = c.transform.position; size = Vector3.one; }
	}

	static TriggerCreakNoise SoundSource()
	{
		var list = GameCreaks();
		return list.Count > 0 ? list[0] : null;
	}

	static int SeedCreaks()
	{
		int count = 0;
		foreach (var c in GameCreaksOnSlientlo())
		{
			BoxOf(c, out Vector3 centre, out Quaternion rotation, out Vector3 size);
			Undo.RegisterCreatedObjectUndo(CreateCreak(c, centre, rotation, size), "Reset creaking floors");
			count++;
		}
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		return count;
	}

	static void AddCreak()
	{
		GameObject floor = CreateCreak(SoundSource(), SpawnPoint(), Quaternion.identity, new Vector3(3f, .7f, 3f));
		CreakGroup(true).gameObject.SetActive(true);
		Finish(floor, "Add creaking floor");
	}

	static void ResetCreaks()
	{
		Transform group = CreakGroup(true);
		for (int c = group.childCount - 1; c >= 0; c--) Undo.DestroyObjectImmediate(group.GetChild(c).gameObject);
		int count = SeedCreaks();
		group.gameObject.SetActive(true);
		Debug.Log($"Hazard Placer: Slientlo set back to the game's {count} creaking floors.");
	}

	// ---- lasers ----

	static void AddLaser(LaserHazard.LaserType type)
	{
		var g = new GameObject(type + " Laser");
		g.transform.SetParent(LaserGroup(true), false);
		LaserGroup(true).gameObject.SetActive(true);
		g.transform.SetPositionAndRotation(SpawnPoint(), Quaternion.identity);
		g.AddComponent<LineRenderer>();   // the laser sets up its own glowing look
		var laser = g.AddComponent<LaserHazard>();
		laser.Type = type;
		laser.Length = Mathf.Max(0f, cyanLength);
		laser.Rotating = newRotating;
		laser.ApplyColour();
		g.AddComponent<HazardDifficulty>();

		// The dark housing the beam comes out of (no collider, so the beam doesn't hit it).
		var emitter = GameObject.CreatePrimitive(PrimitiveType.Cube);
		emitter.name = "Emitter";
		DestroyImmediate(emitter.GetComponent<Collider>());
		emitter.transform.SetParent(g.transform, false);
		var r = emitter.GetComponent<Renderer>();
		SizeHousing(emitter.transform, type);
		r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		Finish(g, "Add " + type + " laser");
	}

	// Change the type, length or timing of the selected lasers.
	static void SelectedLasers()
	{
		var lasers = new List<LaserHazard>();
		foreach (GameObject g in Selection.gameObjects)
		{
			var l = g.GetComponentInParent<LaserHazard>();
			if (l && !lasers.Contains(l)) lasers.Add(l);
		}
		if (lasers.Count == 0) { EditorGUILayout.HelpBox("Select lasers to change their colour, length, timing, thickness or make them rotate.", MessageType.None); return; }
		EditorGUILayout.LabelField($"Selected lasers: {lasers.Count}", EditorStyles.boldLabel);
		EditorGUI.BeginChangeCheck();
		var type = (LaserHazard.LaserType)EditorGUILayout.EnumPopup("Colour", lasers[0].Type);
		float length = EditorGUILayout.FloatField("Length (cyan)", lasers[0].Length);
		float offset = EditorGUILayout.FloatField("Cycle offset (pink/purple)", lasers[0].CycleOffset);
		float thickness = EditorGUILayout.Slider("Thickness", lasers[0].Thickness, .02f, .4f);
		float speed = EditorGUILayout.FloatField("Grow speed m/s (pink)", lasers[0].Speed);
		float outSeconds = EditorGUILayout.FloatField("Seconds out / on (pink, purple)", lasers[0].OutSeconds);
		float inSeconds = EditorGUILayout.FloatField("Seconds in / off (pink, purple)", lasers[0].InSeconds);
		EditorGUILayout.Space(4);
		bool rotating = EditorGUILayout.ToggleLeft("Rotating (sweeps back and forth around where it points)", lasers[0].Rotating);
		float hSweep = lasers[0].HorizontalSweep, vSweep = lasers[0].VerticalSweep, swing = lasers[0].SwingSeconds, hold = lasers[0].HoldSeconds, swingOffset = lasers[0].SwingOffset;
		if (rotating)
		{
			hSweep = EditorGUILayout.Slider("  Horizontal sweep (°)", hSweep, 0f, 360f);
			vSweep = EditorGUILayout.Slider("  Vertical sweep (°)", vSweep, 0f, 180f);
			swing = EditorGUILayout.FloatField("  Seconds to swing across", swing);
			hold = EditorGUILayout.FloatField("  Seconds held at each side", hold);
			swingOffset = EditorGUILayout.FloatField("  Swing offset (s)", swingOffset);
		}
		if (!EditorGUI.EndChangeCheck()) return;
		foreach (var l in lasers)
		{
			Undo.RecordObject(l, "Laser settings");
			Undo.RecordObject(l.gameObject, "Laser settings");
			l.Type = type; l.Length = Mathf.Max(0f, length); l.CycleOffset = offset; l.Thickness = thickness;
			l.Speed = Mathf.Max(.1f, speed); l.OutSeconds = Mathf.Max(0f, outSeconds); l.InSeconds = Mathf.Max(0f, inSeconds);
			l.Rotating = rotating; l.HorizontalSweep = hSweep; l.VerticalSweep = vSweep;
			l.SwingSeconds = Mathf.Max(.05f, swing); l.HoldSeconds = Mathf.Max(0f, hold); l.SwingOffset = swingOffset;
			l.gameObject.name = type + " Laser";
			l.ApplyColour();
			var emitter = l.transform.Find("Emitter");
			if (emitter) SizeHousing(emitter, type);
			EditorUtility.SetDirty(l);
		}
	}

	// The housing the beam comes out of: a dark metal box whose front face is where the beam starts.
	static readonly Vector3 HousingSize = new Vector3(.3f, .3f, .36f);

	static void SizeHousing(Transform emitter, LaserHazard.LaserType type)
	{
		emitter.localPosition = new Vector3(0f, 0f, -HousingSize.z / 2f);
		emitter.localScale = HousingSize;
		emitter.GetComponent<Renderer>().sharedMaterial = Housing(type);
	}

	// Lasers placed earlier get the current housing size and look.
	static void UpdateHousings()
	{
		Transform lasers = LaserGroup(false);
		if (!lasers) return;
		foreach (var l in lasers.GetComponentsInChildren<LaserHazard>(true))
		{
			Transform emitter = l.transform.Find("Emitter");
			if (!emitter || (emitter.localScale == HousingSize && emitter.GetComponent<Renderer>().sharedMaterial == Housing(l.Type))) continue;
			Undo.RecordObject(emitter, "Resize laser housing");
			Undo.RecordObject(emitter.GetComponent<Renderer>(), "Resize laser housing");
			SizeHousing(emitter, l.Type);
			EditorSceneManager.MarkSceneDirty(emitter.gameObject.scene);
		}
	}

	// The laser housing: metal in the laser's own colour, glowing a little so it shows in the dark fog.
	const float HousingGlow = 1f;   // how brightly the housing glows (0 = not at all)

	static Material Housing(LaserHazard.LaserType type)
	{
		if (!AssetDatabase.IsValidFolder(MaterialFolder)) AssetDatabase.CreateFolder("Assets", "Hazards");
		string path = MaterialFolder + "/LaserHousing_" + type + ".mat";
		Color colour = LaserHazard.ColourOf(type);
		var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
		bool isNew = !mat;
		if (isNew) mat = new Material(Shader.Find("Standard"));
		// Bright: full-strength glow in the laser's colour. Kept up to date on existing materials too.
		Color glow = colour * HousingGlow;
		bool changed = isNew || mat.color != colour || mat.GetColor("_EmissionColor") != glow || !mat.IsKeywordEnabled("_EMISSION");
		if (!changed) return mat;
		mat.color = colour;
		mat.SetFloat("_Metallic", .15f);
		mat.SetFloat("_Glossiness", .5f);
		mat.EnableKeyword("_EMISSION");
		mat.SetColor("_EmissionColor", glow);
		mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
		if (isNew) AssetDatabase.CreateAsset(mat, path); else EditorUtility.SetDirty(mat);
		AssetDatabase.SaveAssets();
		return mat;
	}

	// Unlit materials saved as assets, so they're included in the build and don't fade in the dark fog.
	static Material Mat(string name, Color colour)
	{
		if (!AssetDatabase.IsValidFolder(MaterialFolder)) AssetDatabase.CreateFolder("Assets", "Hazards");
		string path = MaterialFolder + "/" + name + ".mat";
		var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (mat) return mat;
		mat = new Material(Shader.Find("Sprites/Default")) { color = colour };
		AssetDatabase.CreateAsset(mat, path);
		AssetDatabase.SaveAssets();
		return mat;
	}

	// Command line (project closed in Unity): -executeMethod HazardPlacer.UpdateMap gives placed lasers the current housing.
	public static void UpdateMap()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Transform lasers = LaserGroup(false);
		UpdateHousings();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log($"HAZARDMAP updated {(lasers ? lasers.childCount : 0)} lasers' housings");
		if (lasers)
			foreach (var l in lasers.GetComponentsInChildren<LaserHazard>(true))
				Debug.Log($"HAZARDMAP {l.name}: housing={l.transform.Find("Emitter")?.GetComponent<Renderer>().sharedMaterial?.name} size={l.transform.Find("Emitter")?.localScale}");
	}

	// Command-line check (with the project closed in Unity): -executeMethod HazardPlacer.SelfTest
	public static void SelfTest()
	{
		EditorSceneManager.OpenScene(SceneFile);
		Debug.Log($"HAZARDTEST game creaks={GameCreaks().Count} on Slientlo={GameCreaksOnSlientlo().Count}");
		OpenMap();
		AddCreak();
		foreach (LaserHazard.LaserType t in System.Enum.GetValues(typeof(LaserHazard.LaserType))) AddLaser(t);
		foreach (var c in CreakGroup(false).GetComponentsInChildren<TriggerCreakNoise>(true))
			Debug.Log($"HAZARDTEST creak at {c.transform.position} size={c.transform.lossyScale} audio={(c.GetComponent<AudioSource>() ? c.GetComponent<AudioSource>().clip : null)} noise={c.NoiseObj} trigger={c.GetComponent<Collider>().isTrigger} tag={c.tag}");
		foreach (var l in LaserGroup(false).GetComponentsInChildren<LaserHazard>(true))
			Debug.Log($"HAZARDTEST laser {l.Type} reach={l.Reach}");
		DestroyImmediate(Root(false).gameObject);   // leave the map as it was
		Debug.Log("HAZARDTEST done");
	}
}
