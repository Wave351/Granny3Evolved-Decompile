using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/LocationSpawnChange.c (Granny 3 Evolved, IL2CPP x64).
public partial class LocationSpawnChange
{
		// Evolved sets the HE_Spawn floor flags directly (Legacy called EnterF1/EnterF2/EnterCellar/EnterOldH).
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			if (LocationPoint == 1f) { HE.Floor1 = true; HE.Floor2 = false; HE.Floor3 = false; HE.Cellar = false; }
			if (LocationPoint == 2f) { HE.Floor1 = false; HE.Floor2 = true; HE.Floor3 = false; HE.Cellar = false; }
			if (LocationPoint == 3f) { HE.Floor1 = false; HE.Floor2 = false; HE.Floor3 = true; HE.Cellar = false; }
			if (LocationPoint == 0f) { HE.Floor1 = false; HE.Floor2 = false; HE.Floor3 = false; HE.Cellar = true; }
		}
}
