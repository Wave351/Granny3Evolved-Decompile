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
using com.ootii.Actors.BoneControllers;

// Ported from Annotated-Pseudocode/AI_Grandpa.c (Granny 3 Evolved, IL2CPP x64).
public partial class AI_Grandpa
{
		public AI_Grandpa()
		{
			DistanceJumpscare = 1.5f; Walk_Speed = 3f; Run_Speed = 5f; Walk_Anim = 1.1f; Run_Anim = 1.2f; TimerMaxShoot = 3f;
			timerDizzyMax = 30f; TimerMaxNoise = .1f; TimerMaxNewNav = 5f; MaxSearch = 5f; DistanceFromNav = 2f; DistanceFromPlayer = 1.5f;
			coneAngle = 35f; rayCount = 7; DoorDistance = 4f; TimerMaxDoor = 2f; NoiseObjectTag = "noiseobject"; TrapPregnant = true;
			TimerMaxTrap = 2f; TimerAfterSearchTrapMax = 6f; MaxTraps = 5f; DistanceRayPL = 6f; MaxBedCaughtTimer = 2.5f;
		}
		private void Start()
		{
			switch (PlayerPrefs.GetInt("Diff"))
			{
				case 0: Run_Speed = 2.5f; Run_Anim = 1.25f; TimerMaxShoot = 2f; SetShootSpeed(.8f); break;
				case 1: Run_Speed = 3.8f; Run_Anim = 1.9f; TimerMaxShoot = 1.8f; SetShootSpeed(1f); break;
				case 2: Run_Speed = 5f; Run_Anim = 2.5f; TimerMaxShoot = 1.1f; SetShootSpeed(1.3f); break;
				case 3: Run_Speed = 8.5f; Run_Anim = 4.25f; TimerMaxShoot = .8f; SetShootSpeed(1.4f); break;
				case 4: Run_Speed = 10.5f; Run_Anim = 5f; DistanceJumpscare = 3.5f; TimerMaxShoot = .5f; SetShootSpeed(1.6f); break;
			}
		}
		// Harder difficulties pump the shotgun faster.
		private void SetShootSpeed(float speed)
		{
			if (Shoot == null || Anim == null) return;
			var state = Anim[Shoot.name];
			if (state != null) state.speed = speed;
		}
		// The exported ootii.dll has no working LookAtMotor; the restored BoneLookFallback turns the head instead.
		private void SetLookTarget(Transform target)
		{
			var look = BC.GetMotor<LookAtMotor>();
			if (look != null) look.TargetTransform = target;
			else BoneLookFallback.SetTarget(BC, target);
		}
		private const float Still = 1e-10f;
		private bool Stationary { get { return Agent.velocity.sqrMagnitude < Still; } }
		private void FixedUpdate()
		{
			// Up close with a clear line to the player: chase.
			RayCheckP.LookAt(Player.position);
			Vector3 rayDir = RayCheckP.TransformDirection(Vector3.forward);
			UnityEngine.Debug.DrawRay(RayCheckP.transform.position, rayDir * DistanceRayPL, Color.red);
			RaycastHit playerHit;
			PlayerTouchedRay = Physics.Raycast(RayCheckP.position, rayDir, out playerHit, DistanceRayPL, LayerExP) && playerHit.collider.CompareTag("Player");
			if (PlayerClose && PlayerTouchedRay && !IsChasing && !PlayerStatus.HidingFromHouseEnemy && !dizzy) ChaseAction();
			var falling = Player.GetComponent<FallingHolder>();
			if (!CaughtPlayer && !IsDying && !dizzy)
			{
				CheckDoors();
				traps = GameObject.FindGameObjectsWithTag("BearTrapActivated");
				SetLookTarget(PlayerHead.transform);
				// Noises pull her over.
				var noise = GameObject.FindGameObjectWithTag(NoiseObjectTag);
				if (noise && !IsChasing && !IsAngry && !IsDying)
				{
					IsFollowingSound = true;
					IsWalking = false;
					IsIdle = false;
					IsSearching = false;
					ResetCertainTimer();
					NoiseObj = noise;
				}
				// After a search she lays a bear trap.
				if (NeedsToBirthTrap)
				{
					TimerAfterSearchTrap += Time.deltaTime;
					if (TimerAfterSearchTrap >= TimerAfterSearchTrapMax) { PlacingTrap = true; NeedsToBirthTrap = false; TimerAfterSearchTrap = 0; }
				}
				else TimerAfterSearchTrap = 0;
				if (PlacingTrap)
				{
					TimerDropTrap += Time.deltaTime;
					if (TimerDropTrap >= TimerMaxTrap)
					{
						Instantiate(PlayerPrefs.GetInt("Textures") == 1 ? BearTrapNM : BearTrap, PointTrap.position, PointTrap.rotation);
						PlacingTrap = false;
						TimerDropTrap = 0;
					}
				}
				else TimerDropTrap = 0;
				if (IsAngry && !IsChasing) ChaseAction();
				FootstepS.volume = PlacingTrap || IsOpeningDoor ? FootstepS.volume - Time.deltaTime * 1.5f : 1f;
				// Grandpa fires the shotgun while he can see the player.
				if (Shotgun.activeSelf && !IsShooting && EnemyVision.Seeing && IsChasing && !falling.isLanding && !falling.isFalling && !PlayerStatus.HidingUnderThings)
				{
					TimerShoot += Time.deltaTime;
					if (TimerShoot >= TimerMaxShoot)
					{
						TimerShoot = 0;
						IsShooting = true;
						StartCoroutine(ShotgunFinishShot());
						Anim.CrossFade(Shoot.name);
					}
				}
				UpdateState();
				UpdateAnimation();
				// Close enough: he grabs the player.
				if (Vector3.Distance(transform.position, Player.position) <= DistanceJumpscare && IsChasing && !Ally.CaughtPlayer && !IsShooting
				    && !WJ.IsJumping && !falling.isLanding && !falling.isFalling && !PlayerStatus.HidingUnderThings && EnemyVision.Seeing
				    && !PlayerStatus.InBed1 && !PlayerStatus.InBed2 && !PlayerStatus.InBed3)
				{
					Agent.speed = 0;
					Agent.enabled = false;
					CaughtPlayer = true;
					PlayerStatus.GrandpaCaughtYou();
					MurderNoiseObjs();
					BC.enabled = false;
					MusicBG.GrannyHunting = false;
					ScreenEffects.NormalSee = false;
					if (PlayerPrefs.GetInt("Textures") == 1) JumpscareHitNM.Play();
					if (Ally.gameObject.activeSelf) Ally.StopAI();
					Anim.CrossFade(JumpscarePlayer.name);
					StartCoroutine(SmackTimer());
				}
			}
			if (dizzy) UpdateDizzy();
			else
			{
				if (dizzyEffect.activeSelf) dizzyEffect.SetActive(false);
				timerDizzy = 0;
			}
			if (CaughtPlayer) LookAtThings(BedTarget().gameObject, 15f);
			// A stunned or dead Grandpa drops the shotgun as an item.
			bool holding = !dizzy && !IsDying;
			Shotgun.GetComponent<Collider>().enabled = !holding;
			Shotgun.layer = LayerMask.NameToLayer(holding ? "Enemy" : "Item");
		}
		private Transform BedTarget()
		{
			if (PlayerStatus.InBed1) return BedCam1;
			if (PlayerStatus.InBed2) return BedCam2;
			if (PlayerStatus.InBed3) return BedCam3;
			return Player;
		}
		private void UpdateDizzy()
		{
			if (!blastOn) Anim.CrossFade(Stunned.name);
			else
			{
				// Blasted by the soul: a short knockback.
				Anim.CrossFade("Grandpa_Blast");
				if (timerDizzy >= 3.9f) RecoverFromDizzy();
			}
			timerDizzy += Time.deltaTime;
			if (timerDizzy >= timerDizzyMax) RecoverFromDizzy();
		}
		private void RecoverFromDizzy()
		{
			timerDizzy = 0;
			ResetAIDecision();
			dizzy = false;
			ChooseWaypoint();
			blastOn = false;
		}
		// Doors in a cone in front of her: plain doors open, locked ones get rattled and then forced, closets only when allowed.
		private void CheckDoors()
		{
			bool found = false;
			float closest = float.PositiveInfinity;
			RaycastHit best = default;
			for (int i = 0; i < rayCount; i++)
			{
				float angle = Mathf.Lerp(-coneAngle, coneAngle, (float)i / (rayCount - 1));
				Vector3 dir = Quaternion.AngleAxis(angle, OpenDoorRay.up) * OpenDoorRay.forward;
				UnityEngine.Debug.DrawRay(OpenDoorRay.position, dir * DoorDistance, Color.yellow);
				RaycastHit hit;
				if (!Physics.Raycast(OpenDoorRay.position, dir, out hit, DoorDistance, LayerEx)) continue;
				string hitName = hit.collider.gameObject.name;
				if ((hit.collider.GetComponent<DoorLogic>() || hitName == "MetalLockerDoor" || hitName == "ClosetDoor") && hit.distance < closest)
				{
					closest = hit.distance;
					best = hit;
					found = true;
				}
			}
			if (!found)
			{
				TimerDoorOpening = 0;
				LockedDoorAttempts = 0;
				IsOpeningDoor = false;
				if (Agent.enabled) Agent.isStopped = false;
				return;
			}
			string name = best.collider.gameObject.name;
			var door = best.collider.GetComponent<DoorLogic>();
			if (door && door.Closed && !door.Locked)
			{
				IsOpeningDoor = true;
				TimerMaxDoor = 2f;
				LockedDoorAttempts = 0;
				TimerDoorOpening += Time.deltaTime;
				if (TimerDoorOpening >= TimerMaxDoor) { door.OpeningClosingAttempts(); TimerDoorOpening = 0; }
				return;
			}
			if (door && door.Locked && !IsSearching && name != "SubwayCrowbarRoomDoor" && name != "ShedDoorMovable" && name != "PrisonerDoor_2")
			{
				TimerDoorOpening += Time.deltaTime * 1.5f;
				IsOpeningDoor = true;
				Agent.isStopped = true;
				var swing = best.collider.GetComponent<Animation>();
				int attempt = TimerDoorOpening > .8f && LockedDoorAttempts == 0 ? 1 : TimerDoorOpening > 1.6f && LockedDoorAttempts == 1 ? 2
					: TimerDoorOpening > 2.4f && LockedDoorAttempts == 2 ? 3 : 0;
				if (attempt != 0)
				{
					// Rattle the locked door.
					LockedDoorAttempts = attempt;
					if (!swing.isPlaying) door.OpeningClosingAttempts();
				}
				else if (TimerDoorOpening > 4.3f && LockedDoorAttempts == 3)
				{
					LockedDoorAttempts = 0;
					if (!swing.isPlaying) door.UnlockDoor(true);
				}
				return;
			}
			bool closet = name == "MetalLockerDoor" || name == "ClosetDoor";
			var animated = best.collider.GetComponent<Animated>();
			if (closet && animated && animated.Closed && CanOpenClosets)
			{
				IsOpeningDoor = true;
				TimerMaxDoor = 1f;
				LockedDoorAttempts = 0;
				TimerDoorOpening += Time.deltaTime;
				Agent.isStopped = true;
				if (TimerDoorOpening >= TimerMaxDoor) { animated.OpeningClosingAttempts(); TimerDoorOpening = 0; }
				return;
			}
			Agent.isStopped = closet && animated && animated.Closed && IsChasing;
			TimerDoorOpening = 0;
			IsOpeningDoor = false;
		}
		private void UpdateState()
		{
			if (IsChasing && !CaughtPlayer)
			{
				ScreenEffects.NormalSee = true;
				CurrentTarget = PlayerSpot.transform.position;
				if (EnemyVision.IsChasing)
				{
					MusicBG.GrannyHunting = true;
					BC.enabled = true;
				}
				IsWalking = false;
				IsIdle = false;
				IsFollowingSound = false;
				Agent.SetDestination(PlayerStatus.HidingUnderThings ? PlayerPos : CurrentTarget);
				// Lost the player at the spot they hid: give up looking soon.
				if (UnSeenPlayer && !IsAngry && !EnemyVision.PlayerSpotted && Stationary && !IsOpeningDoor
				    && Vector3.Distance(transform.position, Agent.destination) <= (Agent.isStopped ? 5f : 2.5f))
					EnemyVision.TimerStopVision = 100f;
				if (!EnemyVision.PlayerSpotted && !IsAngry) TimerBedCaught = 0;
				else if (Stationary && Vector3.Distance(transform.position, Player.position) <= 2.5f
				         && (PlayerStatus.InBed1 || PlayerStatus.InBed2 || PlayerStatus.InBed3))
				{
					// Hiding under the bed doesn't work if she saw it.
					TimerBedCaught += Time.deltaTime;
					if (TimerBedCaught >= MaxBedCaughtTimer && !Ally.CaughtPlayer)
					{
						IsChasing = false;
						TimerBedCaught = 0;
						Agent.baseOffset = 0;
						BC.enabled = false;
						MusicBG.GrannyHunting = false;
						ScreenEffects.NormalSee = false;
						PlayerStatus.GrandpaCaughtYouBed();
						JumpscareBed.Play();
						Anim.Play(UnderBed.name);
						CaughtPlayer = true;
						if (Ally.gameObject.activeSelf) Ally.StopAI();
					}
				}
				Agent.speed = !IsOpeningDoor && !IsShooting ? Run_Speed : 0;
				MurderNoiseObjs();
				TimerNearNoise = 0;
			}
			else if (IsIdle && !IsAngry && !CaughtPlayer)
			{
				TimerChoosing += Time.deltaTime;
				Anim.CrossFade(Idle.name);
				if (TimerChoosing < TimerMaxNewNav) return;
				TimerChoosing = 0;
				ChooseWaypoint();
			}
			else if (IsSearching && !IsAngry && !CaughtPlayer)
			{
				Agent.speed = 0;
				if (Stationary)
				{
					SearchTimer += Time.deltaTime;
					TimerNearNoise = 0;
					Anim.CrossFade(Search.name);
					if (SearchTimer >= MaxSearch)
					{
						SearchTimer = 0;
						IsSearching = false;
						if (TrapPregnant && traps.Length < 5) NeedsToBirthTrap = true;
						Furni.CleanUp();
						SpeakEnemy();
						ChooseWaypoint();
					}
				}
			}
			else if (IsFollowingSound && !IsAngry && !CaughtPlayer)
			{
				IsWalking = false;
				if (NoiseObj != null) CurrentTarget = NoiseObj.transform.position;
				Agent.SetDestination(CurrentTarget);
				Agent.speed = IsOpeningDoor ? 0 : Run_Speed;
				if (Vector3.Distance(transform.position, Agent.destination) > DistanceFromNav) TimerNearNoise = 0;
				else
				{
					TimerNearNoise += Time.deltaTime;
					if (TimerNearNoise >= TimerMaxNoise)
					{
						IsFollowingSound = false;
						IsSearching = true;
						if (NoiseObj != null) MurderNoiseObjs();
						TimerNearNoise = 0;
					}
				}
			}
			else if (IsWalking)
			{
				if (IsFollowingSound || IsAngry) return;
				Agent.speed = !IsOpeningDoor && !PlacingTrap ? Walk_Speed : 0;
				Anim.CrossFade(WalkAnim.name);
				Anim[WalkAnim.name].speed = Walk_Anim;
				Agent.SetDestination(CurrentTarget);
				if (Vector3.Distance(transform.position, Agent.destination) > DistanceFromNav) return;
				// His naps: the two bedrooms and the couch.
				string nav = Waypoints[CurrentWaypoint].gameObject.name;
				if (nav == "granpaNav10") EC.GrandpaySleep(2);
				else if (nav == "granpaNav9") EC.GrandpaySleep(1);
				else if (nav == "granpaNav6") EC.GrandpaySleep(3);
				else
				{
					IsWalking = false;
					IsIdle = true;
				}
			}
		}
		private void UpdateAnimation()
		{
			if (Stationary)
			{
				if (CaughtPlayer) return;
				if (!IsChasing)
				{
					if (!IsIdle && !IsWalking && !IsOpeningDoor && !IsFollowingSound && IsSearching) Anim.CrossFade(Search.name);
					else Anim.CrossFade(Idle.name);
					return;
				}
				if (!IsShooting) Anim.CrossFade(Idle.name);
				else LookAtThings(Player.gameObject, 15f);
				if (!IsOpeningDoor && !IsShooting) LookAtThings(BedTarget().gameObject, 15f);
				return;
			}
			if (IsWalking && !PlacingTrap)
			{
				Anim.CrossFade(WalkAnim.name);
				Anim[WalkAnim.name].speed = Walk_Anim;
			}
			else if (IsFollowingSound || IsSearching || (IsChasing && !IsShooting))
			{
				Anim.CrossFade(WalkAnim.name);
				Anim[WalkAnim.name].speed = Run_Anim;
			}
			else if (IsChasing) LookAtThings(Player.gameObject, 15f);
		}
		// Knocked back by the soul: slower for half a minute afterwards.
		public void BlastedOffSoul()
		{
			Halt();
			if (!Agent.isStopped && Agent.enabled) Agent.enabled = false;
			Anim.CrossFade("Grandpa_Blast");
			dizzy = true;
			EnemyVision.IsChasing = false;
			EnemyVision.enabled = false;
			IsDying = false;
			// Her eyes turn magenta.
			TH.GrandpaEyesNormal = new Color(1f, 0f, 1f, 1f);
			TH.GrandpaEyesSetDark = new Color(1f, 0f, 1f, 1f);
			TH.GrandpaEyeM.material.color = TH.GrandpaEyesNormal;
			timerDizzyMax = 28f;
			blastOn = true;
			Walk_Speed = 3.8f;
			Walk_Anim = 1.9f;
		}
		public void HitWithStone(bool effectOn)
		{
			if (effectOn) dizzyEffect.SetActive(true);
			if (dizzy) return;
			Halt();
			if (!Agent.isStopped && Agent.enabled) Agent.enabled = false;
			dizzy = true;
			Anim.CrossFade(Stunned.name);
			EnemyVision.IsChasing = false;
			EnemyVision.enabled = false;
			IsDying = false;
		}
		private void Halt()
		{
			MurderNoiseObjs();
			TimerNearNoise = 0;
			BC.enabled = false;
			MusicBG.GrannyHunting = false;
			IsChasing = false;
			IsSearching = false;
			IsShooting = false;
			ScreenEffects.NormalSee = false;
			CurrentTarget = transform.position;
		}
		public void DeadDisable()
		{
			Halt();
			EnemyVision.IsChasing = false;
			Agent.enabled = false;
			IsDying = true;
		}
		public void StopAI()
		{
			Halt();
			Agent.enabled = false;
			Anim.CrossFade(Idle.name);
			EnemyVision.enabled = false;
			CaughtPlayer = true;
			IsDying = false;
			dizzy = false;
		}
		public void ResetAIDecision()
		{
			MurderNoiseObjs();
			Agent.enabled = true;
			TimerNearNoise = 0;
			Anim.CrossFade(Idle.name);
			EnemyVision.enabled = true;
			CaughtPlayer = false;
			IsDying = false;
		}
		private IEnumerator ShotgunFinishShot()
		{
			yield return new WaitForSeconds(Anim["shootGun"].length / Anim["shootGun"].speed);
			IsShooting = false;
		}
		public void ChaseAction()
		{
			IsFollowingSound = false;
			IsChasing = true;
			TimerChoosing = 0;
			TimerBedCaught = 0;
			IsSearching = false;
		}
		public void StopChase()
		{
			MurderNoiseObjs();
			EnemyVision.IsChasing = false;
			TimerNearNoise = 0;
			BC.enabled = false;
			MusicBG.GrannyHunting = false;
			IsFollowingSound = false;
			IsChasing = false;
			IsSearching = true;
			ScreenEffects.NormalSee = false;
			CurrentTarget = transform.position;
		}
		public void LookAtThings(GameObject ThatThing, float RotateSpeed)
		{
			Vector3 to = ThatThing.transform.position - transform.position;
			to.y = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(to), Time.deltaTime * RotateSpeed);
		}
		public void SpeakEnemy() { GrannyVoiceSource.PlayOneShot(VoiceLinesEnemy[UnityEngine.Random.Range(0, VoiceLinesEnemy.Length)]); }
		public void MurderNoiseObjs()
		{
			foreach (var noise in GameObject.FindGameObjectsWithTag(NoiseObjectTag)) if (noise != gameObject) Destroy(noise);
		}
		public void ResetCertainTimer() { TimerChoosing = 0; TimerBedCaught = 0; }
		public void ChooseWaypoint()
		{
			do WaypointNumb = UnityEngine.Random.Range(0, Waypoints.Count);
			while (WaypointNumb == CurrentWaypoint);
			CurrentWaypoint = WaypointNumb;
			Transform point = Waypoints[WaypointNumb];
			if (point == null) return;
			CurrentTarget = point.position;
			IsSearching = false;
			IsWalking = true;
			IsIdle = false;
		}
		// The blow lands 0.72 s into the grab.
		private IEnumerator SmackTimer()
		{
			yield return new WaitForSeconds(.72f);
			HitSound.Play();
			PlayerStatus.KnockDeath();
			yield return new WaitForSeconds(.3f);
		}
		private void OnTriggerStay(Collider other) { if (other.gameObject.tag == "Player") PlayerClose = true; }
		private void OnTriggerExit(Collider other) { if (other.gameObject.tag == "Player") PlayerClose = false; }
		public void SetNewBoneControllerStare(Transform t) { SetLookTarget(t); }
}
