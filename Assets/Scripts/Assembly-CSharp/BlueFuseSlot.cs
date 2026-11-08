using UnityEngine;

// The click spot in the blue fuse socket. Its RegPuzzle needs the blue fuse in hand and calls Place: the fuse leaves the
// hand and appears in the socket, like the game's own gate fuse.
public class BlueFuseSlot : MonoBehaviour
{
	public GameObject Hand;
	public GameObject Placed;
	public PickRay Pick;
	public RegPuzzle Puzzle;

	public bool IsPlaced => Placed && Placed.activeSelf;

	void Awake() { if (!BlueFuse.InThisRun) gameObject.SetActive(false); }

	public void Place()
	{
		if (Pick) Pick.OffHoldingItemLogic();
		if (Hand) Hand.SetActive(false);
		if (Placed) Placed.SetActive(true);
		if (Puzzle) Puzzle.enabled = false;
		var col = GetComponent<Collider>();
		if (col) col.enabled = false;
		var sound = GetComponent<AudioSource>();
		if (sound) sound.Play();
	}
}
