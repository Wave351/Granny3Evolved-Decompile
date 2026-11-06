using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EndCam1.c (Granny 3 Evolved, IL2CPP x64).
public partial class EndCam1
{
		// Animation events of the car ending (Grandpa throws dynamite, the view switches outside the car).
		public void BloodSc() { BloodScreen.SetActive(true); }
		public void Fade() { GO.Fadin = true; }
		public void FadeOut() { GO.Fadin = false; }
		public void Send() { GO.StartCoroutine("EndOver"); }   // Gameovers.EndOver is private
		public void DynamiteAppear() { dynamiteGranny.SetActive(true); }
		public void DynamiteFused()
		{
			var body = dynamiteGranny.GetComponent<Rigidbody>();
			if (body == null) return;
			body.isKinematic = false;
			dynamiteGranny.transform.parent = null;
		}
		public void GrandpaSpawn()
		{
			var ai = grandpa.GetComponent<AI_Grandpa>();
			if (ai == null || ai.Anim == null) return;
			ai.Anim.Play("walkExplosion");
			ai.Anim["walkExplosion"].speed = .65f;
			var walk = grandpa.GetComponent<Animation>();
			if (walk == null) return;
			walk.Play("WalkFromCar");
			walk["WalkFromCar"].speed = .7f;
		}
		public void SwitchPOV()
		{
			BloodScreen.transform.parent.gameObject.SetActive(false);
			differentPov.SetActive(true);
			var anim = differentPov.GetComponent<Animation>();
			if (anim == null) return;
			anim["CamoutsideCar"].speed = .6f;
			gameObject.SetActive(false);
		}
}
