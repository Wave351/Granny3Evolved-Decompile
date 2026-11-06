using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CrowChimney : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerWaitBirdReturn_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrowChimney _003C_003E4__this;

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
		public _003CTimerWaitBirdReturn_003Ed__29(int _003C_003E1__state)
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
	private sealed class _003CWaitAfterAttack2_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrowChimney _003C_003E4__this;

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
		public _003CWaitAfterAttack2_003Ed__25(int _003C_003E1__state)
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
	private sealed class _003CWaitAfterAttack_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrowChimney _003C_003E4__this;

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
		public _003CWaitAfterAttack_003Ed__28(int _003C_003E1__state)
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

	public Animation NestBird;

	public Animation Flying;

	public AudioSource Audio;

	public AudioClip Attack;

	public AudioClip Crow;

	public ScreenEffects ScreenEffects;

	public GameObject ColliderPickUp;

	public GameObject BloodKill;

	public bool Killed;

	public TeddyHunt TH;

	public EnemyController EC;

	public bool HurtTemp;

	public GameObject burningLog;

	public float logEndTimer;

	public bool flewff;

	public BoxCollider log;

	public Transform player;

	public Transform playerHead;

	public float pointRadius;

	public Transform body;

	public Quaternion oldRotation;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HurtDart()
	{
	}

	public void HurtBird()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitAfterAttack2_003Ed__25))]
	private IEnumerator WaitAfterAttack2()
	{
		return null;
	}

	public void KillAssBird()
	{
	}

	public void AttemptSteal()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitAfterAttack_003Ed__28))]
	private IEnumerator WaitAfterAttack()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CTimerWaitBirdReturn_003Ed__29))]
	private IEnumerator TimerWaitBirdReturn()
	{
		return null;
	}
}
