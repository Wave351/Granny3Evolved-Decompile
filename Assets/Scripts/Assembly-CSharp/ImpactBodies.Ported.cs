using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ImpactBodies.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class ImpactBodies
{
	// Same compiled constructor as EffectImpactCrystals (shared code).
	public ImpactBodies() { RadiusImpact = 20f; PushImpact = 15f; }

	private void Start()
	{
		foreach (Collider hit in Physics.OverlapSphere(transform.position, RadiusImpact))
		{
			Rigidbody body = hit.GetComponent<Rigidbody>();
			if (body != null)
			{
				Vector3 direction = (hit.transform.position - transform.position).normalized;
				body.AddForce(direction * PushImpact, ForceMode.Impulse);
			}
		}
	}
}
