using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class SpectralCrow : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003COnTriggerEnter_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Collider other;

		public SpectralCrow _003C_003E4__this;

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
		public _003COnTriggerEnter_003Ed__10(int _003C_003E1__state)
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

	public NavMeshAgent agent;

	public Animation birdAnim;

	public Transform[] randomTravelPoints;

	public int currentWaypointIndex;

	public int waypointNumberSelect;

	public float redecideRouteDistance;

	public PickRay pickRay;

	public ScreenEffects SE;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003COnTriggerEnter_003Ed__10))]
	private IEnumerator OnTriggerEnter(Collider other)
	{
		return null;
	}

	public void RandomWaypoint()
	{
	}
}
