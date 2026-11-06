using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FailsafeFallPlayer.c (Granny 3 Evolved, IL2CPP x64).
public partial class FailsafeFallPlayer
{
		// A player who has been falling for over a second here is put back at the reposition spot.
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			if (!FH.Falling || FH.fallDuration <= 1f) return;
			FH.ch.enabled = false;
			FH.transform.position = repositionSpot.position;
			FH.ch.enabled = true;
		}
}
