using UnityEngine;

// Slientlo Never Clear: two of the train switch box's three levers are out in the map (at spots you move in Unity; each
// draws the lever there), and every escape needs both of them pulled. Two lights above the front door go from red to green
// as each is pulled. In the switch box, the two empty places say the lever is somewhere else.
// The moved levers look like wall levers (a base plate and a stick with a knob that swings up when pulled) and don't need
// the generator, unlike the lever left in the box (Slientlo uses Impossible's rules for that).
// Set up by Tools > Evolved Restoration > Set Up Escape Levers; on other difficulties everything is as the original game.
public class EscapeLevers : MonoBehaviour
{
	public static EscapeLevers Instance;

	[Tooltip("The two levers' parents in the switch box (they're moved to the spots at the start of a Slientlo run).")]
	public Transform[] Levers = new Transform[2];
	[Tooltip("Where each lever goes in the map.")]
	public Transform[] Spots = new Transform[2];
	[Tooltip("Click spots left in the switch box where the levers were.")]
	public GameObject[] EmptySlots = new GameObject[2];
	[Tooltip("The two lights above the front door, in the same order as the levers.")]
	public Renderer[] Lights = new Renderer[2];
	public Material Red, Green;
	[Tooltip("Gives the material for each wall lever's base and stick.")]
	public GameObject PlateTemplate;

	const float Down = -40f, Up = 40f;

	EscapeRouteChecks routes;
	readonly WallLever[] wall = new WallLever[2];

	// True when every escape may go ahead: always outside Slientlo, otherwise once both levers are pulled.
	public static bool Ready
	{
		get
		{
			if (!DifficultyLevel.IsSlientlo || !Instance) return true;
			return Instance.Pulled(0) && Instance.Pulled(1);
		}
	}

	public bool Pulled(int i)
	{
		if (!routes) routes = FindAnyObjectByType<EscapeRouteChecks>();
		if (!routes) return false;
		return i == 0 ? routes.FlippedSwitch1 : routes.FlippedSwitch2;
	}

	void Awake()
	{
		if (!DifficultyLevel.IsSlientlo)
		{
			foreach (var s in EmptySlots) if (s) s.SetActive(false);
			gameObject.SetActive(false);
			return;
		}
		Instance = this;
		var metal = PlateTemplate ? PlateTemplate.GetComponent<Renderer>().sharedMaterial : null;
		for (int i = 0; i < 2; i++)
		{
			if (Levers[i] && Spots[i])
			{
				PlaceAt(Levers[i], Spots[i]);
				wall[i] = BuildWallLever(Levers[i], metal, Red);
			}
			if (EmptySlots[i]) EmptySlots[i].SetActive(true);
		}
	}

	void OnDestroy() { if (Instance == this) Instance = null; }

	// A spot is where the lever itself goes. Its parent sits far from the lever, so the parent is moved and turned so that
	// the lever lands on the spot (the lever's pull animation is relative to the parent, so it still works).
	public static void PlaceAt(Transform parent, Transform spot)
	{
		var model = parent.Find("ElectricalLeverOff");
		var offset = model ? parent.InverseTransformPoint(model.position) : Vector3.zero;
		parent.rotation = spot.rotation;
		parent.position = spot.position - parent.TransformVector(offset);
	}

	public class WallLever
	{
		public Transform Handle;
		public Renderer Knob;
		public float Angle = Down;
	}

