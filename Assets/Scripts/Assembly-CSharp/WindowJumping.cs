using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WindowJumping : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CGoingInside_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public WindowJumping _003C_003E4__this;

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
		public _003CGoingInside_003Ed__20(int _003C_003E1__state)
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

	public bool IsJumping;

	public float Timer1;

	public GameObject Player;

	public Animation CamGoOutside;

	public GameObject Camera;

	public GameObject CamPivot;

	public Transform PosOutside;

	public GameObject windowOld;

	public GameObject windowBroken;

	public bool canJump;

	public GameObject climbSFX;

	public GameObject crowBar;

	public bool initiatedEvent;

	public GameObject navObstacle;

	public DoorLogic DLSubwayDoor;

	public GameObject windowCollision;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DestroyWindow()
	{
	}

	public void JumpInside()
	{
	}

	[IteratorStateMachine(typeof(_003CGoingInside_003Ed__20))]
	private IEnumerator GoingInside()
	{
		return null;
	}
}
