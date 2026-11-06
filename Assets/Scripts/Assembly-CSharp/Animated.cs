using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Animated : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CClosingAction_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Animated _003C_003E4__this;

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
		public _003CClosingAction_003Ed__20(int _003C_003E1__state)
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
	private sealed class _003COpeningAction_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Animated _003C_003E4__this;

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
		public _003COpeningAction_003Ed__19(int _003C_003E1__state)
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

	public bool Locked;

	public bool Closed;

	public Animation AnimatedObject;

	public AnimationClip OpenAnimation;

	public AnimationClip CloseAnimation;

	public AnimationClip LockedAnim;

	public bool CanBeLayer;

	public bool GoThru;

	public bool EnabledAnim;

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

	[IteratorStateMachine(typeof(_003COpeningAction_003Ed__19))]
	private IEnumerator OpeningAction()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CClosingAction_003Ed__20))]
	private IEnumerator ClosingAction()
	{
		return null;
	}

	public void UnlockAnimated()
	{
	}

	private void Update()
	{
	}
}
