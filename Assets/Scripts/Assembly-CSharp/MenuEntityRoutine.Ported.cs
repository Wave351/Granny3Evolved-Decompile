using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/MenuEntityRoutine.c (Granny 3 Evolved, IL2CPP x64).
public partial class MenuEntityRoutine
{
		public MenuEntityRoutine() { ShootStatus = true; }
		// Main menu background: Slendrina teleports between points; Grandpa alternates searching and shooting.
		private void Update()
		{
			if (Slendrina.gameObject.activeSelf)
			{
				TimerSlendrina += Time.deltaTime;
				if (TimerSlendrina >= TimerMaxSlendrinaTP)
				{
					var point = TPPointsSlendrina[UnityEngine.Random.Range(0, TPPointsSlendrina.Length)];
					Slendrina.position = point.position;
					Slendrina.rotation = point.rotation;
					TimerSlendrina = 0;
				}
			}
			if (GrandpaAnim == null || GrandpaAnim.transform.parent == null || !GrandpaAnim.transform.parent.gameObject.activeSelf) return;
			if (!ShootStatus)
			{
				TimerSearch += Time.deltaTime;
				if (TimerSearch < TimerMaxSearch) return;
				TimerSearch = 0;
				ShootStatus = true;
				GrandpaAnim.CrossFade("Grandpa_Suspicious", .4f);
				GrannyAnim.CrossFade("Granny_Idle", .6f);
			}
			else
			{
				TimerToShoot += Time.deltaTime;
				if (TimerToShoot < TimerMaxShoot) return;
				TimerToShoot = 0;
				GrandpaAnim["MenuShoot"].speed = .8f;
				GrandpaAnim.CrossFade("MenuShoot");
			}
		}
		public void WindInSearch()
		{
			ShootStatus = false;
			GrandpaAnim.CrossFade("Grandpa_Suspicious", 1f);
			GrannyAnim.CrossFade("Granny_Suspicious", .8f);
		}
}
