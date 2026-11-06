using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FricBatonAnim.c (Granny 3 Evolved, IL2CPP x64).
public partial class FricBatonAnim
{
		// Animation events of the electric baton.
		public void OpenBaton() { var audio = GetComponent<AudioSource>(); if (audio) audio.PlayOneShot(open); }
		public void Whoosh() { var audio = GetComponent<AudioSource>(); if (audio) audio.PlayOneShot(soundWhoosh); }
		public void HitAttempt() { pickRay.AttackBaton(); }
}
