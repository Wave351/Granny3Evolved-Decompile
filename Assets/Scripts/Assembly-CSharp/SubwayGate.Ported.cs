using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SubwayGate.c (Granny 3 Evolved, IL2CPP x64).
public partial class SubwayGate
{
		// The train smashes the subway gates open once.
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != "TheTrain" || smashedTheGateOut) return;
			smashedTheGateOut = true;
			var anim = GetComponent<Animation>();
			if (anim == null) return;
			anim.Play("SubwayGatesOpen");
			crashEffects.SetActive(true);
		}
}
