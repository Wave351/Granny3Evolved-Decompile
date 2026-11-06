using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ImpactEffect.c (Granny 3 Evolved, IL2CPP x64).
public partial class ImpactEffect
{
		private void Start() { playerHead = GameObject.Find("Main Camera"); }
		private void Update()
		{
			if (playerHead != null) transform.LookAt(playerHead.transform.position);
		}
}
