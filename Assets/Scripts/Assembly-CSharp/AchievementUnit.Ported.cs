using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/AchievementUnit.c (Granny 3 Evolved, IL2CPP x64).
public partial class AchievementUnit
{
		// Unlocked achievements are shown in green.
		private void Start()
		{
			if (PlayerPrefs.GetInt(saveAchievement) == 1) ImageAch.color = new Color(0, 1, 0, 1);
		}
}
