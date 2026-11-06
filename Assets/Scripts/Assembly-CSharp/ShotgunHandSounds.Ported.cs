using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ShotgunHandSounds.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class ShotgunHandSounds
{
	public ShotgunHandSounds() { ForceShoot = 20; }
	public void ReloadSound() { var a = GetComponent<AudioSource>(); if (a) a.PlayOneShot(Reload); }
	public void LoadSound() { var a = GetComponent<AudioSource>(); if (a) a.PlayOneShot(Load); }
	public void ShootSound() { AudioSource.PlayClipAtPoint(Shoot, transform.position); CasingShell(); }
	public void CasingShell()
	{
	    var shell = Instantiate(Casing, CasingShot.transform.position, CasingShot.transform.rotation);
	    var body = shell.GetComponent<Rigidbody>();
	    if (body) body.AddForce(shell.transform.forward * ForceShoot, ForceMode.Impulse);
	}
}
