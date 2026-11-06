using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DumbwaiterKiller.c (Granny 3 Evolved, IL2CPP x64).
public partial class DumbwaiterKiller
{
		// The dumbwaiter kills a player under it (unless they are falling or landing).
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag != "Player" || Status.Killed) return;
			var falling = Status.Player ? Status.Player.GetComponent<FallingHolder>() : null;
			if (falling == null || falling.isFalling || falling.isLanding || Status.Killed) return;
			Status.NormalDeath(false);
		}
}
