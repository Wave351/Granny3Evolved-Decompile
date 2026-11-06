using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Inventory.c (Granny 3 Evolved, IL2CPP x64).
public partial class Inventory
{
		public Inventory() { ItemDefs = new List<ItemDefs>(); mainDropSpeed = 5f; }
		// Picking up drops the held item first, then shows the new hand model and plays its pickup sound.
		public void PickupItem(string itemName)
		{
			var def = ItemDefs.Find(d => d.itemName == itemName);
			DropLogic();
			if (def == null) return;
			if (def.handObject) def.handObject.SetActive(true);
			if (Audio && def.pickAudioClip != null) Audio.PlayOneShot(def.pickAudioClip);
		}
		public ItemDefs GetItemDefByName(string name)
		{
			for (int i = 0; i < ItemDefs.Count; i++)
				if (ItemDefs[i].itemName == name) return ItemDefs[i];
			return null;
		}
		// Every item currently in hand is put away and thrown forward as its drop prefab.
		public void DropLogic()
		{
			foreach (var def in ItemDefs)
			{
				if (!def.handObject || !def.handObject.activeSelf) continue;
				def.handObject.SetActive(false);
				var dropped = Instantiate(def.dropPrefabObject, def.dropPoint.position, def.dropPoint.rotation);
				var body = dropped.GetComponent<Rigidbody>();
				if (body != null) body.AddForce(dropped.transform.forward * mainDropSpeed, ForceMode.Impulse);
			}
		}
		public ItemDefs GetItem(string itemName) { return ItemDefs.Find(d => d.itemName == itemName); }
}
