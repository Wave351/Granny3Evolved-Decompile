using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SoundInitiate.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class SoundInitiate
{
	public void SoundPlay1()
	{
		Audio.pitch = 1f;
		Audio.PlayOneShot(Clip1);
	}

	public void SoundPlay2()
	{
		Audio.pitch = 1f;
		Audio.PlayOneShot(Clip2);
	}

	public void SoundPlay3()
	{
		Audio.pitch = 1.4f;
		Audio.PlayOneShot(Clip3);
		ItemSpin.Play("SpinItem");
		P.Play();
		ItemText.SetActive(true);
	}
}
