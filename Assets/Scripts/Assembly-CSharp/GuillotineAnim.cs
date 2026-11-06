using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuillotineAnim : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CWaitForAnimationEnd1_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GuillotineAnim _003C_003E4__this;

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
		public _003CWaitForAnimationEnd1_003Ed__8(int _003C_003E1__state)
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

	public Animation anim;

	public GameObject Gj;

	public AudioClip SoundGuillotine;

	public GameObject CutArea;

	public GameObject NoiseOBJ;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Start1()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitForAnimationEnd1_003Ed__8))]
	public IEnumerator WaitForAnimationEnd1()
	{
		return null;
	}
}
