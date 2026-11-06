using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Crystal_Grandpa.c (Granny 3 Evolved, IL2CPP x64).
public partial class Crystal_Grandpa
{
		// The frozen Grandpa holds the shotgun if the real Grandpa has it.
		private void Start()
		{
			EC = GameObject.Find("EnemyController");
			var controller = EC.GetComponent<EnemyController>();
			if (controller == null || controller.ShotgunGrandpa == null) return;
			if (controller.ShotgunGrandpa.activeSelf) Shotgun.SetActive(true);
		}
}
