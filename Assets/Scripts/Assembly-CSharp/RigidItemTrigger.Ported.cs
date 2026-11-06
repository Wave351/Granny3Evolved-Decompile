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

// Ported from Annotated-Pseudocode/RigidItemTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class RigidItemTrigger
{
		// An item dropping in falls freely (with its drop sound); the trigger removes itself after 7 seconds.
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(7f);
			digestedItem = true;
			Destroy(gameObject);
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer != LayerMask.NameToLayer("Item") || digestedItem) return;
			digestedItem = true;
			other.GetComponent<Rigidbody>().isKinematic = false;
			if (!sfxOn) return;
			var sfx = other.gameObject.GetComponent<EnableSFX>();
			if (sfx != null) sfx.EnableDropSound();
		}
}
