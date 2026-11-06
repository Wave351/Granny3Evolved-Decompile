using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/BearTrapLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class BearTrapLogic
{
		public BearTrapLogic() { RayDistance = 10f; }
		private void Start()
		{
			StartCoroutine(TimerDes());
			Player = GameObject.Find("Player");
			Physics.IgnoreCollision(collMain, Player.GetComponent<Collider>(), true);
		}
		// The trap only snaps on the player when it can actually see them (not through walls).
		private void Update()
		{
			ScEFF = GameObject.Find("ScreenEffects");
			RayB = GameObject.Find("RemoveBearRay");
			if (Player != null) RayCaster.LookAt(Player.transform);
			Vector3 forward = RayCaster.TransformDirection(Vector3.forward);
			int mask = ~LayerMask.GetMask("EnemyPass");
			RaycastHit hit;
			PlayerHit = Physics.Raycast(RayCaster.position, forward, out hit, RayDistance, mask) && hit.collider.gameObject.tag == "Player";
		}
		// An unused trap disappears after 90 seconds.
		private IEnumerator TimerDes()
		{
			yield return new WaitForSeconds(90f);
			if (!TrappedPlayer) Destroy(gameObject);
		}
		private IEnumerator TimerDesItem()
		{
			yield return new WaitForSeconds(10f);
			Destroy(gameObject);
		}
		private void OnTriggerEnter(Collider other)
		{
			string tag = other.gameObject.tag;
			// Armed once it lands.
			if ((tag == "floor" || tag == "grus" || tag == "Noice") && !Placed)
			{
				Placed = true;
				gameObject.tag = "BearTrapActivated";
				if (PlayerPrefs.GetInt("Textures") == 1) Anim.CrossFade("BeartrapLive");
				GetComponent<AudioSource>().PlayOneShot(PlacedTrap);
			}
			// Catching the player: a noise, blood on screen and no moving until it is removed.
			if (tag == "Player" && Placed && !TrappedPlayer && PlayerHit)
			{
				TrappedPlayer = true;
				Instantiate(Noise, transform.position, transform.rotation);
				var effects = ScEFF.GetComponent<ScreenEffects>();
				effects.Blood = 1f;
				effects.Hurt = true;
				effects.Player.GetComponent<MobileFPS>().AbleToMove = false;
				RayB.GetComponent<RayRemoveTraps>().Trapped = true;
				GetComponent<AudioSource>().PlayOneShot(AggroSound);
				Anim.Play(Clip.name);
				GetComponent<Rigidbody>().isKinematic = true;
			}
			// An item or a taser shot sets it off.
			if (other.gameObject.layer != LayerMask.NameToLayer("Item") && other.gameObject.name != "ShootTaser(Clone)") return;
			Spring();
		}
		public void ShootGun() { Spring(); }
		private void Spring()
		{
			if (TrappedPlayer) return;
			TrappedPlayer = true;
			ItemCaughtIt = true;
			Instantiate(Noise, transform.position, transform.rotation);
			var audio = GetComponent<AudioSource>();
			if (audio == null) return;
			audio.PlayOneShot(AggroSound);
			Anim.Play(Clip.name);
			StartCoroutine(TimerDesItem());
		}
}
