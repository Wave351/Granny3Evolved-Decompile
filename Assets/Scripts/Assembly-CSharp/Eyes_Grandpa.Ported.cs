using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Eyes_Grandpa.c (Granny 3 Evolved, IL2CPP x64).
public partial class Eyes_Grandpa
{
		private void FixedUpdate()
		{
			Eyes_Granny.DifficultyVision(AI.PlayerStatus, ref ViewRange, ref MaxVisionConvinced);
			// His voice drops while he is dying.
			if (!AI.IsDying)
			{
				Talk.pitch = OriginalGrandpaPitch;
				Talk.volume = .25f;
			}
			else
			{
				if (Talk.pitch > 0) Talk.pitch -= Time.deltaTime * .2f;
				else if (Talk.pitch < 0) Talk.pitch = 0;
				Talk.volume -= Time.deltaTime * .25f;
			}
			Vector3 playerPos = Player.transform.position;
			SquareMagnitude = (playerPos - transform.position).sqrMagnitude;
			bool inView = Eyes_Granny.InView(CameraView, playerPos);
			RaycastHit hit;
			if (!Physics.Linecast(transform.position, Player.transform.position, out hit) || hit.collider.tag != "Player"
			    || prisonerDoor.Locked || AI.dizzy)
				Seeing = false;
			else if (SquareMagnitude < ViewRange && inView)
			{
				AI.IsChasing = true;
				if (!IsChasing)
				{
					if (!AI.IsDying)
					{
						// Grandpa shouts, which also calls Granny over.
						int clip = UnityEngine.Random.Range(0, YappingClips.Length);
						Instantiate(NoiseObjGranny, AI.transform.position, AI.transform.rotation);
						Talk.PlayOneShot(YappingClips[clip]);
					}
					AI.ChaseAction();
					if (!AI.gameObject.activeSelf || !AI.Ally.IsChasing) SeeS.Play();
				}
				IsChasing = true;
				Seeing = true;
			}
			if (AI.IsChasing && !AI.IsAngry)
			{
				if (!Seeing)
				{
					TimerStopVision += Time.deltaTime;
					if (TimerStopVision < MaxVisionConvinced) return;
					TimerStopVision = 0;
					AI.StopChase();
					IsChasing = false;
				}
				else
				{
					AI.UnSeenPlayer = false;
					TimerStopVision = 0;
				}
			}
		}
}
