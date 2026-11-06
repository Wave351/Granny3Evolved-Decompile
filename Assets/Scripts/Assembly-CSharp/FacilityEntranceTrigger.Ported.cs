using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FacilityEntranceTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class FacilityEntranceTrigger
{
		// Occlusion culling (when the option is on) is turned back on inside the facility and off at the "off" trigger.
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			if (PlayerPrefs.GetInt("OcclusionCullingOP") != 1) return;
			cam.useOcclusionCulling = !off;
		}
}
