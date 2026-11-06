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

// Ported from Annotated-Pseudocode/CribTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class CribTrigger
{
		public CribTrigger() { SpeedFadeSlendrina = .5f; SpeedFadeSlendrinaH = .2f; maxHair = .388235f; slendrinaActive = true; }
		private void Start()
		{
			PlayerPrefs.SetInt("Teddied", 0);
			TeddyM.shader = Shader.Find("Legacy Shaders/Self-Illumin/Bumped Diffuse");
			TeddyM.color = new Color(.481132f, .481132f, .481132f, 1f);
			StartTeddy = TeddyM.color;
			SlendrinaBodyM.color = StartSlendrinaBody;
			SlendrinaHairM.color = StartSlendrinaHair;
		}
		// Slendrina's body and hair fade in while she appears (SlendrinaFades) and out again; the teddy fades away.
		private void Update()
		{
			if (!slendrinaActive)
			{
				TeddyM.color = StartTeddy;
				SlendrinaBodyM.color = StartSlendrinaBody;
				SlendrinaHairM.color = StartSlendrinaHair;
				if (!SlendrinaFades)
				{
					StartSlendrinaBody.a -= Time.deltaTime * SpeedFadeSlendrina;
					StartSlendrinaHair.a -= Time.deltaTime * SpeedFadeSlendrinaH;
				}
				else
				{
					StartSlendrinaBody.a += Time.deltaTime * SpeedFadeSlendrina;
					if (StartSlendrinaHair.a < maxHair) StartSlendrinaHair.a += Time.deltaTime * SpeedFadeSlendrinaH;
				}
				StartTeddy.a = Mathf.Clamp01(StartTeddy.a);
				StartSlendrinaBody.a = Mathf.Clamp01(StartSlendrinaBody.a);
				StartSlendrinaHair.a = Mathf.Clamp01(StartSlendrinaHair.a);
			}
			if (TeddyFade) StartTeddy.a -= Time.deltaTime * .5f;
		}
		// Putting the teddy in the crib summons Slendrina's ghost.
		private void OnTriggerEnter(Collider other)
		{
			var data = other.gameObject.GetComponent<ItemSeedData>();
			if (!data || data.itemName != "Teddy") return;
			Destroy(other.gameObject.GetComponent<ItemSeedData>());
			GlowEffect.SetActive(true);
			TeddyM.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
			TeddyFade = true;
			StartCoroutine(SlendrinaTimer());
			TeddyObj = other.gameObject;
			if (TeddyObj.GetComponent<Outline>() != null) Destroy(TeddyObj.GetComponent<Outline>());
			var ai = Slendrina.GetComponent<AI_Slendrina>();
			if (ai == null) return;
			ai.canAttack = false;
			slendrinaActive = false;
			Slendrina.SetActive(true);
		}
		private IEnumerator SlendrinaTimer()
		{
			yield return new WaitForSeconds(2f);
			Destroy(TeddyObj);
			SlendrinaFades = true;
			Smoke.gameObject.SetActive(true);
			var ai = Slendrina.GetComponent<AI_Slendrina>();
			if (ai == null || ai.Agent == null) yield break;
			ai.Agent.enabled = false;
			Slendrina.transform.position = slendrinaPos.position;
			Smoke.Play();
			yield return new WaitForSeconds(10f);
			Smoke.Stop();
			SlendrinaFades = false;
			yield return new WaitForSeconds(3f);
			var collider = Slendrina.GetComponent<Collider>();
			if (collider == null) yield break;
			collider.enabled = false;
			ItemDropPoint.SetActive(true);
			if (PlayerPrefs.GetInt("SlendrinaEnemy") == 1) rayPick.TimedTextInitiate("Slendrina has now vanished");
			yield return new WaitForSeconds(3f);
			Slendrina.SetActive(false);
			PlayerPrefs.SetInt("Teddied", 1);
		}
		private IEnumerator SlendrinaBegoneEnemy()
		{
			yield return new WaitForSeconds(7f);
			yield return new WaitForSeconds(1.5f);
			SlendrinaFades = false;
			yield return new WaitForSeconds(1f);
			var collider = Slendrina ? Slendrina.GetComponent<Collider>() : null;
			if (collider != null) collider.enabled = false;
		}
		public void SetMaterialToFadeMode(Material m)
		{
			m.SetFloat("_Mode", 2f);
			m.SetInt("_SrcBlend", 5);
			m.SetInt("_DstBlend", 10);
			m.SetInt("_ZWrite", 0);
			m.DisableKeyword("_ALPHATEST_ON");
			m.EnableKeyword("_ALPHABLEND_ON");
			m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			m.renderQueue = 3000;
		}
}
