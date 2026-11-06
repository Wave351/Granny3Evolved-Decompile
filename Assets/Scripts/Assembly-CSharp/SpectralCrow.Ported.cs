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

// Ported from Annotated-Pseudocode/SpectralCrow.c (Granny 3 Evolved, IL2CPP x64).
public partial class SpectralCrow
{
		public SpectralCrow() { redecideRouteDistance = 2f; }
		private void Start()
		{
			RandomWaypoint();
			birdAnim["fly"].speed = 1.15f;
		}
		private void Update()
		{
			if (Vector3.Distance(transform.position, agent.destination) < redecideRouteDistance) RandomWaypoint();
		}
		// Flying into the player hurts them and knocks the held item out; the crow then speeds off for 5 seconds.
		private IEnumerator OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != "Main Camera" || SE.Hurt) yield break;
			SE.HurtPlayer();
			pickRay.CheckItemDropping();
			var audio = GetComponent<AudioSource>();
			if (audio == null) yield break;
			audio.Play();
			RandomWaypoint();
			agent.speed = 9f;
			birdAnim["fly"].speed = 1.7f;
			yield return new WaitForSeconds(5f);
			agent.speed = 5.5f;
			birdAnim["fly"].speed = 1.15f;
		}
		public void RandomWaypoint()
		{
			do waypointNumberSelect = UnityEngine.Random.Range(0, randomTravelPoints.Length);
			while (waypointNumberSelect == currentWaypointIndex);
			currentWaypointIndex = waypointNumberSelect;
			agent.SetDestination(randomTravelPoints[waypointNumberSelect].position);
		}
}
