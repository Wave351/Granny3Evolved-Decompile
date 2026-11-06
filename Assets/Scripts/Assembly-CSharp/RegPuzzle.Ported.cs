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
using UnityEngine.Events;

// Ported from Annotated-Pseudocode/RegPuzzle.c (Granny 3 Evolved, IL2CPP x64).
public partial class RegPuzzle
{
		// A puzzle spot: with the required item in hand, run performedEvent (once, except for a few repeatable spots).
		public void Interacted()
		{
			if (performedAction) return;
			if (!requiredItem.activeSelf) { performedEventNo.Invoke(); return; }
			string n = gameObject.name;
			if (n != "DumbwaiterButton" && n != "FirewoodPlacedOnFlames" && n != "BookPlaceNoMagicCollider") performedAction = true;
			performedEvent.Invoke();
			if (isTimed) StartCoroutine(InitiateTimedEvent());
		}
		public IEnumerator InitiateTimedEvent()
		{
			yield return new WaitForSeconds(timerEventTimed);
			timedEvent.Invoke();
		}
}
