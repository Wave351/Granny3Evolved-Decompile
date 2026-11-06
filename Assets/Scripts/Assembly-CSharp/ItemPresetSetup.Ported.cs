using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ItemPresetSetup.c (Granny 3 Evolved, IL2CPP x64).
public partial class ItemPresetSetup
{
		// Moves the item to its spot for the chosen item preset (Extras spot when Extras is on and one exists).
		private void Start()
		{
			if (PlayerPrefs.GetInt("FlashL") != 0) return;   // the seed system places items instead
			float preset = OM.PresetChoosenCurrent;
			Transform[] mains = { PositionMain_P1, PositionMain_P2, PositionMain_P3, PositionMain_P4, PositionMain_P5, PositionMain_P6 };
			Transform[] extras = { PositionExtra_P1, PositionExtra_P2, PositionExtra_P3, PositionExtra_P4, PositionExtra_P5, PositionExtra_P6 };
			for (int i = 0; i < 6; i++)
			{
				if (preset != i) continue;
				NewPosition = PlayerPrefs.GetInt("Extras") == 1 && extras[i] != null ? extras[i] : mains[i];
			}
			if (!WorldLocal)
			{
				transform.position = NewPosition.position;
				transform.rotation = NewPosition.rotation;
			}
			else
			{
				transform.localPosition = NewPosition.localPosition;
				transform.localRotation = NewPosition.localRotation;
			}
		}
}
