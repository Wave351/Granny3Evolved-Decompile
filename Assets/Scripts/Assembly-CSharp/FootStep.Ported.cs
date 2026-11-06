using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FootStep.c (Granny 3 Evolved, IL2CPP x64).
public partial class FootStep
{
		// Player footstep: house (nightmare variants with NM textures), outside, or water.
		public void PlaySound()
		{
			AudioClip[] clips;
			if (House) clips = PlayerPrefs.GetInt("Textures") == 1 ? new[] { W1M, W2M, W3M, W4M, W5M } : new[] { W1, W2, W3, W4, W5 };
			else if (Outside) clips = new[] { O1, O2, O3, O4, O5 };
			else if (Water) clips = new[] { S1, S2, S3, S4 };
			else return;
			FeetManager.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
		}
}
