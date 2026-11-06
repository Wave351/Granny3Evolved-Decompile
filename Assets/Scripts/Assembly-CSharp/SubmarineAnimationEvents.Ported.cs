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

// Ported from Annotated-Pseudocode/SubmarineAnimationEvents.c (Granny 3 Evolved, IL2CPP x64).
public partial class SubmarineAnimationEvents
{
		// Animation events of the submarine escape.
		public void SubmarinePropellerWork()
		{
			propeller.Play("PropellerMove");
			propellerAudio.Play();
			propellerAudio.pitch = .15f;
			propeller["PropellerMove"].speed = .1f;
			StartCoroutine(timerPropellerFull());
		}
		// The propeller spins up step by step (sound pitch, animation speed, wait).
		private IEnumerator timerPropellerFull()
		{
			yield return new WaitForSeconds(.5f);
			if (propeller == null || propeller["PropellerMove"] == null) yield break;
			float[][] steps = {
				new[] { .3f, .25f, .35f }, new[] { .35f, .35f, .35f }, new[] { .45f, .45f, .35f },
				new[] { .65f, .55f, .35f }, new[] { .75f, .7f, .4f }, new[] { .85f, 1f, .4f } };
			foreach (var step in steps)
			{
				propeller["PropellerMove"].speed = step[1];
				propellerAudio.pitch = step[0];
				yield return new WaitForSeconds(step[2]);
			}
			propellerAudio.pitch = .9f;
			yield return new WaitForSeconds(.4f);
			propellerAudio.pitch = .95f;
			yield return new WaitForSeconds(.4f);
			if (!propellerAudio.isPlaying) propellerAudio.Play();
			propellerAudio.pitch = 1.05f;
		}
		public void EffectBelow() { effectBelowEnable.SetActive(true); }
		public void AngleSwitch()
		{
			camSubAccess.SetActive(true);
			waterSubAccess.SetActive(true);
			camSubmarine.SetActive(false);
			propellerAudio.volume = 0;
		}
		public void EscapeMusicOn() { escapeMusic.SetActive(true); }
}
