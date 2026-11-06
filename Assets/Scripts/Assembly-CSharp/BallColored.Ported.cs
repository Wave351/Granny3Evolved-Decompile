using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/BallColored.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class BallColored
{
	private void Start()
	{
		Color color = RandomColor[UnityEngine.Random.Range(0, RandomColor.Length)];
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material.color = color;
		}
	}
}
