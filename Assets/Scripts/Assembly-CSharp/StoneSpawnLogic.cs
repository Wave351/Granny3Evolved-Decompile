using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StoneSpawnLogic : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CRespawnRoutine_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public StoneSpawnLogic _003C_003E4__this;

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
		public _003CRespawnRoutine_003Ed__8(int _003C_003E1__state)
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

	public GameObject stoneObj;

	public float spawnTimer;

	public float minX;

	public float maxX;

	public float minZ;

	public float maxZ;

	public List<GameObject> spawnedRocks;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003CRespawnRoutine_003Ed__8))]
	private IEnumerator RespawnRoutine()
	{
		return null;
	}

	public void CleanupList()
	{
	}

	public void WipeOutStones()
	{
	}

	public void SpawnStones()
	{
	}
}
