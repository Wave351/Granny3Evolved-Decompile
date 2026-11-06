using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/HidingCamera.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class HidingCamera
{
	public HidingCamera() { cameraRotationSpeed = 2; XReser = 180; YReser = 180; ZReser = -180; }
	private void Start() { ResetRotation(); }
	private void OnEnable() { ResetRotation(); }
	// Hiding-spot camera: free look around Y, or turn to the enemy that caught the player.
	private void Update()
	{
	    if (Paused.IsPaused) return;
	    cameraRotationSpeed = PlayerPrefs.GetFloat("sensitivity", 2f);
	    if (!Status.IsJumpscared) { GetTouchInput(); return; }
	    GameObject enemy = Status.LookAtGranny ? Granny : Status.LookAtGrandpa ? Grandpa : null;
	    if (enemy == null) return;
	    Vector3 dir = Vector3.RotateTowards(playerCamera.forward, enemy.transform.position - playerCamera.position, Time.deltaTime * 15, 0);
	    playerCamera.rotation = Quaternion.LookRotation(dir);
	}
	private void GetTouchInput()
	{
	    rotationY += Input.GetAxis("Mouse X") * cameraRotationSpeed;
	    playerCamera.localRotation = Quaternion.Euler(0, rotationY, 0);
	}
	private void ResetRotation()
	{
	    rotationY = YReser;
	    playerCamera.localRotation = Quaternion.Euler(XReser, YReser, ZReser);
	}
}
