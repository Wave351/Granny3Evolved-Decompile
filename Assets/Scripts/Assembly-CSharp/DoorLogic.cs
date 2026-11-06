using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class DoorLogic : MonoBehaviour
{
	public float OpeningTimer;
	public float ClosingTimer;
	public AudioSource Audio;
	public AudioClip CloseSound;
	public AudioClip OpenSound;
	public AudioClip LockedSound;
	public Animation AnimatedObject;
	public AnimationClip OpenAnimation;
	public AnimationClip CloseAnimation;
	public AnimationClip LockedAnim;
	public bool Locked;
	public bool LockpickAvailable;
	public bool lockPickHighSpedUp;
	public bool Closed;
	public bool CanBeLayer;
	public bool goThruDoor;
	public bool EnabledAnim;
	public float lockPickSpeed;
	public GameObject lockPickCollider;
}
