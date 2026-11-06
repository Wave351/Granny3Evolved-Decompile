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

// Ported from Annotated-Pseudocode/Interactive.c (Granny 3 Evolved, IL2CPP x64).
public partial class Interactive
{
		public Interactive() { isOneTimeAction = true; }
		public void Interacted()
		{
			if (performedAction) return;
			if (isOneTimeAction) performedAction = true;
			performedEvent.Invoke();
			if (isTimed) StartCoroutine(InitiateTimedEvent());
		}
		public IEnumerator InitiateTimedEvent()
		{
			yield return new WaitForSeconds(timerEventTimed);
			timedEvent.Invoke();
		}
}
