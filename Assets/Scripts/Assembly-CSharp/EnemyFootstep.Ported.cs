using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EnemyFootstep.c (Granny 3 Evolved, IL2CPP x64).
public partial class EnemyFootstep
{
		public EnemyFootstep() { CurrentGround = 1; RayDistance = 1.5f; }
		private void Start() { InWater = false; }
		// Ground type under the enemy (from SPRayGround, on layers LS): 1 floor/noise maker, 2 grass, 3 water.
		private void Update()
		{
			RaycastHit hit;
			if (!Physics.Raycast(SPRayGround.position, Vector3.down, out hit, RayDistance, LS)) return;
			if (hit.collider.CompareTag("water")) CurrentGround = 3;
			else if (hit.collider.CompareTag("floor") || hit.collider.gameObject.CompareTag("Noice")) CurrentGround = 1;
			else if (hit.collider.CompareTag("grus")) CurrentGround = 2;
		}
		public void SoundEffectStep()
		{
			AudioClip[] clips = CurrentGround == 1 ? House : CurrentGround == 2 ? Grass : CurrentGround == 3 ? Water : null;
			if (clips == null) return;
			FeetSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
		}
}
