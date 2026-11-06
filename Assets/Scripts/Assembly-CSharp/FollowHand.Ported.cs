using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FollowHand.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class FollowHand
{
	public FollowHand()
	{
		RotateSpeed = 3f;
	}

	private void Start()
	{
		OffSet = transform.position - Head.transform.position;
	}

	private void LateUpdate()
	{
		transform.position = Head.transform.position + OffSet;
		transform.rotation = Quaternion.Slerp(transform.rotation, Head.transform.rotation, Time.deltaTime * RotateSpeed);
	}
}
