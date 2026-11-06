using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/PlankRoof.c (Granny 3 Evolved, IL2CPP x64).
public partial class PlankRoof
{
		public void BreakUp()
		{
			brokenPlanks.SetActive(true);
			Destroy(gameObject);
		}
}
