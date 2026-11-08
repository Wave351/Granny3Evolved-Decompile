using UnityEngine;

// On an escape's final control (like the submarine's launch lever). On Slientlo Never Clear its RegPuzzles only go ahead
// once both escape levers are pulled, on top of what they normally need.
public class NeedsEscapeLevers : MonoBehaviour
{
	RegPuzzle[] puzzles;
	GameObject[] needed;
	GameObject ready;

	void Start()
	{
		if (!DifficultyLevel.IsSlientlo) return;
		puzzles = GetComponents<RegPuzzle>();
		needed = new GameObject[puzzles.Length];
		ready = new GameObject("EscapeLeversReady");
		ready.transform.SetParent(transform, false);
		ready.SetActive(false);
		for (int i = 0; i < puzzles.Length; i++)
		{
			needed[i] = puzzles[i].requiredItem;
			puzzles[i].requiredItem = ready;
		}
	}

	void Update()
	{
		if (!ready || needed.Length == 0) return;
		bool ok = EscapeLevers.Ready && needed[0] && needed[0].activeSelf;
		if (ready.activeSelf != ok) ready.SetActive(ok);
	}
}
