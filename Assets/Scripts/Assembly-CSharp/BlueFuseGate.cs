using System.Collections.Generic;
using UnityEngine;

// On the gate controls' button. On Slientlo Never Clear the button only opens the gate once the generator is running
// (the game's own condition), the blue fuse is in its socket and both escape levers are pulled; otherwise it does what it does without power.
public class BlueFuseGate : MonoBehaviour
{
	public BlueFuseSlot Slot;

	readonly List<(RegPuzzle puzzle, GameObject needed)> wrapped = new List<(RegPuzzle, GameObject)>();
	GameObject bothReady;

	void Awake()
	{
		if (!BlueFuse.InThisRun) return;
		// The button's puzzles check that one object is switched on; give them one that's only on when both are true.
		bothReady = new GameObject("PowerAndBlueFuse");
		bothReady.transform.SetParent(transform, false);
		bothReady.SetActive(false);
		foreach (var puzzle in GetComponents<RegPuzzle>())
		{
			if (!puzzle.requiredItem) continue;
			wrapped.Add((puzzle, puzzle.requiredItem));
			puzzle.requiredItem = bothReady;
		}
	}

	void Update()
	{
		if (!bothReady || wrapped.Count == 0) return;
		bool powered = wrapped[0].needed && wrapped[0].needed.activeSelf;
		bool ready = powered && Slot && Slot.IsPlaced && EscapeLevers.Ready;   // and both escape levers pulled
		if (bothReady.activeSelf != ready) bothReady.SetActive(ready);
	}
}
