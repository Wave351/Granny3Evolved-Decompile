using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class RagdollPL : MonoBehaviour
{
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
}
