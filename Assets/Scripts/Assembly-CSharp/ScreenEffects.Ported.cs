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
using UnityEngine.Rendering.PostProcessing;

// Ported from Annotated-Pseudocode/ScreenEffects.c (Granny 3 Evolved, IL2CPP x64).
public partial class ScreenEffects
{
		public ScreenEffects() { SpeedFadeUp = 1.5f; SpeedFadeDown = .5f; SpeedGo = .1f; SpeedAppear = 1.5f; Max1 = .8f; Max2 = .4f; speedGainGrain = .6f; speedLoseGrain = .08f; }
		private void Start()
		{
			Profile.TryGetSettings(out CA);
			Profile.TryGetSettings(out V);
			Profile.TryGetSettings(out G);
		}
		private void Update()
		{
			CA.intensity.value = Scare1;
			G.intensity.value = grainValue;
			// Chromatic aberration while an enemy is seen (NormalSee).
			if (!NormalSee) { if (Scare1 >= 0) Scare1 -= Time.deltaTime * SpeedGo; }
			else if (Scare1 <= Max1) Scare1 += Time.deltaTime * SpeedAppear;
			// Blood overlay while hurt.
			BloodScreen1.color = new Color(1, 0, 1, Blood);
			if (!Hurt) { if (Blood > 0) Blood -= Time.deltaTime * SpeedFadeDown; }
			else if (Blood < 1) Blood += Time.deltaTime * SpeedFadeUp;
			// Film grain while Slender has the player's attention.
			if (!SlenderAI.PlayerCaughtAttention) { if (grainValue > 0) grainValue -= Time.deltaTime * speedLoseGrain; }
			else if (grainValue < 1) grainValue += Time.deltaTime * speedGainGrain;
		}
		public void HurtPlayer()
		{
			StopCoroutine(HurtPlayerS());
			StartCoroutine(HurtPlayerS());
			Hurt = true;
			Camera.Play("playerHurt");
		}
		private IEnumerator HurtPlayerS()
		{
			yield return new WaitForSeconds(4.5f);
			Hurt = false;
		}
}
