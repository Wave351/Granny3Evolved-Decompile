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

// Ported from Annotated-Pseudocode/NightmareMonster.c (Granny 3 Evolved, IL2CPP x64).
public partial class NightmareMonster
{
		public NightmareMonster() { walkRadius = 8f; idleTime = 6f; blastRecoverMax = 10f; canBlast = true; distanceVision = 7f; timerMasAggro = 2f; }
		private void Start() { StartCoroutine(WanderRoutine()); }
		private void Update()
		{
			if (blasting) return;
			// Seeing the player (through an open prisoner door) makes it stare them down; staying too long gets blasted.
			visionPoint.LookAt(playerHead);
			RaycastHit hit;
			if (Physics.Raycast(visionPoint.position, visionPoint.forward, out hit, distanceVision) && hit.collider.transform == playerHead && !prisonerDoor.Locked)
			{
				intimidated = true;
				anim.CrossFade("Monster_Intimidated");
				Vector3 to = playerHead.transform.position - transform.position;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(to.x, 0, to.z)), Time.deltaTime * 10f);
				wandering = false;
				idle = false;
				agent.isStopped = true;
				timerInTerritory += Time.deltaTime;
				if (timerInTerritory < timerMasAggro || !canBlast) return;
				StartCoroutine(BlastRoutine());
				return;
			}
			intimidated = false;
			timerInTerritory = 0;
			if (wandering || idle || blasting || changingState) return;
			StartCoroutine(WanderRoutine());
		}
		// Walk to random points for one walk-animation length, then idle.
		private IEnumerator WanderRoutine()
		{
			changingState = true;
			yield return null;
			if (!intimidated && !blasting)
			{
				wandering = true;
				agent.isStopped = false;
				anim.CrossFade("Monster_Walk");
				float timer = anim["Monster_Walk"].length;
				while (timer > 0)
				{
					if (intimidated || blasting) { changingState = false; yield break; }
					if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) agent.SetDestination(ChooseRandomPoint());
					yield return null;
					timer -= Time.deltaTime;
				}
				wandering = false;
				if (!intimidated && !blasting) StartCoroutine(IdleRoutine());
			}
			changingState = false;
		}
		private IEnumerator IdleRoutine()
		{
			idle = true;
			agent.isStopped = true;
			anim.CrossFade("Monster_Idle");
			yield return new WaitForSeconds(idleTime);
			idle = false;
			if (!intimidated && !blasting) StartCoroutine(WanderRoutine());
		}
		private IEnumerator BlastRoutine()
		{
			intimidated = false;
			blasting = true;
			canBlast = false;
			agent.isStopped = true;
			anim.CrossFade("Monster_Blast");
			yield return new WaitForSeconds(anim["Monster_Blast"].length);
			blasting = false;
			timerInTerritory = 0;
			StartCoroutine(BlastCooldown());
			if (!intimidated) StartCoroutine(WanderRoutine());
		}
		private IEnumerator BlastCooldown()
		{
			yield return new WaitForSeconds(blastRecoverMax);
			canBlast = true;
		}
		public Vector3 ChooseRandomPoint()
		{
			Vector3 random = UnityEngine.Random.insideUnitSphere * walkRadius + transform.position;
			NavMeshHit hit;
			return NavMesh.SamplePosition(random, out hit, walkRadius, -1) ? hit.position : transform.position;
		}
}
