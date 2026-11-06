using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/NoisyDrop.c (Granny 3 Evolved, IL2CPP x64).
public partial class NoisyDrop
{
		// Only one noisy drop with the same tag may exist: the others are removed.
		private void Start()
		{
			OtherNoises = GameObject.FindGameObjectsWithTag(AppliedTagName);
			foreach (var other in OtherNoises)
				if (other != gameObject) Destroy(other);
		}
		// A moving drop lands (turns kinematic) on floor, water, a noise maker or grass.
		private void OnTriggerStay(Collider other)
		{
			var body = GetComponent<Rigidbody>();
			if (body == null || body.isKinematic) return;
			string tag = other.gameObject.tag;
			if (tag != "floor" && tag != "water" && tag != "Noice" && tag != "grus") return;
			body.isKinematic = true;
		}
}
