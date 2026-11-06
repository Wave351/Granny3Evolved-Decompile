using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public partial class SpectralCrow : MonoBehaviour
{
	public NavMeshAgent agent;
	public Animation birdAnim;
	public Transform[] randomTravelPoints;
	public int currentWaypointIndex;
	public int waypointNumberSelect;
	public float redecideRouteDistance;
	public PickRay pickRay;
	public ScreenEffects SE;
}
