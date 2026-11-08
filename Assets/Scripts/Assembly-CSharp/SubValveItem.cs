using UnityEngine;

// Slientlo Never Clear (with Extras, where the submarine is): a green valve knob has to be put on the submarine hatch and
// turned before the handwheel will open it. Set up by Tools > Evolved Restoration > Set Up Submarine Valve.
// This is the valve lying in the map; it switches itself off (and SeedManager leaves it out) in other runs.
public class SubValveItem : MonoBehaviour
{
	public const string ItemName = "SubValve";
	public static bool InThisRun => DifficultyLevel.IsSlientlo && PlayerPrefs.GetInt("Extras") == 1;

	void Awake() { if (!InThisRun) gameObject.SetActive(false); }
}
