using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class R_Grandpa : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerGo_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public R_Grandpa _003C_003E4__this;

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
		public _003CTimerGo_003Ed__13(int _003C_003E1__state)
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

	public GameObject[] ObjectsDisable;

	public Renderer Skin;

	public Texture2D lavaGrandpa;

	public bool isLava;

	public Color GrandpaDarken;

	public Color SkinC;

	public float TimerFade;

	public float TimerGone;

	public bool Fadin;

	public GameObject EC;

	public GameObject Shotgun;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerGo_003Ed__13))]
	private IEnumerator TimerGo()
	{
		return null;
	}
}
