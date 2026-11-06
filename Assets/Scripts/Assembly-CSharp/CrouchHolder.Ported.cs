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

// Ported from Annotated-Pseudocode/CrouchHolder.c (Granny 3 Evolved, IL2CPP x64).
public partial class CrouchHolder
{
		public CrouchHolder() { Starter = true; RadiusSphere = .5f; RadiusSphere2 = .5f; RadiusSphere3 = 2f; }
		private void Start()
		{
			originalCenter = controller.center;
			originalHeight = controller.height;
		}
		private void Update()
		{
			// The crouch key toggles crouching (not while falling/landing, paused or waking up).
			if (Input.GetKeyDown(CrouchKey) && !HolderF.isLanding && !HolderF.isFalling && !Disabled && !Paused.IsPaused && !days.WakingUp)
			{
				if (!IsCrouched)
				{
					if (Starter)
					{
						StartCrouch();
						IsCrouched = true;
					}
				}
				else if (Starter && CanStandUp())
				{
					StopCrouch();
					IsCrouched = false;
				}
			}
			// Which floor the player stands on picks the footstep sounds.
			RaycastHit hit;
			if (!Physics.Raycast(checkGround.transform.position, Vector3.down, out hit, 1.5f)) return;
			UnityEngine.Debug.DrawRay(checkGround.transform.position, Vector3.down * 1.5f, Color.green);
			if (hit.collider.CompareTag("floor") || hit.collider.CompareTag("Noice")) WhichFloor(false, true, false);
			else if (hit.collider.CompareTag("water")) WhichFloor(false, false, true);
			else if (hit.collider.CompareTag("grus")) WhichFloor(true, false, false);
		}
		// There must be room for the standing capsule.
		public bool CanStandUp()
		{
			float radius = controller.radius;
			Vector3 bottom = transform.position + controller.center;
			Vector3 top = bottom + Vector3.up * (originalHeight - radius);
			return !Physics.CheckCapsule(bottom, top, radius, MasksCrouch, QueryTriggerInteraction.Ignore);
		}
		public void WhichFloor(bool outside, bool house, bool water)
		{
			StepsHold.Outside = outside;
			StepsHold.House = house;
			StepsHold.Water = water;
		}
		public void StartCrouch()
		{
			if (!Starter) return;
			Starter = false;
			Hold.PlayOneShot(C);
			isCrouching = true;
			Anim.Play("PlayerHukarSig");
			StartCoroutine(Crouch());
			FPS2.IsCrouched = true;
		}
		public void StopCrouch()
		{
			if (!Starter) return;
			StandUp(1f);
		}
		// Getting caught stands the player up quickly.
		public void CrouchStopJumpscare() { StandUp(1.5f); }
		private void StandUp(float speed)
		{
			IsCrouched = false;
			Hold.PlayOneShot(S);
			Starter = false;
			On2 = true;
			isCrouching = false;
			Anim["PlayerReserSig"].speed = speed;
			Anim.Play("PlayerReserSig");
			StartCoroutine(Stand());
			FPS2.IsCrouched = false;
		}
		private IEnumerator Crouch()
		{
			yield return new WaitForSeconds(Anim.GetClip("PlayerHukarSig").length);
			Starter = true;
		}
		private IEnumerator Stand()
		{
			yield return new WaitForSeconds(Anim.GetClip("PlayerReserSig").length);
			Starter = true;
		}
}
