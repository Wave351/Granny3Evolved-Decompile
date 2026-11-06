using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ShootLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class ShootLogic
{
		public ShootLogic() { CanPlayerGetShoot = true; }
		private void Update()
		{
			var grandpa = EC.Grandpa ? EC.Grandpa.GetComponent<AI_Grandpa>() : null;
			if (grandpa == null) return;
			// While shooting, Grandpa's spine turns towards the player.
			if (grandpa.IsShooting)
			{
				Vector3 dir = (Player.position - SpineGrandpa.position).normalized;
				Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
				SpineGrandpa.rotation = Quaternion.Slerp(SpineGrandpa.rotation, look, Time.deltaTime * 5f);
			}
			if (!Shooting) { Distance = 0; return; }
			// The shot travels out from the barrel (Distance grows at ShootSpeed).
			Distance += Time.deltaTime * ShootSpeed;
			Rotater.LookAt(Target);
			UnityEngine.Debug.DrawRay(Rotater.position, Rotater.forward * Distance, Color.red);
			RaycastHit hit;
			if (!Physics.Raycast(Rotater.position, Rotater.forward, out hit, Distance, LayersShoot)) return;
			if (grandpa.IsDying) return;
			var falling = Status.Player ? Status.Player.GetComponent<FallingHolder>() : null;
			if (hit.collider.gameObject.tag == "Player" && !WJ.IsJumping && !Status.Killed && falling != null && !falling.isFalling
			    && !falling.isLanding && !grandpa.IsDying && CanPlayerGetShoot && !grandpa.dizzy)
			{
				Instantiate(Impactus, hit.point, Quaternion.identity);
				Instantiate(BloodEffect, hit.point, Quaternion.identity);
				AudioSource.PlayClipAtPoint(ImpactS, hit.point);
				Shooting = false;
				Status.NormalDeath(false);
				if (PlayerPrefs.GetInt("Granny") == 1)
				{
					// Granny stops where she is.
					var granny = EC.Granny ? EC.Granny.GetComponent<AI_Granny>() : null;
					if (granny == null) return;
					granny.MurderNoiseObjs();
					granny.TimerNearNoise = 0;
					granny.BC.enabled = false;
					granny.MusicBG.GrannyHunting = false;
					granny.IsChasing = false;
					granny.IsSearching = false;
					granny.Throwing = false;
					granny.ScreenEffects.NormalSee = false;
					granny.CurrentTarget = granny.transform.position;
					granny.Agent.enabled = false;
					granny.Anim.CrossFade(granny.Idle.name);
					granny.EnemyVision.enabled = false;
					granny.CaughtPlayer = true;
					granny.IsDying = false;
					granny.dizzy = false;
					granny.AtDishes = false;
					granny.AtPiano = false;
				}
			}
			else if (hit.collider.gameObject.tag == "granny" && !Status.IsJumpscared)
			{
				Instantiate(BloodEffect, hit.point, Quaternion.identity);
				AudioSource.PlayClipAtPoint(ImpactS, hit.point);
				Shooting = false;
				EC.GrannyDeathS(0, false);
			}
			else
			{
				string n = hit.collider.gameObject.name;
				if (n == "Subway_TicketRobot" || n == "Subway_TicketRobotHead")
				{
					// The ticket robot takes three shots.
					var robotObject = GameObject.Find("SubwayRobot");
					var robot = robotObject ? robotObject.GetComponent<TicketControlBot>() : null;
					if (robot == null) return;
					if (!robot.dead)
					{
						robot.blowsStruck += 1f;
						if (robot.blowsStruck == 1f) robot.smokeDeath1.SetActive(true);
						else if (robot.blowsStruck == 2f) robot.smokeDeath2.SetActive(true);
						else if (robot.blowsStruck == 3f) robot.DeathUponRobot();
					}
				}
				else
				{
					var data = hit.collider.gameObject.GetComponent<ItemSeedData>();
					if (data && data.itemName == "PropaneTank")
					{
						Instantiate(ExplosionGas, hit.collider.transform.position, hit.collider.transform.rotation);
						Destroy(hit.collider.gameObject);
						UnityEngine.Debug.Log(hit.collider.gameObject.name);
						return;
					}
					Instantiate(SpawnImpact, hit.point, Quaternion.identity);
					var body = hit.collider.GetComponent<Rigidbody>();
					if (body != null) body.AddForce(Rotater.forward * 20f, ForceMode.Impulse);
				}
				AudioSource.PlayClipAtPoint(ImpactS, hit.point);
				Shooting = false;
			}
			UnityEngine.Debug.Log(hit.collider.gameObject.name);
		}
		// Animation event: fire.
		public void Shoot()
		{
			Shooting = true;
			Shot.Play();
			Instantiate(Smoke, Spawner.position, Spawner.rotation);
		}
}
