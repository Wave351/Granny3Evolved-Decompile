using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TrainDoor.c (Granny 3 Evolved, IL2CPP x64).
public partial class TrainDoor
{
		// Train doors: Granny/Grandpa open closed ones; the player toggles them through AnimationCall.
		private void OnTriggerStay(Collider other)
		{
			if (!closed || animDoors.isPlaying) return;
			string tag = other.gameObject.tag;
			if (tag != "granny" && tag != "granpa") return;
			AnimationCall();
		}
		public void AnimationCall()
		{
			if (animDoors.isPlaying) return;
			if (!closed)
			{
				closed = true;
				audioDoors.PlayOneShot(closeSound);
				animDoors.Play(second ? "TrainPairClose2" : "TrainPairClose");
			}
			else
			{
				closed = false;
				audioDoors.PlayOneShot(openSound);
				animDoors.Play(second ? "TrainPairOpen2" : "TrainPairOpen");
			}
		}
}
