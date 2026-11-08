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

// Ported from Annotated-Pseudocode/Escapes.c (Granny 3 Evolved, IL2CPP x64).
public partial class Escapes
{
		private void Start() { }
		private void Update() { }
		public void EscapeGate()
		{
			PauseLogic.enabled = false;
			StopHouse(escapeGateMusic);
			if (!LockPlayer()) return;
			Black.CrossFadeAlpha(1f, .4f, false);
			StartCoroutine(WaitGate());
			Achievements("Gate");
			// Kirky: on Extreme, beat the alligator back, ready the train and get spotted at the gate.
			if (PlayerPrefs.GetInt("Diff") == 3 && alligator.timesHit >= 5.9f && ERC.TrainAvailable && ETG.spotted) PlayerPrefs.SetInt("EX_Kirky", 1);
			PlayerPrefs.SetInt("AllRoutes", 1);
			FinishRun();
		}
		public void EscapeTrain()
		{
			PauseLogic.enabled = false;
			GrannyS.SetActive(false);
			GrandpaS.SetActive(false);
			BG.GrannyHunting = false;
			trainAnim.Play("TrainAway");
			acceleratorAnim.Play("AcceleratorBack");
			escapeTrainMusic.SetActive(true);
			if (!StopMusic()) return;
			if (!LockPlayer()) return;
			PauseLogic.gameObject.SetActive(false);
			moonlightRLT.SetActive(false);
			Black.CrossFadeAlpha(1f, .1f, false);
			StartCoroutine(WaitTrain());
			Achievements("Train");
			FinishRun();
		}
		public void EscapeSubmarine()
		{
			PauseLogic.enabled = false;
			StopHouse(escapeSubmarineMusic);
			LockPlayer();
			PauseLogic.gameObject.SetActive(false);
			moonlightRLT.SetActive(false);
			resetFloorStuff.SetActive(false);
			Black.CrossFadeAlpha(1f, .15f, false);
			StartCoroutine(WaitSubmarine());
			Achievements("Submarine");
			FinishRun();
			RenderSettings.fog = false;
		}
		public void EscapeBalloon()
		{
			if (BL.insideBalloon) BL.FPS.transform.parent = BL.temporaryParentPlayerBalloon;
			StartCoroutine(WaitBalloon());
		}
		// Enemies go quiet, the escape music starts and the house music is parked.
		private void StopHouse(GameObject music)
		{
			GrannyS.SetActive(false);
			GrandpaS.SetActive(false);
			BG.GrannyHunting = false;
			music.SetActive(true);
			StopMusic();
		}
		private bool StopMusic()
		{
			if (BG == null || BG.HouseMusic == null) return false;
			BG.HouseMusic.gameObject.SetActive(false);
			if (BG.MusicG == null) return false;
			BG.MusicG.transform.parent = GameOverTurnOff;
			return true;
		}
		private bool LockPlayer()
		{
			if (MobileFPS == null) return false;
			var crouch = MobileFPS.GetComponent<CrouchHolder>();
			if (crouch == null) return false;
			crouch.enabled = false;
			var falling = MobileFPS.GetComponent<FallingHolder>();
			if (falling == null) return false;
			falling.enabled = false;
			var fps = MobileFPS.GetComponent<MobileFPS>();
			if (fps == null) return false;
			fps.AbleToMove = false;
			fps.isAllowedToMove = false;
			HideHud();
			return true;
		}
		private void HideHud()
		{
			Ray.enabled = false;
			SpaceT.SetActive(false);
			HandHold.SetActive(false);
			UIElem.SetActive(false);
			Texts.SetActive(false);
			EscapeCoinsEarn();
			ScreenEffects.NormalSee = false;
		}
		private void Achievements(string route)
		{
			int diff = PlayerPrefs.GetInt("Diff");
			bool granny = PlayerPrefs.GetInt("Granny") == 1, grandpa = PlayerPrefs.GetInt("Grandpa") == 1;
			if (diff == 2)
			{
				if (granny || grandpa) PlayerPrefs.SetInt("HARD_Escape", 1);
				if (granny && grandpa) PlayerPrefs.SetInt("HARD_Both", 1);
			}
			if (diff == 3) PlayerPrefs.SetInt("EXTREME_Escape", 1);
			if (diff == 3 && !DiedOnce) PlayerPrefs.SetInt("EXTREME_NoDying", 1);
			if (diff == 3) PlayerPrefs.SetInt("EXTREME_" + route, 1);
			if (diff == 4 && !DiedOnce) PlayerPrefs.SetInt("IMP_NoDying", 1);
			if (diff == 4) PlayerPrefs.SetInt("IMP_Escape", 1);
			if (diff == 4 && ERC.AllRoutesOpen) PlayerPrefs.SetInt("IMP_AllRoutes", 1);
			if (diff == 4 && PlayerPrefs.GetInt("FlashL") == 1) PlayerPrefs.SetInt("IMP_Seed", 1);
			if (granny && PlayerPrefs.GetInt("GrannyWP") == 1 && diff == 3) PlayerPrefs.SetInt("EXTREME_Slingy", 1);
			if (diff == 3 && !UsedWPOnce) PlayerPrefs.SetInt("EX_NoKilling", 1);
			if (diff == 3 && !ticketBot.alarmSoundedOnce) PlayerPrefs.SetInt("EX_NoAlarm", 1);
			if (diff == 4) PlayerPrefs.SetInt("IMP_" + route, 1);
		}
		// Bodies cleared, timer stopped, and Slendrina (if on) comes back to watch the ending.
		private void FinishRun()
		{
			if (ERC.AllRoutesOpen) PlayerPrefs.SetInt("AllRoutes", 1);
			ElimBody();
			TimerUISwitch.transform.parent = UINew.transform;
			PS.TimerStopped = true;
			if (PlayerPrefs.GetInt("SlendrinaEnemy") != 1) return;
			var ai = slendrina.GetComponent<AI_Slendrina>();
			if (ai == null) return;
			ai.canAttack = false;
			ai.lookAtSpeed = 0;
			cribTrigger.slendrinaActive = false;
			cribTrigger.SlendrinaFades = false;
			slendrina.SetActive(true);
		}
		// Granny and Grandpa show up at the exit, looking around.
		private void ShowEnemies(Transform grannyEnd, Transform grandpaEnd)
		{
			if (PlayerPrefs.GetInt("Granny") == 1)
			{
				GrannyS.SetActive(true);
				EC.GrannyKilled = false;
				Granny.SetActive(true);
				Granny.GetComponent<NavMeshAgent>().enabled = false;
				var ai = Granny.GetComponent<AI_Granny>();
				ai.dizzy = false;
				ai.dizzyEffect.SetActive(false);
				ai.EnemyVision.enabled = false;
				ai.BC.enabled = false;
				ai.Anim.CrossFade(ai.Search.name);
				ai.enabled = false;
				Granny.transform.position = grannyEnd.position;
				Granny.transform.rotation = grannyEnd.rotation;
			}
			if (PlayerPrefs.GetInt("Grandpa") == 1)
			{
				GrandpaS.SetActive(true);
				EC.GrandpaKilled = false;
				Grandpa.SetActive(true);
				Grandpa.GetComponent<NavMeshAgent>().enabled = false;
				var ai = Grandpa.GetComponent<AI_Grandpa>();
				ai.dizzy = false;
				ai.dizzyEffect.SetActive(false);
				ai.EnemyVision.enabled = false;
				ai.BC.enabled = false;
				ai.Anim.CrossFade(ai.Search.name);
				ai.enabled = false;
				Grandpa.transform.position = grandpaEnd.position;
				Grandpa.transform.rotation = grandpaEnd.rotation;
			}
		}
		private void ShowSlendrina(Transform end)
		{
			if (PlayerPrefs.GetInt("SlendrinaEnemy") != 1) return;
			var ai = slendrina.GetComponent<AI_Slendrina>();
			if (ai == null) return;
			ai.StartPos = end;
			slendrina.transform.position = end.position;
			slendrina.transform.rotation = end.rotation;
			cribTrigger.SlendrinaFades = true;
		}
		private IEnumerator TheEnd(float fade, float wait, float shown)
		{
			Black.CrossFadeAlpha(1f, fade, false);
			yield return new WaitForSeconds(wait);
			TheEndT.SetActive(true);
			yield return new WaitForSeconds(shown);
			TheEndT.SetActive(false);
			yield return new WaitForSeconds(2f);
			SceneManager.LoadScene("Menu");
		}
		private IEnumerator WaitTrain()
		{
			yield return new WaitForSeconds(.2f);
			MobileFPS.SetActive(false);
			cutsceneCamTrain.SetActive(true);
			Black.CrossFadeAlpha(0, .5f, false);
			yield return new WaitForSeconds(7.3f);
			acceleratorAnim.Play("AcceleratorForward");
			yield return new WaitForSeconds(6.5f);
			cutsceneCamTrain.SetActive(false);
			cutsceneCamTrainEscaped.SetActive(true);
			BloodScreens.SetActive(false);
			ShowEnemies(PosGrannyEndTrain, PosGrandpaEndTrain);
			yield return new WaitForSeconds(5f);
			ShowSlendrina(posSlendrinaTrainEnd);
			yield return new WaitForSeconds(5f);
			yield return TheEnd(1.5f, 1.8f, 3f);
		}
		private IEnumerator WaitGate()
		{
			yield return new WaitForSeconds(.5f);
			MobileFPS.SetActive(false);
			Black.CrossFadeAlpha(0, 1.5f, false);
			cutsceneGateCam.SetActive(true);
			BloodScreens.SetActive(false);
			ShowEnemies(PosGrannyEndGate, PosGrandpaEndGate);
			yield return new WaitForSeconds(6f);
			ShowSlendrina(posSlendrinaGateEnd);
			yield return new WaitForSeconds(4f);
			yield return TheEnd(1f, 1.5f, 4f);
		}
		private IEnumerator WaitBalloon()
		{
			yield return new WaitForSeconds(14f);
			// Without the propane tank the balloon falls apart.
			if (BL.propaneTank == null)
			{
				BL.deterioratedBalloon.SetActive(true);
				BL.gameObject.SetActive(false);
				yield break;
			}
			if (MobileFPS.transform.parent != hotAirContainer || !BL.insideBalloon) yield break;
			Black.CrossFadeAlpha(1f, .5f, false);
			HideHud();
			PauseLogic.enabled = false;
			Achievements("Balloon");
			FinishRun();
			yield return new WaitForSeconds(1.2f);
			StopHouse(escapeBalloonMusic);
			MobileFPS.SetActive(false);
			Black.CrossFadeAlpha(0, 1f, false);
			cutsceneCamBalloon.SetActive(true);
			playerSilhouette.SetActive(true);
			BloodScreens.SetActive(false);
			ShowEnemies(PosGrannyEndBalloon, PosGrandpaEndBalloon);
			yield return new WaitForSeconds(20f);
			ShowSlendrina(posSlendrinaBalloonEnd);
			if (PlayerPrefs.GetInt("SlendrinaEnemy") == 1) cribTrigger.maxHair = 1f;
			yield return new WaitForSeconds(3.5f);
			yield return TheEnd(1f, 1.5f, 4f);
		}
		private IEnumerator WaitSubmarine()
		{
			yield return new WaitForSeconds(1f);
			MobileFPS.SetActive(false);
			cutsceneCamSubmarine.SetActive(true);
			Black.CrossFadeAlpha(0, .5f, false);
			submarineObjectTop.SetActive(false);
			slideDoorBottom1.SetActive(false);
			slideDoorBottom2.SetActive(false);
			BloodScreens.SetActive(false);
			BG.Facility.gameObject.SetActive(false);
			yield return new WaitForSeconds(11.4f);
			ShowEnemies(PosGrannyEndSubmarine, PosGrandpaEndSubmarine);
			// Slender waits at the dock too.
			var slender = slenderEnemy.GetComponent<AI_Slender>();
			if (slender != null)
			{
				slender.enabled = false;
				slenderEnemy.transform.position = slenderEndPosition.position;
				slenderEnemy.transform.rotation = slenderEndPosition.rotation;
				yield return new WaitForSeconds(3f);
			}
			ShowSlendrina(posSlendrinaSubmarine);
			yield return new WaitForSeconds(5f);
			yield return TheEnd(1.5f, 1.8f, 3f);
		}
		public void ElimBody()
		{
			foreach (var name in new[] { "Shotgun(Clone)", "GrannyRagdoll(Clone)", "GrandpaRagdoll(Clone)", "GrandpaFreezeDoll(Clone)",
				"GrannyFreezeDoll(Clone)", "GrandpaCrystalBody(Clone)", "GrannyCrystalBody(Clone)" })
			{
				var body = GameObject.Find(name);
				if (body != null) Destroy(body);
			}
		}
		// Escape reward by difficulty (more with enemies on), plus a Slendrina bonus.
		public void EscapeCoinsEarn()
		{
			float coins = PlayerPrefs.GetFloat("Coins", 0);
			int diff = DifficultyLevel.Rules;
			bool granny = PlayerPrefs.GetInt("Granny") == 1, grandpa = PlayerPrefs.GetInt("Grandpa") == 1;
			float reward, slendrinaBonus;
			switch (diff)
			{
				case 0: reward = granny || grandpa ? 2f : .2f; slendrinaBonus = .5f; break;
				case 1: reward = granny || grandpa ? 3.5f : .4f; slendrinaBonus = .75f; break;
				case 2: reward = granny && grandpa ? 5f : granny || grandpa ? 4.5f : .7f; slendrinaBonus = 1f; break;
				case 3: reward = 7f; slendrinaBonus = 1.5f; break;
				case 4: reward = 10f; slendrinaBonus = 2f; break;
				default: reward = -1; slendrinaBonus = 0; break;
			}
			if (reward >= 0)
			{
				coins += reward;
				PlayerPrefs.SetFloat("Coins", coins);
				if (PlayerPrefs.GetInt("SlendrinaEnemy") == 1) PlayerPrefs.SetFloat("Coins", coins + slendrinaBonus);
			}
			soulHuntSFX.SetActive(false);
		}
}
