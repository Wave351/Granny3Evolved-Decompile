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

// Ported from Annotated-Pseudocode/R_Grandpa.c (Granny 3 Evolved, IL2CPP x64).
public partial class R_Grandpa
{
		public R_Grandpa() { TimerFade = .2f; }
		// Grandpa's ragdoll: darkened (or lava skin), then after TimerGone it fades out and is removed.
		private void Start()
		{
			EC = GameObject.Find("EnemyController");
			var controller = EC.GetComponent<EnemyController>();
			if (controller == null || controller.ShotgunGrandpa == null) return;
			if (controller.ShotgunGrandpa.activeSelf) Shotgun.SetActive(true);
			StartCoroutine(TimerGo());
			Skin.material.color = GrandpaDarken;
			if (isLava) Skin.material.mainTexture = lavaGrandpa;
		}
		private void Update()
		{
			if (Fadin)
			{
				Skin.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
				Skin.material.color = SkinC;
				SkinC.a -= Time.deltaTime * TimerFade;
			}
			SkinC.a = Mathf.Clamp01(SkinC.a);
		}
		private IEnumerator TimerGo()
		{
			yield return new WaitForSeconds(TimerGone);
			Fadin = true;
			Skin.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
			Skin.material.mainTexture = null;
			foreach (var o in ObjectsDisable)
			{
				if (o == null) break;
				o.SetActive(false);
				if (Shotgun != null) Destroy(Shotgun);
			}
			yield return new WaitForSeconds(3f);
			Destroy(gameObject);
		}
}
