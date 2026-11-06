using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EscapeTriggerGate.c (Granny 3 Evolved, IL2CPP x64).
public partial class EscapeTriggerGate
{
		// Escaping through the gate; "spotted" when both Granny and Grandpa are chasing.
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag != "Player" || Stopped) return;
			if (AI1.IsChasing && AI2.IsChasing) spotted = true;
			Escapes.EscapeGate();
			Stopped = true;
		}
}
