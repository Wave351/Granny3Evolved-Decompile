using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ShootLogicGranny.c (Granny 3 Evolved, IL2CPP x64).
public partial class ShootLogicGranny
{
		// Granny's slingshot: only with the slingshot weapon; it shoots faster on higher difficulties.
		private void Start()
		{
			if (PlayerPrefs.GetInt("GrannyWP") == 0) GetComponent<ShootLogicGranny>().enabled = false;
			float[] speeds = { .9f, 1.1f, 1.3f, 1.8f, 2.1f };
			int diff = PlayerPrefs.GetInt("Diff");
			if (diff >= 0 && diff <= 4) Anim["Granny_Shoot"].speed = speeds[diff];
		}
		private void Update()
		{
			SPStone.LookAt(Player.position);
			if (Anim.IsPlaying("Granny_Shoot")) return;
			Uncharged.SetActive(false);
			Charged.SetActive(true);
		}
		public void LoadSFX() { SFXShoot.PlayOneShot(loadSling); }
		// Animation event: fire a stone at the player (directed) or straight ahead.
		public void ShootStone(int directed)
		{
			SPStone.LookAt(Player.position);
			Transform from = directed == 1 ? SPStone : SPStoneUnDirected;
			var stone = Instantiate(StoneShoot, from.position, from.rotation);
			var body = stone.GetComponent<Rigidbody>();
			if (body == null) return;
			body.AddForce(stone.transform.forward * ForceSend, ForceMode.Impulse);
			SFXShoot.Play();
		}
}
