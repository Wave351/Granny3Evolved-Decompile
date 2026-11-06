using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Freeze_Grandpa : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CKillAll_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Freeze_Grandpa _003C_003E4__this;

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
		public _003CKillAll_003Ed__12(int _003C_003E1__state)
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

	public GameObject[] Parts;

	public GameObject BodyOriginal;

	public GameObject ParentOfIt;

	public AudioClip FrostBreak;

	public bool Broke;

	public float TimerGone;

	public GameObject EC;

	public GameObject Shotgun;

	public GameObject ImpactB;

	public GameObject Head;

	public GameObject armItem;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003CKillAll_003Ed__12))]
	private IEnumerator KillAll()
	{
		return null;
	}

	public void DestructionRagdolls()
	{
	}
}
