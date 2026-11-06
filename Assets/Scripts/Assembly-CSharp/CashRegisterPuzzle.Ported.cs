using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/CashRegisterPuzzle.c (Granny 3 Evolved, IL2CPP x64).
public partial class CashRegisterPuzzle
{
		public CashRegisterPuzzle() { correctOrder = new List<int>(); }
		// A random button order is written on the note in random colours; the note pad goes to a random spot.
		private void Start()
		{
			correctOrder.Clear();
			currentIndex = 0;
			foreach (int value in buttonValues) correctOrder.Add(value);
			for (int i = 0; i < correctOrder.Count; i++)
			{
				int j = UnityEngine.Random.Range(i, correctOrder.Count);
				int tmp = correctOrder[i];
				correctOrder[i] = correctOrder[j];
				correctOrder[j] = tmp;
			}
			var colors = new List<string>(new[] { "#FF69B4", "#0000FF", "#000000", "#FFFF00", "#FFFFFF", "#800080", "#8B4513", "#00FF00", "#FF0000" });
			codeText1.text = "";
			codeText2.text = "";
			for (int i = 0; i < correctOrder.Count; i++)
			{
				int c = UnityEngine.Random.Range(0, colors.Count);
				string digit = "<color=" + colors[c] + ">" + correctOrder[i].ToString() + "</color>";
				codeText1.text += digit;
				codeText2.text += digit;
				colors.RemoveAt(c);
			}
			var spot = locationsNumber[UnityEngine.Random.Range(0, locationsNumber.Length)];
			numberPad.position = spot.position;
			numberPad.rotation = spot.rotation;
		}
		// Pressing the buttons in order opens the register.
		public void PressButton(int value)
		{
			if (registerUnlocked || inputLocked) return;
			if (value != correctOrder[currentIndex]) return;
			currentIndex++;
			if (currentIndex < correctOrder.Count) return;
			registerUnlocked = true;
			inputLocked = true;
			foreach (var button in buttonsCollision)
			{
				button.enabled = false;
				button.GetComponent<Interactive>().enabled = false;
			}
			var anim = GetComponent<Animation>();
			if (anim == null) return;
			anim.Play("CashRegisterOpen");
			var audio = GetComponent<AudioSource>();
			if (audio == null) return;
			audio.Play();
			paperUI.SetActive(false);
			paperObj.SetActive(false);
		}
		public void ClearButton() { currentIndex = 0; }
}
