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

// Ported from Annotated-Pseudocode/CocoCut.c (Granny 3 Evolved, IL2CPP x64).
public partial class CocoCut
{
		// The guillotine cuts the coconut, or mom's body (dropping the submarine key); every drop is a noise.
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != "GuillotineBlade") return;
			if (Coco.activeSelf)
			{
				Instantiate(Noice, transform.position, transform.rotation);
				Coco.SetActive(false);
				DeadCoco.SetActive(true);
				GetComponent<AudioSource>().Play();
			}
			else if (mom.activeSelf && !momSliced)
			{
				momSliced = true;
				Instantiate(Noice, transform.position, transform.rotation);
				bloodEffect.SetActive(true);
				neckPiece.SetActive(false);
				bloodNeck.SetActive(true);
				headRigid.isKinematic = false;
				headBlood.SetActive(true);
				GetComponent<AudioSource>().Play();
				subKey.transform.position = itemPoint.position;
				subKey.SetActive(true);
				StartCoroutine(MomDisappearTimer());
			}
			else Instantiate(Noice, transform.position, transform.rotation);
			CutArea.SetActive(false);
		}
		// After 8 seconds the body pieces and blood fade out over a second, then the body is removed.
		private IEnumerator MomDisappearTimer()
		{
			yield return new WaitForSeconds(8f);
			var renderersToFade = new List<Renderer>();
			foreach (var piece in momPieces)
			{
				piece.AddComponent<SpikeMelt>();
				renderersToFade.Add(piece.GetComponent<Renderer>());
			}
			foreach (var blood in new[] { bloodNeck, headBlood })
			{
				blood.AddComponent<SpikeMelt>();
				renderersToFade.Add(blood.GetComponent<Renderer>());
			}
			foreach (var r in renderersToFade)
			{
				r.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
				r.material.mainTexture = null;
			}
			float duration = 1f, elapsedTime = 0f;
			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / duration);
				foreach (var r in renderersToFade)
				{
					if (r == null) continue;
					Color c = r.material.color;
					r.material.color = new Color(c.r, c.g, c.b, alpha);
				}
				yield return null;
			}
			foreach (var piece in momPieces) piece.transform.parent.gameObject.SetActive(false);
			mom.SetActive(false);
		}
}
