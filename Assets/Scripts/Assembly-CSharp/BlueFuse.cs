using UnityEngine;

// Slientlo Never Clear: the gate needs a second, blue gate fuse in its own socket (set up by
// Tools > Evolved Restoration > Set Up Blue Fuse). On other difficulties none of this is in the game.
public static class BlueFuse
{
	public const string ItemName = "BlueFuse";
	public static bool InThisRun => DifficultyLevel.IsSlientlo;
}
