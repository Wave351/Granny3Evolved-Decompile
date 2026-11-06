using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class CrowChimney : MonoBehaviour
{
	public Animation NestBird;
	public Animation Flying;
	public AudioSource Audio;
	public AudioClip Attack;
	public AudioClip Crow;
	public ScreenEffects ScreenEffects;
	public GameObject ColliderPickUp;
	public GameObject BloodKill;
	public bool Killed;
	public TeddyHunt TH;
	public EnemyController EC;
	public bool HurtTemp;
	public GameObject burningLog;
	public float logEndTimer;
	public bool flewff;
	public BoxCollider log;
	public Transform player;
	public Transform playerHead;
	public float pointRadius;
	public Transform body;
	public Quaternion oldRotation;
}