	// Hides the switch box lever model and builds a wall lever in its place: in the parent's space +X goes into the wall and
	// +Y is up. The model keeps its collider (made to cover the new lever), click handlers, sound and sparks.
	public static WallLever BuildWallLever(Transform parent, Material metal, Material knobMat)
	{
		var model = parent.Find("ElectricalLeverOff");
		if (!model) return null;
		var modelRenderer = model.GetComponent<Renderer>();
		var mb = modelRenderer.bounds;
		float back = float.MinValue;
		for (int k = 0; k < 8; k++)
		{
			var c = new Vector3(k % 2 == 0 ? mb.min.x : mb.max.x, (k / 2) % 2 == 0 ? mb.min.y : mb.max.y, k / 4 == 0 ? mb.min.z : mb.max.z);
			back = Mathf.Max(back, parent.InverseTransformPoint(c).x);
		}
		modelRenderer.enabled = false;

		var pivot = parent.InverseTransformPoint(model.position);
		float wall = back + .03f; // where the wall is (StickToWall leaves this gap)
		var root = new GameObject("Wall Lever").transform;
		root.SetParent(parent, false);
		root.localPosition = new Vector3(wall, pivot.y, pivot.z);
		root.localRotation = Quaternion.identity;
		var ps = parent.lossyScale;
		root.localScale = new Vector3(1f / ps.x, 1f / ps.y, 1f / ps.z);

		// Everything below is in metres from the wall, -X coming out of it.
		Part(PrimitiveType.Cube, root, new Vector3(-.02f, 0f, 0f), Quaternion.identity, new Vector3(.04f, .32f, .18f), metal);
		Part(PrimitiveType.Cube, root, new Vector3(-.05f, 0f, 0f), Quaternion.identity, new Vector3(.03f, .1f, .1f), metal);
		var handle = new GameObject("Handle").transform;
		handle.SetParent(root, false);
		handle.localPosition = new Vector3(-.065f, 0f, 0f);
		Part(PrimitiveType.Cylinder, handle, new Vector3(0f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), new Vector3(.035f, .06f, .035f), metal);
		Part(PrimitiveType.Cylinder, handle, new Vector3(-.11f, 0f, 0f), Quaternion.Euler(0f, 0f, 90f), new Vector3(.024f, .11f, .024f), metal);
		var knob = Part(PrimitiveType.Sphere, handle, new Vector3(-.23f, 0f, 0f), Quaternion.identity, Vector3.one * .07f, knobMat);

		var lever = new WallLever { Handle = handle, Knob = knob.GetComponent<Renderer>() };
		SetAngle(lever, Down);

		// The click box covers the whole lever (in the model's own space, which the pull animation then turns).
		var box = model.GetComponent<BoxCollider>();
		if (box)
		{
			var b = new Bounds(model.InverseTransformPoint(root.position), Vector3.zero);
			foreach (var r in root.GetComponentsInChildren<Renderer>())
				for (int k = 0; k < 8; k++)
				{
					var rb = r.bounds;
					b.Encapsulate(model.InverseTransformPoint(new Vector3(k % 2 == 0 ? rb.min.x : rb.max.x, (k / 2) % 2 == 0 ? rb.min.y : rb.max.y, k / 4 == 0 ? rb.min.z : rb.max.z)));
				}
			box.center = b.center;
			box.size = b.size;
		}
		return lever;
	}

	static GameObject Part(PrimitiveType type, Transform parent, Vector3 pos, Quaternion rot, Vector3 scale, Material mat)
	{
		var go = GameObject.CreatePrimitive(type);
		var col = go.GetComponent<Collider>();
		if (Application.isPlaying) { col.enabled = false; Destroy(col); } else DestroyImmediate(col);
		go.name = type.ToString();
		go.transform.SetParent(parent, false);
		go.transform.localPosition = pos;
		go.transform.localRotation = rot;
		go.transform.localScale = scale;
		var r = go.GetComponent<Renderer>();
		if (mat) r.sharedMaterial = mat;
		r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		return go;
	}

	// Angle of the stick: negative points down, positive up.
	public static void SetAngle(WallLever lever, float angle)
	{
		lever.Angle = angle;
		lever.Handle.localRotation = Quaternion.Euler(0f, 0f, -angle);
	}

	void Update()
	{
		for (int i = 0; i < 2; i++)
		{
			bool pulled = Pulled(i);
			if (Lights[i])
			{
				var want = pulled ? Green : Red;
				if (want && Lights[i].sharedMaterial != want) Lights[i].sharedMaterial = want;
			}
			var w = wall[i];
			if (w == null) continue;
			if (pulled)
			{
				if (w.Angle < Up) SetAngle(w, Mathf.MoveTowards(w.Angle, Up, 260f * Time.deltaTime));
				if (Green && w.Knob.sharedMaterial != Green) w.Knob.sharedMaterial = Green;
			}
			else if (Levers[i])
			{
				// No generator needed for these two: use the plain click handler, not the "needs the generator" one.
				var model = Levers[i].Find("ElectricalLeverOff");
				var anim = model ? model.GetComponent<Animation>() : null;
				if (model && !(anim && anim.isPlaying)) // not mid-pull (the click turns itself off until the pull counts)
				{
					var puzzle = model.GetComponent<RegPuzzle>();
					if (puzzle && puzzle.enabled) puzzle.enabled = false;
					var click = model.GetComponent<Interactive>();
					if (click && !click.enabled) click.enabled = true;
				}
			}
		}
	}
}
