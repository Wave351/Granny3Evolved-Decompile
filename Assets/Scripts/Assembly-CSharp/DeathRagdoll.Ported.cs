using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DeathRagdoll.c (Granny 3 Evolved, IL2CPP x64).
public partial class DeathRagdoll
{
		private void Start()
		{
			if (!ElectricDeath) return;
			Electrify.SetActive(true);
			Anim.Play("playerElectric");
		}
}
