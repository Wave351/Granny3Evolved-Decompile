using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Paused.c (Granny 3 Evolved, IL2CPP x64).
public partial class Paused
{
		private void Start()
		{
			float volume = PlayerPrefs.GetFloat(PlayerPrefsTest, 1f);
			Volume.value = volume;
			AudioListener.volume = volume;
			PlayerPrefs.SetFloat(PlayerPrefsTest, volume);
			Volume.onValueChanged.AddListener(SetVolume);
			pauseMenu.SetActive(false);
			SensSlider.value = PlayerPrefs.GetFloat("sensitivity", 2f);
		}
		private void SetVolume(float volume)
		{
			AudioListener.volume = volume;
			PlayerPrefs.SetFloat(PlayerPrefsTest, volume);
		}
		private void Update()
		{
			// Escape toggles the pause menu (not while a sub-menu is open).
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (IsPaused && !KeyBindMenu.activeSelf && !EscMenu.activeSelf && !ResMenu.activeSelf) ResumeGame();
				else if (!IsPaused && !EscMenu.activeSelf && !ResMenu.activeSelf) PauseGame();
			}
			foreach (var source in AudiosAffected)
			{
				if (IsPaused) source.Pause();
				else source.UnPause();
			}
			// V shows the run information while paused.
			if (Input.GetKeyDown(KeyCode.V))
			{
				if (IsPaused && !KeyBindMenu.activeSelf && !EscMenu.activeSelf && !ResMenu.activeSelf && !MenuData.activeSelf) MenuData.SetActive(true);
				else if (IsPaused && !KeyBindMenu.activeSelf && !EscMenu.activeSelf && !ResMenu.activeSelf && MenuData.activeSelf) MenuData.SetActive(false);
			}
			if (!MenuData.activeSelf) return;
			string[] difficulties = { "Difficulty : Easy", "Difficulty : Normal", "Difficulty : Hard", "Difficulty : Extreme", "Difficulty : Impossible", "Difficulty : Slientlo Never Clear" };
			int diff = PlayerPrefs.GetInt("Diff");
			if (diff >= 0 && diff < difficulties.Length) Data_Diff.text = difficulties[diff];
			Data_Granny.text = PlayerPrefs.GetInt("Granny") == 1 ? (PlayerPrefs.GetInt("GrannyWP") == 1 ? "Granny : On (Slingshot)" : "Granny : On (Bat)") : "Granny : Off";
			Data_Grandpa.text = PlayerPrefs.GetInt("Grandpa") == 1 ? "Grandpa : On" : "Grandpa : Off";
			Data_S.text = PlayerPrefs.GetInt("SlendrinaEnemy") != 0 ? "Slendrina : On" : "Slendrina : Off";
			Data_Extras.text = PlayerPrefs.GetInt("Extras") != 0 ? "Extras : On" : "Extras : Off";
			if (PlayerPrefs.GetInt("FlashL") == 1)
			{
				// Seed system: show the seed of the active layout.
				int version = PlayerPrefs.GetInt("CURVERSION");
				GameObject layout;
				if (PlayerPrefs.GetInt("Extras") == 1) layout = SeedSysExtras;
				else if (version == 2) layout = SeedSysMain;
				else if (version == 1) layout = SeedSys11;
				else if (version == 0) layout = SeedSys10;
				else return;
				var seed = layout ? layout.GetComponent<SeedManager>() : null;
				if (seed != null) Data_Preset.text = "Seed : " + seed.Seed.ToString();
			}
			else if (OM != null)
			{
				string[] presets = { "Preset : One", "Preset : Two", "Preset : Three", "Preset : Four", "Preset : Five", "Preset : Six" };
				for (int i = 0; i < presets.Length; i++)
					if (OM.PresetChoosenCurrent == i) { Data_Preset.text = presets[i]; return; }
			}
		}
		public void PauseGame()
		{
			pauseMenu.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0;
			IsPaused = true;
			ClickSound.volume = 1f;
		}
		public void ResumeGame()
		{
			pauseMenu.SetActive(false);
			Cursor.visible = false;
			Time.timeScale = 1f;
			IsPaused = false;
		}
		public void GoToMainMenu()
		{
			Time.timeScale = 1f;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			IsPaused = false;
			SceneManager.LoadScene("Menu");
		}
		public void RestartP()
		{
			Time.timeScale = 1f;
			Cursor.visible = false;
			IsPaused = false;
			SceneManager.LoadScene("Scene");
		}
		public void SensitivitySlider()
		{
			PlayerPrefs.SetFloat("sensitivity", SensSlider.value);
			PlayerPrefs.Save();
		}
}
