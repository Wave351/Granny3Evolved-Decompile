using UnityEngine;

// Slientlo Never Clear: chains hold the hot air balloon down until they're cut with the chain cutter (from Granny Legacy).
// Made by Tools > Evolved Restoration > Set Up Chain Cutter. Each chain is cut on its own by clicking it with the cutter in
// hand; the balloon can't be boarded until every chain is cut.
public class BalloonChains : MonoBehaviour
{
	public static BalloonChains Instance;

	[Tooltip("The chains, each hidden once it's cut.")]
	public GameObject[] Chains;
	[Tooltip("The short end left on each chain's ground stake after it's cut (same order as Chains).")]
	public GameObject[] CutEnds;
	public AudioSource CutSound;
	[Tooltip("Metal clank when a cut chain hits the ground.")]
	public AudioClip LandClip;
	[Tooltip("Noise Granny and Grandpa hear when a chain is cut (the game's noise object).")]
	public GameObject NoiseObj;

	bool[] cut;

	// True once every chain is cut.
	public bool IsCut
	{
		get
		{
			if (cut == null) return false;
			foreach (bool c in cut) if (!c) return false;
			return true;
		}
	}

	// True while any chain still stops the balloon from being boarded.
	public static bool Blocking => Instance && Instance.isActiveAndEnabled && !Instance.IsCut;

	// Whether the chain cutter and chains are in this run: Slientlo Never Clear on game version 1.2 (the balloon's version).
	public static bool InThisRun => DifficultyLevel.IsSlientlo && PlayerPrefs.GetInt("CURVERSION") == 2;

	void Awake()
	{
		if (!InThisRun) { gameObject.SetActive(false); return; }
		Instance = this;
		cut = new bool[Chains.Length];
		foreach (var end in CutEnds) if (end) end.SetActive(false);
	}

	void OnDestroy() { if (Instance == this) Instance = null; }

	// Whether this object is one of the chains that hasn't been cut yet.
	public bool IsUncutChain(GameObject chain)
	{
		int i = System.Array.IndexOf(Chains, chain);
		return i >= 0 && !cut[i];
	}

	[Tooltip("Seconds a cut chain lies on the ground before it disappears (the game's DestroyTimer).")]
	public float FallenChainSeconds = 10f;

	// A cut chain comes loose and falls with physics, then disappears like the game's other loose pieces.
	// It stays on its layer, which the player walks through, so it can't block the way while it lies there.
	void Drop(GameObject chain)
	{
		chain.name = "BalloonChain (cut)";   // no longer clickable
		chain.transform.SetParent(null, true);
		var body = chain.AddComponent<Rigidbody>();
		body.mass = 4f;
		body.linearDamping = .3f;
		body.angularDamping = .6f;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		// The basket end drops first and the chain swings down and away from the balloon.
		var away = Vector3.ProjectOnPlane(chain.transform.position - transform.position, Vector3.up).normalized;
		body.AddForce(away * 1.5f + Vector3.down, ForceMode.VelocityChange);
		body.AddTorque(Vector3.Cross(Vector3.up, away) * 2f, ForceMode.VelocityChange);
		var fallen = chain.AddComponent<FallenChain>();
		fallen.LandClip = LandClip;
		fallen.NoiseObj = NoiseObj;
		var timer = chain.AddComponent<DestroyTimer>();
		timer.TimeW = FallenChainSeconds;
		timer.onlyItself = true;
	}

	// Cuts one chain: it falls and later disappears, its end stays on the stake, and the cut is heard.
	public void Cut(GameObject chain)
	{
		int i = System.Array.IndexOf(Chains, chain);
		if (i < 0 || cut[i]) return;
		cut[i] = true;
		Drop(chain);
		if (i < CutEnds.Length && CutEnds[i]) CutEnds[i].SetActive(true);
		if (CutSound && CutSound.clip) AudioSource.PlayClipAtPoint(CutSound.clip, chain.GetComponent<Renderer>().bounds.center, CutSound.volume);
		if (NoiseObj) Instantiate(NoiseObj, chain.transform.position, chain.transform.rotation);
	}
}
