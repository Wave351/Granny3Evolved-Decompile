using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class RegPuzzle : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CInitiateTimedEvent_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RegPuzzle _003C_003E4__this;

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
		public _003CInitiateTimedEvent_003Ed__12(int _003C_003E1__state)
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

	[Header("Core")]
	public GameObject requiredItem;

	public bool moreThanOne;

	public string failedText;

	public bool isTimed;

	public float timerEventTimed;

	public bool performedAction;

	[Header("Actions")]
	public UnityEvent performedEvent;

	public UnityEvent timedEvent;

	public UnityEvent performedEventNo;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Interacted()
	{
	}

	[IteratorStateMachine(typeof(_003CInitiateTimedEvent_003Ed__12))]
	public IEnumerator InitiateTimedEvent()
	{
		return null;
	}
}
