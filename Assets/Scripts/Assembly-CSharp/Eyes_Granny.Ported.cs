using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Eyes_Granny.c (Granny 3 Evolved, IL2CPP x64).
public partial class Eyes_Granny
{
		public Eyes_Granny() { ViewRange = 100f; MaxVisionConvinced = 4f; }
		// Squared view range and how long she keeps chasing after losing sight, by difficulty (shorter while hiding under things).
		internal static void DifficultyVision(PlayerStatus status, ref float viewRange, ref float maxConvinced)
		{
			bool under = status.HidingUnderThings;
			switch (PlayerPrefs.GetInt("Diff"))
			{
				case 0: viewRange = 500f; maxConvinced = under ? 3f : 6f; break;
				case 1: viewRange = 900f; maxConvinced = under ? 4f : 8f; break;
				case 2: viewRange = 1200f; maxConvinced = under ? 5f : 10f; break;
				case 3: viewRange = 1500f; maxConvinced = under ? 5.5f : 12f; break;
				case 4: viewRange = 1500f; maxConvinced = under ? 6f : 16f; break;
			}
		}
		// The player is visible when they are in her camera's view, within range and nothing is in between.
		internal static bool InView(Camera cam, Vector3 player)
		{
			Vector3 v = cam.WorldToViewportPoint(player);
			return v.z > 0 && v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1;
		}
		private void FixedUpdate()
		{
			DifficultyVision(AI.PlayerStatus, ref ViewRange, ref MaxVisionConvinced);
			Vector3 playerPos = Player.transform.position;
			SquareMagnitude = (playerPos - transform.position).sqrMagnitude;
			bool inView = InView(CameraView, playerPos);
			RaycastHit hit;
			if (!Physics.Linecast(transform.position, Player.transform.position, out hit) || hit.collider.tag != "Player"
			    || prisonerDoor.Locked || AI.dizzy)
				Seeing = false;
			else if (SquareMagnitude < ViewRange && inView)
			{
				AI.IsChasing = true;
				if (!IsChasing)
				{
					AI.IsFollowingSound = false; AI.IsWalking = false; AI.IsIdle = false;
					AI.IsSearching = false;
					AI.TimerChoosing = 0; AI.SearchTimer = 0;
					AI.TimerBedCaught = 0;
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
					// Lost the player for long enough: give up and search where she is.
					TimerStopVision = 0;
					AI.MurderNoiseObjs();
					AI.EnemyVision.IsChasing = false;
					AI.TimerNearNoise = 0;
					AI.BC.enabled = false;
					AI.MusicBG.GrannyHunting = false;
					AI.IsFollowingSound = false;
					AI.IsChasing = false;
					AI.IsSearching = true;
					AI.ScreenEffects.NormalSee = false;
					AI.CurrentTarget = AI.transform.position;
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
