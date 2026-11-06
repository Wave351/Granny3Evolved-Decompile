using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TextManager.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class TextManager
{
	public TextManager()
	{
		TimeF = 5f;
		Max = 1.7f;
		ActualTextAmount = 4f;
	}

	private void Update()
	{
		TimeF -= Time.deltaTime * Max;
		if (TimeF < 0f)
		{
			gameObject.SetActive(false);
		}
	}

	public void Add()
	{
		TimeF = ActualTextAmount;
	}

	public void Stop()
	{
		TimeF = 0f;
		gameObject.SetActive(false);
	}
}
