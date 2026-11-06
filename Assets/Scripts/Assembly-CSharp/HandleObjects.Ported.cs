using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/HandleObjects.c (Granny 3 Evolved, IL2CPP x64).
public partial class HandleObjects
{
		private void Start()
		{
			if (PlayerPrefs.GetInt("Extras") == 1)
			{
				foreach (var item in ExtrasItems) item.SetActive(true);
				MainItems.SetActive(true);
				foreach (var item in disabledForExtras) item.SetActive(false);
				submarine.transform.position = teleportSubmarinePoint.position;
				var barrier = barrierFruity.localPosition;
				barrierFruity.localPosition = new Vector3(-1.26f, barrier.y, barrier.z);
			}
			else
			{
				ExtrasPlace.SetActive(false);
				facilityRobot.SetActive(false);
				triggerEatRegister.SetActive(false);
				foreach (var button in registerButtons)
				{
					Destroy(button.GetComponent<Interactive>());
					Destroy(button.GetComponent<BoxCollider>());
				}
			}
			// Seed system: the item layout for Extras or the chosen game version.
			if (PlayerPrefs.GetInt("FlashL") == 1)
			{
				SeedStuff.SetActive(true);
				GameObject layout = null;
				int version = PlayerPrefs.GetInt("CURVERSION");
				if (PlayerPrefs.GetInt("Extras") == 1) layout = Seed_Extra;
				else if (version == 2) layout = Seed_Main;
				else if (version == 1) layout = Seed_11;
				else if (version == 0) layout = Seed_10;
				if (layout != null) layout.SetActive(true);
			}
			// Item preset: 0 = in order (cycles through the six), 1 = random of the first five, 2-7 = a fixed preset.
			int choosing = PlayerPrefs.GetInt("PresetChoosin");
			int preset = -1;
			if (choosing == 0) preset = PlayerPrefs.GetInt("Preset");
			else if (choosing == 1) preset = UnityEngine.Random.Range(0, 5);
			else if (choosing >= 2 && choosing <= 7) preset = choosing - 2;
			if (preset >= 0 && preset <= 5)
			{
				PlayerPrefs.SetInt("Preset", (preset + 1) % 6);   // the next preset for "in order"
				PlayerPrefs.Save();
				PresetChoosenCurrent = preset;
			}
			// With the slingshot weapon option the player gets the baton instead (and Granny has the sling if she is on).
			if (PlayerPrefs.GetInt("GrannyWP") == 1 && PlayerPrefs.GetInt("Granny") == 1) sling.SetActive(false);
			else if (!(PlayerPrefs.GetInt("GrannyWP") == 1 && PlayerPrefs.GetInt("Granny") == 0)) return;
			baton.SetActive(true);
		}
}
