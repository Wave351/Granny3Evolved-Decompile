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

// Ported from Annotated-Pseudocode/MenuShootLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class MenuShootLogic
{
		// Grandpa's shot in the main menu (animation event), then he and Granny look around.
		public void Shoot()
		{
			Shot.Play();
			Instantiate(Smoke, Spawner.position, Spawner.rotation);
			StartCoroutine(TimerGoSearch());
		}
		private IEnumerator TimerGoSearch()
		{
			yield return new WaitForSeconds(.52f);
			MER.ShootStatus = false;
			MER.GrandpaAnim.CrossFade("Grandpa_Suspicious", 1f);
			MER.GrannyAnim.CrossFade("Granny_Suspicious", .8f);
		}
}
