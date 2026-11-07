using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EnemyController.c (Granny 3 Evolved, IL2CPP x64).
public partial class EnemyController
{
		public EnemyController() { MaxTimerRecoverGranny = 60f; MaxTimerRecoverGrandpa = 60f; MaxBedSleep = 30f; }
		private void Start()
		{
			// Granny is always in the house on difficulties 3 and 4, even when switched off.
			int diff = PlayerPrefs.GetInt("Diff");
			if (PlayerPrefs.GetInt("Granny") == 0 && diff != 3 && diff != 4) GrannyStuff.SetActive(false);
			if (PlayerPrefs.GetInt("Grandpa") == 0) GrandpaStuff.SetActive(false);
			Grandpa.SetActive(false);
			Granny.SetActive(false);
		}
		private void Update()
		{
			if (!GrannyKilled) TimerGrannyRecover = 0;
			else
			{
				TimerGrannyRecover += Time.deltaTime;
				if (TimerGrannyRecover >= MaxTimerRecoverGranny)
				{
					TimerGrannyRecover = 0;
					GrannyKilled = false;
					SpawnGranny();
				}
			}
			if (!GrandpaKilled) TimerGrandpaRecover = 0;
			else
			{
				TimerGrandpaRecover += Time.deltaTime;
				if (TimerGrandpaRecover >= MaxTimerRecoverGrandpa)
				{
					TimerGrandpaRecover = 0;
					GrandpaKilled = false;
					SpawnGrandpa();
				}
			}
			// Grandpa naps for MaxBedSleep seconds, then gets up.
			if (!GrandpaBed.activeSelf) TimerInBed = 0;
			else
			{
				TimerInBed += Time.deltaTime;
				if (TimerInBed >= MaxBedSleep)
				{
					GrandpaBed.SetActive(false);
					MurderNoise2();
					Grandpa.SetActive(true);
					Grandpa.GetComponent<AI_Grandpa>().ChooseWaypoint();
				}
			}
			ShotgunGrandpaBed.SetActive(ShotgunGrandpa.activeSelf);
		}
		// type: 0 normal, 1 lava, 2 frozen, 3 crystal (the special bodies only come from the slingshot).
		public void GrannyDeathS(int type, bool isSling)
		{
			int diff = PlayerPrefs.GetInt("Diff");
			Instantiate(N1, RagdollPSGranny.transform.position, RagdollPSGranny.transform.rotation);
			GrannyKilled = true;
			GameObject body = Instantiate(RagdollBody(type, isSling, RagdollGranny, RagdollGrannyFrozen, RagdollGrannyCrystal),
				RagdollPSGranny.transform.position, RagdollPSGranny.transform.rotation);
			TH.AlwaysAngry1 = false;
			Granny.GetComponent<AI_Granny>().DeadDisable();
			Granny.SetActive(false);
			MaxTimerRecoverGranny = Recovery(diff, MaxTimerRecoverGranny);
			var ragdoll = body.GetComponent<R_Granny>();
			if (diff >= 0 && diff <= 4)
			{
				GrannyGoneText.text = "Granny is now gone for " + RecoveryText(diff);
				if (ragdoll) ragdoll.TimerGone = BodyTime(diff);
			}
			if (type == 1 && isSling && ragdoll) ragdoll.isLava = true;
			if (GrannyGoneText == null) return;
			var text = GrannyGoneText.GetComponent<TextManager>();
			if (text == null) return;
			text.Add();
			GrannyGoneText.gameObject.SetActive(true);
			// She drops whatever she was doing at the sink or the piano.
			var ai = Granny.GetComponent<AI_Granny>();
			if (ai == null) return;
			ai.AtDishes = false;
			ai.dizzy = false;
			if (ai.DishesDone == null) return;
			ai.DishesDone.SetActive(false);
			ai.AtPiano = false;
			if (ai.PianoSing == null) return;
			ai.PianoSing.SetActive(false);
			ES.UsedWPOnce = true;
		}
		public void GrandpaDeathS(int type, bool isSling) { DefeatGrandpa(type, isSling, false); }
		public void GrandpaDeathSBed(int type, bool isSling) { DefeatGrandpa(type, isSling, true); }
		private void DefeatGrandpa(int type, bool isSling, bool inBed)
		{
			int diff = PlayerPrefs.GetInt("Diff");
			Instantiate(N2, RagdollPSGrandpa.transform.position, RagdollPSGrandpa.transform.rotation);
			GrandpaKilled = true;
			Transform spot = inBed ? RagdollPSGrandpaBed : RagdollPSGrandpa;
			GameObject body = Instantiate(RagdollBody(type, isSling, RagdollGrandpa, RagdollGrandpaFrozen, RagdollGrandpaCrystal),
				spot.transform.position, spot.transform.rotation);
			TH.AlwaysAngry2 = false;
			if (inBed) GrandpaBed.SetActive(false);
			Grandpa.GetComponent<AI_Grandpa>().DeadDisable();
			Grandpa.SetActive(false);
			MaxTimerRecoverGrandpa = Recovery(diff, MaxTimerRecoverGrandpa);
			var ragdoll = body.GetComponent<R_Grandpa>();
			if (diff >= 0 && diff <= 4)
			{
				GrandpaGoneText.text = "Grandpa is now gone for " + RecoveryText(diff);
				if (ragdoll) ragdoll.TimerGone = BodyTime(diff);
			}
			if (type == 1 && isSling && ragdoll) ragdoll.isLava = true;
			if (GrandpaGoneText == null) return;
			var text = GrandpaGoneText.GetComponent<TextManager>();
			if (text == null) return;
			text.Add();
			GrandpaGoneText.gameObject.SetActive(true);
			if (inBed) { ES.UsedWPOnce = true; return; }
			var ai = Grandpa.GetComponent<AI_Grandpa>();
			if (ai == null) return;
			ai.dizzy = false;
			ES.UsedWPOnce = true;
		}
		private static GameObject RagdollBody(int type, bool isSling, GameObject normal, GameObject frozen, GameObject crystal)
		{
			if (!isSling) return normal;
			if (type == 2) return frozen;
			if (type == 3) return crystal;
			return normal;
		}
		private static float Recovery(int diff, float current)
		{
			switch (diff)
			{
				case 0: return 150f;
				case 1: return 90f;
				case 2: return 60f;
				case 3: return 30f;
				case 4: return 15f;
			}
			return current;
		}
		private static string RecoveryText(int diff)
		{
			switch (diff)
			{
				case 0: return "2 minutes and 30 seconds";
				case 1: return "1 minute and 30 seconds";
				case 2: return "1 minute ";
				case 3: return "30 seconds ";
				default: return "15 seconds ";
			}
		}
		// How long the body lies there.
		private static float BodyTime(int diff)
		{
			switch (diff)
			{
				case 0: return 60f;
				case 1: return 30f;
				case 4: return 7.5f;
				default: return 15f;
			}
		}
		public void MurderNoise1()
		{
			foreach (var noise in GameObject.FindGameObjectsWithTag("noiseobject")) if (noise != gameObject) Destroy(noise);
		}
		public void MurderNoise2()
		{
			foreach (var noise in GameObject.FindGameObjectsWithTag("noiseobject_grandpa")) if (noise != gameObject) Destroy(noise);
		}
		public void EnemyRearrange()
		{
			// The original sends both of them to Pos1Granny.
			var agent = Granny.GetComponent<NavMeshAgent>();
			if (agent == null) return;
			agent.enabled = false;
			agent.baseOffset = -.15f;
			Granny.transform.position = Pos1Granny.position;
			Granny.SetActive(false);
			agent = Grandpa.GetComponent<NavMeshAgent>();
			if (agent == null) return;
			agent.enabled = false;
			agent.baseOffset = -.15f;
			Grandpa.transform.position = Pos1Granny.position;
			Grandpa.SetActive(false);
		}
		public void SpawnGranny()
		{
			var ai = Granny.GetComponent<AI_Granny>();
			if (ai == null) return;
			ai.IsWalking = false;
			ai.IsIdle = false;
			ai.NeedsToBirthTrap = false;
			ai.IsSearching = false;
			ai.IsChasing = false;
			ai.UnSeenPlayer = false;
			ai.IsFollowingSound = false;
			ai.dizzy = false;
			ai.IsDying = false;
			ai.ResetCertainTimer();
			ai.PlayerClose = false;
			var agent = Granny.GetComponent<NavMeshAgent>();
			agent.enabled = false;
			ai.Anim.Play(ai.Idle.name);
			ai.Throwing = false;
			Granny.SetActive(true);
			MurderNoise1();
			// Spawn on a different floor from the player.
			Transform spot = null;
			int r;
			if (SpawnCheck.Floor1) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Granny : r == 2 ? Pos2Granny : r == 3 ? Pos4Granny : null; }
			else if (SpawnCheck.Floor2) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Granny : r == 2 ? Pos3Granny : r == 3 ? Pos4Granny : null; }
			else if (SpawnCheck.Floor3) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos4Granny : r == 2 ? Pos3Granny : r == 3 ? Pos2Granny : null; }
			else if (SpawnCheck.Cellar) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Granny : r == 2 ? Pos3Granny : r == 3 ? Pos2Granny : null; }
			else { r = UnityEngine.Random.Range(1, 5); spot = r == 1 ? Pos3Granny : r == 2 ? Pos4Granny : r == 3 ? Pos1Granny : r == 4 ? Pos2Granny : null; }
			if (spot != null) Granny.transform.position = spot.position;
			GrannyKilled = false;
			TimerGrannyRecover = 0;
			EF1.InWater = false;
			if (ai.EnemyVision == null) return;
			ai.EnemyVision.IsChasing = false;
			agent.enabled = true;
			ai.EnemyVision.enabled = true;
			ai.CaughtPlayer = false;
			ai.ChooseWaypoint();
			var steps = ai.Anim.GetComponent<EnemyFootstep>();
			if (steps == null) return;
			steps.CurrentGround = 1f;
			ai.AtDishes = false;
			ai.AtPiano = false;
		}
		public void SpawnGrandpa()
		{
			var ai = Grandpa.GetComponent<AI_Grandpa>();
			if (ai == null) return;
			ai.IsWalking = false;
			ai.IsIdle = false;
			ai.NeedsToBirthTrap = false;
			ai.IsSearching = false;
			ai.IsChasing = false;
			ai.UnSeenPlayer = false;
			ai.dizzy = false;
			ai.IsFollowingSound = false;
			ai.IsDying = false;
			ai.ResetCertainTimer();
			ai.PlayerClose = false;
			var agent = Grandpa.GetComponent<NavMeshAgent>();
			agent.enabled = false;
			ai.Anim.Play(ai.Idle.name);
			ai.Anim.GetComponent<ShootLogic>().Shooting = false;
			Grandpa.SetActive(true);
			MurderNoise2();
			Transform spot = null;
			int r;
			if (SpawnCheck.Floor1) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Grandpa : r == 2 || r == 3 ? Pos2Grandpa : null; }
			else if (SpawnCheck.Floor2) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Grandpa : r == 2 ? Pos3Grandpa : r == 3 ? Pos4Grandpa : null; }
			else if (SpawnCheck.Floor3) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos3Grandpa : r == 2 ? Pos2Grandpa : r == 3 ? Pos4Grandpa : null; }
			else if (SpawnCheck.Cellar) { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos1Grandpa : r == 2 ? Pos2Grandpa : r == 3 ? Pos3Grandpa : null; }
			else { r = UnityEngine.Random.Range(1, 4); spot = r == 1 ? Pos4Grandpa : r == 2 ? Pos2Grandpa : r == 3 ? Pos1Grandpa : null; }
			if (spot != null) Grandpa.transform.position = spot.position;
			GrandpaKilled = false;
			TimerGrandpaRecover = 0;
			if (ai.EnemyVision == null) return;
			ai.EnemyVision.IsChasing = false;
			agent.enabled = true;
			ai.EnemyVision.enabled = true;
			ai.CaughtPlayer = false;
			EF2.InWater = false;
			ai.ChooseWaypoint();
			var steps = ai.Anim.GetComponent<EnemyFootstep>();
			if (steps != null) steps.CurrentGround = 1f;
		}
		// 1 and 2: bedrooms, 3: couch.
		public void GrandpaySleep(int SpotChoosen)
		{
			Grandpa.SetActive(false);
			Transform bed = SpotChoosen == 1 ? Bedroom1Spot : SpotChoosen == 2 ? Bedroom2Spot : SpotChoosen == 3 ? CouchSpot : null;
			Transform gun = SpotChoosen == 1 ? ShotgunBedSpot_1 : SpotChoosen == 2 ? ShotgunBedSpot_2 : SpotChoosen == 3 ? ShotgunCouchSpot : null;
			if (bed != null)
			{
				GrandpaBed.transform.position = bed.position;
				GrandpaBed.transform.rotation = bed.rotation;
				ShotgunGrandpaBed.transform.position = gun.position;
				ShotgunGrandpaBed.transform.rotation = gun.rotation;
			}
			GrandpaBed.SetActive(true);
		}
		public void SlendrinaSpawn()
		{
			if (PlayerPrefs.GetInt("SlendrinaEnemy") != 1) { Slendrina.SetActive(false); return; }
			Slendrina.SetActive(true);
			Slendrina.transform.position = SlendrinaStartPos.position;
			var ai = Slendrina.GetComponent<AI_Slendrina>();
			if (ai != null) ai.EnemySpawn();
		}
}
