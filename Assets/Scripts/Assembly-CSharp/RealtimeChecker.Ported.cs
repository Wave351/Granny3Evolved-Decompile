using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/RealtimeChecker.c (Granny 3 Evolved, IL2CPP x64).
public partial class RealtimeChecker
{
		// With offLightsViaToggle, the particle lights follow the "RealtimeLights" option; otherwise the object is removed.
		private void Start()
		{
			if (offLightsViaToggle)
			{
				if (PlayerPrefs.GetInt("RealtimeLights") == 0)
				{
					var lights = ps.lights;
					lights.enabled = false;
				}
				return;
			}
			gameObject.SetActive(false);
		}
}
