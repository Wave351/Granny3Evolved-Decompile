using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FallingHolder : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CWaitForAnimationEnd_003Ed__32 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FallingHolder _003C_003E4__this;

		public string animName;

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
		public _003CWaitForAnimationEnd_003Ed__32(int _003C_003E1__state)
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

	[Header("Core")]
	public CharacterController ch;

	public MobileFPS FPS;

	public PlayerStatus PlayerStatus;

	public PickRay Ray1;

	public DoorRay Ray2;

	public FootStep fsPlayer;

	[Header("Animations")]
	public Animation playerAnimation;

	public AnimationClip LandBad;

	[Header("Fall Settings")]
	public float FallDurationHold;

	public float FallChecker;

	public float FallMega;

	public float Speed;

	[Header("Ground Checking")]
	public float groundCheckDis;

	public LayerMask groundLayer;

	[Header("Audio")]
	public GameObject LandSound;

	public GameObject LandSoundGrass;

	public AudioSource Falling;

	public AudioSource LandTiny;

	[Header("Fall Values")]
	public bool CanFallSound;

	public bool isFalling;

	public bool isLanding;

	public bool Fell;

	public bool Damaged;

	public bool DeathFall;

	public float fallDuration;

	public float DurateCan;

	[Header("Camera Stuff")]
	public Transform Cam;

	public bool CanCamSmooth;

	public Transform CamPivot;

	private void Update()
	{
	}

	private void CheckForLanding()
	{
	}

	public void HandleLanding(bool hit)
	{
	}

	[IteratorStateMachine(typeof(_003CWaitForAnimationEnd_003Ed__32))]
	private IEnumerator WaitForAnimationEnd(string animName)
	{
		return null;
	}

	public void TestStop()
	{
	}
}
