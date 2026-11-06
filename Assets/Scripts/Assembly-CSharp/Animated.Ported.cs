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

// Ported from Annotated-Pseudocode/Animated.c (Granny 3 Evolved, IL2CPP x64).
public partial class Animated
{
		public Animated() { OpeningTimer = .5f; ClosingTimer = .5f; Closed = true; CanBeLayer = true; EnabledAnim = true; }
		// Doors, drawers and lids: while swinging they are on the "Door" layer, afterwards back on "Non-Ground"
		// (an open GoThru door stays passable).
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
		public void AttemptCloseNewDay()
		{
			if (!Closed && !Locked) CloseAction();
		}
		private IEnumerator OpeningAction()
		{
			yield return new WaitForSeconds(OpeningTimer);
			if (CanBeLayer && !GoThru) gameObject.layer = LayerMask.NameToLayer("Non-Ground");
		}
		private IEnumerator ClosingAction()
		{
			yield return new WaitForSeconds(ClosingTimer);
			if (CanBeLayer) gameObject.layer = LayerMask.NameToLayer("Non-Ground");
		}
		public void UnlockAnimated() { Locked = false; }
}
