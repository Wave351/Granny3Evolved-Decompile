using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EndCam2.c (Granny 3 Evolved, IL2CPP x64).
public partial class EndCam2
{
		// Animation events of the alligator ending.
		public void BloodSc() { BloodScreen.SetActive(true); }
		public void Fade() { GO.Fadin = true; }
		public void FadeOut() { GO.Fadin = false; }
		public void Send() { GO.StartCoroutine("EndOver"); }   // Gameovers.EndOver is private
		public void StartUp()
		{
			GO.Fadin = true;
			var ai = alligator.GetComponent<AI_Alligator>();
			if (ai == null) return;
			ai.StopAllCoroutines();
			ai.enabled = false;
			alligator.transform.position = posMainBody.position;
			alligator.transform.rotation = posMainBody.rotation;
			alligatorAnim.transform.rotation = posBodyAnim.rotation;
			alligatorAnim.Play("startAnim");
			alligatorBehaviorAnim.Play("idle");
		}
		public void growlSFX() { alligatorGrowl.SetActive(true); }   // same compiled code as SubmarineAnimationEvents.EscapeMusicOn
		public void AlligatorRepos() { alligatorAnim.transform.localPosition = new Vector3(0, -.76f, 0); }
		public void GrandpaShoot()
		{
			grandpaAnim.CrossFade("shootGun");
			grandpaAnim["shootGun"].speed = 1.4f;
		}
		public void BloodyEffect() { Instantiate(bloodEffect, transform.position, Quaternion.identity); }
		public void AlligatorBite()
		{
			alligatorBehaviorAnim.Play("attack");
			alligatorBehaviorAnim["attack"].speed = .7f;
		}
}
