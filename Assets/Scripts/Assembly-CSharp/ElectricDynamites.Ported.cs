using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/ElectricDynamites.c (Granny 3 Evolved, IL2CPP x64).
public partial class ElectricDynamites
{
		public ElectricDynamites() { normalSpawnTimer = 10f; restrictedSpawnTimer = 20f; lastSpawnI = -1; }
		private void Start() { StartCoroutine(DynamiteSpawn()); }
		// The dynamite tracker: shown while the tracker is in hand; a defused box (no dynamite left) is removed.
		private void Update()
		{
			var trackerAudio = trackerHand ? trackerHand.GetComponent<AudioSource>() : null;
			if (!trackerHand.activeSelf)
			{
				if (elecDynamites.activeSelf) elecDynamites.SetActive(false);
				if (currentTarget == null) return;
			}
			else
			{
				if (!elecDynamites.activeSelf) elecDynamites.SetActive(true);
				if (currentTarget == null)
				{
					if (lastSpawnI == -1) return;
					locationText.text = "Tracking unavailable";
					if (trackerAudio) trackerAudio.playOnAwake = false;
					return;
				}
			}
			var box = currentTarget.GetComponent<DBC>();
			if (box != null && box.dynamite == null)
			{
				if (box.stopped) return;
				box.stopped = true;
				if (box.disappearEffect != null)
				{
					box.disappearEffect.SetActive(true);
					box.disappearEffect.transform.parent = null;
				}
				Destroy(box.gameObject);
				currentTarget = null;
				return;
			}
			if (trackerAudio) trackerAudio.playOnAwake = true;
		}
		// A new dynamite box appears at a random spot every normalSpawnTimer seconds; after one is defused,
		// tracking is unavailable for restrictedSpawnTimer seconds.
		private IEnumerator DynamiteSpawn()
		{
			while (true)
			{
				var trackerAudio = trackerHand ? trackerHand.GetComponent<AudioSource>() : null;
				if (currentTarget == null)
				{
					if (lastSpawnI != -1)
					{
						if (trackerAudio == null) yield break;
						trackerAudio.playOnAwake = false;
						locationText.text = "Tracking unavailable. be back later";
						yield return new WaitForSeconds(restrictedSpawnTimer);
						lastSpawnI = -1;
						continue;
					}
				}
				else
				{
					Destroy(currentTarget);
					if (trackerAudio) trackerAudio.playOnAwake = true;
				}
				int i;
				do i = UnityEngine.Random.Range(0, spawnsElectricDynamite.Length);
				while (i == lastSpawnI);
				var spawn = spawnsElectricDynamite[i];
				lastSpawnI = i;
				currentTarget = Instantiate(dynamiteBox, spawn.transform.position, spawn.transform.rotation);
				currentTarget.transform.parent = elecDynamites.transform;
				currentTarget.SetActive(true);
				locationText.text = spawn.locationName;
				if (trackerAudio == null) yield break;
				trackerAudio.Play();
				yield return new WaitForSeconds(normalSpawnTimer);
			}
		}
}
