using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/BGManager.c (Granny 3 Evolved, IL2CPP x64).
public partial class BGManager
{
		public BGManager() { MaxHouse = .7f; MaxOutside = .7f; MaxFacility = .4f; MaxHouseMusic = .3f; MaxGranny = .8f; SpeedChaseFade = 2f; }
		private void Start()
		{
			int music = PlayerPrefs.GetInt("Music");
			if (music == 1) { Music.isOn = true; MusicG.SetActive(true); }
			else if (music == 0) { Music.isOn = false; MusicG.SetActive(false); }
		}
		private void Update()
		{
			const float fadeOut = .4f;
			// Chase music fades in while Granny is hunting; the house music fades out meanwhile.
			if (!GrannyHunting)
			{
				ChaseNormal.volume -= SpeedChaseFade * Time.deltaTime;
				if (ChaseNormal.isPlaying && ChaseNormal.volume == 0) ChaseNormal.Stop();
				if (HouseMusic.volume < MaxHouseMusic) HouseMusic.volume += FadeBackBG * MaxHouseMusic * Time.deltaTime;
			}
			else
			{
				if (ChaseNormal.volume < MaxGranny) ChaseNormal.volume += SpeedChaseFade * Time.deltaTime;
				if (HouseMusic.volume > 0) HouseMusic.volume -= FadeBackBG * fadeOut * Time.deltaTime;
				if (!ChaseNormal.isPlaying) ChaseNormal.Play();
			}
			// Ambience for the area the player is in fades in; the others fade out.
			if (CanFade)
			{
				if (InHouse) Crossfade(House, MaxHouse, Outside, Facility);
				else if (InOutside) Crossfade(Outside, MaxOutside, House, Facility);
				else if (InFacility) Crossfade(Facility, MaxFacility, Outside, House);
			}
			if (!Music.isOn)
			{
				PlayerPrefs.SetInt("Music", 0);
				PlayerPrefs.Save();
				MusicG.SetActive(false);
			}
			else if (MusicG != null)
			{
				MusicG.SetActive(true);
				PlayerPrefs.SetInt("Music", 1);
				PlayerPrefs.Save();
			}
		}
		private void Crossfade(AudioSource on, float max, AudioSource off1, AudioSource off2)
		{
			const float fadeOut = .4f;
			if (on.volume < max) on.volume += max * Speed * Time.deltaTime;
			if (off1.volume > 0) off1.volume -= Speed * fadeOut * Time.deltaTime;
			if (off2.volume > 0) off2.volume -= Speed * fadeOut * Time.deltaTime;
		}
}
