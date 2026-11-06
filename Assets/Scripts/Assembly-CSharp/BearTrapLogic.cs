using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class BearTrapLogic : MonoBehaviour
{
	public AudioClip PlacedTrap;
	public AudioClip AggroSound;
	public bool Placed;
	public bool TrappedPlayer;
	public Animation Anim;
	public AnimationClip Clip;
	public GameObject RayB;
	public GameObject ScEFF;
	public GameObject Noise;
	public GameObject Player;
	public float RayDistance;
	public bool PlayerHit;
	public Transform RayCaster;
	public bool ItemCaughtIt;
	public Collider collMain;
}
