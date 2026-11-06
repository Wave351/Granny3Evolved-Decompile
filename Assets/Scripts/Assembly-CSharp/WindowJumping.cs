using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class WindowJumping : MonoBehaviour
{
	public bool IsJumping;
	public float Timer1;
	public GameObject Player;
	public Animation CamGoOutside;
	public GameObject Camera;
	public GameObject CamPivot;
	public Transform PosOutside;
	public GameObject windowOld;
	public GameObject windowBroken;
	public bool canJump;
	public GameObject climbSFX;
	public GameObject crowBar;
	public bool initiatedEvent;
	public GameObject navObstacle;
	public DoorLogic DLSubwayDoor;
	public GameObject windowCollision;
}
