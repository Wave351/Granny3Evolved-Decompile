using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public partial class NightmareMonster : MonoBehaviour
{
	[Header("Core")]
	public NavMeshAgent agent;
	public Animation anim;
	public bool intimidated;
	public bool blasting;
	public bool wandering;
	public bool idle;
	public float walkRadius;
	public float idleTime;
	public float blastRecoverMax;
	public bool canBlast;
	public bool changingState;
	[Header("Vision")]
	public Transform visionPoint;
	public Transform playerHead;
	public float distanceVision;
	public float timerInTerritory;
	public float timerMasAggro;
	public GameObject noisyGranny;
	public DoorLogic prisonerDoor;
}
