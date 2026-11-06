using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/StoneHand.c (Granny 3 Evolved, IL2CPP x64).
public partial class StoneHand
{
		// The held slingshot stone takes the look of the equipped skin.
		private void Start()
		{
			if (PlayerPrefs.GetInt("Slingshot_MagentaON") == 1) Apply(stoneMagenta, stoneMagentaTexture);
			if (PlayerPrefs.GetInt("Slingshot_LavaON") == 1) Apply(stoneLava, stoneLavaTexture);
			if (PlayerPrefs.GetInt("Slingshot_IceON") == 1) Apply(stoneIcy, stoneIcyTexture);
		}
		private void Apply(Mesh mesh, Texture texture)
		{
			mf.mesh = mesh;
			var renderer = mf.GetComponent<Renderer>();
			if (renderer) renderer.material.mainTexture = texture;
		}
}
