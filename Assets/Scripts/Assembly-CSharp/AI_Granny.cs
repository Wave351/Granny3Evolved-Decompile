using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using com.ootii.Actors.BoneControllers;

public class AI_Granny : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CSlingshotRockThrow_003Ed__127 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AI_Granny _003C_003E4__this;

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
		public _003CSlingshotRockThrow_003Ed__127(int _003C_003E1__state)
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
	private sealed class _003CSmackTimer_003Ed__135 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AI_Granny _003C_003E4__this;

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
		public _003CSmackTimer_003Ed__135(int _003C_003E1__state)
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

	[Header("Allies")]
	public AI_Grandpa Ally;

	[Header("Animation")]
	public Animation Anim;

	public AnimationClip Idle;

	public AnimationClip WalkAnim;

	public AnimationClip JumpscarePlayer;

	public AnimationClip UnderBed;

	public AnimationClip Search;

	public AnimationClip Shoot;

	public AnimationClip Stunned;

	[Header("Attacks")]
	public AudioSource JumpscareHit;

	public AudioSource JumpscareHitNM;

	public AudioSource JumpscareBed;

	public AudioSource HitSound;

	public float DistanceJumpscare;

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

	public Eyes_Granny EnemyVision;

	public bool IsDying;

	public GameObject NormalWeapon;

	public MeshRenderer RGBat;

	public GameObject slingshotWP;

	public bool EquippedWP;

	public bool Throwing;

	public float TimerShoot;

	public float TimerMaxShoot;

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

	public Vector3 DefaultRaycastDoors;

	public Transform catchTransform;

	[Header("Routines")]
	public GameObject DishesDone;

	public AnimationClip WashingDishes;

	public Transform SP_Dishes;

	public float TimerWashingDishes;

	public float TimerMaxDishes;

	public bool AtDishes;

	public GameObject PianoSing;

	public AnimationClip PianoKeyPressing;

	public Transform SP_Piano;

	public float TimerPianoSing;

	public float TimerMaxPiano;

	public bool AtPiano;

	public GameObject PianoDoor;

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

	[IteratorStateMachine(typeof(_003CSlingshotRockThrow_003Ed__127))]
	private IEnumerator SlingshotRockThrow()
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

	[IteratorStateMachine(typeof(_003CSmackTimer_003Ed__135))]
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
}
