using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// A laser placed with Tools > Evolved Restoration > Hazard Placer. It fires along its blue (forward) axis,
// stops at the first wall or object it hits, and kills the player on touch.
//   Red:    always on, as long as it takes to reach a wall.
//   Pink:   grows out until it reaches the wall, stays out for 1 second, shrinks back in, stays in for 1 second.
//           The timers only start once it has reached the wall (or is fully back in).
//   Purple: flickers: on for OutSeconds, off for InSeconds (1 second each by default).
//   Cyan:   always on, but only as long as Length (it still stops at walls).
// It looks like a real laser: a bright core inside a soft glow, flares where it starts and where it hits, and its width
// slowly swells and shrinks. It also runs in the editor (without killing anyone), so placed lasers can be seen working.
[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class LaserHazard : MonoBehaviour
{
	public enum LaserType { Red, Pink, Purple, Cyan }

	public LaserType Type = LaserType.Red;
	[Tooltip("Cyan only: how far the laser reaches, in metres.")]
	public float Length = 3f;
	[Tooltip("Pink: seconds to wait before it first grows out. Purple: seconds to shift its flicker by. Lets lasers next to each other take turns.")]
	public float CycleOffset;
	[Tooltip("Pink only: how fast it grows out and shrinks back, in metres per second.")]
	public float Speed = 6f;
	[Tooltip("Pink: seconds it stays out once it reaches the wall. Purple: seconds it stays on.")]
	public float OutSeconds = 1f;
	[Tooltip("Pink: seconds it stays fully in before growing out again. Purple: seconds it stays off.")]
	public float InSeconds = 1f;
	[Tooltip("Width of the glowing beam, in metres.")]
	public float Thickness = .09f;

	[Header("Rotating (any colour)")]
	[Tooltip("Sweeps back and forth between two angles around the way it's pointed.")]
	public bool Rotating;
	[Tooltip("How far it swings left and right, in degrees (the whole swing, centred on where it points).")]
	public float HorizontalSweep = 60f;
	[Tooltip("How far it tilts up and down, in degrees (the whole swing, centred on where it points).")]
	public float VerticalSweep;
	[Tooltip("Seconds to swing from one side to the other.")]
	public float SwingSeconds = 2f;
	[Tooltip("Seconds it stays at each side before swinging back.")]
	public float HoldSeconds = 1f;
	[Tooltip("Seconds to shift the swing by, so rotating lasers next to each other can move out of step.")]
	public float SwingOffset;
	[HideInInspector] public float Width = .03f;   // old setting, no longer used

	public const float MaxRange = 100f;
	const float PulseSeconds = 1.6f;  // idle animation: one slow swell and shrink
	const float PulseAmount = .22f;   // how much the width swells (22%)

	LineRenderer glow, core;
	GameObject visuals;   // hidden object holding the beam and flares
	Transform startFlare, endFlare;
	MaterialPropertyBlock block;
	PlayerStatus player;
	float lastTime = -1f;
	float flickerSeed;   // so lasers next to each other don't flicker in step

	static Material beamMat, flareMat;

	public static Color ColourOf(LaserType type)
	{
		switch (type)
		{
			case LaserType.Pink: return new Color(1f, .3f, .75f);
			case LaserType.Purple: return new Color(.6f, .15f, 1f);
			case LaserType.Cyan: return new Color(0f, 1f, 1f);
			default: return new Color(1f, .05f, .02f);
		}
	}

	static Material MakeMat(string texture)
	{
		var shader = Shader.Find("Hazards/LaserAdditive");
		if (!shader) shader = Shader.Find("Sprites/Default");
		return new Material(shader) { mainTexture = Resources.Load<Texture2D>("Hazards/" + texture), hideFlags = HideFlags.HideAndDontSave };
	}

	void OnEnable()
	{
		if (!beamMat) beamMat = MakeMat("LaserBeam");
		if (!flareMat) flareMat = MakeMat("LaserFlare");
		block ??= new MaterialPropertyBlock();

		// The beam and flares live on a separate hidden object, not inside the laser. Inside it, their changing size
		// would drag the editor's move/scale handle around (it centres on everything in the selection).
		var unused = GetComponent<LineRenderer>();   // older lasers drew their beam here
		unused.positionCount = 0;
		unused.enabled = false;
		if (visuals) DestroyVisuals();
		visuals = new GameObject("Laser Visuals (" + name + ")") { hideFlags = VisualFlags };
		if (!Application.isPlaying && gameObject.scene.IsValid() && visuals.scene != gameObject.scene)
			UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(visuals, gameObject.scene);
		glow = MakeChild("Glow", typeof(LineRenderer)).GetComponent<LineRenderer>();
		SetUpLine(glow);
		core = MakeChild("Core", typeof(LineRenderer)).GetComponent<LineRenderer>();
		SetUpLine(core);
		startFlare = MakeFlare("Start Flare");
		endFlare = MakeFlare("End Flare");
		ApplyColour();
		// Pink starts fully in; a cycle offset delays its first grow.
		pinkPhase = PinkPhase.In; pinkLength = 0f; pinkTimer = -Mathf.Max(0f, CycleOffset);
		flickerSeed = Random.value * 100f;
#if UNITY_EDITOR
		if (!Application.isPlaying) EditorApplication.update += EditorTick;
#endif
	}

	void OnDisable()
	{
#if UNITY_EDITOR
		EditorApplication.update -= EditorTick;
#endif
		DestroyVisuals();
	}

	// In the editor the beam and flares are hidden and never saved in the map; in the game they're ordinary hidden objects.
	static HideFlags VisualFlags => Application.isPlaying ? HideFlags.HideInHierarchy : HideFlags.HideAndDontSave;

	// The beam and flares are rebuilt each time, never saved in the map.
	void DestroyVisuals()
	{
		if (!visuals) return;
		if (Application.isPlaying) Destroy(visuals); else DestroyImmediate(visuals);
		visuals = null;
	}

	GameObject MakeChild(string name, params System.Type[] components)
	{
		var g = new GameObject(name, components) { hideFlags = VisualFlags };
		g.transform.SetParent(visuals.transform, false);
		return g;
	}

	Transform MakeFlare(string name)
	{
		var g = MakeChild(name, typeof(MeshFilter), typeof(MeshRenderer), typeof(LaserFlareBillboard));
		g.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
		var r = g.GetComponent<MeshRenderer>();
		r.sharedMaterial = flareMat;
		r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		r.receiveShadows = false;
		return g.transform;
	}

	void SetUpLine(LineRenderer line)
	{
		line.useWorldSpace = true;
		line.positionCount = 2;
		line.numCapVertices = 4;
		line.alignment = LineAlignment.View;
		line.textureMode = LineTextureMode.Stretch;
		line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		line.receiveShadows = false;
		line.sharedMaterial = beamMat;
	}

	public void ApplyColour()
	{
		if (!glow || !core) return;
		Color c = ColourOf(Type);
		glow.startColor = glow.endColor = c;
		core.startColor = core.endColor = Color.Lerp(c, Color.white, .65f);   // white-hot middle
		foreach (var flare in new[] { startFlare, endFlare })
		{
			if (!flare) continue;
			var r = flare.GetComponent<MeshRenderer>();
			r.GetPropertyBlock(block);
			block.SetColor("_Color", c);
			r.SetPropertyBlock(block);
		}
	}

	// The beam's full reach before walls: cyan uses Length, the rest go on until something stops them.
	public float Reach => Type == LaserType.Cyan ? Mathf.Max(0f, Length) : MaxRange;

	// Where the beam would stop (wall, object or the player), up to Reach. Triggers (like creaking floors) don't stop it.
	public float DistanceToHit(out RaycastHit hit, out bool hitSomething)
	{
		// Player-only walls don't stop lasers.
		hitSomething = Physics.Raycast(transform.position, Direction, out hit, Reach, ~PlayerOnlyWall.Mask, QueryTriggerInteraction.Ignore);
		return hitSomething ? hit.distance : Reach;
	}

	static float Now
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return (float)EditorApplication.timeSinceStartup;
#endif
			return Time.time;
		}
	}

	void Update() { if (Application.isPlaying) Tick(); }

