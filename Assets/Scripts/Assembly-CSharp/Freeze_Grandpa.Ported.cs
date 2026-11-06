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

// Ported from Annotated-Pseudocode/Freeze_Grandpa.c (Granny 3 Evolved, IL2CPP x64).
public partial class Freeze_Grandpa
{
		// Frozen Grandpa shatters the same way; the shotgun (if he had it) stays on his arm piece.
		private void Start()
		{
			EC = GameObject.Find("EnemyController");
			var controller = EC.GetComponent<EnemyController>();
			if (controller == null || controller.ShotgunGrandpa == null) return;
			if (controller.ShotgunGrandpa.activeSelf) Shotgun.SetActive(true);
			if (!Broke)
			{
				Broke = true;
				var audio = GetComponent<AudioSource>();
				if (audio == null) return;
				audio.PlayOneShot(FrostBreak);
				foreach (var part in Parts)
				{
					BodyOriginal.SetActive(false);
					part.SetActive(true);
					part.transform.parent = null;
					part.layer = LayerMask.NameToLayer("Item");
					if (Shotgun != null)
					{
						Shotgun.transform.parent = armItem.transform;
						Physics.IgnoreCollision(Shotgun.GetComponent<Collider>(), armItem.GetComponent<Collider>(), true);
					}
					ImpactB.SetActive(true);
				}
			}
			StartCoroutine(KillAll());
		}
		private IEnumerator KillAll()
		{
			yield return new WaitForSeconds(TimerGone);
			DestructionRagdolls();
		}
		public void DestructionRagdolls()
		{
			foreach (var part in Parts) Destroy(part);
			Destroy(ParentOfIt);
		}
}
