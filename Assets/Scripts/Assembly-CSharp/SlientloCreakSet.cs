using UnityEngine;

// Sits on the "Creak Floors - Slientlo Never Clear" group made by the Hazard Placer.
// On Slientlo Never Clear this group's floors replace the game's own creaking floors, like Legacy's per-difficulty sets.
public class SlientloCreakSet : MonoBehaviour
{
	void Start()
	{
		if (!DifficultyLevel.IsSlientlo) return;
		foreach (var creak in FindObjectsByType<TriggerCreakNoise>(FindObjectsInactive.Include))
			if (!creak.transform.IsChildOf(transform)) creak.gameObject.SetActive(false);
	}
}
