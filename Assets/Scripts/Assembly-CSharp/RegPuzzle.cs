using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public partial class RegPuzzle : MonoBehaviour
{
	[Header("Core")]
	public GameObject requiredItem;
	public bool moreThanOne;
	public string failedText;
	public bool isTimed;
	public float timerEventTimed;
	public bool performedAction;
	[Header("Actions")]
	public UnityEvent performedEvent;
	public UnityEvent timedEvent;
	public UnityEvent performedEventNo;
}
