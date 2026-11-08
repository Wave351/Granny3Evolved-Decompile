using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using C = ChainCutterSetup;

// Tools > Evolved Restoration > Set Up Escape Levers (Slientlo Never Clear): two of the train switch box's levers go out
// into the map at placeable spots, every escape needs both pulled, two lights above the front door show which are on, and
// the empty places in the box say the lever is elsewhere. Running it again keeps the spots and the lights where they are.
public static class EscapeLeversSetup
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string Folder = "Assets/EscapeLevers";
	const string Switches = "EntireMansion/Property/ImportantInteractables/ElectricSwitches";
	const string RootName = "== Added Hazards ==";
	public const string LightsName = "Escape Lever Lights";
	public const string SpotsName = "Escape Lever Spots";
	public const string Message = "You have to find the lever somewhere in the map";

	[MenuItem("Tools/Evolved Restoration/Set Up Escape Levers")]
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
		Debug.Log("LEVERS done and saved");
	}

	static Material LightMat(string name, Color colour)
	{
		string path = $"{Folder}/{name}.mat";
		var m = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!m) { m = new Material(Shader.Find("Standard")); AssetDatabase.CreateAsset(m, path); }
		m.color = colour;
		m.EnableKeyword("_EMISSION");
		m.SetColor("_EmissionColor", colour * 2.2f);
		m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
		EditorUtility.SetDirty(m);
		return m;
	}

	// Dark worn metal for the wall levers' base and stick.
	static Material MetalMat()
	{
		string path = $"{Folder}/LeverMetal.mat";
		var m = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!m) { m = new Material(Shader.Find("Standard")); AssetDatabase.CreateAsset(m, path); }
		m.color = new Color(.5f, .5f, .52f);
		m.SetFloat("_Metallic", .35f);
		m.SetFloat("_Glossiness", .45f);
		m.EnableKeyword("_EMISSION");
		m.SetColor("_EmissionColor", new Color(.05f, .05f, .055f));
		EditorUtility.SetDirty(m);
		return m;
	}

	static Transform Root()
	{
		var root = C.All<Transform>().FirstOrDefault(t => t.name == RootName && t.parent == null);
		return root ? root : new GameObject(RootName).transform;
	}

	public static Transform Lever(int i) => C.Find($"{Switches}/ElectricSwitchParent{i + 1}");

	// The lever model's place in its parent (to draw it at a spot in the editor).
	public static Transform LeverModel(int i) => Lever(i) ? Lever(i).Find("ElectricalLeverOff") : null;

	// Turns a spot so the lever stands out from the wall behind it, and puts it against that wall.
	// A lever's back (towards the box's back wall) is its parent's +X.
	public static bool StickToWall(Transform spot)
	{
		Physics.SyncTransforms();
		var back = spot.right;
		if (!Physics.Raycast(spot.position - back * .3f, back, out RaycastHit hit, 2f, ~PlayerOnlyWall.Mask, QueryTriggerInteraction.Ignore)) return false;
		var model = LeverModel(0);
		float depth = .04f;
		if (model)
		{
			var b = model.GetComponent<Renderer>().bounds;
			var parent = model.parent;
			float pivotX = parent.InverseTransformPoint(model.position).x;
			depth = Enumerable.Range(0, 8).Select(k => parent.InverseTransformPoint(new Vector3(k % 2 == 0 ? b.min.x : b.max.x, (k / 2) % 2 == 0 ? b.min.y : b.max.y, k / 4 == 0 ? b.min.z : b.max.z)).x).Max() - pivotX + .03f;
		}
		spot.rotation = Quaternion.FromToRotation(Vector3.right, -hit.normal);
		spot.position = hit.point - spot.right * depth;
		return true;
	}

	// Finds the nearest wall around a spot (any direction) and puts the spot against it, lever facing out.
	public static bool StickToNearestWall(Transform spot)
	{
		Physics.SyncTransforms();
		float best = float.MaxValue; Vector3 bestDir = Vector3.zero;
		for (int k = 0; k < 16; k++)
		{
			var d = Quaternion.Euler(0f, k * 22.5f, 0f) * Vector3.forward;
			if (!Physics.Raycast(spot.position - d * .5f, d, out RaycastHit hit, 2.5f, ~PlayerOnlyWall.Mask, QueryTriggerInteraction.Ignore)) continue;
			if (Vector3.Dot(hit.normal, d) > -.5f || Mathf.Abs(hit.normal.y) > .5f) continue; // only walls facing back at us
			float away = Mathf.Abs(hit.distance - .5f);
			if (away < best) { best = away; bestDir = -hit.normal; }
		}
		if (best == float.MaxValue) return false;
		bestDir.y = 0f;
		spot.rotation = Quaternion.FromToRotation(Vector3.right, bestDir.normalized);
		spot.position -= bestDir.normalized * .5f; // step out so StickToWall's ray starts in front of the wall
		return StickToWall(spot);
	}

	static void Build()
	{
		if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets", "EscapeLevers");
		var red = LightMat("LeverLightRed", new Color(1f, .08f, .05f));
		var green = LightMat("LeverLightGreen", new Color(.1f, 1f, .2f));
		var door = C.Find("EntireMansion/Property/OpenableInteractables/DoorsAndOpenables/MainDoorHolder/MainDoor");
		var player = C.Find("PlayerStuff/Player");
		if (!door || !Lever(0) || !Lever(1)) { Debug.LogError("LEVERS front door or levers not found"); return; }
		var doorBounds = door.GetComponentsInChildren<Renderer>(true).Select(r => r.bounds).Aggregate((a, b) => { a.Encapsulate(b); return a; });
		// The door is thin one way: that's the way the wall faces. Inside is towards where the player starts.
		var s = doorBounds.size;
		var thin = s.x < s.z ? Vector3.right : Vector3.forward;
		if (player && Vector3.Dot(player.position - doorBounds.center, thin) < 0) thin = -thin;
		var along = Vector3.Cross(Vector3.up, thin).normalized;

		// Lights above the front door (kept where they were moved to).
		var root = Root();
		var lights = root.Cast<Transform>().FirstOrDefault(t => t.name == LightsName);
		if (!lights)
		{
			lights = new GameObject(LightsName).transform;
			lights.SetParent(root, false);
			lights.SetPositionAndRotation(new Vector3(doorBounds.center.x, doorBounds.max.y + .42f, doorBounds.center.z), Quaternion.LookRotation(thin));
		}
		foreach (Transform child in lights.Cast<Transform>().ToList()) Object.DestroyImmediate(child.gameObject);
		if (!lights.TryGetComponent(out EscapeLevers lever)) lever = lights.gameObject.AddComponent<EscapeLevers>();
		lever.Red = red; lever.Green = green;
		lever.Lights = new Renderer[2];
		for (int i = 0; i < 2; i++)
		{
			var bulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			bulb.name = "Light " + (i + 1);
			Object.DestroyImmediate(bulb.GetComponent<Collider>());
			bulb.transform.SetParent(lights, false);
			bulb.transform.localPosition = new Vector3(i == 0 ? -.22f : .22f, 0f, 0f);
			bulb.transform.localScale = Vector3.one * .2f;
			var r = bulb.GetComponent<Renderer>();
			r.sharedMaterial = red;
			r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			lever.Lights[i] = r;
		}

		// Spots for the two levers (kept where they were moved to); first time, on the wall either side of the front door.
		var spots = root.Cast<Transform>().FirstOrDefault(t => t.name == SpotsName);
		if (!spots) { spots = new GameObject(SpotsName).transform; spots.SetParent(root, false); }
		lever.Spots = new Transform[2];
		lever.Levers = new[] { Lever(0), Lever(1) };
		for (int i = 0; i < 2; i++)
		{
			string name = "Escape Lever Spot " + (i + 1);
			var spot = spots.Cast<Transform>().FirstOrDefault(t => t.name == name);
			if (!spot)
			{
				spot = new GameObject(name).transform;
				spot.SetParent(spots, false);
				// Beside the door on the inside, 1.3 m above the floor found under that point (measured from the lights, which
				// sit over the door's middle; the door's own bounds take in more than the door).
				var beside = lights.position + lights.right * (i == 0 ? -1.6f : 1.6f) + lights.forward * .6f;
				float floor = Physics.Raycast(beside, Vector3.down, out RaycastHit fh, 8f, ~PlayerOnlyWall.Mask, QueryTriggerInteraction.Ignore) ? fh.point.y : beside.y - 2.6f;
				spot.position = new Vector3(beside.x, floor + 1.3f, beside.z);
				spot.rotation = Quaternion.FromToRotation(Vector3.right, -lights.forward);
				Debug.Log($"LEVERS spot {i + 1} {(StickToWall(spot) ? "on the wall" : "NOT on a wall (move it)")} at {spot.position}");
			}
			else Debug.Log($"LEVERS spot {i + 1} {(StickToNearestWall(spot) ? "put against the nearest wall" : "has NO wall nearby (move it)")} at {spot.position}, facing {-spot.right}");
			lever.Spots[i] = spot;
		}

		// The back plate a moved lever is mounted on, laid out in the lever's own space (copied onto each lever in the game).
		var model = LeverModel(0);
		var parent = model.parent;
		var lb = model.GetComponent<Renderer>().bounds;
		var corners = Enumerable.Range(0, 8).Select(k => parent.InverseTransformPoint(new Vector3(k % 2 == 0 ? lb.min.x : lb.max.x, (k / 2) % 2 == 0 ? lb.min.y : lb.max.y, k / 4 == 0 ? lb.min.z : lb.max.z))).ToArray();
		var min = new Vector3(corners.Min(c => c.x), corners.Min(c => c.y), corners.Min(c => c.z));
		var max = new Vector3(corners.Max(c => c.x), corners.Max(c => c.y), corners.Max(c => c.z));
		var plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
		plate.name = "Lever Back Plate (template)";
		plate.transform.SetParent(lights, false);
		plate.transform.localPosition = new Vector3(max.x + .015f, (min.y + max.y) / 2f, (min.z + max.z) / 2f);
		plate.transform.localRotation = Quaternion.identity;
		plate.transform.localScale = new Vector3(.03f, (max.y - min.y) + .14f, (max.z - min.z) + .16f);
		plate.GetComponent<Renderer>().sharedMaterial = MetalMat();
		plate.SetActive(false);
		lever.PlateTemplate = plate;

		// The empty places in the switch box.
		lever.EmptySlots = new GameObject[2];
		var pick = C.All<PickRay>().FirstOrDefault();
		for (int i = 0; i < 2; i++)
		{
			string name = "Empty Lever Slot " + (i + 1);
			var switches = C.Find(Switches);
			var old = switches.Cast<Transform>().FirstOrDefault(t => t.name == name);
			if (old) Object.DestroyImmediate(old.gameObject);
			var lm = LeverModel(i);
			var b = lm.GetComponent<Renderer>().bounds;
			var slot = new GameObject(name);
			slot.transform.SetParent(switches, false);
			slot.transform.position = b.center;
			var ps = switches.lossyScale;
			slot.transform.localScale = new Vector3(1f / ps.x, 1f / ps.y, 1f / ps.z);
			slot.layer = lm.gameObject.layer;
			var box = slot.AddComponent<BoxCollider>();
			box.size = b.size + Vector3.one * .04f;
			var interactive = slot.AddComponent<Interactive>();
			interactive.isOneTimeAction = false;
			interactive.performedEvent = new UnityEngine.Events.UnityEvent();
			interactive.timedEvent = new UnityEngine.Events.UnityEvent();
			if (pick) UnityEventTools.AddStringPersistentListener(interactive.performedEvent, pick.TimedTextInitiate, Message);
			slot.SetActive(false);
			lever.EmptySlots[i] = slot;
		}

		// The submarine's launch lever also waits for both levers.
		var subLever = C.Find("EntireMansion/Property/Design/Granny_OmGi-Stuff/MiniSubmarine/SubmarineCockpit/SubmarineDragParent/SubmarineLever");
		if (subLever && !subLever.GetComponent<NeedsEscapeLevers>()) subLever.gameObject.AddComponent<NeedsEscapeLevers>();
		EditorUtility.SetDirty(lights.gameObject);
		Debug.Log($"LEVERS lights at {lights.position}, spots at {lever.Spots[0].position} and {lever.Spots[1].position}; submarine lever {(subLever ? "hooked" : "NOT found")}");
	}

	// Puts the lights and lever spots back to where they start, then rebuilds and saves. -executeMethod EscapeLeversSetup.ResetPlacesBatch
	public static void ResetPlacesBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var root = Root();
		foreach (var name in new[] { LightsName, SpotsName })
		{
			var t = root.Cast<Transform>().FirstOrDefault(c => c.name == name);
			if (t) Object.DestroyImmediate(t.gameObject);
		}
		Build();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
	}

	// Command line: pictures of the lights (red, and one green) and of each lever at its spot (nothing is saved).
	public static void Preview()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var l = C.All<EscapeLevers>().First();
		var lp = l.transform.position; var f = l.transform.forward;
		Shot(dir + "/levers_lights_red.png", lp + f * 2.5f + Vector3.down * .8f, lp);
		l.Lights[0].sharedMaterial = l.Green;
		Shot(dir + "/levers_lights_one_green.png", lp + f * 2.5f + Vector3.down * .8f, lp);
		for (int i = 0; i < 2; i++)
		{
			var lever = l.Levers[i]; var spot = l.Spots[i];
			EscapeLevers.PlaceAt(lever, spot);
			var w = EscapeLevers.BuildWallLever(lever, l.PlateTemplate.GetComponent<Renderer>().sharedMaterial, l.Red);
			var c = w.Handle.position;
			var from = c - spot.right * .65f + Vector3.up * .12f + spot.forward * .3f;
			Shot($"{dir}/levers_spot{i + 1}.png", from, c);
			EscapeLevers.SetAngle(w, 40f);
			w.Knob.sharedMaterial = l.Green;
			Shot($"{dir}/levers_spot{i + 1}_pulled.png", from, c);
			var col = LeverModel(i).GetComponent<BoxCollider>();
			Debug.Log($"LEVERS lever {i + 1} click box {col.bounds.center} size {col.bounds.size}");
		}
		Debug.Log("LEVERS pictures done");
	}

	static void Shot(string file, Vector3 from, Vector3 lookAt)
	{
		var go = new GameObject("PreviewCam");
		var cam = go.AddComponent<Camera>();
		cam.transform.position = from; cam.transform.LookAt(lookAt);
		cam.fieldOfView = 55f; cam.nearClipPlane = .02f;
		var rt = new RenderTexture(900, 600, 24);
		cam.targetTexture = rt; cam.Render();
		RenderTexture.active = rt;
		var tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, 900, 600), 0, 0);
		System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
		RenderTexture.active = null;
		Object.DestroyImmediate(go); Object.DestroyImmediate(rt);
	}
}
