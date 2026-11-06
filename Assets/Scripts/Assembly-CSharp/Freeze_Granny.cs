using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Freeze_Granny : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CKillAll_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Freeze_Granny _003C_003E4__this;

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
		public _003CKillAll_003Ed__10(int _003C_003E1__state)
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

	public GameObject ParentOfIt;

	public AudioClip FrostBreak;

	public bool Broke;

	public float TimerGone;

	public GameObject ImpactB;

	public MeshCollider CollTop;

	public GameObject EC;

	public GameObject bat;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003CKillAll_003Ed__10))]
	private IEnumerator KillAll()
	{
		return null;
	}

	public void DestructionRagdolls()
	{
	}
}
