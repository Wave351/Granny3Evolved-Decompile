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

// Ported from Annotated-Pseudocode/ContainSlendrinaItem.c (Granny 3 Evolved, IL2CPP x64).
public partial class ContainSlendrinaItem
{
		// For 1.3 seconds after the start an item dropping in here is held inside Slendrina's container.
		private void Start() { StartCoroutine(StopPlacedItemLogic()); }
		private IEnumerator StopPlacedItemLogic()
		{
			yield return new WaitForSeconds(1.3f);
			PlacedItemIn = false;
		}
		private void OnTriggerStay(Collider other)
		{
			if (PlacedItemIn || other.gameObject.layer != LayerMask.NameToLayer("Item")) return;
			PlacedItemIn = true;
			other.gameObject.transform.parent = slendrinaItemContainer;
			var body = other.GetComponent<Rigidbody>();
			if (body == null) return;
			body.isKinematic = false;
			var sfx = other.GetComponent<EnableSFX>();
			if (sfx != null && sfx.triggerSFXDrop != null) sfx.triggerSFXDrop.SetActive(true);
		}
}
