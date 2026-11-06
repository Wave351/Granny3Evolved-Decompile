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

// Ported from Annotated-Pseudocode/FallingHolder.c (Granny 3 Evolved, IL2CPP x64).
public partial class FallingHolder
{
		public FallingHolder() { FallDurationHold = 1f; FallChecker = 1f; FallMega = 3.5f; Speed = .3f; groundCheckDis = .2f; }
		private void Update()
		{
			if (DurateCan > .5f) Falling.volume = .25f;
			if (!ch.isGrounded && !FPS.InElev)
			{
				// In the air: time the fall.
				if (!isLanding)
				{
					isFalling = true;
					CanFallSound = true;
					fallDuration += Time.deltaTime * Speed;
					DurateCan += Time.deltaTime * 4f;
					return;
				}
			}
			else
			{
				// Landed: a long fall kills, a medium one knocks the player down (drops the item on Impossible/Hollow),
				// a short one is a small landing.
				DurateCan = 0;
				if (fallDuration > FallMega)
				{
					if (!Fell)
					{
						CanFallSound = false; isFalling = false;
						Falling.volume = 0;
						fallDuration = 0;
						PlayerStatus.NormalDeath(false);
						PlayerStatus.DamageFeet();
					}
				}
				else if (fallDuration > FallDurationHold)
				{
					if (!Damaged)
					{
						Damaged = true;
						HandleLanding(false);
						CanFallSound = false; isFalling = false;
						DurateCan = 0;
						int diff = PlayerPrefs.GetInt("Diff");
						if (diff == 4 || diff == 5) Ray1.CheckItemDropping();
					}
				}
				else if (fallDuration > FallChecker)
				{
					var crouch = GetComponent<CrouchHolder>();
					if (crouch.isCrouching) GetComponent<CrouchHolder>().StopCrouch();
					LandTiny.Play();
					CanFallSound = false; isFalling = false;
					Falling.volume = 0;
					fallDuration = 0;
				}
				else
				{
					CanFallSound = false; isFalling = false;
					fallDuration = 0;
					Falling.volume = 0;
				}
				if (!isLanding) return;
			}
			if (GetComponent<CrouchHolder>().isCrouching) GetComponent<CrouchHolder>().StopCrouch();
		}
		private void CheckForLanding()
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDis)) HandleLanding(false);
		}
		// A hard landing (hit = false) or being knocked down (hit = true, drops the held item).
		public void HandleLanding(bool hit)
		{
			isLanding = true;
			FPS.ReserRotation();
			string clip;
			if (!hit)
			{
				(fsPlayer.Outside ? LandSoundGrass : LandSound).SetActive(true);
				clip = "playerLandBad";
			}
			else clip = "KnockedDown";
			CanFallSound = false; isFalling = false;
			Falling.volume = 0;
			fallDuration = 0;
			FPS.isAllowedToMove = false;
			CanCamSmooth = true;
			FPS.CanFade = true;
			playerAnimation.Play(clip);
			var crouch = GetComponent<CrouchHolder>();
			if (crouch == null) return;
			if (crouch.isCrouching) GetComponent<CrouchHolder>().StopCrouch();
			if (hit) Ray1.CheckItemDropping();
			StartCoroutine(WaitForAnimationEnd(clip));
		}
		private IEnumerator WaitForAnimationEnd(string animName)
		{
			var clip = playerAnimation ? playerAnimation.GetClip(animName) : null;
			if (clip == null) yield break;
			yield return new WaitForSeconds(clip.length);
			LandSound.SetActive(false);
			LandSoundGrass.SetActive(false);
			FPS.isAllowedToMove = true;
			CanCamSmooth = false;
			FPS.ResetRotation();
			FPS.CanFade = false;
			fallDuration = 0;
			yield return new WaitForSeconds(.2f);
			Ray1.buttonClicked = false;
			Ray1.buttonClicked2 = false;
			Ray2.buttonClicked = false;
			isLanding = false;
			Damaged = false;
		}
		public void TestStop()
		{
			LandSound.SetActive(false);
			LandSoundGrass.SetActive(false);
			CanCamSmooth = false;
			FPS.ResetRotation();
			FPS.CanFade = false;
			fallDuration = 0;
			isLanding = false;
			Damaged = false;
		}
}
