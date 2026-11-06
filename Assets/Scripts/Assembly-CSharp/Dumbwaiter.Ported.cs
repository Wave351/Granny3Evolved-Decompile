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

// Ported from Annotated-Pseudocode/Dumbwaiter.c (Granny 3 Evolved, IL2CPP x64).
public partial class Dumbwaiter
{
		// The dumbwaiter's buttons only work while it is not moving; the player riding it counts as "in the elevator".
		private void Update()
		{
			foreach (var button in buttons) button.enabled = !travelling;
			soundTravelLoop.SetActive(travelling);
			if (player.activeSelf) return;
			var fps = player ? player.GetComponent<MobileFPS>() : null;
			if (fps != null && fps.InElev) fps.InElev = false;
		}
		public void ElevatorMovementStart()
		{
			if (travelling) return;
			string clip;
			if (!above) { above = true; clip = "MathissUpp"; }
			else { above = false; clip = "MathissNer"; }
			dumbwaiterAnim.Play(clip);
			StartCoroutine(TimerElevatorMovement(clip));
			travelling = true;
		}
		private IEnumerator TimerElevatorMovement(string anim)
		{
			yield return new WaitForSeconds(dumbwaiterAnim[anim].length);
			travelling = false;
		}
		public void DisableTravel() { travelling = false; }
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			var fps = player ? player.GetComponent<MobileFPS>() : null;
			if (fps != null) fps.InElev = true;
		}
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.tag != "Player") return;
			var fps = player ? player.GetComponent<MobileFPS>() : null;
			if (fps != null && fps.InElev) fps.InElev = false;
		}
}
