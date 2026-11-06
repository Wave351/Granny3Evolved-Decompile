using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public partial class Interactive : MonoBehaviour
{
	[Header("Core")]
	public bool isTimed;
	public float timerEventTimed;
	public bool isOneTimeAction;
	public bool performedAction;
	[Header("Actions")]
	public UnityEvent performedEvent;
	public UnityEvent timedEvent;
}
