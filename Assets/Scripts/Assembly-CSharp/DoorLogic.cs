using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CClosingAction_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DoorLogic _003C_003E4__this;

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
		public _003CClosingAction_003Ed__26(int _003C_003E1__state)
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
	private sealed class _003CEnemyOpeningFromLockStateTimed_003Ed__28 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DoorLogic _003C_003E4__this;

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
		public _003CEnemyOpeningFromLockStateTimed_003Ed__28(int _003C_003E1__state)
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
	private sealed class _003COpeningAction_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DoorLogic _003C_003E4__this;

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
		public _003COpeningAction_003Ed__25(int _003C_003E1__state)
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

	public float OpeningTimer;

	public float ClosingTimer;

	public AudioSource Audio;

	public AudioClip CloseSound;

	public AudioClip OpenSound;

	public AudioClip LockedSound;

	public Animation AnimatedObject;

	public AnimationClip OpenAnimation;

	public AnimationClip CloseAnimation;

	public AnimationClip LockedAnim;

	public bool Locked;

	public bool LockpickAvailable;

	public bool lockPickHighSpedUp;

	public bool Closed;

	public bool CanBeLayer;

	public bool goThruDoor;

	public bool EnabledAnim;

	public float lockPickSpeed;

	public GameObject lockPickCollider;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OpenAction()
	{
	}

	public void CloseAction()
	{
	}

	public void OpeningClosingAttempts()
	{
	}

	public void AttemptCloseNewDay()
	{
	}

	[IteratorStateMachine(typeof(_003COpeningAction_003Ed__25))]
	private IEnumerator OpeningAction()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CClosingAction_003Ed__26))]
	private IEnumerator ClosingAction()
	{
		return null;
	}

	public void EnemyOpeningFromLockState()
	{
	}

	[IteratorStateMachine(typeof(_003CEnemyOpeningFromLockStateTimed_003Ed__28))]
	public IEnumerator EnemyOpeningFromLockStateTimed()
	{
		return null;
	}

	public void UnlockDoor(bool lockpick)
	{
	}
}
