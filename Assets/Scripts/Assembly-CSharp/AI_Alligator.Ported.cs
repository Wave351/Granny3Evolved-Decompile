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

// Ported from Annotated-Pseudocode/AI_Alligator.c (Granny 3 Evolved, IL2CPP x64).
public partial class AI_Alligator
{
		public AI_Alligator() { timerDeathPlayer = 10f; detectDistanceAttack = 2.5f; }
		private void Update()
		{
			if (onHunt && !PS.Killed)
			{
				if (!killingPlayer && !hurt)
				{
					// Swim after the player; close enough, it bites.
					animAlli["walk"].speed = 1.8f;
					animAlli.CrossFade("walk");
					agent.SetDestination(player.position);
					if (Vector3.Distance(transform.position, player.position) <= detectDistanceAttack)
					{
						killingPlayer = true;
						StartCoroutine(AttackRoutine());
						PS.PlayerGettingStopped();
						PS.lookAtAlligator = true;
						animAlli.CrossFade("attack");
						hurtAudio.PlayOneShot(waterSplash);
						agent.SetDestination(transform.position);
					}
				}
				else if (killingPlayer)
				{
					Vector3 to = player.position - transform.position;
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(to.x, 0, to.z)), Time.deltaTime * 5f);
				}
				// Hunting music.
				if (!aggroAudio.isPlaying) { aggroAudio.Play(); return; }
				if (aggroAudio.volume >= .9f) return;
				aggroAudio.volume += Time.deltaTime * 1.5f;
				return;
			}
			if (aggroAudio.isPlaying)
			{
				if (aggroAudio.volume == 0) aggroAudio.Stop();
				else aggroAudio.volume -= Time.deltaTime * .35f;
			}
			// The player died some other way: the alligator sinks back.
			if (PS.Killed)
			{
				onHunt = false;
				StartCoroutine(AlligatorFadeOutExternalDeath());
			}
		}
		// The player fell in: it rises at the awake point and hunts after 0.9 seconds.
		public void BeastAwaken()
		{
			awakens = true;
			ascendAudio.Play();
			transform.position = awakePoint.position;
			transform.rotation = awakePoint.rotation;
			animAlli.CrossFade("idle");
			positionAnim.Play("startAnim");
			StartCoroutine(PrepareToAttack());
			waterCollision.enabled = false;
		}
		// Hit by a weapon: it stops for 1.4 seconds (hits only count once the train escape is ready).
		public void HurtAlligatorWeapon()
		{
			hurt = true;
			agent.SetDestination(transform.position);
			animAlli.CrossFade("hit");
			StartCoroutine(HurtAlligator());
			if (ERC.TrainAvailable) timesHit += 1f;
		}
		private IEnumerator AttackRoutine()
		{
			yield return new WaitForSeconds(.45f);
			onHunt = false;
			PS.NormalDeath(false);
			PS.DamageSpider();
			killingPlayer = false;
			PS.lookAtAlligator = false;
			yield return new WaitForSeconds(1f);
			animAlli.CrossFade("idle");
			onHunt = false;
			yield return new WaitForSeconds(4.5f);
			Sink();
		}
		private IEnumerator AlligatorFadeOutExternalDeath()
		{
			yield return new WaitForSeconds(4.5f);
			Sink();
		}
		private void Sink()
		{
			positionAnim.Play("endAnim");
			var sphere = GetComponent<SphereCollider>();
			if (sphere == null) return;
			sphere.enabled = false;
			agent.enabled = false;
			waterCollision.enabled = true;
		}
		private IEnumerator HurtAlligator()
		{
			yield return new WaitForSeconds(1.4f);
			hurt = false;
		}
		private IEnumerator PrepareToAttack()
		{
			yield return new WaitForSeconds(.9f);
			awakens = false;
			onHunt = true;
			agent.enabled = true;
			var sphere = GetComponent<SphereCollider>();
			if (sphere != null) sphere.enabled = true;
		}
}
