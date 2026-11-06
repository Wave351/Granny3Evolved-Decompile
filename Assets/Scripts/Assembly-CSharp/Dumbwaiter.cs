using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dumbwaiter : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerElevatorMovement_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Dumbwaiter _003C_003E4__this;

		public string anim;

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
		public _003CTimerElevatorMovement_003Ed__9(int _003C_003E1__state)
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

	public bool above;

	public bool travelling;

	public Animation dumbwaiterAnim;

	public BoxCollider[] buttons;

	public GameObject soundTravelLoop;

	public GameObject player;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ElevatorMovementStart()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerElevatorMovement_003Ed__9))]
	private IEnumerator TimerElevatorMovement(string anim)
	{
		return null;
	}

	public void DisableTravel()
	{
	}

	private void OnTriggerStay(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}
}
