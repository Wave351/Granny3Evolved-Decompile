using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/NMMonsterAnim.c (Granny 3 Evolved, IL2CPP x64).
public partial class NMMonsterAnim
{
		// Animation events of the nightmare monster.
		public void RandomStep() { audio.PlayOneShot(walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)]); }
		public void StartStopBlast(int i)
		{
			if (i == 0)
			{
				powerfulBlast.Play();
				var sound = powerfulBlast ? powerfulBlast.GetComponent<AudioSource>() : null;
				if (sound) sound.Play();
			}
			else if (i == 1) powerfulBlast.Stop();
		}
		// Its fog makes a noise that both Granny and Grandpa come to.
		public void FogAppearance()
		{
			Instantiate(noisyGranny, transform.position, Quaternion.identity);
			Instantiate(noisyGrandpa, transform.position, Quaternion.identity);
		}
		public void BlastStatus(int i)
		{
			if (i == 0) blasting = false;
			else if (i == 1) blasting = true;
		}
		// The blast knocks the player down.
		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.tag != "Player" || !blasting) return;
			if (FH.isFalling || FH.isLanding || PS.IsJumpscared) return;
			FH.HandleLanding(true);
		}
}
