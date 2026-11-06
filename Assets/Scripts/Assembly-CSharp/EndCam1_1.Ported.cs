using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EndCam1_1.c (Granny 3 Evolved, IL2CPP x64).
public partial class EndCam1_1
{
		// Animation events of the dynamite ending: Granny is blown up and replaced by a ragdoll.
		public void BloodSc() { BloodScreen.SetActive(true); }
		public void Fade() { GO.Fadin = true; }
		public void FadeOut() { GO.Fadin = false; }
		public void Send() { GO.StartCoroutine("EndOver"); }   // Gameovers.EndOver is private
		public void Explosion()
		{
			explosion.SetActive(true);
			dynamiteGranny.SetActive(false);
			granny.SetActive(false);
			Instantiate(ragdollGranny, granny.transform.position, granny.transform.rotation);
		}
}
