using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class CrouchHolder : MonoBehaviour
{
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
}
