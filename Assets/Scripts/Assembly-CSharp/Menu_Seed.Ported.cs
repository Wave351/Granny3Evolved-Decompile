using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Menu_Seed.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class Menu_Seed
{
	public Menu_Seed() { SeedPrefKey = "GameSeed"; }
	private void Start()
	{
		if (string.IsNullOrEmpty(SeedPrefKey)) SeedPrefKey = "GameSeed";
		if (SeedInput != null)
		{
			SeedInput.contentType = TMP_InputField.ContentType.IntegerNumber;
			SeedInput.characterLimit = 9;
			SeedInput.text = PlayerPrefs.GetInt(SeedPrefKey, 0).ToString();
			SeedInput.interactable = false;
		}
		if (RandomToggle != null) RandomToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("RandomSeed", 0) == 1);
	}
	private void Update()
	{
		int random = RandomToggle != null && RandomToggle.isOn ? 1 : 0;
		if (PlayerPrefs.GetInt("RandomSeed", 0) != random) { PlayerPrefs.SetInt("RandomSeed", random); PlayerPrefs.Save(); }
		if (IsTypingSeed && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))) ExitTypingModeAndSave();
		if (SavingOrNo != null) SavingOrNo.SetActive(IsTypingSeed);
	}
	public void BeginTypingSeed()
	{
		if (RandomToggle != null && RandomToggle.isOn) return;
		IsTypingSeed = true; Cursor.visible = false;
		if (SeedInput != null) { SeedInput.interactable = true; SeedInput.ActivateInputField(); SeedInput.Select(); }
	}
	public void ExitTypingModeAndSave()
	{
		IsTypingSeed = false; Cursor.visible = true; Cursor.lockState = CursorLockMode.Confined;
		int seed = 0;
		if (SeedInput != null) { int.TryParse((SeedInput.text ?? "").Trim(), out seed); SeedInput.interactable = false; }
		PlayerPrefs.SetInt(SeedPrefKey, seed); PlayerPrefs.Save();
	}
}
