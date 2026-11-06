using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/BellShop.c (Granny 3 Evolved, IL2CPP x64).
public partial class BellShop
{
		// The shop door rings the bell (a noise Granny hears) unless it is already swinging.
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != "ShopDoor" || animBell.isPlaying) return;
			audio.PlayOneShot(bellNoise);
			Instantiate(noisyObj, other.transform.position, Quaternion.identity);
			animBell.Play("klocka");
		}
}
