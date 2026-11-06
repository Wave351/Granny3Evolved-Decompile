using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TriggerLethalWater.c (Granny 3 Evolved, IL2CPP x64).
public partial class TriggerLethalWater
{
		private void OnTriggerEnter(Collider other)
		{
			// The player falling in wakes the alligator; Granny and Grandpa stop chasing.
			if (other.gameObject.tag == "Player" && !alligator.onHunt && !alligator.awakens)
			{
				AudioSource.PlayClipAtPoint(playerWater, other.transform.position);
				alligator.BeastAwaken();
				if (PlayerPrefs.GetInt("Granny") == 1 && EC.Granny.activeSelf)
				{
					var granny = EC.Granny.GetComponent<AI_Granny>();
					if (granny && !granny.IsDying) granny.StopAI();
				}
				if (PlayerPrefs.GetInt("Grandpa") == 1 && EC.Grandpa.activeSelf)
				{
					var grandpa = EC.Grandpa.GetComponent<AI_Grandpa>();
					if (grandpa && !grandpa.IsDying) grandpa.StopAI();
				}
			}
			string n = other.gameObject.name;
			if (other.gameObject.layer != LayerMask.NameToLayer("Item") && n != "head" && n != "SlendrinaMomHeadOnly") return;
			// Stones, balloon packages and the propane tank come back out; mom's body goes to the cellar; other items just splash.
			if (n == "Stone(Clone)" || n == "HotAirBalloonPackage_1" || n == "HotAirBalloonPackage_2" || n == "HotAirBalloonPackage_3" || n == "PropaneTank")
			{
				other.gameObject.transform.position = teleportStonePoint.position;
				return;
			}
			var mom = other.GetComponent<MomPickUp>();
			if (!mom)
			{
				AudioSource.PlayClipAtPoint(water, other.transform.position);
				return;
			}
			if (GameObject.Find("MomBodyInCell(Clone)") != null) return;
			Instantiate(momInCellarBody, momInCellarBody.transform.position, momInCellarBody.transform.rotation).SetActive(true);
			AudioSource.PlayClipAtPoint(water, other.transform.position);
			Destroy(mom.destructionObject);
			var rayObject = GameObject.Find("PickRay");
			if (rayObject == null) return;
			var ray = rayObject.GetComponent<PickRay>();
			if (ray) ray.TimedTextInitiate("Mom's body has teleported in the cellar");
		}
}
