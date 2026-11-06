using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SmokeAmmo.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class SmokeAmmo
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		canGoOff = true;
	}

	private void Update()
	{
		if (canGoOff && rigid.linearVelocity.magnitude < 0.1f && !e.isStopped)
		{
			ParticleSystem.MainModule main = e.main;
			main.loop = false;
		}
	}
}
