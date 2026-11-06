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

// Ported from Annotated-Pseudocode/DestroyTimer.c (Granny 3 Evolved, IL2CPP x64).
public partial class DestroyTimer
{
		public DestroyTimer() { TimeW = 10f; }
		private void Start() { StartCoroutine(TimerGoes()); }
		// Destroys the parent after TimeW seconds, or only this object when onlyItself is set or there is no parent.
		private IEnumerator TimerGoes()
		{
			yield return new WaitForSeconds(TimeW);
			if (!onlyItself && transform.parent != null) Destroy(transform.parent.gameObject);
			else Destroy(gameObject);
		}
}
