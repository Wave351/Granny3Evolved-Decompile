using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/BedHide.c (Granny 3 Evolved, IL2CPP x64).
public partial class BedHide
{
		public BedHide() { Bed = 1; MaxAgain = 2; }
		private void SetInBed(bool value)
		{
			if (Bed == 1f) PlayerStatus.InBed1 = value;
			else if (Bed == 2f) PlayerStatus.InBed2 = value;
			else if (Bed == 3f) PlayerStatus.InBed3 = value;
		}
		private void Update()
		{
			if (TimerAgain < MaxAgain) { TimerAgain += Time.deltaTime; ButtonSprite.enabled = false; }
			else ButtonSprite.enabled = true;
			ButtonSprite.sprite = InBed ? OutSprite : InSprite;
			var crouch = StatusPL.Player ? StatusPL.Player.GetComponent<CrouchHolder>() : null;
			var falling = StatusPL.Player ? StatusPL.Player.GetComponent<FallingHolder>() : null;
			if (NearBed && !StatusPL.IsJumpscared && !StatusPL.Killed && crouch && crouch.Starter
			    && !AISlendrina.PlayerCaughtAttention && falling && !falling.isLanding && !falling.isFalling)
			{
				ButtonSprite.gameObject.SetActive(true);
				if (!Input.GetKeyUp(KeyCode.R) || TimerAgain <= .9f || Paused.IsPaused) return;
				if (!InBed)
				{
					// Getting in while an enemy is watching gives the player away; out of sight it makes them lose track.
					if (GrannyEye.Seeing && GrannyEye.TimerStopVision < .7f) GrannyEye.PlayerSpotted = true;
					else if (!GrannyEye.Seeing && GrannyEye.TimerStopVision > .7f) GrannyEye.AI.UnSeenPlayer = true;
					if (GrandpaEye.Seeing && GrandpaEye.TimerStopVision < .7f) GrandpaEye.PlayerSpotted = true;
					else if (!GrandpaEye.Seeing && GrandpaEye.TimerStopVision > .7f) GrandpaEye.AI.UnSeenPlayer = true;
					RayP.T_HighlightItem.SetActive(false);
					Player.SetActive(false);
					var fps = Player ? Player.GetComponent<MobileFPS>() : null;
					if (!fps) return;
					fps.ResetRotation();
					Player.transform.position = PlayerNewPos.position;
					Player.transform.rotation = PlayerNewPos.rotation;
					SetInBed(true);
					Canvas.enabled = false;
					TimerAgain = 0;
					Handheld.Head = CamHand;
					CamInside.SetActive(true);
					InBed = true;
					Audio.PlayOneShot(GetIn);
				}
				else
				{
					Player.SetActive(true);
					Handheld.Head = OriginalCam;
					TimerAgain = 0;
					SetInBed(false);
					Canvas.enabled = true;
					var stand = Player ? Player.GetComponent<CrouchHolder>() : null;
					if (!stand) return;
					stand.StopCrouch();
					CamInside.SetActive(false);
					InBed = false;
					Audio.PlayOneShot(GetOut);
					GrannyEye.PlayerSpotted = false;
					GrandpaEye.PlayerSpotted = false;
				}
				return;
			}
			ButtonSprite.gameObject.SetActive(false);
		}
		public void DisableDeath() { Handheld.Head = OriginalCam; TimerAgain = 0; Canvas.enabled = true; }
		private void OnTriggerEnter(Collider other) { if (other.gameObject.tag == "Player") NearBed = true; }
		private void OnTriggerExit(Collider other) { if (other.gameObject.tag == "Player") NearBed = false; }
}
