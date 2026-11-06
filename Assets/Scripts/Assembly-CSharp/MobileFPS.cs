using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MobileFPS : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDragHeadTimer_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MobileFPS _003C_003E4__this;

		public float timerBack;

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
		public _003CDragHeadTimer_003Ed__47(int _003C_003E1__state)
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
	public Transform playerCamera;

	public CharacterController characterController;

	public Camera playerCamera2;

	public Transform CamPivot;

	[Header("Movement Related")]
	public float moveSpeed;

	public float SpeedMove;

	public float SpeedMoveCrouch;

	public float FallSpeed;

	public bool isMoving;

	private Vector3 moveDirection;

	[Header("Camera Rotate")]
	public float cameraRotationSpeed;

	public float maxXRotation;

	public float minXRotation;

	public Animation CameraAnim;

	public bool RotateXReser;

	public float rotationX;

	public float SpeedFade;

	[Header("Player Status")]
	public bool IsCrouched;

	public bool isAllowedToMove;

	public bool AbleToMove;

	public bool CanFade;

	public bool CamK;

	public bool InWeb;

	public GameObject momHand;

	[Header("Ground Checking")]
	public Transform groundCheck;

	public float groundCheckRadius;

	public LayerMask groundLayerMask;

	[Header("Other Components")]
	public FallingHolder FallingHolder;

	public PlayerStatus PS;

	public Days SystemDay;

	[Header("Platform Related")]
	public GameObject PlayerStuff;

	public bool InElev;

	public float ElevatorYAxis;

	public Vector3 LastElevPos;

	public GameObject ElevatorObj;

	public bool WasOnPlatform;

	private void Start()
	{
	}

	private void Awake()
	{
	}

	private void Update()
	{
	}

	private void GetTouchInput()
	{
	}

	private void HandlePlayerMovement(bool isGrounded)
	{
	}

	private void HandlePlayerMovementS(bool isGrounded)
	{
	}

	public void ApplyFinalMovements()
	{
	}

	public void HandleVerticalMovement()
	{
	}

	public void AdjustSpeed(float newSpeed)
	{
	}

	public void ReserRotation()
	{
	}

	public void DragHead(float timerToBack)
	{
	}

	[IteratorStateMachine(typeof(_003CDragHeadTimer_003Ed__47))]
	private IEnumerator DragHeadTimer(float timerBack)
	{
		return null;
	}

	public void ForcefulCameraAvailability()
	{
	}

	public void ResetRotationSlendrina()
	{
	}

	public void ResetRotation()
	{
	}

	public void OnControllerColliderHit(ControllerColliderHit OBJ)
	{
	}
}
