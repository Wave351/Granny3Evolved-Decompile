using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TriggerBackgroundNoise.c (Granny 3 Evolved, IL2CPP x64).
public partial class TriggerBackgroundNoise
{
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			if (Outside) { BGManager.InHouse = false; BGManager.InOutside = true; BGManager.InFacility = false; }
			else if (Inside) { BGManager.InHouse = true; BGManager.InOutside = false; BGManager.InFacility = false; }
			else if (Facility) { BGManager.InFacility = true; BGManager.InHouse = false; BGManager.InOutside = false; }
		}
}
