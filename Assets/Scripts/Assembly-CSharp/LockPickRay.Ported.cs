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

// Ported from Annotated-Pseudocode/LockPickRay.c (Granny 3 Evolved, IL2CPP x64).
public partial class LockPickRay
{
		public LockPickRay() { distanceRay = 5f; nameCollHitLockPick = "LockPickingCollider"; }
		private void Update()
		{
			// The first time the lock pick is picked up, show how to use it for 5.5 seconds.
			if (!lockPickActivated && lockPickUnused.gameObject.activeSelf)
			{
				lockPickActivated = true;
				StartCoroutine(HowtoUseLockPick());
				lockPickHintKeybind.SetActive(true);
				indicatorKeyLockPick.SetActive(false);
			}
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			UnityEngine.Debug.DrawRay(transform.position, forward * distanceRay, Color.green);
			RaycastHit hit;
			var falling = Player.GetComponent<FallingHolder>();
			availableToLockPick = false;
			if (Physics.Raycast(transform.position, forward, out hit, distanceRay) && !falling.isFalling && !falling.isLanding
			    && !PS.IsJumpscared && hit.collider.gameObject.name == nameCollHitLockPick)
			{
				var door = hit.collider.GetComponentInParent<DoorLogic>();
				if (door.LockpickAvailable && door.Locked)
				{
					availableToLockPick = true;
					if (currentDoor == null) currentDoor = hit.collider.GetComponentInParent<DoorLogic>();
				}
			}
			if (!availableToLockPick)
			{
				lockPickUnused.enabled = true;
				lockPickAvailable.SetActive(false);
				lockPickingSound.SetActive(false);
				lockPicking = false;
				currentDoor = null;
				lockPickMeter.fillAmount = 0;
				lockPickProgress = .08f;
				return;
			}
			// Hold the interact key at a lockable door to pick it.
			lockPickUnused.enabled = false;
			lockPickAvailable.SetActive(true);
			lockPickMeter.fillAmount = lockPickProgress;
			if (!Input.GetKey(PRay.MainInteract) || Paused.IsPaused)
			{
				lockPickingSound.SetActive(false);
				lockPicking = false;
				lockPickProgress = .08f;
				return;
			}
			lockPickProgress += Time.deltaTime * currentDoor.lockPickSpeed;
			lockPickingSound.SetActive(true);
			if (!doorLockPicking.isPlaying) doorLockPicking.Play("PicklockMove");
			if (lockPickProgress < 1f) return;
			currentDoor.UnlockDoor(true);
			var audio = PRay ? PRay.GetComponent<AudioSource>() : null;
			if (audio) audio.PlayOneShot(lockPickedSound);
		}
		private IEnumerator HowtoUseLockPick()
		{
			yield return new WaitForSeconds(5.5f);
			lockPickHintKeybind.SetActive(false);
			indicatorKeyLockPick.SetActive(true);
		}
}
