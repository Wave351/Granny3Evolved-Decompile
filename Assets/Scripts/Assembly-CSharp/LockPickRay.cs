using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LockPickRay : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHowtoUseLockPick_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LockPickRay _003C_003E4__this;

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
		public _003CHowtoUseLockPick_003Ed__20(int _003C_003E1__state)
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

	public Image lockPickUnused;

	public GameObject lockPickAvailable;

	public Animation doorLockPicking;

	public float distanceRay;

	public float lockPickProgress;

	public string nameCollHitLockPick;

	public DoorLogic currentDoor;

	public bool availableToLockPick;

	public bool lockPicking;

	public Image lockPickMeter;

	public GameObject lockPickingSound;

	public PickRay PRay;

	public GameObject Player;

	public PlayerStatus PS;

	public AudioClip lockPickedSound;

	public GameObject lockPickHintKeybind;

	public GameObject indicatorKeyLockPick;

	public bool lockPickActivated;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CHowtoUseLockPick_003Ed__20))]
	private IEnumerator HowtoUseLockPick()
	{
		return null;
	}
}
