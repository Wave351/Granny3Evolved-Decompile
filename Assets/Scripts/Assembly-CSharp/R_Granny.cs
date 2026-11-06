using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class R_Granny : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerGo_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public R_Granny _003C_003E4__this;

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
		public _003CTimerGo_003Ed__19(int _003C_003E1__state)
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

	public Texture2D lavaGranny;

	public bool isLava;

	public Renderer Skin;

	public Renderer F1;

	public Renderer F2;

	public Color SkinC;

	public Color GrannyDarken;

	public Color FC;

	public float TimerFade;

	public float TimerGone;

	public bool Fadin;

	public GameObject GrannyStuff;

	public GameObject PS;

	public GameObject EC;

	public GameObject slingWP;

	public GameObject bat;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerGo_003Ed__19))]
	private IEnumerator TimerGo()
	{
		return null;
	}
}
