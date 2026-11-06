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

// Ported from Annotated-Pseudocode/PropaneExplosion.c (Granny 3 Evolved, IL2CPP x64).
public partial class PropaneExplosion
{
		public PropaneExplosion() { PushImpact = 15f; CanKillPlayer = true; deathTimer = .2f; availableLethal = true; }
		// The blast is only lethal for deathTimer seconds.
		private IEnumerator Start()
		{
			EC = GameObject.Find("EnemyController");
			PlayerStatus = GameObject.Find("PlayerManage");
			yield return new WaitForSeconds(deathTimer);
			availableLethal = false;
		}
		private void OnTriggerEnter(Collider hit)
		{
			if (!availableLethal) return;
			var ec = EC ? EC.GetComponent<EnemyController>() : null;
			if (hit.tag == "granny")
			{
				if (ec == null) return;
				if (!ec.GrannyKilled)
				{
					var ai = ec.Granny ? ec.Granny.GetComponent<AI_Granny>() : null;
					if (ai == null) return;
					if (!ai.CaughtPlayer)
					{
						ec.GrannyDeathS(0, false);
						var impact = Instantiate(ImpactObject, transform.position, Quaternion.identity).GetComponent<ImpactBodies>();
						if (impact == null) return;
						impact.PushImpact = 12f;
					}
				}
			}
			if (hit.tag == "granpa")
			{
				if (ec == null) return;
				if (!ec.GrandpaKilled)
				{
					var ai = ec.Grandpa ? ec.Grandpa.GetComponent<AI_Grandpa>() : null;
					if (ai == null) return;
					if (!ai.CaughtPlayer)
					{
						ec.GrandpaDeathS(0, false);
						var impact = Instantiate(ImpactObject, transform.position, Quaternion.identity).GetComponent<ImpactBodies>();
						if (impact == null) return;
						impact.PushImpact = 12f;
					}
				}
			}
			var body = hit.GetComponent<Rigidbody>();
			if (CanKillPlayer && hit.tag == "Player")
			{
				var status = PlayerStatus ? PlayerStatus.GetComponent<PlayerStatus>() : null;
				if (status == null) return;
				if (!status.Killed && status.Player != null && status.Player.activeSelf) status.NormalDeath(false);
			}
			string n = hit.gameObject.name;
			if (n == "GrandpaHitBody")
			{
				var controllerObject = GameObject.Find("EnemyController");
				var controller = controllerObject ? controllerObject.GetComponent<EnemyController>() : null;
				if (controller == null) return;
				if (!controller.GrandpaKilled)
				{
					var ai = controller.Grandpa ? controller.Grandpa.GetComponent<AI_Grandpa>() : null;
					if (ai == null) return;
					if (!ai.CaughtPlayer) controller.GrandpaDeathSBed(0, false);
				}
			}
			if (n == "Subway_TicketRobot")
			{
				var bot = hit.gameObject.GetComponentInParent<TicketControlBot>();
				if (bot == null) return;
				if (!bot.dead) bot.DeathUponRobot();
			}
			if (n == "Facility_TicketRobot" || n == "Facility_TicketRobotHead")
			{
				// Propane can't destroy the facility robot.
				var robotObject = GameObject.Find("FacilityRobot");
				var rayObject = GameObject.Find("PickRay");
				var robot = robotObject ? robotObject.GetComponent<FacilityBot>() : null;
				if (robot == null) return;
				if (!robot.dead)
				{
					var ray = rayObject ? rayObject.GetComponent<PickRay>() : null;
					if (ray == null) return;
					ray.TimedTextInitiate("This robot is too strong. I might need something else");
				}
			}
			if (n == "CrowNest" || n == "ColliderBirdNest")
			{
				var nest = GameObject.Find("CrowNest");
				var crow = nest ? nest.GetComponent<CrowChimney>() : null;
				if (crow == null) return;
				crow.KillAssBird();
			}
			if (n == "BlockShopPlank")
			{
				if (!Break(hit.gameObject, "BlockShopPlankBroke")) return;
				Destroy(hit.GetComponent<RegPuzzle>());
			}
			if (n == "ImpossiblePlank")
			{
				if (!Break(hit.gameObject, "PlankBroke")) return;
				var timer = hit.gameObject.GetComponent<DestroyTimer>();
				if (timer == null) return;
				timer.enabled = true;
				Destroy(hit.GetComponent<RegPuzzle>());
			}
			if (body == null) return;
			body.AddForce((hit.transform.position - transform.position).normalized * PushImpact, ForceMode.Impulse);
		}
		private static bool Break(GameObject plank, string brokenName)
		{
			var audio = plank.GetComponent<AudioSource>();
			if (audio == null) return false;
			audio.Play();
			var body = plank.GetComponent<Rigidbody>();
			if (body == null) return false;
			body.isKinematic = false;
			plank.layer = LayerMask.NameToLayer("Item");
			plank.name = brokenName;
			return true;
		}
}
