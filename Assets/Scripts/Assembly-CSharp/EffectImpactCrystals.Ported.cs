using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EffectImpactCrystals.c (Granny 3 Evolved, IL2CPP x64).
public partial class EffectImpactCrystals
{
		public EffectImpactCrystals() { RadiusImpact = 20f; PushImpact = 15f; }
		// Blows the crystal shards outwards.
		private void Start()
		{
			foreach (var hit in Physics.OverlapSphere(transform.position, RadiusImpact))
			{
				var body = hit.GetComponent<Rigidbody>();
				if (body == null || hit.gameObject.name != "ShardPiece") continue;
				body.AddForce((hit.transform.position - transform.position).normalized * PushImpact, ForceMode.Impulse);
			}
		}
}
