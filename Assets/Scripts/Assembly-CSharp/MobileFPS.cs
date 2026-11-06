using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class MobileFPS : MonoBehaviour
{
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
}
