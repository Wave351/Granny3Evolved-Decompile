using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public partial class LockPickRay : MonoBehaviour
{
	public Image lockPickUnused;
	public GameObject lockPickAvailable;
	public Animation doorLockPicking;
	public float distanceRay;
	public float lockPickProgress;
	public string nameCollHitLockPick;
	public DoorLogic currentDoor;
	public bool availableToLockPick;
	public bool lockPicking;
	public Image lockPickMeter;
	public GameObject lockPickingSound;
	public PickRay PRay;
	public GameObject Player;
	public PlayerStatus PS;
	public AudioClip lockPickedSound;
	public GameObject lockPickHintKeybind;
	public GameObject indicatorKeyLockPick;
	public bool lockPickActivated;
}
