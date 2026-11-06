using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/TriggerFall.c (Granny 3 Evolved, IL2CPP x64).
public partial class TriggerFall
{
		private void Start() { Rigidbody = GetComponent<Rigidbody>(); }
		// Falls when the player, an item or a fired taser touches it (or, for bumpers, when it touches the floor).
		private void OnTriggerStay(Collider other)
		{
			GameObject obj = other.gameObject;
			if ((obj.tag == "Player" || obj.layer == LayerMask.NameToLayer("Item") || obj.name == "ShootTaser(Clone)") && !Fell)
			{
				Fall();
				if (hasNecessaryComponents) foreach (var o in activatedObjects) o.SetActive(true);
			}
			if (other.gameObject.tag == "golv" && IsBumper && !Fell) Fall();
		}
		private void Fall()
		{
			if (TriggerSound) GetComponent<AudioSource>().PlayOneShot(Falls);
			Fell = true;
			Rigidbody.isKinematic = false;
			Debug.Log("Falling");
		}
}
