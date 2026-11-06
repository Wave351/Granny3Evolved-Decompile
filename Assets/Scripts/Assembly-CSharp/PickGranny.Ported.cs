using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/PickGranny.c (Granny 3 Evolved, IL2CPP x64).
public partial class PickGranny
{
		// With the slingshot weapon option, Granny picks a dropped slingshot back up.
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.name != "Slingshot(Clone)") return;
			if (PlayerPrefs.GetInt("GrannyWP") != 1) return;
			if (AI.dizzy || AI.IsDying) return;
			AI.slingshotWP.SetActive(true);
			Destroy(other.gameObject);
		}
}
