using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/PuzzleDef.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class PuzzleDef
{
	public PuzzleDef()
	{
		allowedItemNames = new List<string>();
		excludeItemNames = new List<string>();
		Priority = 16;
		requiredItemNames = new List<string>();
		forbiddenCombos = new List<PuzzleForbiddenCombo>();
		containedItems = new List<string>();
	}
}
