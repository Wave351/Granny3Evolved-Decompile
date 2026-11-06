using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Texture_Changing.c (Granny 3 Evolved, IL2CPP x64).
public partial class Texture_Changing
{
		// Start is empty in Evolved (Legacy also set the texture there).
		private void Update()
		{
			Material.mainTexture = PlayerPrefs.GetInt("Textures") != 1 ? Default : DefaultNM;
		}
}
