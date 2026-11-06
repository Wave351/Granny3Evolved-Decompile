using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/PickGrandpa.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class PickGrandpa
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "shotgun" && !AI.IsDying && !AI.dizzy)
		{
			AI.Shotgun.SetActive(true);
			UnityEngine.Object.Destroy(other.gameObject);
		}
	}
}
