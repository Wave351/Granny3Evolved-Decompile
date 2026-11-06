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

// Ported from Annotated-Pseudocode/DoorLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class DoorLogic
{
		public DoorLogic() { lockPickSpeed = .07f; }
		public void OpenAction()
		{
			AnimatedObject.Play(OpenAnimation.name);
			if (CanBeLayer) gameObject.layer = LayerMask.NameToLayer("Door");
			Audio.PlayOneShot(OpenSound);
			Closed = false;
			StartCoroutine(OpeningAction());
		}
		public void CloseAction()
		{
			AnimatedObject.Play(CloseAnimation.name);
			if (CanBeLayer) gameObject.layer = LayerMask.NameToLayer("Door");
			Audio.PlayOneShot(CloseSound);
			Closed = true;
			StartCoroutine(ClosingAction());
		}
		public void OpeningClosingAttempts()
		{
			if (Locked)
			{
				AnimatedObject.Play(LockedAnim.name);
				Audio.PlayOneShot(LockedSound);
				return;
			}
			if (!Closed) CloseAction();
			else OpenAction();
		}
		// Each new day open doors close again, and lock-pickable doors lock again.
		public void AttemptCloseNewDay()
		{
			if (!Closed && !Locked) CloseAction();
			if (!LockpickAvailable) return;
			Locked = true;
			lockPickCollider.SetActive(true);
		}
		private IEnumerator OpeningAction()
		{
			yield return new WaitForSeconds(OpeningTimer);
			if (CanBeLayer && !goThruDoor) gameObject.layer = LayerMask.NameToLayer("Non-Ground");
		}
		private IEnumerator ClosingAction()
		{
			yield return new WaitForSeconds(ClosingTimer);
			if (CanBeLayer) gameObject.layer = LayerMask.NameToLayer("Non-Ground");
		}
		// An enemy gets through a locked door: it unlocks, then closes and locks again after 4 seconds.
		public void EnemyOpeningFromLockState()
		{
			Locked = false;
			StartCoroutine(EnemyOpeningFromLockStateTimed());
		}
		public IEnumerator EnemyOpeningFromLockStateTimed()
		{
			yield return new WaitForSeconds(4f);
			if (!Closed) CloseAction();
			Locked = true;
		}
		// Unlocking by lock pick makes the next pick a little faster (more with lockPickHighSpedUp).
		public void UnlockDoor(bool lockpick)
		{
			Locked = false;
			if (lockpick) lockPickSpeed += .05f;
			if (lockPickCollider != null) lockPickCollider.SetActive(false);
			if (lockPickHighSpedUp) lockPickSpeed += .2f;
		}
}
