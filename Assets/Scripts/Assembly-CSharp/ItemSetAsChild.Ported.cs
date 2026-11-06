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

// Ported from Annotated-Pseudocode/ItemSetAsChild.c (Granny 3 Evolved, IL2CPP x64).
public partial class ItemSetAsChild
{
		// Items dropped in become children of this container (so they move with it); a gold coin locks it.
		// After 7 seconds it stops accepting items (and optionally plays its closing animation).
		private void Start() { StartCoroutine(SetItOff()); }
		private IEnumerator SetItOff()
		{
			yield return new WaitForSeconds(7f);
			PlacedItemIn = true;
			if (AnimationLater) Anim.Play(CloseDoor.name);
		}
		private void OnTriggerEnter(Collider other)
		{
			if (PlacedItemIn) return;
			string n = other.gameObject.name;
			if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
			{
				if (n == "HotAirBalloonPackage_1" || n == "HotAirBalloonPackage_2" || n == "HotAirBalloonPackage_3" || n == "PropaneTank") return;
				if (offSomeSpecifics && (n == "SecurityRelay / Extras" || n == "CastleFigure / Extras")) return;
			}
			else
			{
				if (n != "GoldCoin / 1" || PlacedItemIn) return;
				PlacedItemIn = true;
			}
			other.transform.SetParent(transform);
		}
}
