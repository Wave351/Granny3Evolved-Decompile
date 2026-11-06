using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EndCam3.c (Granny 3 Evolved, IL2CPP x64).
public partial class EndCam3
{
		// Animation events of the train escape ending.
		public void BloodSc() { BloodScreen.SetActive(true); }
		public void Fade() { GO.Fadin = true; }
		public void FadeOut() { GO.Fadin = false; }
		public void Send() { GO.StartCoroutine("EndOver"); }   // Gameovers.EndOver is private
		public void MoveTrain() { trainAnim.Play("TrainAway"); }
		public void GranpdaDance() { grandpaAnim.CrossFade("Grandpa_Dancing"); }
}
