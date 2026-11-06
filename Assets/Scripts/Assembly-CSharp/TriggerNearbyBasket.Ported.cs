using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TriggerNearbyBasket.c (Granny 3 Evolved, IL2CPP x64).
public partial class TriggerNearbyBasket
{
		// The basket's blocking collider is off while the player stands next to it (so it can be reached).
		private void Update()
		{
			if (player.activeSelf || collisionBasketCollider.enabled) return;
			collisionBasketCollider.enabled = true;
			rendererBasketCollider.enabled = true;
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			collisionBasketCollider.enabled = false;
			rendererBasketCollider.enabled = false;
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			collisionBasketCollider.enabled = true;
			rendererBasketCollider.enabled = true;
		}
}
