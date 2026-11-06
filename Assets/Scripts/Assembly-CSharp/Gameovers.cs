using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Gameovers : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CEndOver_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Gameovers _003C_003E4__this;

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
		public _003CEndOver_003Ed__35(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CGameOverStart_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Gameovers _003C_003E4__this;

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
		public _003CGameOverStart_003Ed__36(int _003C_003E1__state)
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

	public GameObject Scene1;

	public GameObject Scene2;

	public GameObject Scene3;

	public GameObject mobileFPS;

	public GameObject carGameOverMusic;

	public GameObject roofGameOverMusic;

	public GameObject trainGameOverMusic;

	public GameObject GrannyStuff;

	public GameObject GrandpaStuff;

	public GameObject GrannyThere;

	public GameObject Granpa;

	public Transform PosGrannyEnd_Car;

	public Transform PosGrandpaEnd_Car;

	public Transform PosGrannyEnd_Roof;

	public Transform PosGrandpaEnd_Roof;

	public Transform PosGrannyEnd_Train;

	public Transform PosGrandpaEnd_Train;

	public GameObject BG;

	public GameObject OldB;

	public GameObject GameOverBox;

	public bool Fadin;

	public Image Black;

	public Color BlackColor;

	public bool AbleBlack;

	public float Speed;

	public float Speed2;

	public GameObject TextEnd;

	public GameObject BGN;

	public GameObject MainUIElem;

	public Transform train;

	public GameObject moonlight;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void GameOverCall()
	{
	}

	public void EndProcess()
	{
	}

	[IteratorStateMachine(typeof(_003CEndOver_003Ed__35))]
	private IEnumerator EndOver()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CGameOverStart_003Ed__36))]
	private IEnumerator GameOverStart()
	{
		return null;
	}
}
