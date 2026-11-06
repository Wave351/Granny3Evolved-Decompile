using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DeathTriggerBot.c (Granny 3 Evolved, IL2CPP x64).
public partial class DeathTriggerBot
{
		// The facility bot kills Granny/Grandpa (with a small impact) and electrocutes the player.
		private void OnTriggerEnter(Collider hit)
		{
			if (hit.tag == "granny" && !EC.GrannyKilled && !EC.Granny.GetComponent<AI_Granny>().CaughtPlayer)
			{
				EC.GrannyDeathS(0, false);
				Instantiate(ImpactObject, transform.position, Quaternion.identity).GetComponent<ImpactBodies>().PushImpact = 4f;
			}
			if (hit.tag == "granpa" && !EC.GrandpaKilled && !EC.Grandpa.GetComponent<AI_Grandpa>().CaughtPlayer)
			{
				EC.GrandpaDeathS(0, false);
				Instantiate(ImpactObject, transform.position, Quaternion.identity).GetComponent<ImpactBodies>().PushImpact = 4f;
			}
			if (hit.tag == "Player" && !PS.Killed && PS.Player.activeSelf) PS.NormalDeath(true);
		}
}
