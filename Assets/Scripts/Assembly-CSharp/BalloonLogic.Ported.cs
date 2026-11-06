using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/BalloonLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class BalloonLogic
{
		public BalloonLogic() { blendInt = 1; inflateSpeedBalloon = 5f; timerInflateOff = 25f; }
		private void Update()
		{
			// While the player rides the balloon, Granny/Grandpa catch them at their door ray instead of their body.
			if (!FPS.gameObject.activeSelf && insideBalloon)
			{
				insideBalloon = false;
				FPS.transform.parent = playerStuff;
			}
			if (FPS.gameObject.activeSelf && insideBalloon)
			{
				AI1.catchTransform = AI1.OpenDoorRay;
				AI2.catchTransform = AI2.OpenDoorRay;
			}
			else
			{
				if (AI1.catchTransform != AI1.transform) AI1.catchTransform = AI1.transform;
				if (AI2.catchTransform != AI2.transform) AI2.catchTransform = AI2.transform;
			}
			if (floating) return;
			// The burner only burns with the gas valve open and the propane tank in place.
			if ((!valveGasOn || propaneTank == null) && fireGas.activeSelf) fireGas.SetActive(false);
			if (valveGasOn) gasSFX.SetActive(true);
			else if (gasSFX.activeSelf) gasSFX.SetActive(false);
			if (valveGasOn && fireGas.activeSelf)
			{
				// Inflate (blend shape towards 0); fully inflated, the balloon rises and the escape starts.
				float step = Time.deltaTime * inflateSpeedBalloon;
				b1.SetBlendShapeWeight(blendInt, Mathf.MoveTowards(b1.GetBlendShapeWeight(blendInt), 0, step));
				b2.SetBlendShapeWeight(blendInt, Mathf.MoveTowards(b2.GetBlendShapeWeight(blendInt), 0, Time.deltaTime * inflateSpeedBalloon));
				if (b2.GetBlendShapeWeight(blendInt) != 0) return;
				var anim = GetComponent<Animation>();
				if (anim == null) return;
				anim.Play("BalloonUp");
				valveGas.name = "GasValve_InWorks";
				floating = true;
				if (E.BL.insideBalloon) E.BL.FPS.transform.parent = E.BL.temporaryParentPlayerBalloon;
				E.StartCoroutine("WaitBalloon");   // Escapes.WaitBalloon is private
				return;
			}
			if (valveGasOn) return;
			if (fireGas.activeSelf) return;
			// Without gas it deflates back (towards 100).
			b1.SetBlendShapeWeight(blendInt, Mathf.MoveTowards(b1.GetBlendShapeWeight(blendInt), 100, Time.deltaTime * 55f));
			b2.SetBlendShapeWeight(blendInt, Mathf.MoveTowards(b2.GetBlendShapeWeight(blendInt), 100, Time.deltaTime * 55f));
		}
		public void BalloonContainPlayer()
		{
			if (insideBalloon) FPS.transform.parent = temporaryParentPlayerBalloon;
		}
		// Getting in or out of the balloon basket.
		public void EnterHotAirBalloon()
		{
			if (!insideBalloon)
			{
				insideBalloon = true;
				FPS.isAllowedToMove = false;
				FPS.ResetRotation();
				FPS.characterController.enabled = false;
				FPS.AbleToMove = false;
				FPS.transform.position = temporaryParentPlayerBalloon.position;
				FPS.transform.rotation = temporaryParentPlayerBalloon.rotation;
				FPS.characterController.enabled = true;
			}
			else
			{
				insideBalloon = false;
				FPS.isAllowedToMove = false;
				FPS.ResetRotation();
				FPS.characterController.enabled = false;
				FPS.transform.position = exteriorPositionTeleport.position;
				FPS.transform.rotation = exteriorPositionTeleport.rotation;
				FPS.characterController.enabled = true;
				FPS.AbleToMove = true;
			}
			FPS.isAllowedToMove = true;
			var crouch = FPS ? FPS.GetComponent<CrouchHolder>() : null;
			if (crouch != null && crouch.IsCrouched) crouch.StopCrouch();
		}
}
