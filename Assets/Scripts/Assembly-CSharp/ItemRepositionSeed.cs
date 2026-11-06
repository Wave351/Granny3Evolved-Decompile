using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemRepositionSeed : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ItemRepositionSeed _003C_003E4__this;

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
		public _003CStart_003Ed__34(int _003C_003E1__state)
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

	public bool RepositionedItem;

	[Header("Items")]
	public Transform GateFuse;

	public Transform ShedKey;

	public Transform GenCable;

	public Transform BridgeCrank;

	public Transform Coin;

	public Transform ElecSwitch;

	public Transform Accelerator;

	public Transform TrainKey;

	public Transform Firewood;

	public Transform Matches;

	public Transform Coconut;

	public Transform PDKey;

	public Transform Plank;

	public Transform Teddy;

	public Transform WPKey;

	public Transform SafeKey;

	public Transform Valve;

	public Transform Propane;

	public Transform BalloonPart1;

	public Transform BalloonPart2;

	public Transform BalloonPart3;

	public Transform Book;

	public Transform Castle;

	public Transform Handwheel;

	public Transform IntenseCoin;

	public Transform Painting;

	public Transform SecurityRelay;

	public Transform SubLever;

	public bool Rigid;

	public Dictionary<string, (Transform item, string spotName)> itemMap;

	public void Awake()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
	}

	public Transform FindChildByName(Transform parent, string name)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CStart_003Ed__34))]
	private IEnumerator Start()
	{
		return null;
	}
}
