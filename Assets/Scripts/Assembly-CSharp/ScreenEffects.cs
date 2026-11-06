using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ScreenEffects : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHurtPlayerS_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScreenEffects _003C_003E4__this;

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
		public _003CHurtPlayerS_003Ed__30(int _003C_003E1__state)
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

	public Image BloodScreen1;

	public Image BloodBitenScreen;

	public Image BloodScreenHit;

	public Image BloodScreenGameover;

	public Image BloodScreenDay4;

	public bool Hurt;

	public float Blood;

	public float SpeedFadeUp;

	public float SpeedFadeDown;

	public PostProcessProfile Profile;

	public ChromaticAberration CA;

	public Vignette V;

	public DepthOfField DOF;

	public Grain G;

	public bool NormalSee;

	public float SpeedGo;

	public float SpeedAppear;

	public float Scare1;

	public float Scare2;

	public float Max1;

	public float Max2;

	public float grainValue;

	public float speedGainGrain;

	public float speedLoseGrain;

	public AI_Slender SlenderAI;

	public Animation Camera;

	public GameObject Player;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void HurtPlayer()
	{
	}

	[IteratorStateMachine(typeof(_003CHurtPlayerS_003Ed__30))]
	private IEnumerator HurtPlayerS()
	{
		return null;
	}
}
