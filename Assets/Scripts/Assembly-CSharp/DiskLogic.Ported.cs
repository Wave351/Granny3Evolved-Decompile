using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DiskLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class DiskLogic
{
		public DiskLogic() { pedestalMoveSpeed = .1f; diskRotateSpeed = .2f; }
		// Two pedestals sink when their filled vase is placed; when both are done the disk turns to 252.6 degrees.
		private void Update()
		{
			grannyPedestal.transform.localPosition = new Vector3(4.21257f, grannyPedestalHeight, 6.30877f);
			grandpaPedestal.transform.localPosition = new Vector3(3.416454f, grandpaPedestalHeight, 6.30877f);
			disk.localEulerAngles = new Vector3(0, 0, diskRotation);
			if (!grannyVaseFinished) MovePedestal(vaseGranny, vaseGrannyFilled, grannyPedestalSound, ref grannyPedestalHeight, ref grannyVaseFinished);
			if (!grandpaVaseFinished) MovePedestal(vaseGrandpa, vaseGrandpaFilled, grandpaPedestalSound, ref grandpaPedestalHeight, ref grandpaVaseFinished);
			if (grandpaVaseFinished && grannyVaseFinished && diskRotation < 252.619f)
			{
				diskRotation += diskRotateSpeed;
				diskMovementSound.SetActive(true);
			}
			else
			{
				if (!(grandpaVaseFinished && grannyVaseFinished) && diskRotation > 77.9f) diskRotation -= diskRotateSpeed;
				diskMovementSound.SetActive(false);
			}
			vasePlaceCollider1.SetActive(!vaseGranny.activeSelf);
			vasePlaceCollider2.SetActive(!vaseGrandpa.activeSelf);
		}
		private void MovePedestal(GameObject vase, bool filled, GameObject sound, ref float height, ref bool finished)
		{
			const float bottom = -1.55f;
			if (!vase.activeSelf || !filled)
			{
				// No filled vase: rise back up to -1.55.
				if (height < bottom) { sound.GetComponent<AudioSource>().volume = 1; height += Time.deltaTime * pedestalMoveSpeed; }
				else sound.GetComponent<AudioSource>().volume = 0;
			}
			else if (bottom < height) { sound.GetComponent<AudioSource>().volume = 1; height -= Time.deltaTime * pedestalMoveSpeed; }
			else
			{
				sound.GetComponent<AudioSource>().volume = 0;
				vase.GetComponent<Collider>().enabled = false;
				finished = true;
			}
		}
}
