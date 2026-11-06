using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ResetFloorTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class ResetFloorTrigger
{
		// Mom's body dropped out of the map is moved back into the cellar.
		private void OnTriggerStay(Collider other)
		{
			var mom = other.GetComponent<MomPickUp>();
			if (!mom) return;
			if (GameObject.Find("MomBodyInCell(Clone)") != null) return;
			var body = Instantiate(momInCellarBody, momInCellarBody.transform.position, momInCellarBody.transform.rotation);
			body.SetActive(true);
			Destroy(mom.destructionObject);
			var rayObject = GameObject.Find("PickRay");
			if (rayObject == null) return;
			var ray = rayObject.GetComponent<PickRay>();
			if (ray) ray.TimedTextInitiate("Mom's body has teleported in the cellar");
		}
}
