using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/KeyBindManage.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class KeyBindManage
{
	private static readonly string[] BindingKeys = { "PrimaryInt_KEY", "SecondaryInt_KEY", "Dropping_KEY", "Crouch_KEY", "WP_KEY" };
	private static readonly KeyCode[] Defaults = { KeyCode.E, KeyCode.F, KeyCode.Space, KeyCode.C, KeyCode.Mouse0 };
	private static readonly KeyCode[] DetectableKeys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
	private void Start() { LoadBindings(); }
	private void LoadBindings()
	{
		KeyCode[] keys = new KeyCode[5];
		TMP_Text[] labels = Labels();
		for (int i = 0; i < keys.Length; i++)
		{
			if (!Enum.TryParse(PlayerPrefs.GetString(BindingKeys[i], Defaults[i].ToString()), out keys[i])) keys[i] = Defaults[i];
			if (labels[i] != null) labels[i].text = keys[i].ToString();
		}
		PrimaryInteraction = keys[0]; SecondaryInteraction = keys[1]; Dropping = keys[2]; Crouching = keys[3]; Shooting = keys[4];
		if (PickRay != null) { PickRay.MainInteract = keys[0]; PickRay.DropKey = keys[2]; PickRay.ShootKey = keys[4]; }
		if (DoorRay != null) DoorRay.InteractKey = keys[0];
		if (BearRay != null) BearRay.KeyInteract = keys[1];
		if (CrouchHolder != null) CrouchHolder.CrouchKey = keys[3];
	}
	private void Update()
	{
		GameObject[] tabs = { PrimaryB, SecondaryB, DropB, CrouchB, ShootB };
		int current = Mathf.Clamp(Mathf.RoundToInt(CurrentKeyBindTab), 0, 4);
		for (int i = 0; i < tabs.Length; i++) if (tabs[i] != null && tabs[i].activeSelf != (i == current)) tabs[i].SetActive(i == current);
		if (WaitingForKey) DetectKeyPress();
	}
	public void StartRebind(int i)
	{
		if (WaitingForKey) return;
		WaitingForKey = true; NewKey = KeyCode.None;
		SetBackButtons(false);
		var label = Labels()[Mathf.Clamp(Mathf.RoundToInt(CurrentKeyBindTab), 0, 4)];
		if (label != null) label.text = "Press a key...";
	}
	public void DetectKeyPress()
	{
		foreach (var key in DetectableKeys)
		{
			if (!Input.GetKeyDown(key)) continue;
			if (key == KeyCode.Return || key == KeyCode.KeypadEnter) { ConfirmNewKey(); return; }
			NewKey = key;
			if (PressEnterText != null) PressEnterText.SetActive(true);
			var label = Labels()[Mathf.Clamp(Mathf.RoundToInt(CurrentKeyBindTab), 0, 4)];
			if (label != null) label.text = key.ToString();
		}
	}
	public void ConfirmNewKey()
	{
		WaitingForKey = false;
		int current = Mathf.Clamp(Mathf.RoundToInt(CurrentKeyBindTab), 0, 4);
		PlayerPrefs.SetString(BindingKeys[current], NewKey.ToString()); PlayerPrefs.Save();
		LoadBindings(); SetBackButtons(true);
		if (PressEnterText != null) PressEnterText.SetActive(false);
	}
	public void BackOrNextTab(int i)
	{
		CurrentKeyBindTab += i == 0 ? -1f : 1f;
		if (CurrentKeyBindTab > 4f) CurrentKeyBindTab = 0f;
		else if (CurrentKeyBindTab < 0f) CurrentKeyBindTab = 4f;
	}
	private TMP_Text[] Labels() { return new[] { PrimaryText, SecondaryText, DropText, CrouchText, ShootText }; }
	private void SetBackButtons(bool enabled) { if (BackButton != null) foreach (var button in BackButton) if (button != null) button.interactable = enabled; }
}
