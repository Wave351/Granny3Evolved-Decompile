using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class TicketControlBot : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CActivateTicket_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TicketControlBot _003C_003E4__this;

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
		public _003CActivateTicket_003Ed__46(int _003C_003E1__state)
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
	private sealed class _003CAlarmSound_003Ed__49 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TicketControlBot _003C_003E4__this;

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
		public _003CAlarmSound_003Ed__49(int _003C_003E1__state)
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
	private sealed class _003CAlertActivate_003Ed__48 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TicketControlBot _003C_003E4__this;

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
		public _003CAlertActivate_003Ed__48(int _003C_003E1__state)
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
	private sealed class _003CTicketInsertionRobo_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TicketControlBot _003C_003E4__this;

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
		public _003CTicketInsertionRobo_003Ed__47(int _003C_003E1__state)
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

	[Header("Core")]
	public Transform idlePoint;

	public NavMeshAgent agent;

	public PlayerStatus PS;

	[Header("Patrol Logic / Death / Hunt")]
	public bool travelBack;

	public bool hunting;

	public bool alert;

	public float blowsStruck;

	public bool dead;

	public bool gaveTicket;

	public bool ticketStartedAction;

	public bool ticketTravel;

	public GameObject smokeDeath1;

	public GameObject smokeDeath2;

	public GameObject smokeDeath3;

	public GameObject lampObject;

	public float huntingTimer;

	public float huntTimerMax;

	public float timerFadeTravel;

	public float timerFadeMaxTravel;

	public float timerAlert;

	public float timerAlertMax;

	public Transform player;

	public Transform playerHead;

	public Animation headUnitAnim;

	public GameObject doorActivator;

	public GameObject triggerNoisy;

	public GameObject smokeParticlesFlyOff;

	[Header("Vision")]
	public Transform sight1;

	public Transform sight2;

	public LayerMask layersRay;

	[Header("Helpful")]
	public Transform activatePoint;

	public RegPuzzle actionDoorActivatorTrains;

	public GameObject ticketTrain;

	[Header("SFX")]
	public GameObject patrolSFX;

	public GameObject alarmSFX;

	public GameObject ticketSFX;

	public AudioClip robotDefeat;

	public bool fadingPatrolSFX;

	public bool alarmSoundedOnce;

	public Transform SPNoisy;

	public GameObject noisyGrandpa;

	public GameObject noisyGranny;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SilentButDeadly()
	{
	}

	public void DeathUponRobot()
	{
	}

	[IteratorStateMachine(typeof(_003CActivateTicket_003Ed__46))]
	private IEnumerator ActivateTicket()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CTicketInsertionRobo_003Ed__47))]
	private IEnumerator TicketInsertionRobo()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CAlertActivate_003Ed__48))]
	private IEnumerator AlertActivate()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CAlarmSound_003Ed__49))]
	private IEnumerator AlarmSound()
	{
		return null;
	}
}
