using UnityEngine;

// The blue fuse lying in the map; switches itself off (and SeedManager leaves it out) outside Slientlo Never Clear.
public class BlueFuseItem : MonoBehaviour
{
	void Awake() { if (!BlueFuse.InThisRun) gameObject.SetActive(false); }
}
