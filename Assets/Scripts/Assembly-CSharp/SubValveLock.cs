using System.Collections;
using UnityEngine;

// On the click spot on the submarine hatch. First click (green valve in hand): the valve goes on the hatch.
// Second click: it turns. Until it has turned, the hatch's handwheel slot does nothing.
public class SubValveLock : MonoBehaviour
{
	public GameObject Hand;
	public GameObject Placed;
	public PickRay Pick;
	public RegPuzzle Puzzle;
	[Tooltip("The hatch's handwheel slot, held back until the valve has been turned.")]
	public RegPuzzle WheelPuzzle;
	public AudioSource TurnSound;
	public float TurnSeconds = 1.4f;
	public float Turns = 2f;
	[Tooltip("The knob sits at this place relative to this click spot, so moving or turning the spot in Unity moves the knob too.")]
	public bool KnobFollowsSpot;
	public Vector3 KnobLocalPosition;
	public Quaternion KnobLocalRotation = Quaternion.identity;
	[Tooltip("Shown when the hatch's valve stem is clicked without the knob.")]
	public string MissingText = "The valve knob is missing. Find it somewhere in the map";

	public bool IsPlaced { get; private set; }
	public bool Turned { get; private set; }

	GameObject wheelNeeded, ready;

	void Awake()
	{
		if (!SubValveItem.InThisRun) { gameObject.SetActive(false); if (Placed) Placed.SetActive(false); return; }
		if (Placed) Placed.SetActive(false);
		PlaceKnob();
		BuildStem();
		// Clicking the bare stem without the knob says what's missing.
		if (Puzzle && Pick)
		{
			if (Puzzle.performedEventNo == null) Puzzle.performedEventNo = new UnityEngine.Events.UnityEvent();
			Puzzle.performedEventNo.AddListener(() => { if (!IsPlaced) Pick.TimedTextInitiate(MissingText); });
		}
		// The handwheel slot checks that its item is in hand; give it an object that's only on once the valve has turned.
		if (WheelPuzzle && WheelPuzzle.requiredItem)
		{
			wheelNeeded = WheelPuzzle.requiredItem;
			ready = new GameObject("ValveTurnedAndHandwheel");
			ready.transform.SetParent(transform, false);
			ready.SetActive(false);
			WheelPuzzle.requiredItem = ready;
		}
	}

	// A bare metal stem on a round plate where the knob goes, so the hatch shows something is missing. It stays under the
	// knob once that's on. Part of the door, so it swings with it.
	public void BuildStem()
	{
		if (!Placed) return;
		// The middle of the knob's shape (PlaceKnob has already put it where it goes).
		var shape = Placed.GetComponent<MeshFilter>() ? Placed.GetComponent<MeshFilter>().sharedMesh : null;
		var knobAt = shape ? Placed.transform.TransformPoint(shape.bounds.center) : Placed.transform.position;
		var outward = transform.forward;
		var surface = transform.position - outward * .12f; // the spot is 12 cm out from the door
		float length = Mathf.Max(.1f, Vector3.Dot(knobAt - surface, outward) + .02f);
		var metal = WheelPuzzle && WheelPuzzle.GetComponentInChildren<Renderer>() ? WheelPuzzle.GetComponentInChildren<Renderer>().sharedMaterial : null;
		var basePoint = knobAt - outward * Vector3.Dot(knobAt - surface, outward);
		Part("Valve Stem Plate", basePoint + outward * .012f, .17f, .024f, outward, metal);
		Part("Valve Stem", basePoint + outward * (length / 2f), .045f, length, outward, metal);
		Part("Valve Stem Tip", basePoint + outward * length, .07f, .02f, outward, metal);
	}

	// Parts go under this spot: it's part of the door (so they swing with it) and its scale cancels the door's, so sizes
	// here are in metres. A unit cylinder is 2 high along its Y, which is turned to point out of the door.
	void Part(string name, Vector3 centre, float width, float length, Vector3 along, Material mat)
	{
		var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		go.name = name;
		var col = go.GetComponent<Collider>(); if (Application.isPlaying) { col.enabled = false; Destroy(col); } else DestroyImmediate(col);
		go.transform.SetParent(transform, false);
		go.transform.position = centre;
		go.transform.rotation = Quaternion.FromToRotation(Vector3.up, along);
		go.transform.localScale = new Vector3(width, length / 2f, width);
		var r = go.GetComponent<Renderer>();
		if (mat) r.sharedMaterial = mat;
	}

	public void PlaceKnob()
	{
		if (KnobFollowsSpot && Placed)
			Placed.transform.SetPositionAndRotation(transform.TransformPoint(KnobLocalPosition), transform.rotation * KnobLocalRotation);
	}

	void Update()
	{
		if (!ready) return;
		bool ok = Turned && wheelNeeded && wheelNeeded.activeSelf;
		if (ready.activeSelf != ok) ready.SetActive(ok);
	}

	// Called by the click spot's RegPuzzle: puts the valve on, or turns it once it's on.
	public void Use()
	{
		if (!IsPlaced)
		{
			IsPlaced = true;
			if (Pick) Pick.OffHoldingItemLogic();
			if (Hand) Hand.SetActive(false);
			if (Placed) Placed.SetActive(true);
			// Next click turns it: no item needed any more.
			if (Puzzle) { Puzzle.requiredItem = gameObject; Puzzle.performedAction = false; }
			return;
		}
		if (Turned) return;
		StartCoroutine(TurnValve());
	}

	IEnumerator TurnValve()
	{
		if (Puzzle) Puzzle.enabled = false;
		if (TurnSound) TurnSound.Play();
		var t = Placed.transform;
		var start = t.localRotation;
		var axis = Placed.GetComponent<SubValveAxis>();
		Vector3 local = axis ? axis.LocalAxis : Vector3.up;
		for (float time = 0f; time < TurnSeconds; time += Time.deltaTime)
		{
			float a = Mathf.SmoothStep(0f, 1f, time / TurnSeconds) * 360f * Turns;
			t.localRotation = start * Quaternion.AngleAxis(a, local);
			yield return null;
		}
		t.localRotation = start;
		Turned = true;
		var col = GetComponent<Collider>();
		if (col) col.enabled = false;
	}
}
