using UnityEngine;

// Added hazards (creaking floors and lasers placed with Tools > Evolved Restoration > Hazard Placer)
// are used on Slientlo Never Clear only; on other difficulties they switch themselves off.
// Creaking floors are see-through orange boxes in the editor and invisible in the game.
public class HazardDifficulty : MonoBehaviour
{
	void Awake()
	{
		if (!DifficultyLevel.IsSlientlo) { gameObject.SetActive(false); return; }
		if (GetComponent<TriggerCreakNoise>() && TryGetComponent(out MeshRenderer box)) box.enabled = false;
	}
}
