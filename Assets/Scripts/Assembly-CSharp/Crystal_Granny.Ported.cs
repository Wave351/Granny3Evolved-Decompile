using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Crystal_Granny.c (Granny 3 Evolved, IL2CPP x64).
public partial class Crystal_Granny
{
		// The frozen Granny shows her bat only when her weapon option is the bat.
		private void Start()
		{
			if (PlayerPrefs.GetInt("GrannyWP") == 0) bat.SetActive(true);
		}
}
