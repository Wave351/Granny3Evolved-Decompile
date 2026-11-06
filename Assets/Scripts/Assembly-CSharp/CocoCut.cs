using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CocoCut : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CMomDisappearTimer_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CocoCut _003C_003E4__this;

		private List<Renderer> _003CrenderersToFade_003E5__2;

		private float _003Cduration_003E5__3;

		private float _003CelapsedTime_003E5__4;

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
		public _003CMomDisappearTimer_003Ed__17(int _003C_003E1__state)
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

	public GameObject Coco;

	public GameObject DeadCoco;

	public GameObject mom;

	public GameObject neckPiece;

	public GameObject bloodNeck;

	public GameObject headBlood;

	public Rigidbody headRigid;

	public GameObject bloodEffect;

	public GameObject[] momPieces;

	public bool momSliced;

	public Transform itemPoint;

	public GameObject subKey;

	public GameObject Noice;

	public GameObject CutArea;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	[IteratorStateMachine(typeof(_003CMomDisappearTimer_003Ed__17))]
	private IEnumerator MomDisappearTimer()
	{
		return null;
	}
}