#if UNITY_EDITOR
	void EditorTick()
	{
		if (!this || Application.isPlaying) return;
		Tick();
		SceneView.RepaintAll();
	}
#endif

	// Pink: grows out at Speed until it reaches the wall, stays there for OutSeconds, shrinks back in at Speed, then stays
	// in for InSeconds. Each timer only starts once the beam has fully reached the wall (or is fully back in).
	enum PinkPhase { In, Growing, Out, Shrinking }
	PinkPhase pinkPhase;
	float pinkLength, pinkTimer;
	bool pinkTouching;   // the shrinking beam reached whatever is in its way this frame

	float PinkLength(float distance, float dt)
	{
		float speed = Mathf.Max(.1f, Speed);
		pinkTouching = false;
		switch (pinkPhase)
		{
			case PinkPhase.In:
				pinkLength = 0f;
				pinkTimer += dt;
				if (pinkTimer >= InSeconds) { pinkPhase = PinkPhase.Growing; pinkTimer = 0f; }
				break;
			case PinkPhase.Growing:
				pinkLength = Mathf.MoveTowards(pinkLength, distance, speed * dt);
				if (pinkLength >= distance - .001f) { pinkPhase = PinkPhase.Out; pinkTimer = 0f; }
				break;
			case PinkPhase.Out:
				pinkLength = distance;   // follows the wall if something moves in the way
				pinkTimer += dt;
				if (pinkTimer >= OutSeconds) { pinkPhase = PinkPhase.Shrinking; pinkTimer = 0f; }
				break;
			case PinkPhase.Shrinking:
				// Whatever is in the beam's way this frame is touched before the beam pulls back from it.
				float reach = Mathf.Min(pinkLength, distance);
				pinkTouching = reach >= distance - .001f;
				pinkLength = Mathf.MoveTowards(reach, 0f, speed * dt);
				if (pinkLength <= .001f) { pinkPhase = PinkPhase.In; pinkTimer = 0f; }
				break;
		}
		return Mathf.Min(pinkLength, distance);
	}

	// ---------- rotating ----------

	Quaternion sweep = Quaternion.identity;   // the current swing, relative to where the laser points
	Vector3 emitterRest;                       // the housing's place when not swung
	bool emitterRestKnown;

	// Where the beam goes right now: the laser's blue arrow, turned by the current swing.
	public Vector3 Direction => transform.rotation * sweep * Vector3.forward;

	// 0 at one side of the swing, 1 at the other: hold, swing across, hold, swing back.
	float SwingPosition(float now)
	{
		float hold = Mathf.Max(0f, HoldSeconds), swing = Mathf.Max(.05f, SwingSeconds);
		float t = Mathf.Repeat(now + SwingOffset, 2f * (hold + swing));
		if (t < hold) return 0f;
		t -= hold;
		if (t < swing) return Mathf.SmoothStep(0f, 1f, t / swing);
		t -= swing;
		if (t < hold) return 1f;
		t -= hold;
		return Mathf.SmoothStep(1f, 0f, t / swing);
	}

	void UpdateSweep(float now)
	{
		if (!Rotating) { sweep = Quaternion.identity; }
		else
		{
			float s = SwingPosition(now) * 2f - 1f;   // -1 .. 1
			sweep = Quaternion.Euler(-s * VerticalSweep * .5f, s * HorizontalSweep * .5f, 0f);
		}
		// The housing turns with the beam, around the point the beam starts from.
		var emitter = transform.Find("Emitter");
		if (!emitter) return;
		// It always sits straight behind the beam's start, so its rest place follows from its distance (even if the map
		// was saved mid-swing).
		if (!emitterRestKnown) { emitterRest = new Vector3(0f, 0f, -emitter.localPosition.magnitude); emitterRestKnown = true; }
		var pos = sweep * emitterRest;
		if ((emitter.localPosition - pos).sqrMagnitude > 1e-8f) emitter.localPosition = pos;
		if (Quaternion.Angle(emitter.localRotation, sweep) > .01f) emitter.localRotation = sweep;
	}

	void Tick()
	{
		if (!glow || !core) return;
		float now = Now;
		UpdateSweep(now);
		float dt = lastTime < 0f ? 0f : Mathf.Clamp(now - lastTime, 0f, .1f);
		lastTime = now;
		// Purple's flicker: on for OutSeconds, then off for InSeconds.
		float onTime = Mathf.Max(0f, OutSeconds), offTime = Mathf.Max(0f, InSeconds);
		float cycle = onTime + offTime > 0f ? Mathf.Repeat(now + CycleOffset, onTime + offTime) : 0f;
		float distance = DistanceToHit(out RaycastHit hit, out bool hitSomething);

		// How much of the beam is out right now.
		float length;
		switch (Type)
		{
			case LaserType.Pink:
				length = PinkLength(distance, dt);
				break;
			case LaserType.Purple:
				length = cycle < onTime || offTime <= 0f ? distance : 0f;
				break;
			default:
				length = distance;
				break;
		}
		bool on = length > .001f;

		// Idle animation: the width slowly swells and shrinks, with a faint flicker.
		float pulse = 1f + PulseAmount * Mathf.Sin(now * Mathf.PI * 2f / PulseSeconds + CycleOffset)
			+ .06f * (Mathf.PerlinNoise(now * 9f, flickerSeed) - .5f);
		float width = Mathf.Max(.01f, Thickness) * pulse;

		Vector3 start = transform.position, end = start + Direction * length;
		foreach (var line in new[] { glow, core })
		{
			line.enabled = on;
			line.SetPosition(0, start);
			line.SetPosition(1, end);
		}
		glow.startWidth = glow.endWidth = width * 2.6f;
		core.startWidth = core.endWidth = width * .55f;
#if UNITY_EDITOR
		// Scene view only: far away the beam is thinner than a pixel, which Unity draws as broken, jagged dashes.
		// Keep each end at least a couple of pixels wide for the Scene view camera. The game is unchanged.
		if (!Application.isPlaying && SceneView.lastActiveSceneView && SceneView.lastActiveSceneView.camera)
		{
			var cam = SceneView.lastActiveSceneView.camera;
			float pixel = 2f * Mathf.Tan(cam.fieldOfView * .5f * Mathf.Deg2Rad) / Mathf.Max(1, cam.pixelHeight);   // metres per pixel at 1 m
			float near = Vector3.Distance(cam.transform.position, start) * pixel, far = Vector3.Distance(cam.transform.position, end) * pixel;
			glow.startWidth = Mathf.Max(glow.startWidth, near * 4f); glow.endWidth = Mathf.Max(glow.endWidth, far * 4f);
			core.startWidth = Mathf.Max(core.startWidth, near * 1.5f); core.endWidth = Mathf.Max(core.endWidth, far * 1.5f);
		}
#endif

		startFlare.gameObject.SetActive(on);
		startFlare.position = start;
		startFlare.localScale = Vector3.one * width * 5f;
		// A glow where it hits a wall (not at the end of a cyan beam that stops in mid-air).
		bool hitsHere = hitSomething && ((on && hit.distance <= length + .01f) || (Type == LaserType.Pink && pinkTouching));
		endFlare.gameObject.SetActive(hitsHere);
		endFlare.position = end - Direction * .01f;
		endFlare.localScale = Vector3.one * width * (6f + 1.5f * Mathf.Sin(now * 23f));

		// Only kills in the game, and only with the part of the beam that's out.
		if (!Application.isPlaying || !hitsHere || !IsPlayer(hit.collider)) return;
		if (!player) player = FindAnyObjectByType<PlayerStatus>();
		if (player && !player.Killed && player.Player && player.Player.activeSelf) player.NormalDeath(true);
	}

	static bool IsPlayer(Collider c)
	{
		for (Transform t = c.transform; t; t = t.parent) if (t.CompareTag("Player")) return true;
		return false;
	}

	void OnValidate() => ApplyColour();
}
