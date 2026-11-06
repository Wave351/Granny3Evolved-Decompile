using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DoorOpenInstant.c (Granny 3 Evolved, IL2CPP x64).
public partial class DoorOpenInstant
{
		// Granny or Grandpa walking into a closed door opens it straight away.
		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("granny") && !other.CompareTag("granpa")) return;
			if (!DL.Closed || DL.AnimatedObject.isPlaying) return;
			DL.UnlockDoor(false);
			DL.OpeningClosingAttempts();
		}
}
