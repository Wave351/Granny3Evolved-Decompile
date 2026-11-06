using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TrainPower.c (Granny 3 Evolved, IL2CPP x64).
public partial class TrainPower
{
		// Sparks appear once all three switches are flipped.
		private void Update()
		{
			if (!ERC.FlippedSwitch1 || !ERC.FlippedSwitch2 || !ERC.FlippedSwitch3) return;
			if (!ElectricitySpark.activeSelf) ElectricitySpark.SetActive(true);
		}
}
