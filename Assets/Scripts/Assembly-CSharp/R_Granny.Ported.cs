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

// Ported from Annotated-Pseudocode/R_Granny.c (Granny 3 Evolved, IL2CPP x64).
public partial class R_Granny
{
		public R_Granny() { TimerFade = .2f; }
		// Granny's ragdoll: darkened, holding her slingshot if she had it; then it fades out and is removed.
		private void Start()
		{
			StartCoroutine(TimerGo());
			Skin.material.color = GrannyDarken;
			EC = GameObject.Find("EnemyController");
			if (PlayerPrefs.GetInt("GrannyWP") == 1) bat.SetActive(false);
			var controller = EC.GetComponent<EnemyController>();
			if (controller == null || controller.Granny == null) return;
			var ai = controller.Granny.GetComponent<AI_Granny>();
			if (ai == null || ai.slingshotWP == null) return;
			if (ai.slingshotWP.activeSelf) slingWP.SetActive(true);
		}
		private void Update()
		{
			if (Fadin)
			{
				Skin.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
				Skin.material.color = SkinC;
				F1.material.color = FC;
				F2.material.color = FC;
				SkinC.a -= Time.deltaTime * TimerFade;
				FC.a -= Time.deltaTime * TimerFade;
			}
			SkinC.a = Mathf.Clamp01(SkinC.a);
			FC.a = Mathf.Clamp01(FC.a);
			if (GrannyStuff == null) GrannyStuff = GameObject.Find("Granny-Stuff");
			if (PS == null) PS = GameObject.Find("PlayerManage");
			if (isLava) Skin.material.mainTexture = lavaGranny;
		}
		private IEnumerator TimerGo()
		{
			yield return new WaitForSeconds(TimerGone);
			Fadin = true;
			var shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
			Skin.material.shader = shader;
			F1.material.shader = shader;
			F2.material.shader = shader;
			Skin.material.mainTexture = null;
			F1.material.mainTexture = null;
			F2.material.mainTexture = null;
			foreach (var o in ObjectsDisable) o.SetActive(false);
			if (slingWP != null) slingWP.SetActive(false);
			yield return new WaitForSeconds(3f);
			Destroy(gameObject);
		}
}
