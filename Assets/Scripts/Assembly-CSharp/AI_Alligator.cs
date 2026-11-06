using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public partial class AI_Alligator : MonoBehaviour
{
	[Header("Core")]
	public NavMeshAgent agent;
	public Animation animAlli;
	public PlayerStatus PS;
	public bool awakens;
	public bool hurt;
	public bool onHunt;
	public bool killingPlayer;
	public float timerDeathPlayer;
	public float detectDistanceAttack;
	public Transform player;
	public MeshCollider waterCollision;
	public float timesHit;
	public EscapeRouteChecks ERC;
	[Header("SFX")]
	public AudioSource ascendAudio;
	public AudioSource hurtAudio;
	public AudioSource aggroAudio;
	public AudioClip waterSplash;
	[Header("Awakening")]
	public Transform awakePoint;
	public Animation positionAnim;
}
