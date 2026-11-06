using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/GuillotineAnim.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class GuillotineAnim
{
	public void Start1()
	{
		UnityEngine.Object.Instantiate(NoiseOBJ, CutArea.transform.position, CutArea.transform.rotation);
		AudioSource.PlayClipAtPoint(SoundGuillotine, Gj.transform.position);
		anim.Play("giljotinOn");
		StartCoroutine(WaitForAnimationEnd1());
	}

	public IEnumerator WaitForAnimationEnd1()
	{
		yield return new WaitForSeconds(anim.GetClip("giljotinOn").length);
		CutArea.SetActive(true);
	}
}
