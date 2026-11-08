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

// Ported from Annotated-Pseudocode/CrowChimney.c (Granny 3 Evolved, IL2CPP x64).
public partial class CrowChimney
{
		public CrowChimney() { logEndTimer = 60f; }
		private void Start() { oldRotation = body.rotation; }
		private void Update()
		{
			if (Training.Hold(null, Flying)) return;
			// A burning log in the fireplace makes the crow fly away for a while.
			if (burningLog.activeSelf && !flewff)
			{
				flewff = true;
				NestBird.CrossFade("fly");
				Flying.Play("FlyAway");
				StartCoroutine(TimerWaitBirdReturn());
			}
			// Close by, the crow turns its body to watch the player's head.
			Quaternion goal;
			if (Vector3.Distance(transform.position, player.position) > pointRadius || flewff) goal = oldRotation;
			else
			{
				Vector3 to = playerHead.position - body.position;
				if (to.y > 0) to.y *= .2f;
				goal = (to - Vector3.zero).sqrMagnitude >= 1e-10f ? Quaternion.LookRotation(to) : body.rotation;
			}
			if (!ColliderPickUp.activeSelf)
			{
				if (flewff) body.localRotation = new Quaternion(0, 0, 0, 0);
			}
			else body.rotation = Quaternion.Slerp(body.rotation, goal, Time.deltaTime * 8f);
		}
		public void HurtDart()
		{
			NestBird.CrossFade("attack");
			StartCoroutine(WaitAfterAttack());
			Audio.PlayOneShot(Crow);
		}
		// Hit by a stone: it can't be robbed for 3.5 seconds.
		public void HurtBird()
		{
			ColliderPickUp.SetActive(false);
			HurtTemp = true;
			var collider = GetComponent<Collider>();
			if (collider == null) return;
			collider.enabled = false;
			NestBird.CrossFade("hurt");
			StartCoroutine(WaitAfterAttack2());
			Audio.PlayOneShot(Crow);
		}
		private IEnumerator WaitAfterAttack2()
		{
			yield return new WaitForSeconds(3.5f);
			HurtTemp = false;
			NestBird.CrossFade("idle");
			if (Killed) yield break;
			ColliderPickUp.SetActive(true);
			var collider = GetComponent<Collider>();
			if (collider != null) collider.enabled = true;
		}
		// Killing the crow makes Granny and Grandpa permanently angry.
		public void KillAssBird()
		{
			Killed = true;
			ColliderPickUp.SetActive(false);
			if (NestBird.gameObject.activeSelf)
			{
				BloodKill.transform.position = NestBird.transform.position;
				BloodKill.SetActive(true);
				BloodKill.transform.parent = null;
				Flying.gameObject.SetActive(false);
			}
			if (PlayerPrefs.GetInt("Granny") == 1 && EC.Granny.activeSelf) TH.AlwaysAngry1 = true;
			int grandpa = PlayerPrefs.GetInt("Grandpa");
			if (grandpa != 1 && grandpa != 2) return;
			if (EC.Grandpa.activeSelf) TH.AlwaysAngry2 = true;
		}
		// Reaching into the nest: the crow pecks the player.
		public void AttemptSteal()
		{
			ScreenEffects.HurtPlayer();
			NestBird.CrossFade("attack");
			NestBird["attack"].speed = 1.8f;
			StartCoroutine(WaitAfterAttack());
			Audio.PlayOneShot(Attack);
		}
		private IEnumerator WaitAfterAttack()
		{
			yield return new WaitForSeconds(1.5f);
			NestBird.CrossFade("idle");
		}
		private IEnumerator TimerWaitBirdReturn()
		{
			yield return new WaitForSeconds(logEndTimer);
			burningLog.SetActive(false);
			Flying.CrossFade("FlyBack", .1f);
			yield return new WaitForSeconds(Flying["FlyBack"].length);
			NestBird.CrossFade("idle");
			flewff = false;
			log.enabled = true;
			HurtTemp = false;
			if (Killed) yield break;
			ColliderPickUp.SetActive(true);
			var collider = GetComponent<Collider>();
			if (collider != null) collider.enabled = true;
		}
}
