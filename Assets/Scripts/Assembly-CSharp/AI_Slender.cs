using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public partial class AI_Slender : MonoBehaviour
{
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
}
