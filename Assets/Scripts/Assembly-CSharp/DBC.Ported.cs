using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DBC.c (Granny 3 Evolved, IL2CPP x64).
public partial class DBC
{
		// Marks the box stopped once its dynamite has been removed.
		private void Update()
		{
			if (dynamite == null && !stopped) stopped = true;
		}
}
