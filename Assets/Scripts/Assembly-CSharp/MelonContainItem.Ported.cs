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

// Ported from Annotated-Pseudocode/MelonContainItem.c (Granny 3 Evolved, IL2CPP x64).
public partial class MelonContainItem
{
		private void Start() { StartCoroutine(StopPlacedItemLogic()); }
		// Items only count as hidden in the melon when dropped in during the first 6 seconds after it was placed.
		private IEnumerator StopPlacedItemLogic() { yield return new WaitForSeconds(6); PlacedItemIn = true; }
		// An item dropped into the melon is held inside it at that item's spot.
		private void OnTriggerEnter(Collider other)
		{
			if (PlacedItemIn || other.gameObject.layer != LayerMask.NameToLayer("Item")) return;
			PlacedItemIn = true;
			other.gameObject.transform.parent = MelonDropPoint;
			other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			var data = other.gameObject.GetComponent<ItemSeedData>();
			if (!data) return;
			Transform spot;
			switch (data.itemName)
			{
				case "SafeKey": spot = SafeKey; break;
				case "Coin": spot = Coin; break;
				case "PDKey": spot = PDKey; break;
				case "WPKey": spot = WPKey; break;
				case "ShedKey": spot = ShedKey; break;
				case "TrainKey": spot = TrainKey; break;
				case "Valve": spot = Valve; break;
				default: return;
			}
			other.gameObject.transform.position = spot.position;
			other.gameObject.transform.rotation = spot.rotation;
		}
}
