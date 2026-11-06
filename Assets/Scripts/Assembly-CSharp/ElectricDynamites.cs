using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ElectricDynamites : MonoBehaviour
{
	[Serializable]
	public class Spawns
	{
		public Transform transform;

		public string locationName;
	}

	[CompilerGenerated]
	private sealed class _003CDynamiteSpawn_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ElectricDynamites _003C_003E4__this;

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
		public _003CDynamiteSpawn_003Ed__12(int _003C_003E1__state)
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

	[Header("Main Logic")]
	public GameObject elecDynamites;

	public GameObject dynamiteBox;

	public float normalSpawnTimer;

	public float restrictedSpawnTimer;

	public int lastSpawnI;

	[Header("Spawns")]
	public Spawns[] spawnsElectricDynamite;

	public GameObject currentTarget;

	[Header("Tracker")]
	public GameObject trackerHand;

	public TMP_Text locationText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CDynamiteSpawn_003Ed__12))]
	private IEnumerator DynamiteSpawn()
	{
		return null;
	}
}
