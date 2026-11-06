using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AI_Slender : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CscreenDistort_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AI_Slender _003C_003E4__this;

		private float _003CdistortSpeed_003E5__2;

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
		public _003CscreenDistort_003Ed__51(int _003C_003E1__state)
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

	[Header("Main Components")]
	public Transform SlenderTransform;

	public Renderer SlenderMesh;

	public Camera CameraPlayer;

	public PlayerStatus PS;

	public float NearDis;

	public float FarDis;

	[Header("Positioning")]
	public Vector3 StartPosV;

	public Transform StartPos;

	public Transform LookAtPoint;

	public Transform stareHead;

	public Transform LookRayCast;

	public Transform[] locationsTeleport;

	public GameObject CamPivot;

	public Transform PlayerTransform;

	public MobileFPS FPS;

	public Transform HeadPL;

	[Header("Values")]
	public float Health;

	public float DamageSpeed;

	public bool PlayerCaughtAttention;

	public bool IsRenderingSlender;

	public bool SlenderTouchesPlayer;

	public LayerMask LayersRay;

	public float TimerTeleport;

	public float TimerMaxTeleport;

	public float TimerIdle;

	public float TimerMaxIdle;

	public bool canAttack;

	public bool PathingToPlayer;

	public float lookAtSpeed;

	[Header("Audio")]
	public AudioSource SawSource;

	public AudioSource SlenderHunts;

	public AudioClip StareFar;

	public AudioClip StareNear;

	public float CloseDistance;

	public float TeleportAwayDistance;

	[Header("UI")]
	public Image BloodScreen;

	public float BorderBloodValue;

	public float SpeedBorderFade;

	public float SpeedLookingFade;

	public float SpeedBorderFadeGo;

	public float SpeedLookingFadeGo;

	[Header("Others")]
	public EnemyController EC;

	public FallingHolder Fall;

	public BGManager BGM;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private bool CameraRenderSlendrina(Renderer MeshE, Camera Player)
	{
		return false;
	}

	public void EnemySpawn()
	{
	}

	public void ReturnIdle()
	{
	}

	public void DisappearLogic()
	{
	}

	public void Teleport()
	{
	}

	[IteratorStateMachine(typeof(_003CscreenDistort_003Ed__51))]
	private IEnumerator screenDistort()
	{
		return null;
	}
}
