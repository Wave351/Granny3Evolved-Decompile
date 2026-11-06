using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ToggleOn_CertainColor.c (Granny 3 Evolved, IL2CPP x64).
public partial class ToggleOn_CertainColor
{
		// A slingshot-colour toggle: saves its colour while on and paints the slingshot material.
		private void Start()
		{
			if (PlayerPrefs.GetInt(ItemColoredPref) == PrefIntSpecific) GetComponent<Toggle>().isOn = true;
		}
		private void Update()
		{
			var toggle = GetComponent<Toggle>();
			if (!toggle.isOn) PlayerPrefs.SetInt(ItemColoredPref, 0);
			else
			{
				PlayerPrefs.SetInt(ItemColoredPref, PrefIntSpecific);
				slingshotMat.mainTexture = allowNM && PlayerPrefs.GetInt("Textures") == 1 ? nightmareTexture : textureApplied;
			}
			if (PlayerPrefs.GetInt(SpecificNameForColorPref) == 1) toggle.interactable = true;
		}
}
