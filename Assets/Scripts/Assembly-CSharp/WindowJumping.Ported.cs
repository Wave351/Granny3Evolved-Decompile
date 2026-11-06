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

// Ported from Annotated-Pseudocode/WindowJumping.c (Granny 3 Evolved, IL2CPP x64).
public partial class WindowJumping
{
		// Once the crowbar is taken, the subway door unlocks and the window can be climbed through.
		private void Update()
		{
			if (crowBar != null || initiatedEvent) return;
			initiatedEvent = true;
			navObstacle.SetActive(false);
			DLSubwayDoor.Locked = false;
			windowCollision.SetActive(false);
		}
		public void DestroyWindow()
		{
			windowOld.SetActive(false);
			windowBroken.SetActive(true);
		}
		// Climbing through: the camera plays the jump animation, then goes back to the player.
		public void JumpInside()
		{
			IsJumping = true;
			Player.SetActive(false);
			Player.transform.rotation = PosOutside.rotation;
			Player.transform.position = PosOutside.position;
			var fps = Player.GetComponent<MobileFPS>();
			if (fps == null) return;
			fps.ResetRotation();
			var pivot = CamPivot.GetComponent<Animation>();
			if (pivot == null) return;
			pivot.Play("idle");
			Camera.transform.parent = CamGoOutside.transform;
			Camera.transform.localEulerAngles = Vector3.zero;
			Camera.transform.localPosition = Vector3.zero;
			CamGoOutside.gameObject.SetActive(true);
			CamGoOutside.Play("PlayerJumpWindow", PlayMode.StopSameLayer);
			var audio = CamGoOutside.GetComponent<AudioSource>();
			if (audio == null) return;
			audio.Play();
			StartCoroutine(GoingInside());
			climbSFX.SetActive(true);
			windowCollision.layer = LayerMask.NameToLayer("StairColliders");
		}
		private IEnumerator GoingInside()
		{
			yield return new WaitForSeconds(Timer1);
			Camera.transform.parent = CamPivot.transform;
			var pivot = CamPivot ? CamPivot.GetComponent<Animation>() : null;
			if (pivot == null) yield break;
			pivot.Play("idle");
			Player.SetActive(true);
			Camera.transform.localPosition = new Vector3(0, .14168f, 0);
			Camera.transform.localRotation = Quaternion.identity;
			IsJumping = false;
			Camera.transform.localScale = Vector3.one;
			CamGoOutside.gameObject.SetActive(false);
			var collider = crowBar ? crowBar.GetComponent<BoxCollider>() : null;
			if (collider != null) collider.enabled = true;
		}
}
