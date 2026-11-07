using UnityEditor;
using UnityEngine;

// Editor-only test helper: unlocks every slingshot skin and sets the coin balance.
// Writes the same PlayerPrefs keys the game uses: ToggleOn_CertainColor enables a skin when "Slingshot_<Skin>" == 1,
// the toy capsule shows "Slingshot_<Skin>_Fragments" out of 6, and MainMenu / the shop read the float "Coins".
public static class GrannyCheats
{
	static readonly string[] Skins = { "Default", "Lava", "Ice", "Magenta" };

	[MenuItem("Tools/Evolved Restoration/Cheats/Unlock All Skins + 200 Coins")]
	static void UnlockAllSkinsAndCoins()
	{
		foreach (var skin in Skins)
		{
			PlayerPrefs.SetInt("Slingshot_" + skin, 1);
			if (skin != "Default") PlayerPrefs.SetInt("Slingshot_" + skin + "_Fragments", 6);
		}
		PlayerPrefs.SetFloat("Coins", 200f);
		PlayerPrefs.Save();
		Debug.Log($"[GrannyCheats] Unlocked {Skins.Length} slingshot skins and set coins to 200.");
	}
}
