using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/StoneCheckPresence.c (Granny 3 Evolved, IL2CPP x64).
public partial class StoneCheckPresence
{
		// A thrown stone that lands here is put back at the reposition point.
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != "Stone(Clone)") return;
			other.transform.position = repositionPoint.position;
		}
}
