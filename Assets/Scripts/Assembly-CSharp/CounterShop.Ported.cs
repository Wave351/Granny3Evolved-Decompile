using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/CounterShop.c (Granny 3 Evolved, IL2CPP x64).
public partial class CounterShop
{
		// Two-page shop list; items behind it are hidden while the inventory is open.
		private void Update()
		{
			if (CurrentList == 1f) { ListNumb.text = "1 / 2"; BackB.SetActive(false); NextB.SetActive(true); L1.SetActive(true); L2.SetActive(false); }
			else if (CurrentList == 2f) { ListNumb.text = "2 / 2"; BackB.SetActive(true); NextB.SetActive(false); L1.SetActive(false); L2.SetActive(true); }
			bool open = Inventory.activeSelf;
			if (DisableOnInventory == null) return;
			foreach (var o in DisableOnInventory) o.SetActive(!open);
		}
		public void Back() { CurrentList -= 1f; }
		public void Next() { CurrentList += 1f; }
}
