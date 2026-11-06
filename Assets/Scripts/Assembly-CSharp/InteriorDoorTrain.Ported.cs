using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/InteriorDoorTrain.c (Granny 3 Evolved, IL2CPP x64).
public partial class InteriorDoorTrain
{
		// Granny or Grandpa standing at a closed train door opens it.
		private void OnTriggerStay(Collider other)
		{
			if (!anim.Closed || anim.AnimatedObject.isPlaying) return;
			string tag = other.gameObject.tag;
			if (tag != "granny" && tag != "granpa") return;
			anim.OpeningClosingAttempts();
		}
}
