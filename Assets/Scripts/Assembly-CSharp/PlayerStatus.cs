using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDeathCount_003Ed__76 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerStatus _003C_003E4__this;

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
		public _003CDeathCount_003Ed__76(int _003C_003E1__state)
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
	private sealed class _003CDeathCountBed_003Ed__77 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerStatus _003C_003E4__this;

		public float Timer;

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
		public _003CDeathCountBed_003Ed__77(int _003C_003E1__state)
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
	private sealed class _003CSquidDeathCount_003Ed__68 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerStatus _003C_003E4__this;

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
		public _003CSquidDeathCount_003Ed__68(int _003C_003E1__state)
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

	public GameObject Player;

	public PickRay PickRay;

	public Days Days;

	public GameObject TimerUI;

	public float TimerCurrentRun;

	public TMP_Text TimerText;

	public GameObject UIMain;

	public GameObject TextUI;

	public Image BlackScreen;

	public GameObject RagdollPlayer;

	public Transform SpawnRagdollPos;

	public Transform SpawnRagdollPos2;

	public GameObject BloodSpatterHit;

	public bool CanBloodSpatter;

	public ScreenEffects ScreenEffects;

	public bool IsJumpscared;

	public bool Killed;

	public AudioClip SoundHurt;

	public bool lookAtAlligator;

	public bool LookAtGranny;

	public bool LookAtGrandpa;

	public float LookSpeed;

	public GameObject PauseSet;

	public Transform alligatorPLook;

	public GameObject GrannyPLook;

	public GameObject GrandpaPLook;

	public GameObject Camera;

	public GameObject CameraPivot;

	public Vector3 FinalLookDirection;

	public float MaxFOV;

	public float SpeedFOVFade;

	public Camera PlayerCam;

	[Header("HidingSpots")]
	public bool HidingFromMom;

	public bool HidingUnderThings;

	public bool HidingInClosets;

	public bool HidingFromHouseEnemy;

	public GameObject TeddyHand;

	public bool HasTeddy;

	public bool InBed1;

	public bool InBed2;

	public bool InBed3;

	public BedHide BedHide1;

	public BedHide BedHide2;

	public BedHide BedHide3;

	public Transform BedCam1;

	public Transform BedCam2;

	public Transform BedCam3;

	public GameObject Granny;

	public GameObject Grandpa;

	public TeddyHunt TH;

	public EnemyController EC;

	public GameObject E1;

	public GameObject E2;

	public Escapes Escapedo;

	public ClosetUnhid CUnh1;

	public ClosetUnhid CUnh2;

	public ClosetUnhid CUnh3;

	public ClosetUnhid CUnh4;

	public bool TimerStopped;

	public Animation ShakeAnim;

	public Eyes_Granny EyesGranny;

	public Eyes_Grandpa EyesGrandpa;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PlayerGettingStopped()
	{
	}

	public void DamageFeet()
	{
	}

	public void DamageSpider()
	{
	}

	public void SquidDeath()
	{
	}

	[IteratorStateMachine(typeof(_003CSquidDeathCount_003Ed__68))]
	private IEnumerator SquidDeathCount()
	{
		return null;
	}

	public void GrannyCaughtYou()
	{
	}

	public void GrannyCaughtYouBed()
	{
	}

	public void GrandpaCaughtYou()
	{
	}

	public void GrandpaCaughtYouBed()
	{
	}

	public void KnockDeath()
	{
	}

	public void NormalDeath(bool electric)
	{
	}

	public void SpawnRagdoll()
	{
	}

	[IteratorStateMachine(typeof(_003CDeathCount_003Ed__76))]
	private IEnumerator DeathCount()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDeathCountBed_003Ed__77))]
	private IEnumerator DeathCountBed(float Timer)
	{
		return null;
	}
}
