using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class Dumbwaiter : MonoBehaviour
{
	public bool above;
	public bool travelling;
	public Animation dumbwaiterAnim;
	public BoxCollider[] buttons;
	public GameObject soundTravelLoop;
	public GameObject player;
}
