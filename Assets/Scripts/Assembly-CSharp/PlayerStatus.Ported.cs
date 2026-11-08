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

// Ported from Annotated-Pseudocode/PlayerStatus.c (Granny 3 Evolved, IL2CPP x64).
public partial class PlayerStatus
{
		public PlayerStatus() { LookSpeed = 15f; MaxFOV = 50f; SpeedFOVFade = 4f; }
		private void Start() { }
		private void Update()
		{
			// Caught: the camera is pulled toward the enemy's face (and zooms in a little).
			if (LookAtGrandpa) LookToward(GrandpaPLook.transform, 20f, true);
			if (LookAtGranny)
			{
				LookToward(GrannyPLook.transform, 20f, true);
				FinalLookDirection = (GrannyPLook.transform.position - CameraPivot.transform.position).normalized;
				FinalLookDirection.y = 0;
			}
			if (lookAtAlligator) LookToward(alligatorPLook, 15f, false);
			if (!Days.WakingUp && !TimerStopped)
			{
				TimerCurrentRun += Time.deltaTime;
				int minutes = Mathf.FloorToInt(TimerCurrentRun / 60f);
				int seconds = Mathf.FloorToInt(TimerCurrentRun % 60f);
				int milliseconds = Mathf.FloorToInt(TimerCurrentRun * 1000f % 1000f);
				TimerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
			}
			else if (TimerStopped) TimerText.color = Color.red;
			// L toggles the run timer.
			int timer = PlayerPrefs.GetInt("GameTimer");
			if (timer == 0 && TimerUI.activeSelf) TimerUI.SetActive(false);
			if (!TimerUI.activeSelf && PlayerPrefs.GetInt("GameTimer") == 1) TimerUI.SetActive(true);
			if (Input.GetKeyDown(KeyCode.L)) PlayerPrefs.SetInt("GameTimer", PlayerPrefs.GetInt("GameTimer") == 0 ? 1 : 0);
			if (!Player.activeSelf && PickRay.shotWithSling) PickRay.shotWithSling = false;
		}
		private void LookToward(Transform target, float speed, bool zoom)
		{
			if (zoom && PlayerCam.fieldOfView > MaxFOV) PlayerCam.fieldOfView -= Time.deltaTime * SpeedFOVFade;
			Vector3 direction = target.position - CameraPivot.transform.position;
			CameraPivot.transform.rotation = Quaternion.Slerp(CameraPivot.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * speed);
		}
		public void PlayerGettingStopped()
		{
			IsJumpscared = true;
			var fps = Player.GetComponent<MobileFPS>();
			if (fps == null) return;
			fps.isAllowedToMove = false;
			fps.AbleToMove = false;
			var crouch = Player.GetComponent<CrouchHolder>();
			if (crouch == null) return;
			if (crouch.IsCrouched) crouch.CrouchStopJumpscare();
			if (crouch.isCrouching) crouch.CrouchStopJumpscare();
			crouch.Disabled = true;
		}
		public void DamageFeet()
		{
			AudioSource.PlayClipAtPoint(SoundHurt, Player.transform.position);
			ScreenEffects.BloodScreenHit.gameObject.SetActive(true);
		}
		public void DamageSpider() { ScreenEffects.BloodBitenScreen.gameObject.SetActive(true); }
		// Dragged under by the squid: the screen fades and the day restarts.
		public void SquidDeath()
		{
			if (Killed) return;
			Killed = true;
			E1.SetActive(false);
			E2.SetActive(false);
			PickRay.DeathAction();
			UIMain.SetActive(false);
			TextUI.SetActive(false);
			CalmEnemies();
			HidingFromMom = false;
			StartCoroutine(SquidDeathCount());
			ScreenEffects.NormalSee = true;
			PlayerGettingStopped();
			var fps = Player.GetComponent<MobileFPS>();
			if (fps == null) return;
			fps.CanFade = true;
			fps.SpeedFade = 20f;
			Escapedo.DiedOnce = true;
		}
		private void CalmEnemies()
		{
			TH.AlwaysAngry1 = false;
			TH.AlwaysAngry2 = false;
			EC.GrannyKilled = false;
			EC.GrandpaKilled = false;
			EC.GrandpaBed.SetActive(false);
		}
		private IEnumerator SquidDeathCount()
		{
			yield return new WaitForSeconds(3.4f);
			BlackScreen.CrossFadeAlpha(1f, .3f, false);
			yield return new WaitForSeconds(4.5f);
			var fps = Player.GetComponent<MobileFPS>();
			if (fps == null) yield break;
			fps.CanFade = false;
			IsJumpscared = false;
			Days.StartNew();
			Killed = false;
			ScreenEffects.NormalSee = false;
			HidingFromMom = false;
			HidingFromHouseEnemy = false;
			ResetHiding();
			yield return new WaitForSeconds(2f);
			Player.SetActive(false);
		}
		private void ResetHiding()
		{
			CUnh1.Hid = false;
			CUnh2.Hid = false;
			CUnh3.Hid = false;
			CUnh4.Hid = false;
			EyesGranny.PlayerSpotted = false;
			EyesGrandpa.PlayerSpotted = false;
		}
		public void GrannyCaughtYou()
		{
			if (Training.Spare()) return; // training: enemies can't kill
			if (Killed) return;
			EC.GrannyKilled = false;
			EC.GrandpaBed.SetActive(false);
			EC.GrandpaKilled = false;
			PlayerGettingStopped();
			LookAtGranny = true;
			IsJumpscared = true;
		}
		public void GrannyCaughtYouBed()
		{
			if (Training.Spare()) return; // training: enemies can't kill
			if (Killed) return;
			StartCoroutine(DeathCountBed(1.6f));
			EC.GrandpaBed.SetActive(false);
			EC.GrannyKilled = false;
			EC.GrandpaKilled = false;
			LookAtGranny = true;
			IsJumpscared = true;
			Escapedo.DiedOnce = true;
		}
		public void GrandpaCaughtYou()
		{
			if (Training.Spare()) return; // training: enemies can't kill
			if (Killed) return;
			PlayerGettingStopped();
			EC.GrandpaBed.SetActive(false);
			EC.GrannyKilled = false;
			EC.GrandpaKilled = false;
			LookAtGrandpa = true;
			IsJumpscared = true;
		}
		public void GrandpaCaughtYouBed()
		{
			if (Training.Spare()) return; // training: enemies can't kill
			if (Killed) return;
			StartCoroutine(DeathCountBed(2.1f));
			EC.GrandpaBed.SetActive(false);
			EC.GrannyKilled = false;
			EC.GrandpaKilled = false;
			LookAtGrandpa = true;
			IsJumpscared = true;
			Escapedo.DiedOnce = true;
		}
		// Knocked out: the ragdoll falls at the second spawn point.
		public void KnockDeath()
		{
			if (Training.Spare()) return; // training: enemies can't kill
			if (Killed) return;
			E1.SetActive(false);
			E2.SetActive(false);
			EC.GrannyKilled = false;
			EC.GrandpaKilled = false;
			Killed = true;
			PickRay.DeathAction();
			Player.SetActive(false);
			UIMain.SetActive(false);
			TextUI.SetActive(false);
			if (ScreenEffects == null || ScreenEffects.BloodScreenHit == null) return;
			ScreenEffects.BloodScreenHit.gameObject.SetActive(true);
			Instantiate(RagdollPlayer, SpawnRagdollPos2.position, SpawnRagdollPos2.transform.rotation);
			StartCoroutine(DeathCount());
			ScreenEffects.NormalSee = false;
			LeaveBeds();
			HidingFromMom = false;
			TH.AlwaysAngry1 = false;
			TH.AlwaysAngry2 = false;
			EC.GrandpaBed.SetActive(false);
			if (PlayerPrefs.GetInt("Granny") == 1) Granny.GetComponent<AI_Granny>().StopAI();
			if (PlayerPrefs.GetInt("Grandpa") == 1) Grandpa.GetComponent<AI_Grandpa>().StopAI();
			Escapedo.DiedOnce = true;
			if (!CanBloodSpatter) return;
			Instantiate(BloodSpatterHit, CameraPivot.transform.position, BloodSpatterHit.transform.rotation);
		}
		private void LeaveBeds()
		{
			BedHide1.InBed = false;
			BedHide2.InBed = false;
			BedHide3.InBed = false;
			BedHide1.NearBed = false;
			BedHide2.NearBed = false;
			BedHide3.NearBed = false;
			InBed1 = false;
			InBed2 = false;
			InBed3 = false;
		}
		public void NormalDeath(bool electric)
		{
			// No dying in mid-fall; the landing handles that.
			var falling = Player.GetComponent<FallingHolder>();
			if (falling == null || falling.isFalling || falling.isLanding || Killed) return;
			E1.SetActive(false);
			E2.SetActive(false);
			Killed = true;
			PickRay.DeathAction();
			Player.SetActive(false);
			UIMain.SetActive(false);
			TextUI.SetActive(false);
			ScreenEffects.BloodScreenHit.gameObject.SetActive(true);
			GameObject body = Instantiate(RagdollPlayer, SpawnRagdollPos.position, SpawnRagdollPos.transform.rotation);
			StartCoroutine(DeathCount());
			ScreenEffects.NormalSee = false;
			LeaveBeds();
			HidingFromMom = false;
			CalmEnemies();
			if (PlayerPrefs.GetInt("Granny") == 1) Granny.GetComponent<AI_Granny>().StopAI();
			int grandpa = PlayerPrefs.GetInt("Grandpa");
			if (grandpa == 1 || grandpa == 2) Grandpa.GetComponent<AI_Grandpa>().StopAI();
			Escapedo.DiedOnce = true;
			if (!electric || body == null) return;
			var ragdoll = body.GetComponent<DeathRagdoll>();
			if (ragdoll != null) ragdoll.ElectricDeath = true;
		}
		public void SpawnRagdoll()
		{
			Vector3 forward = CameraPivot.transform.forward;
			forward.y = 0;
			if (forward.sqrMagnitude < .0001f) { forward = Player.transform.forward; forward.y = 0; }
			Instantiate(RagdollPlayer, SpawnRagdollPos2.position, Quaternion.LookRotation(forward));
		}
		private IEnumerator DeathCount()
		{
			yield return new WaitForSeconds(3.5f);
			BlackScreen.CrossFadeAlpha(1f, 1f, false);
			yield return new WaitForSeconds(1.5f);
			Days.PlayerBackInTheProperSpot();
			Player.SetActive(true);
			BedHide1.NearBed = false;
			BedHide2.NearBed = false;
			BedHide3.NearBed = false;
			InBed1 = false;
			InBed2 = false;
			InBed3 = false;
			LookAtGranny = false;
			LookAtGrandpa = false;   // the original clears both with one two-byte write
			HidingFromMom = false;
			IsJumpscared = false;
			Days.StartNew();
			Killed = false;
			ScreenEffects.NormalSee = false;
			if (ScreenEffects == null || ScreenEffects.BloodBitenScreen == null) yield break;
			ScreenEffects.BloodBitenScreen.gameObject.SetActive(false);
			ResetHiding();
		}
		private IEnumerator DeathCountBed(float Timer)
		{
			IsJumpscared = true;
			yield return new WaitForSeconds(Timer);
			BlackScreen.CrossFadeAlpha(1f, .2f, false);
			yield return new WaitForSeconds(2f);
			Player.SetActive(true);
			PickRay.DeathAction();
			yield return new WaitForSeconds(2f);
			Player.SetActive(true);
			Days.PlayerBackInTheProperSpot();
			BedHide1.DisableDeath();
			BedHide2.DisableDeath();
			BedHide3.DisableDeath();
			LeaveBeds();
			LookAtGranny = false;
			LookAtGrandpa = false;   // the original clears both with one two-byte write
			HidingFromMom = false;
			IsJumpscared = false;
			Days.StartNew();
			Killed = false;
			ScreenEffects.NormalSee = false;
			ResetHiding();
		}
}
