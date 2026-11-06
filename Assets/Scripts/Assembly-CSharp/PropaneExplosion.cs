using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class PropaneExplosion : MonoBehaviour
{
	public float PushImpact;
	public bool CanKillPlayer;
	public GameObject PlayerStatus;
	public GameObject EC;
	public GameObject ImpactObject;
	public float deathTimer;
	public bool availableLethal;
}
