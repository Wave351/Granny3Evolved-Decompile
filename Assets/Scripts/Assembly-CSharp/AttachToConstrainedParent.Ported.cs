using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/AttachToConstrainedParent.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class AttachToConstrainedParent
{
	private void Start()
	{
		localPos = parent.InverseTransformPoint(transform.position);
		attached = true;
	}

	private void LateUpdate()
	{
		if (attached)
		{
			transform.position = parent.TransformPoint(localPos);
		}
		if (!forceIt)
		{
			return;
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Vector3 local = child.localPosition;
			child.localPosition = new Vector3(local.x, 0f, local.z);
		}
	}
}
