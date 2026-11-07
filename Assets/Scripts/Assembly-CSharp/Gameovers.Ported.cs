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

// Ported from Annotated-Pseudocode/Gameovers.c (Granny 3 Evolved, IL2CPP x64).
public partial class Gameovers
{
		public Gameovers() { Speed = 2f; Speed2 = 1.5f; }
		private void Start() { }
		// Black overlay fades out while Fadin, otherwise fades in.
		private void Update()
		{
			if (!AbleBlack) return;
			Black.color = BlackColor;
			float a = BlackColor.a + (Fadin ? -Time.deltaTime * Speed : Time.deltaTime * Speed2);
			BlackColor.a = Mathf.Clamp01(a);
		}
		public void GameOverCall()
		{
			foreach (var name in new[] { "Shotgun(Clone)", "GrannyRagdoll(Clone)", "GrandpaRagdoll(Clone)", "GrandpaFreezeDoll(Clone)",
				"GrannyFreezeDoll(Clone)", "GrandpaCrystalBody(Clone)", "GrannyCrystalBody(Clone)" })
			{
				var leftover = GameObject.Find(name);
				if (leftover != null) Destroy(leftover);
			}
			GrannyThere.SetActive(false);
			Granpa.SetActive(false);
			OldB.SetActive(false);
			Black.gameObject.SetActive(true);
			BG.transform.parent = GameOverBox.transform;
			AbleBlack = true;
			StartCoroutine(GameOverStart());
			RenderSettings.fog = false;
			MainUIElem.SetActive(false);
		}
		public void EndProcess() { StartCoroutine(EndOver()); }
		private IEnumerator EndOver()
		{
			yield return new WaitForSeconds(2f);
			TextEnd.SetActive(true);
			yield return new WaitForSeconds(4f);
			var image = TextEnd.GetComponent<Image>();
			if (image == null) yield break;
			image.CrossFadeAlpha(0, 1.5f, false);
			yield return new WaitForSeconds(3.5f);
			SceneManager.LoadScene("Menu");
		}
		// The game-over scenes rotate: car, roof, train.
		private IEnumerator GameOverStart()
		{
			yield return new WaitForSeconds(0);
			int which = PlayerPrefs.GetInt("GameOverWhich");
			GrannyStuff.SetActive(true);
			GrandpaStuff.SetActive(true);
			GameObject scene; Transform grannyEnd, grandpaEnd; GameObject music; string grandpaAnim;
			switch (which)
			{
				case 0: scene = Scene1; grannyEnd = PosGrannyEnd_Car; grandpaEnd = PosGrandpaEnd_Car; music = carGameOverMusic; grandpaAnim = "LookInCar"; break;
				case 1: scene = Scene2; grannyEnd = PosGrannyEnd_Roof; grandpaEnd = PosGrandpaEnd_Roof; music = roofGameOverMusic; grandpaAnim = "Grandpa_Idle"; break;
				case 2: scene = Scene3; grannyEnd = PosGrannyEnd_Train; grandpaEnd = PosGrandpaEnd_Train; music = trainGameOverMusic; grandpaAnim = "Grandpa_Idle"; break;
				default: scene = null; grannyEnd = grandpaEnd = null; music = null; grandpaAnim = null; break;
			}
			if (scene != null)
			{
				if (which == 0) mobileFPS.SetActive(false);
				scene.SetActive(true);
				GrannyThere.SetActive(true);
				var granny = GrannyThere.GetComponent<AI_Granny>();
				granny.EnemyVision.enabled = false;
				granny.dizzy = false;
				granny.dizzyEffect.SetActive(false);
				granny.Agent.enabled = false;
				granny.enabled = false;
				granny.Anim.Play(granny.Idle.name);
				if (which == 0) granny.Anim.Play("dropDynamite");
				GrannyThere.transform.position = grannyEnd.position;
				GrannyThere.transform.rotation = grannyEnd.rotation;
				var bg = BG.GetComponent<BGManager>();
				if (which == 2) { bg.InHouse = true; bg.InOutside = false; }
				else { bg.InHouse = false; bg.InOutside = true; bg.MaxOutside = .65f; }
				music.SetActive(true);
				PlayerPrefs.SetInt("GameOverWhich", (which + 1) % 3);
				Granpa.SetActive(true);
				var grandpa = Granpa.GetComponent<AI_Grandpa>();
				grandpa.EnemyVision.enabled = false;
				grandpa.dizzy = false;
				grandpa.dizzyEffect.SetActive(false);
				grandpa.Agent.enabled = false;
				grandpa.Anim.Play(grandpaAnim);
				grandpa.enabled = false;
				grandpa.Shotgun.SetActive(true);
				Granpa.transform.position = grandpaEnd.position;
				Granpa.transform.rotation = grandpaEnd.rotation;
				if (which == 2)
				{
					// They ride along on the train.
					Granpa.transform.parent = train;
					GrannyThere.transform.parent = train;
				}
				moonlight.SetActive(which != 2);
			}
			var leftover = GameObject.Find("P_R_D(Clone)");
			if (leftover != null) Destroy(leftover);
		}
}
