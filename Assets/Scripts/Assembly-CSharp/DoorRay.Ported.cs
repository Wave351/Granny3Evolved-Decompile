using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DoorRay.c (Granny 3 Evolved, IL2CPP x64).
public partial class DoorRay
{
		public DoorRay() { RaycastDis = 5f; }
		private void Update()
		{
			if (Paused.IsPaused) return;
			var falling = Player.GetComponent<FallingHolder>();
			if (Input.GetKeyDown(InteractKey) && Ring.activeSelf && !Paused.IsPaused && !falling.isFalling && !WJ.IsJumping && !PlayerStatus.IsJumpscared)
				buttonClicked = true;
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			UnityEngine.Debug.DrawRay(transform.position, forward * RaycastDis, Color.green);
			RaycastHit hit;
			if (!Physics.Raycast(transform.position, forward, out hit, RaycastDis)
			    || falling.isFalling || falling.isLanding || WJ.IsJumping || PlayerStatus.IsJumpscared || dayStuff.WakingUp)
			{
				Nothing();
				return;
			}
			GameObject target = hit.collider.gameObject;
			// Lockable doors.
			var door = target.GetComponent<DoorLogic>();
			if (door && !door.AnimatedObject.isPlaying)
			{
				if (!ShowRing()) return;
				door.OpeningClosingAttempts();
				Ring.SetActive(false);
				return;
			}
			// Other doors, drawers and lids.
			var animated = target.GetComponent<Animated>();
			if (animated && !animated.AnimatedObject.isPlaying)
			{
				if (!ShowRing()) return;
				animated.OpeningClosingAttempts();
				Ring.SetActive(false);
				return;
			}
			var crouch = Player.GetComponent<CrouchHolder>();
			if (target.name == "WindowOpen" && !target.GetComponent<Animation>().isPlaying && !BG.InOutside
			    && !crouch.Anim.isPlaying && !crouch.IsCrouched)
			{
				if (!ShowRing()) return;
				Ring.SetActive(false);
				return;
			}
			if (target.name == "PlayPianoCollider")
			{
				// The piano is loud: Granny comes.
				if (!ShowRing()) return;
				var piano = hit.collider.GetComponent<AudioSource>();
				if (piano == null) return;
				piano.Play();
				Instantiate(NoiseGranny, PianoNoiseSpawn.position, Quaternion.identity);
				return;
			}
			var interactive = hit.collider.GetComponent<Interactive>();
			if (interactive && interactive.enabled)
			{
				var sound = hit.collider.GetComponent<AudioSource>();
				if (sound && !sound.isPlaying)
				{
					if (!ShowRing()) return;
					interactive.Interacted();
					if (target.name != "CollisionFlushToilet") return;
					// Flushing the toilet is a noise for both of them.
					Instantiate(NoiseGranny, ToiletNoiseSpawn.position, Quaternion.identity);
					Instantiate(NoiseGrandpa, ToiletNoiseSpawn.position, Quaternion.identity);
					return;
				}
				if (!sound)
				{
					if (!ShowRing()) return;
					interactive.Interacted();
					return;
				}
			}
			if (target.name == "WaterCoverEntireHouse")
			{
				PickRay.TurnOffItemTexts();
				Ring.SetActive(true);
				if (!buttonClicked) return;
				buttonClicked = false;
				if (PickRay.H_PAVAS.activeSelf || PickRay.H_GRVAS.activeSelf)
				{
					PickRay.TimedTextInitiate("This water is disgusting and dangerous. Not worthy of being in the vase");
					return;
				}
				PickRay.StopSetOfTexts();
				PickRay.T_NeedShit.SetActive(true);
				var text = PickRay.T_NeedShit.GetComponent<TextManager>();
				if (text == null) return;
				text.Add();
				PickRay.TimedTextInitiate("Foul water");
				return;
			}
			if (target.name == "ColliderBirdNest" && !ScreenEffects.Hurt)
			{
				if (!ShowRing()) return;
				CC.AttemptSteal();
				Ring.SetActive(false);
				return;
			}
			if (target.name == "GuillotineHandle" && !GA.anim.isPlaying)
			{
				if (!ShowRing()) return;
				GA.Start1();
				Ring.SetActive(false);
				return;
			}
			if (target.name == "JumpColliderWindow" && !PickRay.ShootHead.isPlaying && !crouch.Anim.isPlaying && !crouch.IsCrouched)
			{
				if (!ShowRing()) return;
				WJ.JumpInside();
				Ring.SetActive(false);
				target.name = "JumpedColliderWindow";
				return;
			}
			// Hot air balloon parts just get highlighted.
			GameObject highlight = null;
			switch (target.name)
			{
				case "GasValve_Collider": highlight = gasValveHighlight; break;
				case "BalloonCollider": highlight = firstHotAirPart3Highlight; break;
				case "PropaneTank_Collider": highlight = propaneTankHighlight; break;
				case "PlaceBasketCollision": highlight = firstHotAirPart1Highlight; break;
				case "HotAirBalloonTopCollider": highlight = firstHotAirPart2Highlight; break;
			}
			if (highlight == null) { Nothing(); return; }
			highlight.SetActive(true);
		}
		// Shows the interaction ring; true when the player clicked this frame.
		private bool ShowRing()
		{
			PickRay.T_HighlightItem.SetActive(false);
			Ring.SetActive(true);
			if (!buttonClicked) return false;
			buttonClicked = false;
			return true;
		}
		private void Nothing()
		{
			Ring.SetActive(false);
			buttonClicked = false;
			firstHotAirPart1Highlight.SetActive(false);
			firstHotAirPart2Highlight.SetActive(false);
			firstHotAirPart3Highlight.SetActive(false);
			propaneTankHighlight.SetActive(false);
			if (gasValveHighlight != null) gasValveHighlight.SetActive(false);
		}
		private void HandleButtonClick() { buttonClicked = true; }
}
