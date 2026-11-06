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

// Ported from Annotated-Pseudocode/Freeze_Granny.c (Granny 3 Evolved, IL2CPP x64).
public partial class Freeze_Granny
{
		// Frozen Granny shatters into item pieces, which are removed after TimerGone seconds.
		private void Start()
		{
			EC = GameObject.Find("EnemyController");
			if (PlayerPrefs.GetInt("GrannyWP") == 0) bat.SetActive(true);
			if (!Broke)
			{
				Broke = true;
				var audio = GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.PlayOneShot(FrostBreak);
					foreach (var part in Parts)
					{
						CollTop.enabled = true;
						part.GetComponent<Rigidbody>().isKinematic = false;
						part.transform.parent = null;
						part.layer = LayerMask.NameToLayer("Item");
						ImpactB.SetActive(true);
					}
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
