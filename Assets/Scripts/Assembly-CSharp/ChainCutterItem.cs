using UnityEngine;

// The chain cutter lying in the map. It's only in runs that have the balloon chains (Slientlo Never Clear, game version 1.2);
// otherwise it switches itself off, and SeedManager leaves it out.
public class ChainCutterItem : MonoBehaviour
{
	public const string ItemName = "ChainCutter";

	void Awake()
	{
		if (!BalloonChains.InThisRun) gameObject.SetActive(false);
	}
}
