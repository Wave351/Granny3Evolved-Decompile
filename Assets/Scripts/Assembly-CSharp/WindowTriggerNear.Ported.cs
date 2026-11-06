using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/WindowTriggerNear.c (Granny 3 Evolved, IL2CPP x64).
public partial class WindowTriggerNear
{
		// canJump stays on while the player is in the trigger; it is cleared every 0.3 seconds.
		private void Start() { StartCoroutine(OffTrigger()); }
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag == "Player") WJ.canJump = true;
		}
		private IEnumerator OffTrigger()
		{
			yield return new WaitForSeconds(.3f);
			WJ.canJump = false;
			StartCoroutine(OffTrigger());
		}
}
