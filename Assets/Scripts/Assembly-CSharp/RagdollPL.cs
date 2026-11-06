using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RagdollPL : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDestroyTimer_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RagdollPL _003C_003E4__this;

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
		public _003CDestroyTimer_003Ed__33(int _003C_003E1__state)
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
	private sealed class _003CMoveCamToInitial_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RagdollPL _003C_003E4__this;

		private Vector3 _003CstartPos_003E5__2;

		private float _003Ct_003E5__3;

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
		public _003CMoveCamToInitial_003Ed__36(int _003C_003E1__state)
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

	public AudioClip[] Sounds;

	public AudioClip[] GrassFalls;

	public AudioClip Water;

	public bool OnlyOnce;

	public float TimesDropped;

	public bool CanWater;

	public bool Stopped;

	public bool DestroyOnSound;

	public bool WaterDes;

	public bool LookCamVal;

	public GameObject CameraPlayer;

	public GameObject ManagerPL;

	public GameObject RagdollParent;

	public Vector3 ImpactDirection;

	public Rigidbody NeckRigidbody;

	public Rigidbody MainBodyRigidbody;

	public float NeckDrag;

	public float DownDrag;

	public float GrannyForce;

	public float GrandpaForce;

	public float NormalForce;

	public float HowManyBumps;

	public float MaxBumps;

	public float NeckBounceForce;

	public float BodyBounceForce;

	public Transform InitialPositionCam;

	public float CamMoveDuration;

	public Transform RagdollTransform;

	public GameObject ElectricSparkles;

	private void Start()
	{
	}

	public void LookAt(Transform target)
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	[IteratorStateMachine(typeof(_003CDestroyTimer_003Ed__33))]
	private IEnumerator DestroyTimer()
	{
		return null;
	}

	public void PlayRandomSound()
	{
	}

	public void ImpactBodyFloor()
	{
	}

	[IteratorStateMachine(typeof(_003CMoveCamToInitial_003Ed__36))]
	private IEnumerator MoveCamToInitial()
	{
		return null;
	}
}
