using UnityEngine;

// The game always runs as version 1.2 with Extras and Nightmare on. The menu shows these locked on (MainMenu); this also
// sets them before the first scene loads, so nothing can read an old value.
public static class ForcedSettings
{
	public static void Apply()
	{
		PlayerPrefs.SetInt("CURVERSION", 2);
		PlayerPrefs.SetInt("Extras", 1);
		PlayerPrefs.SetInt("Textures", 1);
		PlayerPrefs.SetInt("Diff", 5); // Slientlo Never Clear, the only difficulty
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Boot()
	{
		Apply();
		PlayerPrefs.Save();
	}
}
