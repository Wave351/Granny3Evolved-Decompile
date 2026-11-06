using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MelonContainItem : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStopPlacedItemLogic_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MelonContainItem _003C_003E4__this;

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
		public _003CStopPlacedItemLogic_003Ed__11(int _003C_003E1__state)
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

	public bool PlacedItemIn;

	public Transform MelonDropPoint;

	public Transform SafeKey;

	public Transform Coin;

	public Transform PDKey;

	public Transform WPKey;

	public Transform ShedKey;

	public Transform TrainKey;

	public Transform Valve;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CStopPlacedItemLogic_003Ed__11))]
	private IEnumerator StopPlacedItemLogic()
	{
		return null;
	}

	private void OnTriggerEnter(Collider other)
	{
	}
}
