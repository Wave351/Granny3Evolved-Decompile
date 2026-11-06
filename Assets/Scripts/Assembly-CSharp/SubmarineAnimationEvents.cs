using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SubmarineAnimationEvents : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CtimerPropellerFull_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SubmarineAnimationEvents _003C_003E4__this;

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
		public _003CtimerPropellerFull_003Ed__9(int _003C_003E1__state)
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

	public GameObject effectBelowEnable;

	public Animation propeller;

	public Transform alligator;

	public GameObject waterSubAccess;

	public GameObject camSubAccess;

	public GameObject camSubmarine;

	public AudioSource propellerAudio;

	public GameObject escapeMusic;

	public void SubmarinePropellerWork()
	{
	}

	[IteratorStateMachine(typeof(_003CtimerPropellerFull_003Ed__9))]
	private IEnumerator timerPropellerFull()
	{
		return null;
	}

	public void EffectBelow()
	{
	}

	public void AngleSwitch()
	{
	}

	public void EscapeMusicOn()
	{
	}
}
