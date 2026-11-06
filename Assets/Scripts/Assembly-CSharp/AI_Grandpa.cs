using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using com.ootii.Actors.BoneControllers;

public class AI_Grandpa : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CShotgunFinishShot_003Ed__110 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AI_Grandpa _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CShotgunFinishShot_003Ed__110(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CSmackTimer_003Ed__118 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AI_Grandpa _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CSmackTimer_003Ed__118(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Alies")]
	public AI_Granny Ally;

	[Header("Animation")]
	public Animation Anim;

	public AnimationClip Idle;

	public AnimationClip WalkAnim;

	public AnimationClip JumpscarePlayer;

	public AnimationClip UnderBed;

	public AnimationClip Stunned;

	public AnimationClip Search;

	public AnimationClip Shoot;

	[Header("Attacks")]
	public AudioSource JumpscareHitNM;

	public AudioSource JumpscareBed;

	public AudioSource HitSound;

	public float DistanceJumpscare;

	public bool IsShooting;

	public float TimerShoot;

	public float TimerMaxShoot;

	[Header("Floats")]
	public float Walk_Speed;

	public float Run_Speed;

	public float Walk_Anim;

	public float Run_Anim;

	[Header("Navigation")]
	public NavMeshAgent Agent;

	public Vector3 CurrentTarget;

	public int CurrentWaypoint;

	public List<Transform> Waypoints;

	public int WaypointNumb;

	public Transform Player;

	public BoneController BC;

	public Transform PlayerHead;

	public PlayerStatus PlayerStatus;

	public WindowJumping WJ;

	public Furniture Furni;

	public bool UnSeenPlayer;

	public Eyes_Grandpa EnemyVision;

	public bool IsDying;

	public GameObject Shotgun;

	public bool CaughtPlayer;

	public bool IsOpeningDoor;

	public bool IsSearching;

	public bool IsAngry;

	public bool IsFollowingSound;

	public bool IsChasing;

	public bool IsWalking;

	public bool IsIdle;

	public bool dizzy;

	public float timerDizzy;

	public float timerDizzyMax;

	public GameObject dizzyEffect;

	public float TimerNearNoise;

	public float TimerMaxNoise;

	public float TimerMaxNewNav;

	public float TimerChoosing;

	public float SearchTimer;

	public float MaxSearch;

	public AudioSource GrannyVoiceSource;

	public AudioClip[] VoiceLinesEnemy;

	public float DistanceFromNav;

	public float DistanceFromPlayer;

	public bool PlayerClose;

	public float coneAngle;

	public int rayCount;

	public float DoorDistance;

	public float TimerDoorOpening;

	public float TimerMaxDoor;

	public bool CanOpenClosets;

	public bool InSecurityRoom;

	public float LockedDoorAttempts;

	public AudioClip LockedDoor;

	public AudioClip UnlockDoorSound;

	public TeddyHunt TH;

	public bool blastOn;

	public GameObject NoiseObj;

	public Transform OpenDoorRay;

	public string NoiseObjectTag;

	public BGManager MusicBG;

	public ScreenEffects ScreenEffects;

	public bool TrapPregnant;

	public float TimerDropTrap;

	public float TimerMaxTrap;

	public float TimerAfterSearchTrap;

	public float TimerAfterSearchTrapMax;

	public GameObject BearTrap;

	public GameObject BearTrapNM;

	public Transform PointTrap;

	public float MaxTraps;

	public GameObject[] traps;

	public bool PlacingTrap;

	public bool NeedsToBirthTrap;

	public float DistanceRayPL;

	public Transform RayCheckP;

	public bool PlayerTouchedRay;

	public LayerMask LayerExP;

	public Transform BedCam1;

	public Transform BedCam2;

	public Transform BedCam3;

	public float TimerBedCaught;

	public float MaxBedCaughtTimer;

	public Vector3 PlayerPos;

	public Transform PlayerSpot;

	public EnemyController EC;

	public Paused Pause;

	public AudioSource FootstepS;

	public LayerMask LayerEx;

	public Transform catchTransform;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	public void BlastedOffSoul()
	{
	}

	public void HitWithStone(bool effectOn)
	{
	}

	public void DeadDisable()
	{
	}

	public void StopAI()
	{
	}

	public void ResetAIDecision()
	{
	}

	[IteratorStateMachine(typeof(_003CShotgunFinishShot_003Ed__110))]
	private IEnumerator ShotgunFinishShot()
	{
		return null;
	}

	public void ChaseAction()
	{
	}

	public void StopChase()
	{
	}

	public void LookAtThings(GameObject ThatThing, float RotateSpeed)
	{
	}

	public void SpeakEnemy()
	{
	}

	public void MurderNoiseObjs()
	{
	}

	public void ResetCertainTimer()
	{
	}

	public void ChooseWaypoint()
	{
	}

	[IteratorStateMachine(typeof(_003CSmackTimer_003Ed__118))]
	private IEnumerator SmackTimer()
	{
		return null;
	}

	private void OnTriggerStay(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}

	public void SetNewBoneControllerStare(Transform t)
	{
	}
}
