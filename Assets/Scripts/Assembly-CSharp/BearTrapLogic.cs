using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BearTrapLogic : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerDes_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BearTrapLogic _003C_003E4__this;

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
		public _003CTimerDes_003Ed__17(int _003C_003E1__state)
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
	private sealed class _003CTimerDesItem_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BearTrapLogic _003C_003E4__this;

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
		public _003CTimerDesItem_003Ed__18(int _003C_003E1__state)
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

	public AudioClip PlacedTrap;

	public AudioClip AggroSound;

	public bool Placed;

	public bool TrappedPlayer;

	public Animation Anim;

	public AnimationClip Clip;

	public GameObject RayB;

	public GameObject ScEFF;

	public GameObject Noise;

	public GameObject Player;

	public float RayDistance;

	public bool PlayerHit;

	public Transform RayCaster;

	public bool ItemCaughtIt;

	public Collider collMain;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerDes_003Ed__17))]
	private IEnumerator TimerDes()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CTimerDesItem_003Ed__18))]
	private IEnumerator TimerDesItem()
	{
		return null;
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	public void ShootGun()
	{
	}
}
