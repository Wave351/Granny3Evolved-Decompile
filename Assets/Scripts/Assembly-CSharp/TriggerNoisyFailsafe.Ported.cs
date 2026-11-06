using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TriggerNoisyFailsafe.c (Granny 3 Evolved, IL2CPP x64).
public partial class TriggerNoisyFailsafe
{
		// Noise objects that fall out of reach are put back at the respawn point.
		private void OnTriggerStay(Collider other)
		{
			string tag = other.gameObject.tag;
			if (tag != "noiseobject" && tag != "noiseobject_grandpa") return;
			other.gameObject.transform.position = respawnPoint.position;
		}
}
