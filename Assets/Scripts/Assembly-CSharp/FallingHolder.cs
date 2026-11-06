using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class FallingHolder : MonoBehaviour
{
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
}
