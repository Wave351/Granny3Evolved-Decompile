using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public partial class TicketControlBot : MonoBehaviour
{
	[Header("Core")]
	public Transform idlePoint;
	public NavMeshAgent agent;
	public PlayerStatus PS;
	[Header("Patrol Logic / Death / Hunt")]
	public bool travelBack;
	public bool hunting;
	public bool alert;
	public float blowsStruck;
	public bool dead;
	public bool gaveTicket;
	public bool ticketStartedAction;
	public bool ticketTravel;
	public GameObject smokeDeath1;
	public GameObject smokeDeath2;
	public GameObject smokeDeath3;
	public GameObject lampObject;
	public float huntingTimer;
	public float huntTimerMax;
	public float timerFadeTravel;
	public float timerFadeMaxTravel;
	public float timerAlert;
	public float timerAlertMax;
	public Transform player;
	public Transform playerHead;
	public Animation headUnitAnim;
	public GameObject doorActivator;
	public GameObject triggerNoisy;
	public GameObject smokeParticlesFlyOff;
	[Header("Vision")]
	public Transform sight1;
	public Transform sight2;
	public LayerMask layersRay;
	[Header("Helpful")]
	public Transform activatePoint;
	public RegPuzzle actionDoorActivatorTrains;
	public GameObject ticketTrain;
	[Header("SFX")]
	public GameObject patrolSFX;
	public GameObject alarmSFX;
	public GameObject ticketSFX;
	public AudioClip robotDefeat;
	public bool fadingPatrolSFX;
	public bool alarmSoundedOnce;
	public Transform SPNoisy;
	public GameObject noisyGrandpa;
	public GameObject noisyGranny;
}
