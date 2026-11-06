using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CribTrigger : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CSlendrinaBegoneEnemy_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CribTrigger _003C_003E4__this;

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
		public _003CSlendrinaBegoneEnemy_003Ed__26(int _003C_003E1__state)
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
	private sealed class _003CSlendrinaTimer_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CribTrigger _003C_003E4__this;

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
		public _003CSlendrinaTimer_003Ed__25(int _003C_003E1__state)
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

	public Material TeddyM;

	public Material SlendrinaBodyM;

	public Material SlendrinaHairM;

	public Color StartSlendrinaBody;

	public Color StartSlendrinaHair;

	public Color StartTeddy;

	public Transform slendrinaPos;

	public GameObject Slendrina;

	public GameObject GlowEffect;

	public bool TeddyFade;

	public float SpeedFadeSlendrina;

	public float SpeedFadeSlendrinaH;

	public float maxHair;

	public bool slendrinaActive;

	public bool SlendrinaFades;

	public ParticleSystem Smoke;

	public GameObject TeddyObj;

	public Transform PlayerP;

	public Animation SlendrinaAnim;

	public GameObject ItemDropPoint;

	public Animation ShakeAnim;

	public PickRay rayPick;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	[IteratorStateMachine(typeof(_003CSlendrinaTimer_003Ed__25))]
	private IEnumerator SlendrinaTimer()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CSlendrinaBegoneEnemy_003Ed__26))]
	private IEnumerator SlendrinaBegoneEnemy()
	{
		return null;
	}

	public void SetMaterialToFadeMode(Material m)
	{
	}
}
