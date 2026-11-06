using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ShopProduct.c (Granny 3 Evolved, IL2CPP x64).
public partial class ShopProduct
{
		private void Update()
		{
			if (PlayerPrefs.GetInt(NameProduct) != 1) return;
			if (OptionPurchased != null) OptionPurchased.SetActive(true);
			PurchasedText.SetActive(true);
		}
		public void AttemptBuyProduct()
		{
			float coins = PlayerPrefs.GetFloat("Coins", 0);
			if (Price <= coins)
			{
				PlayerPrefs.SetFloat("Coins", coins - Price);
				PlayerPrefs.SetInt(NameProduct, 1);
				BuySound.Play();
			}
			else if (TextNotEnough != null)
			{
				var text = TextNotEnough.GetComponent<TextManager>();
				if (text == null) return;
				text.TimeF = text.ActualTextAmount;   // restart the "not enough coins" message timer
				TextNotEnough.SetActive(true);
			}
		}
}
