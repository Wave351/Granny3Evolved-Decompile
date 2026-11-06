using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/MoonlightStateTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class MoonlightStateTrigger
{
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			moonlight.SetActive(!OffState);
		}
}
