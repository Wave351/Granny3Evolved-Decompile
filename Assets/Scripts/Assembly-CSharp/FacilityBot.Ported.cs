using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FacilityBot.c (Granny 3 Evolved, IL2CPP x64).
public partial class FacilityBot
{
		public FacilityBot() { huntTimerMax = .3f; distanceElectricCharge = 6.4f; }
		private void Start()
		{
			transform.position = idlePoint.position;
			agent.enabled = true;
			transform.localScale = new Vector3(57f, 57f, 57f);
		}
		private void Update()
		{
			if (dead)
			{
				agent.enabled = false;
				patrolSFX.SetActive(false);
				return;
			}
			// Two sight rays aimed at the player's head start the hunt.
			sight1.LookAt(playerHead.position);
			sight2.LookAt(playerHead.position);
			UnityEngine.Debug.DrawRay(sight1.position, sight1.forward * Mathf.Infinity, Color.red);
			UnityEngine.Debug.DrawRay(sight2.position, sight2.forward * Mathf.Infinity, Color.red);
			RaycastHit hit;
			if (Physics.Raycast(sight1.position, sight1.forward, out hit, Mathf.Infinity, layersRay)
			    || Physics.Raycast(sight2.position, sight2.forward, out hit, Mathf.Infinity, layersRay))
			{
				if (hit.collider.gameObject.tag == "Player" && !travelBack && !hunting) hunting = true;
			}
			// Patrol hum and electricity sound.
			var patrol = patrolSFX.GetComponent<AudioSource>();
			if (!fadingPatrolSFX)
			{
				if (patrol.volume != 0) patrol.volume -= Time.deltaTime * 1.6f;
				if (electricitySFX.volume != 0) electricitySFX.volume -= Time.deltaTime * 1.5f;
			}
			else
			{
				if (patrol.volume != 1f) patrol.volume += Time.deltaTime * 1.1f;
				if (Vector3.Distance(transform.position, player.position) > distanceElectricCharge)
				{
					if (electricitySFX.volume != 0) electricitySFX.volume -= Time.deltaTime * 1.5f;
				}
				else if (electricitySFX.volume != .4f) electricitySFX.volume += Time.deltaTime * 1.5f;
			}
			if (hunting)
			{
				// Chase the player; close up, the arms discharge (the death trigger is on).
				fadingPatrolSFX = true;
				agent.stoppingDistance = 3f;
				agent.destination = player.transform.position;
				bool close = Vector3.Distance(transform.position, player.position) <= distanceElectricCharge;
				foreach (var discharge in electricDischarge)
				{
					if (close && !discharge.isPlaying) discharge.Play();
					else if (!close && discharge.isPlaying) discharge.Stop();
				}
				deathTrigger.SetActive(close);
				handsAnim.CrossFade(close ? "RobotArmsShoot" : "RobotArmsAggro");
				huntingTimer += Time.deltaTime;
				if ((agent.velocity - Vector3.zero).sqrMagnitude >= 1e-10f) return;
				if (huntingTimer <= huntTimerMax) return;
				// Stuck in place: give up the hunt, facing the player.
				hunting = false;
				Vector3 to = player.position - transform.position;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(to.x, 0, to.z)), Time.deltaTime * 10f);
				return;
			}
			if (travelBack)
			{
				// Walk back to the idle point.
				agent.stoppingDistance = 0;
				fadingPatrolSFX = true;
				huntingTimer = 0;
				agent.destination = idlePoint.position;
				if (agent.pathPending) return;
				if (agent.remainingDistance > agent.stoppingDistance + .05f) return;
				travelBack = false;
				timerFadeTravel = 0;
				return;
			}
			fadingPatrolSFX = false;
			huntingTimer = 0;
			// Away from the idle point for too long: go back.
			if (Vector3.Distance(transform.position, idlePoint.position) <= .2f) timerFadeTravel = 0;
			else
			{
				timerFadeTravel += Time.deltaTime;
				if (timerFadeTravel >= timerFadeMaxTravel) { travelBack = true; timerFadeTravel = 0; }
			}
			foreach (var discharge in electricDischarge) if (discharge.isPlaying) discharge.Stop();
			deathTrigger.SetActive(false);
			handsAnim.CrossFade("RobotArmsIdle");
		}
		// Destroyed by an electric blast: the head blows off and its door bursts apart.
		public void DeathUponRobot()
		{
			hunting = false;
			dead = true;
			travelBack = false;
			lampObject.transform.parent = headUnitAnim.transform;
			headUnitAnim.Play("RobotHeadBlow");
			agent.enabled = false;
			var patrol = patrolSFX.GetComponent<AudioSource>();
			if (patrol == null) return;
			patrol.PlayOneShot(robotDefeat);
			electricitySFX.gameObject.SetActive(false);
			foreach (var part in partsDoor)
			{
				part.isKinematic = false;
				part.transform.parent = null;
				part.gameObject.AddComponent<DestroyTimer>();
				var timer = part.gameObject.GetComponent<DestroyTimer>();
				timer.onlyItself = true;
				timer.TimeW = 20f;
			}
			handsAnim.CrossFade("RobotArmsDeath");
			foreach (var discharge in electricDischarge) if (discharge.isPlaying) discharge.Stop();
			deathTrigger.SetActive(false);
			explosionDoor.SetActive(true);
			lightObjDoor.material.color = new Color(0, 0, 0, 1);
			var obstacle = GetComponent<NavMeshObstacle>();
			if (obstacle != null) obstacle.enabled = true;
		}
}
