using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/ColorToyCapsule.c (Granny 3 Evolved, IL2CPP x64).
public partial class ColorToyCapsule
{
		private void Start()
		{
			tiers = new Tier[] {
				new Tier { name = "Magenta", chance = 1f, items = new[] { "Magenta" } },
				new Tier { name = "Ice", chance = 3f, items = new[] { "Ice" } },
				new Tier { name = "Lava", chance = 6f, items = new[] { "Lava" } } };
			UpdateRarityUI();
			UpdateFragmentTexts();
		}
		private void Update() { slingshotOBJDisplay.SetActive(menuInventory.activeSelf); }
		private void UpdateFragmentTexts()
		{
			LavaFragmentsText.text = PlayerPrefs.GetInt("Slingshot_Lava_Fragments", 0).ToString() + "/6";
			IceFragmentsText.text = PlayerPrefs.GetInt("Slingshot_Ice_Fragments", 0).ToString() + "/6";
			MagentaFragmentsText.text = PlayerPrefs.GetInt("Slingshot_Magenta_Fragments", 0).ToString() + "/6";
		}
		// "Color Rarity" list with each tier's current percentage.
		public void UpdateRarityUI()
		{
			float total = ActiveChance();
			string text = "Color Rarity :\n";
			foreach (var tier in tiers)
			{
				float pct = AdjustedChance(tier) / total * 100f;
				text = text + "- " + tier.name + " (" + pct.ToString("F1") + "%)\n";
			}
			ColorRarityText.text = text;
		}
		// One coin per capsule.
		public void AttemptInsertCoin()
		{
			if (AnimatedToyCapsule.isPlaying) return;
			float coins = PlayerPrefs.GetFloat("Coins", 0);
			if (coins >= 1f)
			{
				PlayerPrefs.SetFloat("Coins", coins - 1f);
				InvB.SetActive(false);
				BackB.SetActive(false);
				InsertCoin.SetActive(false);
				AnimatedToyCapsule.Play("TakeOutBall");
				UnlockCertainSkin();
				StartCoroutine(TimerToyCapsule());
			}
			else if (TextNotEnough != null)
			{
				var text = TextNotEnough.GetComponent<TextManager>();
				if (text == null) return;
				text.Add();
				TextNotEnough.SetActive(true);
			}
		}
		public bool IsTierFullyUnlocked(Tier tier)
		{
			foreach (var item in tier.items) if (PlayerPrefs.GetInt("Slingshot_" + item) == 0) return false;
			return true;
		}
		// Fully unlocked tiers are ten times less likely.
		public float AdjustedChance(Tier tier) { return IsTierFullyUnlocked(tier) ? tier.chance * .1f : tier.chance; }
		public float ActiveChance()
		{
			float sum = 0;
			foreach (var tier in tiers) sum += AdjustedChance(tier);
			return sum;
		}
		// Each capsule is a fragment of a weighted-random colour; 6 fragments unlock the skin.
		public void UnlockCertainSkin()
		{
			GiveCoinBack = false;
			float roll = UnityEngine.Random.Range(0f, ActiveChance());
			float cumulative = 0;
			foreach (var tier in tiers)
			{
				cumulative += AdjustedChance(tier);
				if (roll > cumulative) continue;
				if (tier.items == null) return;
				string item = tier.items[0];
				string key = "Slingshot_" + item;
				string fragmentsKey = key + "_Fragments";
				ApplySlingshotSkin(item);
				if (PlayerPrefs.GetInt(key, 0) == 1)
				{
					GiveCoinBack = true;
					ItemText.text = "> " + tier.name + " Duplicate! <";
				}
				else
				{
					int fragments = PlayerPrefs.GetInt(fragmentsKey, 0) + 1;
					if (fragments < 6) ItemText.text = "> " + tier.name + " Fragment " + fragments.ToString() + "/6 <";
					else
					{
						fragments = 6;
						PlayerPrefs.SetInt(key, 1);
						ItemText.text = "> " + tier.name + " Skin Unlocked! <";
					}
					PlayerPrefs.SetInt(fragmentsKey, fragments);
					PlayerPrefs.Save();
				}
				UpdateFragmentTexts();
				gamejoltMenuCode.SkinToCloud();
				return;
			}
		}
		public void ApplySlingshotSkin(string item)
		{
			slingshotFragment.SetActive(true);
			Texture2D texture = item == "Lava" ? Slingshot_Lava : item == "Ice" ? Slingshot_Ice : item == "Magenta" ? Slingshot_Magenta : null;
			if (texture != null) fragmentRenderer.material.mainTexture = texture;
		}
		private IEnumerator TimerToyCapsule()
		{
			yield return new WaitForSeconds(7f);
			InvB.SetActive(true);
			BackB.SetActive(true);
			InsertCoin.SetActive(true);
			slingshotFragment.SetActive(false);
			ItemText.gameObject.SetActive(false);
			UpdateFragmentTexts();
			UpdateRarityUI();
		}
}
