using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CrouchHolder : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CCrouch_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrouchHolder _003C_003E4__this;

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
		public _003CCrouch_003Ed__34(int _003C_003E1__state)
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
	private sealed class _003CStand_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CrouchHolder _003C_003E4__this;

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
		public _003CStand_003Ed__35(int _003C_003E1__state)
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
	public CharacterController controller;

	public MobileFPS FPS2;

	public FallingHolder HolderF;

	public FootStep StepsHold;

	public Days days;

	public KeyCode CrouchKey;

	public Transform checkGround;

	[Header("Animations")]
	public Animation Cam;

	public Animation Anim;

	[Header("Audio")]
	public AudioClip C;

	public AudioClip S;

	public AudioSource Hold;

	[Header("Crouch/Stand State")]
	public bool On1;

	public bool On2;

	public bool isCrouching;

	public bool IsCrouched;

	public bool Starter;

	public bool Disabled;

	[Header("Above Obstacle Check")]
	public Transform UpCheck;

	public float RadiusSphere;

	public float RadiusSphere2;

	public float RadiusSphere3;

	public bool IsBelow;

	public LayerMask MasksCrouch;

	[Header("Character Controller (Unusued)")]
	public Vector3 originalCenter;

	public float originalHeight;

	public Vector3 originalPosition;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public bool CanStandUp()
	{
		return false;
	}

	public void WhichFloor(bool outside, bool house, bool water)
	{
	}

	public void StartCrouch()
	{
	}

	public void StopCrouch()
	{
	}

	public void CrouchStopJumpscare()
	{
	}

	[IteratorStateMachine(typeof(_003CCrouch_003Ed__34))]
	private IEnumerator Crouch()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CStand_003Ed__35))]
	private IEnumerator Stand()
	{
		return null;
	}
}
