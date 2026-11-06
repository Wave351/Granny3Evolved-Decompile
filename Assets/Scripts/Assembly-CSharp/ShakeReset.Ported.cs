using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ShakeReset.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class ShakeReset
{
	public void ResetLogic()
	{
		transform.localEulerAngles = Vector3.zero;
	}
}
