using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TeddyHunt.c (Granny 3 Evolved, IL2CPP x64).
public partial class TeddyHunt
{
		public TeddyHunt() { fadeSpeed = .5f; NeedPlay = true; }
		private void Start()
		{
			audioSource.loop = true;
			audioSource.volume = 0;
			if (DifficultyLevel.Rules == 4) { GrannyEyesNormal = GrannyEyesSetDark; GrandpaEyesNormal = GrandpaEyesSetDark; }
			GrannyEyeM.material.color = GrannyEyesNormal;
			GrandpaEyeM.material.color = GrandpaEyesNormal;
		}
		public void StopAll() { SupposedToFade = false; }
		public void StopEnding()
		{
			SupposedToFade = false;
			AlreadyHunted = false;
			TimerGone = 5f;
			gameObject.SetActive(false);
		}
		// Carrying the teddy makes Granny and Grandpa angry (red eyes, hunt music); 5 seconds after dropping it they calm down.
		private void Update()
		{
			if (!SupposedToFade)
			{
				audioSource.volume -= fadeSpeed * Time.deltaTime;
				if (NeedPlay && audioSource.isPlaying && audioSource.volume == 0) audioSource.Stop();
			}
			else
			{
				if (audioSource.volume < .7f) audioSource.volume += fadeSpeed * Time.deltaTime;
				if (NeedPlay && !audioSource.isPlaying) audioSource.Play();
			}
			if (!AlwaysAngry1 && !AlwaysAngry2)
			{
				if (!TeddyH.activeSelf)
				{
					if (AlreadyHunted) TimerGone += Time.deltaTime;
					if (TimerGone < 5f) return;
					TimerGone = 0;
					Granny.IsAngry = false;
					if (!Granny.EnemyVision.Seeing) Granny.StopChase();
					Grandpa.IsAngry = false;
					if (!Grandpa.EnemyVision.Seeing) Grandpa.StopChase();
					GrannyEyeM.material.color = GrannyEyesNormal;
					GrandpaEyeM.material.color = GrandpaEyesNormal;
					SupposedToFade = false;
					AlreadyHunted = false;
					return;
				}
				SupposedToFade = true;
				AlreadyHunted = true;
				GrannyEyeM.material.color = GrannyEyesAngry;
				GrandpaEyeM.material.color = GrandpaEyesAngry;
				Granny.IsAngry = true;
				Grandpa.IsAngry = true;
			}
			else
			{
				// Always-angry modes.
				SupposedToFade = true;
				AlreadyHunted = true;
				if (AlwaysAngry1) { Granny.IsAngry = true; GrannyEyeM.material.color = GrannyEyesAngry; }
				if (AlwaysAngry2) { GrandpaEyeM.material.color = GrandpaEyesAngry; Grandpa.IsAngry = true; }
			}
			TimerGone = 0;
		}
}
