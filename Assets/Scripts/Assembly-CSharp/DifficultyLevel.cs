using UnityEngine;

// Added difficulty "Slientlo Never Clear" (Diff = 5). It plays by Impossible's rules,
// but Granny and Grandpa run 65% faster, walk 21% faster, open doors 20% faster, and Slendrina drains you faster.
public static class DifficultyLevel
{
	public const int Impossible = 4;
	public const int Slientlo = 5;
	public const float SlientloSpeedScale = 1.65f;     // running (Impossible +50%, then +10%)
	public const float SlientloWalkSpeedScale = 1.21f; // walking (Impossible +10%, then +10%)
	public const float SlientloShootScale = 1.3f;      // Grandpa's shooting
	public const float SlientloDoorScale = 1.2f;       // opening doors

	public static int Current => PlayerPrefs.GetInt("Diff");
	public static bool IsSlientlo => Current == Slientlo;

	// The difficulty to use for Impossible-style rules: Slientlo counts as Impossible.
	public static int Rules => Current == Slientlo ? Impossible : Current;

	// Multipliers for Granny and Grandpa's running and walking (movement and animation) speed.
	public static float SpeedScale => IsSlientlo ? SlientloSpeedScale : 1f;
	public static float WalkSpeedScale => IsSlientlo ? SlientloWalkSpeedScale : 1f;
	public static float DoorSpeedScale => IsSlientlo ? SlientloDoorScale : 1f;
}
