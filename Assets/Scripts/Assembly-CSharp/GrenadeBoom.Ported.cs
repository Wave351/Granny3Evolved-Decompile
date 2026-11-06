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

// Ported from Annotated-Pseudocode/GrenadeBoom.c (Granny 3 Evolved, IL2CPP x64).
public partial class GrenadeBoom
{
		private void Start() { StartCoroutine(TimerExplosion()); }
		private IEnumerator TimerExplosion()
		{
			yield return new WaitForSeconds(Timer);
			Instantiate(Explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
}
