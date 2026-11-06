using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/DischargeElectricity.c (Granny 3 Evolved, IL2CPP x64).
public partial class DischargeElectricity
{
		public DischargeElectricity() { timerStartTrigger = .5f; }
		// Live for 0.25 seconds after timerStartTrigger, then removed.
		private IEnumerator Start()
		{
			EC = GameObject.Find("EnemyController");
			yield return new WaitForSeconds(timerStartTrigger);
			ableToImpact = true;
			yield return new WaitForSeconds(.25f);
			Destroy(gameObject);
		}
		// The magic blast knocks Granny's/Grandpa's soul out (soul book captured) and pushes rigidbodies away.
		public void OnTriggerStay(Collider hit)
		{
			if (!ableToImpact) return;
			var controller = EC ? EC.GetComponent<EnemyController>() : null;
			if (hit.tag == "granny" && ableToImpact && controller && !controller.GrannyKilled)
			{
				var granny = controller.Granny ? controller.Granny.GetComponent<AI_Granny>() : null;
				if (granny && !granny.CaughtPlayer)
				{
					granny.BlastedOffSoul();
					Blast(granny.OpenDoorRay.position);
				}
			}
			if (hit.tag == "granpa" && ableToImpact && controller && !controller.GrandpaKilled)
			{
				var grandpa = controller.Grandpa ? controller.Grandpa.GetComponent<AI_Grandpa>() : null;
				if (grandpa && !grandpa.CaughtPlayer)
				{
					grandpa.BlastedOffSoul();
					Blast(grandpa.OpenDoorRay.position);
				}
			}
			var body = hit.GetComponent<Rigidbody>();
			if (body == null) return;
			Vector3 away = (hit.transform.position - transform.position).normalized;
			body.AddForce(away * 4f, ForceMode.Impulse);
		}
		private void Blast(Vector3 at)
		{
			magicBlast.transform.parent = null;
			magicBlast.transform.position = at;
			magicBlast.SetActive(true);
			var rayObject = GameObject.Find("PickRay");
			if (rayObject != null)
			{
				var ray = rayObject.GetComponent<PickRay>();
				if (ray == null) return;
				ray.CaptureSoulBook();
			}
			ableToImpact = false;
		}
}
