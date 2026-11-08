using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Slientlo Never Clear: the train's electric switch box is closed and padlocked; the padlock key opens it and the padlock
// and the box door fall off. Tools > Evolved Restoration > Set Up Switch Box Lock.
public static class SwitchBoxLock
{
	const string SceneFile = "Assets/1_Scenes/Scene.unity";
	const string SlotPath = "EntireMansion/Property/ImportantInteractables/MissingElectricSwitchCollider";

	static string PathOf(Transform t)
	{
		string path = t.name;
		for (t = t.parent; t; t = t.parent) path = t.name + "/" + path;
		return path;
	}

	const string DoorPath = "EntireMansion/Property/ImportantInteractables/Subway_ElectricityBoxDoor";
	const string PadlockSource = "EntireMansion/Property/ImportantInteractables/VasePedestalPuzzleRoom/PedestalPadlockLock";
	const string PadlockName = "SwitchBoxPadlock";
	const float DoorSize = 1.69f;

	static Transform Find(string path) =>
		Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(t => t.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(t) && PathOf(t) == path);

	[MenuItem("Tools/Evolved Restoration/Set Up Switch Box Lock")]
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
		Debug.Log("SWITCHBOX done and saved");
	}

	// Where the door sits shut: hinged on its edge at the pivot, flat in front of the switch levers, facing the player.
	// The door model is thin along its local X, hinged along local Z (vertical), and reaches out along local -Y.
	static void ClosedPose(Transform door, out Vector3 position, out Quaternion rotation)
	{
		rotation = Quaternion.LookRotation(Vector3.up, Vector3.back);   // local Z up, local Y towards -Z (so the door reaches +Z)
		var levers = Resources.FindObjectsOfTypeAll<Transform>()
			.Where(t => t.gameObject.scene.IsValid() && t.name == "ElectricalLeverOff" && t.GetComponent<MeshFilter>()).ToList();
		float front = levers.Count > 0
			? levers.Min(l => { var mf = l.GetComponent<MeshFilter>(); var b = mf.sharedMesh.bounds; return Enumerable.Range(0, 8)
				.Select(i => l.TransformPoint(new Vector3(i % 2 == 0 ? b.min.x : b.max.x, (i / 2) % 2 == 0 ? b.min.y : b.max.y, i / 4 == 0 ? b.min.z : b.max.z)).x).Min(); })
			: door.position.x;
		// The door already stands level with the box frame, just turned a few degrees open: square it up where it is.
		position = door.position;
		Debug.Log($"SWITCHBOX door now at {door.position} right {door.right} up {door.up} forward {door.forward}; levers' front x {front:0.000}; shut at {position}");
	}

	// The padlock copy's front and up in the world, guessed from its model: its thinnest side is its front/back.
	static Vector3 PadlockFront(Transform padlock)
	{
		var b = padlock.GetComponent<MeshFilter>().sharedMesh.bounds.size;
		Vector3 axis = b.x <= b.y && b.x <= b.z ? Vector3.right : b.y <= b.z ? Vector3.up : Vector3.forward;
		return padlock.TransformDirection(axis).normalized;
	}

	static Vector3 PadlockUp(Transform padlock)
	{
		// Its longest side is up and down (shackle on top).
		var b = padlock.GetComponent<MeshFilter>().sharedMesh.bounds.size;
		Vector3 axis = b.x >= b.y && b.x >= b.z ? Vector3.right : b.y >= b.z ? Vector3.up : Vector3.forward;
		var up = padlock.TransformDirection(axis).normalized;
		return Vector3.Dot(up, Vector3.up) < 0 ? -up : up;
	}

	static void Build()
	{
		foreach (var old in Resources.FindObjectsOfTypeAll<SwitchBoxPadlock>().Where(p => p.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(p)).ToList())
			Object.DestroyImmediate(old.gameObject);
		var door = Find(DoorPath);
		var source = Find(PadlockSource);
		var slot = Find(SlotPath);
		if (!door || !source || !slot) { Debug.LogError("SWITCHBOX door, padlock or slot not found"); return; }
		ClosedPose(door, out Vector3 shutAt, out Quaternion shutRot);

		var padlock = Object.Instantiate(source.gameObject, door.parent);
		padlock.name = PadlockName;
		padlock.SetActive(true);
		// Hung on the door's red handle (near its free edge, at mid-height), facing the player (-X).
		var free = shutAt + Vector3.forward * (DoorSize - .19f) + Vector3.down * .1f + Vector3.left * .13f;
		padlock.transform.SetPositionAndRotation(free, Quaternion.LookRotation(Vector3.left, Vector3.up) * Quaternion.Inverse(Quaternion.LookRotation(PadlockFront(source), PadlockUp(source))) * source.rotation);
		padlock.transform.localScale = source.localScale * 2.5f;   // the game's padlock is only ~5 cm; big enough to see on the box
		var body = padlock.GetComponent<Rigidbody>();
		if (body) body.isKinematic = true;

		var puzzle = padlock.GetComponent<RegPuzzle>();
		var lockScript = padlock.AddComponent<SwitchBoxPadlock>();
		lockScript.Door = door.GetComponent<Rigidbody>();
		lockScript.DoorInteractive = door.GetComponent<Interactive>();
		lockScript.ClosedPosition = shutAt;
		lockScript.ClosedRotation = shutRot;
		lockScript.Slot = slot.GetComponent<Collider>();
		lockScript.Padlock = body;
		lockScript.PadlockSound = padlock.GetComponent<AudioSource>();
		lockScript.Puzzle = puzzle;
		// The copy's actions opened the vase pedestal; this one only opens the box.
		puzzle.performedEvent = new UnityEngine.Events.UnityEvent();
		UnityEditor.Events.UnityEventTools.AddPersistentListener(puzzle.performedEvent, lockScript.Unlock);
		puzzle.performedAction = false;
		for (int i = 0; i < puzzle.performedEventNo.GetPersistentEventCount(); i++)
			Debug.Log($"SWITCHBOX padlock 'no key' action kept: {puzzle.performedEventNo.GetPersistentTarget(i)}.{puzzle.performedEventNo.GetPersistentMethodName(i)}");
		Debug.Log($"SWITCHBOX padlock at {free}, needs {(puzzle.requiredItem ? puzzle.requiredItem.name : "nothing")}");
		EditorUtility.SetDirty(padlock);
	}

	// Command line: pictures of the box shut and padlocked (nothing is saved). -executeMethod SwitchBoxLock.Preview -previewDir <folder>
	public static void Preview()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var padlock = Resources.FindObjectsOfTypeAll<SwitchBoxPadlock>().First(p => p.gameObject.scene.IsValid());
		padlock.Door.transform.SetPositionAndRotation(padlock.ClosedPosition, padlock.ClosedRotation);
		var centre = padlock.ClosedPosition + Vector3.forward * DoorSize / 2f;
		Shot(dir + "/switchbox_front.png", centre + Vector3.left * 2.2f + Vector3.up * .2f, centre);
		Shot(dir + "/switchbox_angle.png", centre + new Vector3(-1.6f, .3f, -1.4f), centre);
		Shot(dir + "/switchbox_padlock.png", padlock.transform.position + new Vector3(-.6f, .15f, -.25f), padlock.transform.position);
		Debug.Log("SWITCHBOX pictures done");
	}

	static void Shot(string file, Vector3 from, Vector3 lookAt)
	{
		var go = new GameObject("PreviewCam");
		var cam = go.AddComponent<Camera>();
		cam.transform.position = from; cam.transform.LookAt(lookAt);
		cam.fieldOfView = 55f; cam.nearClipPlane = .02f;
		var light = new GameObject("PreviewLight").AddComponent<Light>();
		light.type = LightType.Directional; light.intensity = 1.1f; light.transform.rotation = Quaternion.Euler(30f, 70f, 0f);
		var rt = new RenderTexture(900, 600, 24);
		cam.targetTexture = rt; cam.Render();
		RenderTexture.active = rt;
		var tex = new Texture2D(900, 600, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, 900, 600), 0, 0);
		System.IO.File.WriteAllBytes(file, tex.EncodeToPNG());
		RenderTexture.active = null;
		Object.DestroyImmediate(go); Object.DestroyImmediate(light.gameObject); Object.DestroyImmediate(rt);
	}

	// Command line: lists what's around any object (by path), with a picture. -executeMethod SwitchBoxLock.SurveyAt -at <path> -previewDir <folder>
	public static void SurveyAt()
	{
		var args = System.Environment.GetCommandLineArgs();
		string Arg(string k) { int i = System.Array.IndexOf(args, k); return i >= 0 && i + 1 < args.Length ? args[i + 1] : null; }
		EditorSceneManager.OpenScene(SceneFile);
		var centre = Find(Arg("-at"));
		if (!centre) { Debug.LogError("SURVEY not found: " + Arg("-at")); return; }
		Debug.Log($"SURVEY {PathOf(centre)} at {centre.position} rot {centre.eulerAngles} scale {centre.lossyScale} collider {(centre.GetComponent<Collider>() ? centre.GetComponent<Collider>().bounds.ToString() : "-")}");
		foreach (var r in Resources.FindObjectsOfTypeAll<MeshFilter>().Where(m => m.gameObject.scene.IsValid() && m.sharedMesh))
		{
			var t = r.transform; var b = r.sharedMesh.bounds;
			var c = t.TransformPoint(b.center);
			float d = Vector3.Distance(c, centre.position);
			var size = Vector3.Scale(b.size, t.lossyScale);
			if (d > 1.2f && !(size.magnitude > 2f && Vector3.Distance(centre.position, c) < size.magnitude * .6f)) continue;
			Debug.Log($"SURVEY near {d:0.00} m: {PathOf(t)} mesh {r.sharedMesh.name} ({r.sharedMesh.vertexCount} verts, {r.sharedMesh.subMeshCount} parts) centre {c} size {size} active {r.gameObject.activeInHierarchy} mats {string.Join(",", r.GetComponent<Renderer>().sharedMaterials.Select(m => m ? m.name : "-"))}");
		}
		string dir = Arg("-previewDir");
		if (dir != null)
		{
			RenderSettings.fog = false;
			var n = centre.GetComponent<Collider>() ? centre.GetComponent<Collider>().bounds.center : centre.position;
			foreach (var (name, offset) in new[] { ("front", centre.forward), ("back", -centre.forward), ("right", centre.right), ("left", -centre.right) })
			{
				var from = n + offset.normalized * 1.1f + Vector3.up * .2f;
				if (Physics.Linecast(from, n, out RaycastHit hit) && (hit.point - n).sqrMagnitude > .05f) continue;   // blocked side
				Shot($"{dir}/survey_{name}.png", from, n);
			}
		}
	}

	// Command line: wide pictures of the gate controls, and the triangles of the combined scenery model around the fuse socket.
	// -executeMethod SwitchBoxLock.FuseSocketLook -previewDir <folder>
	public static void FuseSocketLook()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var slot = Find("EntireMansion/Property/ImportantInteractables/GateControl/FusePlaceCollider");
		var c = slot.GetComponent<Collider>().bounds.center;
		var face = -slot.forward;   // the socket faces the player on this side
		Shot(dir + "/gate_wide.png", c + face * 2.6f + Vector3.up * .3f, c);
		Shot(dir + "/gate_wide_left.png", c + (face * 2.2f + slot.right * 1.2f) + Vector3.up * .3f, c);
		Shot(dir + "/gate_wide_right.png", c + (face * 2.2f - slot.right * 1.2f) + Vector3.up * .3f, c);
		foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>().Where(g => g.scene.IsValid() && g.name.StartsWith("GateControl")))
			foreach (var t in go.GetComponentsInChildren<Transform>(true)) Debug.Log($"FUSESOCKET part {PathOf(t)} at {t.position}");
		// Which combined models have triangles at the socket.
		var box = new Bounds(c + face * .05f, new Vector3(.6f, .6f, .4f));
		foreach (var mf in Resources.FindObjectsOfTypeAll<MeshFilter>().Where(m => m.gameObject.scene.IsValid() && m.sharedMesh && m.sharedMesh.isReadable))
		{
			var mesh = mf.sharedMesh;
			if (!mesh.name.StartsWith("Combined")) continue;
			var v = mesh.vertices;
			var mats = mf.GetComponent<Renderer>().sharedMaterials;
			for (int s = 0; s < mesh.subMeshCount; s++)
			{
				var tri = mesh.GetTriangles(s);
				int n = 0;
				for (int i = 0; i < tri.Length; i += 3)
					if (box.Contains(mf.transform.TransformPoint(v[tri[i]])) && box.Contains(mf.transform.TransformPoint(v[tri[i + 1]])) && box.Contains(mf.transform.TransformPoint(v[tri[i + 2]]))) n++;
				if (n > 0) Debug.Log($"FUSESOCKET {PathOf(mf.transform)} '{mesh.name}' part {s} ({(s < mats.Length && mats[s] ? mats[s].name : "-")}): {n} triangles inside the socket box");
			}
		}
	}

	// Cuts the gate controls' fuse socket (grey plate and round hole) out of the board's own model (CamMonitor, the one
	// its collider uses) into a model of its own, centred on the socket. Saved as Assets/GateFuse/FuseSocket.asset.
	public static Mesh ExtractFuseSocket(out Vector3 socketCentre)
	{
		var board = Find("EntireMansion/Property/ImportantInteractables/GateControl/GateControlBoard");
		var fuse = Find("EntireMansion/Property/ImportantInteractables/GateControl/CamMonitorFusePlaced");
		var source = board.GetComponent<MeshCollider>().sharedMesh;
		socketCentre = new Vector3(fuse.position.x, fuse.position.y, fuse.position.z);
		// The plate and hole only: inside the frame's walls sideways (the frame is ~0.47 m wide inside), from in front of the
		// plate to behind the hole.
		var box = new Bounds(socketCentre + Vector3.forward * .05f, new Vector3(.43f, .43f, .9f));
		var v = source.vertices; var n = source.normals; var uv = source.uv; var tri = source.triangles;
		var map = new System.Collections.Generic.Dictionary<int, int>();
		var nv = new System.Collections.Generic.List<Vector3>(); var nn = new System.Collections.Generic.List<Vector3>(); var nuv = new System.Collections.Generic.List<Vector2>();
		var nt = new System.Collections.Generic.List<int>();
		for (int i = 0; i < tri.Length; i += 3)
		{
			var w = new[] { board.TransformPoint(v[tri[i]]), board.TransformPoint(v[tri[i + 1]]), board.TransformPoint(v[tri[i + 2]]) };
			if (!w.All(box.Contains)) continue;
			for (int k = 0; k < 3; k++)
			{
				int old = tri[i + k];
				if (!map.TryGetValue(old, out int idx))
				{
					idx = nv.Count; map[old] = idx;
					nv.Add(w[k] - socketCentre);
					nn.Add(board.TransformDirection(n[old]).normalized);
					nuv.Add(uv.Length > old ? uv[old] : Vector2.zero);
				}
				nt.Add(idx);
			}
		}
		// Mirrored scale on the board flips winding; keep faces pointing the way their normals do.
		for (int i = 0; i < nt.Count; i += 3)
		{
			var face = Vector3.Cross(nv[nt[i + 1]] - nv[nt[i]], nv[nt[i + 2]] - nv[nt[i]]);
			if (Vector3.Dot(face, nn[nt[i]] + nn[nt[i + 1]] + nn[nt[i + 2]]) < 0) { int t = nt[i + 1]; nt[i + 1] = nt[i + 2]; nt[i + 2] = t; }
		}
		var mesh = new Mesh { name = "FuseSocket" };
		mesh.SetVertices(nv); mesh.SetNormals(nn); mesh.SetUVs(0, nuv); mesh.SetTriangles(nt, 0);
		mesh.RecalculateBounds();
		if (!AssetDatabase.IsValidFolder("Assets/GateFuse")) AssetDatabase.CreateFolder("Assets", "GateFuse");
		AssetDatabase.DeleteAsset("Assets/GateFuse/FuseSocket.asset");
		AssetDatabase.CreateAsset(mesh, "Assets/GateFuse/FuseSocket.asset");
		Debug.Log($"FUSESOCKET cut {nt.Count / 3} triangles ({nv.Count} points) from {source.name}; size {mesh.bounds.size}, front at {mesh.bounds.min.z:0.000}, back at {mesh.bounds.max.z:0.000}");
		return mesh;
	}

	// Command line: cuts the socket and shows a copy on the wall left of the gate controls (nothing is saved to the map).
	// -executeMethod SwitchBoxLock.FuseSocketTest -previewDir <folder>
	public static void FuseSocketTest()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		Physics.SyncTransforms();
		ExtractFuseSocket(out Vector3 centre);
		var mesh = BuildBlueSocket(out Material plate);
		var boardMat = Find("EntireMansion/Property/ImportantInteractables/GateControl/GateControlBoard").GetComponent<Renderer>().sharedMaterial;
		var copy = new GameObject("BlueFuseSocket (test)");
		copy.AddComponent<MeshFilter>().sharedMesh = mesh;
		copy.AddComponent<MeshRenderer>().sharedMaterials = new[] { plate, boardMat };
		// On the wall left of the frame: find the wall with a ray from the player's side.
		var from = new Vector3(centre.x - .75f, centre.y, centre.z - 1.5f);
		if (Physics.Raycast(from, Vector3.forward, out RaycastHit wall, 3f))
		{
			copy.transform.position = new Vector3(from.x, from.y, wall.point.z - .002f);   // back of the housing on the wall
			Debug.Log($"FUSESOCKET wall left of the frame at z {wall.point.z:0.000} ({wall.collider.name}); copy at {copy.transform.position}");
		}
		else Debug.LogWarning("FUSESOCKET no wall found left of the frame");
		var look = copy.transform.position;
		Shot(dir + "/socket_copy_front.png", look + new Vector3(.35f, .1f, -1.6f), look + new Vector3(.35f, 0f, 0f));
		Shot(dir + "/socket_copy_close.png", look + new Vector3(-.15f, .1f, -.7f), look);
		Shot(dir + "/socket_copy_side.png", look + new Vector3(-.8f, .1f, -.6f), look);
	}

	// The board texture's grey plate area (UV rectangle), found by reading the texture around where the plate is painted.
	static Rect PlateUV(Transform board, Vector3 socketCentre)
	{
		var col = board.GetComponent<MeshCollider>();
		var probe = socketCentre + new Vector3(.08f, .08f, 0f);   // on the plate, clear of the hole
		if (!col.Raycast(new Ray(probe + Vector3.back * 1f, Vector3.forward), out RaycastHit hit, 3f)) { Debug.LogWarning("FUSESOCKET plate not hit"); return new Rect(0, 0, 1, 1); }
		var mat = board.GetComponent<Renderer>().sharedMaterial;
		var tex = new Texture2D(2, 2);
		tex.LoadImage(System.IO.File.ReadAllBytes(AssetDatabase.GetAssetPath(mat.mainTexture)));
		int w = tex.width, h = tex.height;
		var px = tex.GetPixels();
		int sx = Mathf.Clamp((int)(Mathf.Repeat(hit.textureCoord.x, 1f) * w), 0, w - 1), sy = Mathf.Clamp((int)(Mathf.Repeat(hit.textureCoord.y, 1f) * h), 0, h - 1);
		// Average colour around the probe, then grow the area of similar (grey) pixels.
		Color seed = Color.black; int cnt = 0;
		for (int dy = -3; dy <= 3; dy++) for (int dx = -3; dx <= 3; dx++) { int x = Mathf.Clamp(sx + dx, 0, w - 1), y = Mathf.Clamp(sy + dy, 0, h - 1); seed += px[y * w + x]; cnt++; }
		seed /= cnt;
		bool Grey(Color c) { Color.RGBToHSV(c, out _, out float s, out float v); Color.RGBToHSV(seed, out _, out _, out float sv); return s < .25f && Mathf.Abs(v - sv) < .35f; }
		int minX = sx, maxX = sx, minY = sy, maxY = sy;
		var seen = new bool[w * h]; var q = new System.Collections.Generic.Queue<int>(); q.Enqueue(sy * w + sx); seen[sy * w + sx] = true;
		while (q.Count > 0)
		{
			int i = q.Dequeue(); int x = i % w, y = i / w;
			minX = Mathf.Min(minX, x); maxX = Mathf.Max(maxX, x); minY = Mathf.Min(minY, y); maxY = Mathf.Max(maxY, y);
			foreach (var (nx, ny) in new[] { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) })
			{
				if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
				int j = ny * w + nx;
				if (seen[j] || !Grey(px[j])) continue;
				seen[j] = true; q.Enqueue(j);
			}
		}
		var rect = Rect.MinMaxRect((minX + 2f) / w, (minY + 2f) / h, (maxX - 1f) / w, (maxY - 1f) / h);
		Debug.Log($"FUSESOCKET plate texture area {rect} of a {w}x{h} texture (seed colour {seed})");
		return rect;
	}

	// The blue fuse's socket: a square housing for a wall, with a round hole in its front, the game's own round socket
	// (cut from the board) inside, the board's grey plate texture on the front, tinted blue. Pivot at the middle of its back.
	public static Mesh BuildBlueSocket(out Material material)
	{
		var board = Find("EntireMansion/Property/ImportantInteractables/GateControl/GateControlBoard");
		var ring = ExtractFuseSocket(out Vector3 centre);
		var rv = ring.vertices;
		float front = rv.Min(p => p.z);
		float holeR = rv.Where(p => p.z < front + .01f).Max(p => new Vector2(p.x, p.y).magnitude);
		const float half = .23f;   // 46 cm square: room around the fuse's cap, like the original plate
		float depth = Mathf.Max(.1f, rv.Max(p => p.z) - front + .005f);   // deep enough to hold the whole round socket
		// How the real plate's texture is laid out: the texture point at three of its corners (clear of the hole).
		var col = board.GetComponent<MeshCollider>();
		Vector2 TexAt(float x, float y)
		{
			var p = centre + new Vector3(x, y, 0f);
			return col.Raycast(new Ray(p + Vector3.back, Vector3.forward), out RaycastHit h, 3f) ? h.textureCoord : Vector2.zero;
		}
		const float corner = .14f;
		Vector2 t00 = TexAt(-corner, -corner), t10 = TexAt(corner, -corner), t01 = TexAt(-corner, corner);
		Debug.Log($"FUSESOCKET plate texture at corners {t00} {t10} {t01}");
		var verts = new System.Collections.Generic.List<Vector3>(); var norms = new System.Collections.Generic.List<Vector3>();
		var uvs = new System.Collections.Generic.List<Vector2>(); var tris = new System.Collections.Generic.List<int>();
		Vector2 UV(float x, float y) => t00 + (t10 - t00) * ((x + corner) / (2f * corner)) + (t01 - t00) * ((y + corner) / (2f * corner));
		// Front face (faces -Z, at z = 0 here; moved later) with a round hole: rings between the hole and the square edge.
		const int seg = 32;
		for (int i = 0; i < seg; i++)
		{
			float a = i / (float)seg * Mathf.PI * 2f;
			var dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
			var inner = dir * holeR;
			var outer = dir * (half / Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y)));   // out to the square's edge
			verts.Add(new Vector3(inner.x, inner.y, 0f)); norms.Add(Vector3.back); uvs.Add(UV(inner.x, inner.y));
			verts.Add(new Vector3(outer.x, outer.y, 0f)); norms.Add(Vector3.back); uvs.Add(UV(outer.x, outer.y));
		}
		for (int i = 0; i < seg; i++)
		{
			int a = i * 2, b = ((i + 1) % seg) * 2;
			tris.AddRange(new[] { a, b + 1, a + 1, a, b, b + 1 });
		}
		// Square corners so the front reaches all the way out (the ring above stops at the edge midpoints between samples).
		// Sides of the housing, from the front back to the wall.
		var corners = new[] { new Vector2(-half, -half), new Vector2(half, -half), new Vector2(half, half), new Vector2(-half, half) };
		for (int c = 0; c < 4; c++)
		{
			var p0 = corners[c]; var p1 = corners[(c + 1) % 4];
			var nrm = new Vector3(p0.y == p1.y ? 0f : Mathf.Sign(p0.x), p0.x == p1.x ? 0f : Mathf.Sign(p0.y), 0f);
			if (p0.y == p1.y) nrm = new Vector3(0f, Mathf.Sign(p0.y), 0f); else nrm = new Vector3(Mathf.Sign(p0.x), 0f, 0f);
			int s = verts.Count;
			verts.Add(new Vector3(p0.x, p0.y, 0f)); verts.Add(new Vector3(p1.x, p1.y, 0f)); verts.Add(new Vector3(p1.x, p1.y, depth)); verts.Add(new Vector3(p0.x, p0.y, depth));
			for (int k = 0; k < 4; k++) norms.Add(nrm);
			uvs.Add(UV(p0.x, p0.y)); uvs.Add(UV(p1.x, p1.y)); uvs.Add(UV(p1.x, p1.y * .9f)); uvs.Add(UV(p0.x, p0.y * .9f));
			tris.AddRange(new[] { s, s + 1, s + 2, s, s + 2, s + 3 });
		}
		// Fix winding so every face points along its normal.
		for (int i = 0; i < tris.Count; i += 3)
		{
			var f = Vector3.Cross(verts[tris[i + 1]] - verts[tris[i]], verts[tris[i + 2]] - verts[tris[i]]);
			if (Vector3.Dot(f, norms[tris[i]]) < 0) { int t = tris[i + 1]; tris[i + 1] = tris[i + 2]; tris[i + 2] = t; }
		}
		// The game's round socket, its front flush with the housing's front.
		int baseIndex = verts.Count;
		var rn = ring.normals; var ruv = ring.uv; var rt = ring.triangles;
		for (int i = 0; i < rv.Length; i++) { verts.Add(new Vector3(rv[i].x, rv[i].y, rv[i].z - front)); norms.Add(rn[i]); uvs.Add(ruv[i]); }
		var socketTris = rt.Select(i => i + baseIndex).ToList();
		// Pivot at the middle of the back (so it sits flush on a wall): shift everything by -depth.
		for (int i = 0; i < verts.Count; i++) verts[i] -= new Vector3(0f, 0f, depth);
		var mesh = new Mesh { name = "BlueFuseSocket", subMeshCount = 2 };
		mesh.SetVertices(verts); mesh.SetNormals(norms); mesh.SetUVs(0, uvs);
		mesh.SetTriangles(tris, 0); mesh.SetTriangles(socketTris, 1);
		mesh.RecalculateBounds();
		AssetDatabase.DeleteAsset("Assets/GateFuse/BlueFuseSocket.asset");
		AssetDatabase.CreateAsset(mesh, "Assets/GateFuse/BlueFuseSocket.asset");
		var boardMat = board.GetComponent<Renderer>().sharedMaterial;
		material = AssetDatabase.LoadAssetAtPath<Material>("Assets/GateFuse/BlueFuseSocketPlate.mat");
		if (!material) { material = new Material(boardMat); AssetDatabase.CreateAsset(material, "Assets/GateFuse/BlueFuseSocketPlate.mat"); }
		material.CopyPropertiesFromMaterial(boardMat);
		material.color = new Color(.62f, .8f, 1f);   // blue tint: this one's for the blue fuse
		Glow(material, boardMat.mainTexture, new Color(.62f, .8f, 1f) * GlowStrength);
		EditorUtility.SetDirty(material);
		Debug.Log($"FUSESOCKET blue socket built: hole radius {holeR:0.000}, housing {half * 2f:0.00} m square, {depth:0.00} m deep");
		return mesh;
	}

	const string BlueSocketName = "BlueFuseSocket";
	// The map is lit by baked lighting only, which an added object doesn't get, so it would render black.
	// A faint glow with its own texture stands in for the baked light around it.
	internal const float GlowStrength = .45f;

	internal static void Glow(Material m, Texture tex, Color colour)
	{
		m.EnableKeyword("_EMISSION");
		m.SetTexture("_EmissionMap", tex);
		m.SetColor("_EmissionColor", colour);
		m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
	}

	// The board's material with the same faint glow (for the round socket inside the blue socket).
	internal static Material GlowingCopy(Material source, string path, Color tint)
	{
		var m = AssetDatabase.LoadAssetAtPath<Material>(path);
		if (!m) { m = new Material(source); AssetDatabase.CreateAsset(m, path); }
		m.CopyPropertiesFromMaterial(source);
		m.color = tint;
		Glow(m, source.mainTexture, tint * GlowStrength);
		EditorUtility.SetDirty(m);
		return m;
	}

	// Adds the blue fuse's socket to the map, on the wall left of the gate controls (Slientlo only). If it's already there,
	// its model is rebuilt and it stays where it was moved to.
	[MenuItem("Tools/Evolved Restoration/Add Blue Fuse Socket")]
	public static void AddBlueSocket()
	{
		if (EditorSceneManager.GetActiveScene().path != SceneFile)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
			EditorSceneManager.OpenScene(SceneFile);
		}
		var g = MakeBlueSocket();
		Selection.activeGameObject = g;
		if (SceneView.lastActiveSceneView) SceneView.lastActiveSceneView.Frame(new Bounds(g.transform.position, Vector3.one * 1.5f), false);
		EditorSceneManager.MarkSceneDirty(g.scene);
	}

	public static void AddBlueSocketBatch()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var g = MakeBlueSocket();
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log($"FUSESOCKET added {PathOf(g.transform)} at {g.transform.position} and saved");
	}

	static GameObject MakeBlueSocket()
	{
		Physics.SyncTransforms();
		var mesh = BuildBlueSocket(out Material plate);
		var gateControl = Find("EntireMansion/Property/ImportantInteractables/GateControl");
		var boardMat = Find("EntireMansion/Property/ImportantInteractables/GateControl/GateControlBoard").GetComponent<Renderer>().sharedMaterial;
		var existing = gateControl.Cast<Transform>().FirstOrDefault(t => t.name == BlueSocketName);
		GameObject g;
		if (existing) g = existing.gameObject;
		else
		{
			g = new GameObject(BlueSocketName);
			Undo.RegisterCreatedObjectUndo(g, "Add blue fuse socket");
			g.transform.SetParent(gateControl, false);
			// On the wall left of the frame, the back of the housing against it.
			var socket = Find("EntireMansion/Property/ImportantInteractables/GateControl/CamMonitorFusePlaced").position;
			var from = new Vector3(socket.x - .75f, socket.y, socket.z - 1.5f);
			g.transform.position = Physics.Raycast(from, Vector3.forward, out RaycastHit wall, 3f)
				? new Vector3(from.x, from.y, wall.point.z - .002f) : new Vector3(from.x, from.y, socket.z - .2f);
			g.transform.rotation = Quaternion.identity;
		}
		// GateControl is scaled up ~4.6x; undo that so the socket is its real size.
		var ps = g.transform.parent.lossyScale;
		g.transform.localScale = new Vector3(1f / ps.x, 1f / ps.y, 1f / ps.z);
		if (!g.TryGetComponent(out MeshFilter filter)) filter = g.AddComponent<MeshFilter>();
		filter.sharedMesh = mesh;
		if (!g.TryGetComponent(out MeshRenderer renderer)) renderer = g.AddComponent<MeshRenderer>();
		renderer.sharedMaterials = new[] { plate, GlowingCopy(boardMat, "Assets/GateFuse/BlueFuseSocketRing.mat", Color.white) };
		// Solid housing (the hole is for the fuse's own click spot, added with the blue fuse).
		if (!g.TryGetComponent(out BoxCollider box)) box = g.AddComponent<BoxCollider>();
		box.center = mesh.bounds.center; box.size = mesh.bounds.size;
		if (!g.GetComponent<HazardDifficulty>()) g.AddComponent<HazardDifficulty>();   // Slientlo Never Clear only
		EditorUtility.SetDirty(g);
		return g;
	}

	// Command line: pictures of the submarine hatch, looking at the handwheel's face from both sides (nothing is saved).
	public static void SubmarineLook()
	{
		var args = System.Environment.GetCommandLineArgs();
		int at = System.Array.IndexOf(args, "-previewDir");
		string dir = at >= 0 && at + 1 < args.Length ? args[at + 1] : System.IO.Path.GetTempPath();
		EditorSceneManager.OpenScene(SceneFile);
		RenderSettings.fog = false;
		var wheel = Find("EntireMansion/Property/Design/Granny_OmGi-Stuff/MiniSubmarine/SubmarineDoorAnimated/DoorSubmarine/ColliderPlaceWheel/HandwheelSubmarinePlaced");
		var door = Find("EntireMansion/Property/Design/Granny_OmGi-Stuff/MiniSubmarine/SubmarineDoorAnimated/DoorSubmarine");
		wheel.gameObject.SetActive(true);
		var mf = wheel.GetComponent<MeshFilter>(); var b = mf.sharedMesh.bounds;
		var c = wheel.TransformPoint(b.center);
		// The wheel is thin along one of its axes: that's the way the hatch faces.
		Vector3 thin = b.size.x <= b.size.y && b.size.x <= b.size.z ? wheel.right : b.size.y <= b.size.z ? wheel.up : wheel.forward;
		var doorCentre = door.GetComponent<Renderer>().bounds.center;
		var outward = Vector3.Dot(thin, c - door.parent.position) >= 0 ? thin : -thin;
		Debug.Log($"SUBLOOK wheel centre {c}, hatch faces {outward}, door centre {doorCentre}, door size {door.GetComponent<Renderer>().bounds.size}, door rot {door.eulerAngles}");
		foreach (var (name, n) in new[] { ("out", outward), ("in", -outward) })
		{
			var side = Vector3.Cross(n, Vector3.up).sqrMagnitude > .01f ? Vector3.Cross(n, Vector3.up).normalized : Vector3.right;
			Shot($"{dir}/sub_{name}_straight.png", c + n * 2.4f, c);
			Shot($"{dir}/sub_{name}_angle.png", c + n * 2f + side * 1.2f + Vector3.up * .6f, c);
		}
		Shot($"{dir}/sub_wide.png", c + outward * 5f + Vector3.up * 2f, c);
	}

	// Command line: every renderer whose drawn box overlaps the fuse socket (works for objects merged into combined models).
	public static void SocketRenderers()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var fuse = Find("EntireMansion/Property/ImportantInteractables/GateControl/CamMonitorFusePlaced");
		var probe = new Bounds(fuse.position, new Vector3(.3f, .3f, .6f));
		foreach (var r in Resources.FindObjectsOfTypeAll<Renderer>().Where(r => r.gameObject.scene.IsValid() && !EditorUtility.IsPersistent(r)))
		{
			if (!r.bounds.Intersects(probe) || r.bounds.size.magnitude > 6f) continue;
			var mf = r.GetComponent<MeshFilter>(); var mc = r.GetComponent<MeshCollider>();
			Debug.Log($"SOCKETR {PathOf(r.transform)} bounds {r.bounds.center} size {r.bounds.size} mesh {(mf && mf.sharedMesh ? mf.sharedMesh.name : "-")} collider mesh {(mc && mc.sharedMesh ? mc.sharedMesh.name : "-")} mats {string.Join(",", r.sharedMaterials.Select(m => m ? m.name : "-"))} active {r.gameObject.activeInHierarchy}");
		}
	}

	// Command line: lists what's around the switch slot, to find the box and its door.
	public static void Survey()
	{
		EditorSceneManager.OpenScene(SceneFile);
		var slot = Resources.FindObjectsOfTypeAll<Transform>().First(t => t.gameObject.scene.IsValid() && PathOf(t) == SlotPath);
		var col = slot.GetComponent<Collider>();
		Debug.Log($"SWITCHBOX slot at {slot.position} rot {slot.eulerAngles} collider {(col ? col.bounds.ToString() : "-")}");
		foreach (var r in Resources.FindObjectsOfTypeAll<MeshFilter>().Where(m => m.gameObject.scene.IsValid() && m.sharedMesh))
		{
			var t = r.transform;
			var b = r.sharedMesh.bounds;
			var centre = t.TransformPoint(b.center);
			float d = Vector3.Distance(centre, slot.position);
			if (d > 1.6f) continue;
			var size = Vector3.Scale(b.size, t.lossyScale);
			Debug.Log($"SWITCHBOX near {d:0.00} m: {PathOf(t)} mesh {r.sharedMesh.name} centre {centre} size {size} rot {t.eulerAngles} active {r.gameObject.activeInHierarchy} collider {(r.GetComponent<Collider>() ? r.GetComponent<Collider>().GetType().Name : "none")}");
		}
	}
}
