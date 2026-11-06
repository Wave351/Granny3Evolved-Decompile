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

// Ported from Annotated-Pseudocode/SpikeMelt.c (Granny 3 Evolved, IL2CPP x64).
public partial class SpikeMelt
{
		public SpikeMelt() { spikeMeltTimer = .3f; }
		// After 0.3 seconds the spike fades out over spikeMeltTimer seconds.
		private IEnumerator Start()
		{
			yield return new WaitForSeconds(.3f);
			float t = 0;
			Color c = GetComponent<Renderer>().material.color;
			while (t < spikeMeltTimer)
			{
				t += Time.deltaTime;
				c.a = 1f - Mathf.Clamp01(t / spikeMeltTimer);
				var renderer = GetComponent<Renderer>();
				if (renderer == null || renderer.material == null) yield break;
				renderer.material.color = c;
				yield return null;
			}
			c.a = 0;
			var last = GetComponent<Renderer>();
			if (last != null && last.material != null) last.material.color = c;
		}
}
