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

// Ported from Annotated-Pseudocode/LoadingDetail.c (Granny 3 Evolved, IL2CPP x64).
public partial class LoadingDetail
{
		public LoadingDetail() { SecondPass = .3f; }
		private void Start() { StartCoroutine(TimerGo()); }
		private IEnumerator TimerGo()
		{
			string[] frames = { "Loading.", "Loading..", "Loading...", "Loading.", "Loading..", "Loading..." };
			foreach (string frame in frames)
			{
				Text.text = frame;
				yield return new WaitForSeconds(SecondPass);
			}
			StartCoroutine(TimerGo());
		}
}
