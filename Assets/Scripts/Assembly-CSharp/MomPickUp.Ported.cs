using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/MomPickUp.c (Granny 3 Evolved, IL2CPP x64).
public partial class MomPickUp
{
		public void SelfDestruction() { Destroy(destructionObject); }
		public void BodiesStandby()
		{
			foreach (var body in rigidBodies) body.linearVelocity = Vector3.zero;
		}
}
