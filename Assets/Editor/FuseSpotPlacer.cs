using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using C = ChainCutterSetup;

// Tools > Evolved Restoration > Item Spot Placer: where the yellow gate fuse (the game's own), the blue gate fuse (Slientlo)
// and the submarine valve knob (Slientlo with Extras) can be. Each has a spot per item preset (the yellow fuse also has
// Extras spots in some); the blue fuse and the valve also have their own seed spots. The game puts the item exactly on its
// spot, so moving a spot moves the item. Each spot shows a copy of its item in the Scene view.
// The yellow fuse's seed mode uses the game's shared item spots, so only its presets are listed.
public class FuseSpotPlacer : EditorWindow
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string ExtrasOnly = "(Extras only)";

	class Item
	{
		public string Name, Note, PresetPrefix, SeedHolder, SeedPrefix;
		public Color Colour;
		public bool ExtrasToggle;
		public System.Action Apply;

		public ItemPresetSetup Find() => PresetPrefix == null
			? C.All<ItemPresetSetup>().FirstOrDefault(s => s.name.StartsWith("GateFuse"))
			: C.All<ItemPresetSetup>().FirstOrDefault(s => s.PositionMain_P1 && s.PositionMain_P1.name.StartsWith(PresetPrefix));

		public List<Transform> Seeds()
		{
			if (SeedHolder == null) return new List<Transform>();
			var holder = C.All<Transform>().FirstOrDefault(t => t.name == SeedHolder);
			return holder ? holder.Cast<Transform>().ToList() : new List<Transform>();
		}
	}

	static readonly Item[] Items =
	{
		new Item { Name = "Yellow gate fuse", Note = "every difficulty", Colour = new Color(1f, .85f, .2f) },
		new Item { Name = "Blue gate fuse", Note = "Slientlo", Colour = new Color(.4f, .62f, 1f), PresetPrefix = "- BlueFusePreset",
			SeedHolder = "BlueFuseSeedSpots", SeedPrefix = "BlueFuseSeedSpot ", ExtrasToggle = true, Apply = BlueFuseSetup.Run },
		new Item { Name = "Submarine valve knob", Note = "Slientlo with Extras", Colour = new Color(.35f, 1f, .4f), PresetPrefix = "- SubValvePreset",
			SeedHolder = "SubValveSeedSpots", SeedPrefix = "SubValveSeedSpot ", Apply = SubValveSetup.Run },
	};

	bool show = true;
	Vector2 scroll;

	[MenuItem("Tools/Evolved Restoration/Item Spot Placer")]
	static void Open() => GetWindow<FuseSpotPlacer>("Item Spots");

	void OnEnable() { show = EditorPrefs.GetBool("FuseSpotPlacer.Show", true); SceneView.duringSceneGui += Draw; }
	void OnDisable() => SceneView.duringSceneGui -= Draw;
	void OnSelectionChange() => Repaint();

	static Transform[] Mains(ItemPresetSetup s) => new[] { s.PositionMain_P1, s.PositionMain_P2, s.PositionMain_P3, s.PositionMain_P4, s.PositionMain_P5, s.PositionMain_P6 };
	static Transform[] Extras(ItemPresetSetup s) => new[] { s.PositionExtra_P1, s.PositionExtra_P2, s.PositionExtra_P3, s.PositionExtra_P4, s.PositionExtra_P5, s.PositionExtra_P6 };

	// Every spot with its label and colour.
	static List<(Transform spot, string label, Color colour, ItemPresetSetup item)> AllSpots()
	{
		var list = new List<(Transform, string, Color, ItemPresetSetup)>();
		foreach (var it in Items)
		{
			var item = it.Find();
			if (!item) continue;
			var m = Mains(item); var e = Extras(item);
			for (int i = 0; i < 6; i++)
			{
				if (m[i]) list.Add((m[i], $"{it.Name} preset {i + 1}", it.Colour, item));
				if (e[i] && e[i] != m[i]) list.Add((e[i], $"{it.Name} preset {i + 1} (Extras)", it.Colour, item));
			}
			var seeds = it.Seeds();
			for (int i = 0; i < seeds.Count; i++)
				list.Add((seeds[i], $"{it.Name} seed {i + 1}" + (seeds[i].name.Contains(ExtrasOnly) ? " (Extras only)" : ""), Color.Lerp(it.Colour, Color.white, .45f), item));
		}
		return list;
	}

	// A copy of the item at every spot it isn't standing on, drawn by the Scene view camera so it's lit normally.
	void Draw(SceneView view)
	{
		if (!show || Event.current.type != EventType.Repaint || EditorSceneManager.GetActiveScene().path != SceneFile) return;
		foreach (var (spot, label, colour, item) in AllSpots())
		{
			if ((item.transform.position - spot.position).sqrMagnitude > .0001f)
				foreach (var r in item.GetComponentsInChildren<MeshRenderer>(true))
				{
					var f = r.GetComponent<MeshFilter>();
					if (!f || !f.sharedMesh || r.sharedMaterials.Length == 0) continue;
					var m = AtSpot(spot, item) * item.transform.worldToLocalMatrix * r.transform.localToWorldMatrix;
					for (int k = 0; k < f.sharedMesh.subMeshCount; k++)
						Graphics.DrawMesh(f.sharedMesh, m, r.sharedMaterials[Mathf.Min(k, r.sharedMaterials.Length - 1)], 0, view.camera, k);
				}
			Handles.color = colour;
			Handles.DrawWireDisc(spot.position, Vector3.up, Selection.Contains(spot.gameObject) ? .45f : .3f);
			Handles.Label(spot.position + Vector3.up * .25f, label, EditorStyles.whiteBoldLabel);
		}
		Handles.color = Color.white;
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		if (EditorSceneManager.GetActiveScene().path != SceneFile)
		{
			if (GUILayout.Button("Open the map", GUILayout.Height(28)) && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorSceneManager.OpenScene(SceneFile);
			return;
		}
		bool was = show;
		show = EditorGUILayout.ToggleLeft("Show the spots in the Scene view", show);
		if (show != was) { EditorPrefs.SetBool("FuseSpotPlacer.Show", show); SceneView.RepaintAll(); }
		EditorGUILayout.HelpBox("Select a spot and move or turn it in the Scene view; the item goes exactly there. Drop puts it on whatever is below. " +
			"Save the map (Ctrl+S) when done, then build the game.", MessageType.None);
		scroll = EditorGUILayout.BeginScrollView(scroll);
		foreach (var it in Items) ItemSection(it);
		EditorGUILayout.EndScrollView();
	}

	void ItemSection(Item it)
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"{it.Name} ({it.Note})", EditorStyles.boldLabel);
		var item = it.Find();
		if (!item) { EditorGUILayout.HelpBox(it.Name + " isn't set up yet (see Tools > Evolved Restoration).", MessageType.Info); return; }
		PresetRows(item);
		if (it.SeedHolder == null)
		{
			EditorGUILayout.HelpBox("In seed mode it uses the game's shared item spots, like the other original items.", MessageType.None);
			return;
		}
		EditorGUILayout.LabelField("Seed spots (the seed picks one)", EditorStyles.miniBoldLabel);
		var seeds = it.Seeds();
		for (int i = 0; i < seeds.Count; i++)
		{
			var s = seeds[i];
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Seed " + (i + 1), GUILayout.Width(70));
			if (it.ExtrasToggle)
			{
				bool extras = s.name.Contains(ExtrasOnly);
				bool want = GUILayout.Toggle(extras, "Extras only", GUILayout.Width(85));
				if (want != extras) { Undo.RecordObject(s.gameObject, "Extras only"); s.name = want ? s.name + " " + ExtrasOnly : s.name.Replace(" " + ExtrasOnly, ""); MarkDirty(); }
			}
			SpotButtons(s, item);
			if (GUILayout.Button("Remove", GUILayout.Width(62)) && seeds.Count > 1) { Undo.DestroyObjectImmediate(s.gameObject); MarkDirty(); GUIUtility.ExitGUI(); }
			EditorGUILayout.EndHorizontal();
		}
		if (GUILayout.Button("Add a seed spot where the Scene view is looking"))
		{
			var holder = C.All<Transform>().First(t => t.name == it.SeedHolder);
			int n = 1;
			while (holder.Cast<Transform>().Any(t => t.name == it.SeedPrefix + n || t.name.StartsWith(it.SeedPrefix + n + " "))) n++;
			var spot = new GameObject(it.SeedPrefix + n).transform;
			Undo.RegisterCreatedObjectUndo(spot.gameObject, "Add seed spot");
			spot.SetParent(holder, false);
			var template = seeds.FirstOrDefault();
			spot.SetPositionAndRotation(SceneView.lastActiveSceneView ? SceneView.lastActiveSceneView.pivot : Vector3.zero, template ? template.rotation : item.transform.rotation);
			Drop(spot, item);
			Selection.activeTransform = spot;
			MarkDirty();
		}
		EditorGUILayout.HelpBox("After adding or removing seed spots" + (it.ExtrasToggle ? " or changing Extras only" : "") +
			", press Apply so the seed system uses them. Just moving spots needs nothing more than saving the map.", MessageType.None);
		if (GUILayout.Button($"Apply {it.Name.ToLower()} seed spots (rebuilds it, keeps every spot)", GUILayout.Height(26)))
		{
			it.Apply();
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}
	}

	void PresetRows(ItemPresetSetup item)
	{
		var m = Mains(item); var e = Extras(item);
		for (int i = 0; i < 6; i++)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Preset " + (i + 1), GUILayout.Width(70));
			if (m[i]) SpotButtons(m[i], item); else GUILayout.Label("no spot");
			EditorGUILayout.EndHorizontal();
			if (e[i] && e[i] != m[i])
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("   Extras", GUILayout.Width(70));
				SpotButtons(e[i], item);
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	void SpotButtons(Transform spot, ItemPresetSetup item)
	{
		GUI.color = Selection.Contains(spot.gameObject) ? Color.yellow : Color.white;
		if (GUILayout.Button("Select", GUILayout.Width(55)))
		{
			Selection.activeTransform = spot;
			if (SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.Frame(new Bounds(spot.position, Vector3.one * 2f), false);
		}
		GUI.color = Color.white;
		if (GUILayout.Button("Drop", GUILayout.Width(45))) { Undo.RecordObject(spot, "Drop item spot"); Drop(spot, item); MarkDirty(); }
	}

	// Puts the spot on whatever is below so the item rests on it (keeping its turn).
	static void Drop(Transform spot, ItemPresetSetup item)
	{
		Physics.SyncTransforms();
		float lift = .03f;
		// The item's lowest point below its pivot when turned like the spot.
		var toSpot = AtSpot(spot, item) * item.transform.worldToLocalMatrix;
		float min = float.MaxValue;
		foreach (var r in item.GetComponentsInChildren<MeshRenderer>(true))
		{
			var f = r.GetComponent<MeshFilter>();
			if (!f || !f.sharedMesh) continue;
			var m = toSpot * r.transform.localToWorldMatrix;
			foreach (var v in f.sharedMesh.vertices) min = Mathf.Min(min, m.MultiplyPoint3x4(v).y);
		}
		if (min != float.MaxValue) lift = spot.position.y - min;
		var below = Physics.RaycastAll(spot.position + Vector3.up * .4f, Vector3.down, 3f, ~0, QueryTriggerInteraction.Ignore)
			.Where(h => !h.collider.transform.IsChildOf(item.transform) && !h.collider.GetComponent<ItemSeedData>()).OrderBy(h => h.distance).ToArray();
		if (below.Length == 0) { Debug.LogWarning($"{spot.name}: nothing below it to drop onto."); return; }
		spot.position = below[0].point + Vector3.up * (lift + .005f);
	}

	// Command line: lists every spot. -executeMethod FuseSpotPlacer.Describe
	public static void Describe()
	{
		EditorSceneManager.OpenScene(SceneFile);
		foreach (var f in Items.Select(i => i.Find()))
			if (f) Debug.Log($"FUSESPOTS {f.name} at {f.transform.position} worldLocal={f.WorldLocal}");
		foreach (var f in Items.Select(i => i.Find()))
			if (f) foreach (var r in f.GetComponentsInChildren<Renderer>(true))
				Debug.Log($"FUSESPOTS renderer {f.name}/{r.name} {r.GetType().Name} enabled={r.enabled} active={r.gameObject.activeSelf} mesh={(r.GetComponent<MeshFilter>()?.sharedMesh ? r.GetComponent<MeshFilter>().sharedMesh.name : "-")} mats={string.Join(",", r.sharedMaterials.Select(m => m ? m.name + "/" + m.shader.name : "null"))}");
	}

	// The game copies only a spot's position and turn onto the item (the item keeps its own size, whatever scale the spot gets from its parents).
	static Matrix4x4 AtSpot(Transform spot, ItemPresetSetup item) => Matrix4x4.TRS(spot.position, spot.rotation, item.transform.lossyScale);

	static void MarkDirty() { EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); SceneView.RepaintAll(); }
}
