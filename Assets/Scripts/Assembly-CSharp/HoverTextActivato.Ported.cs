using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Ported from Annotated-Pseudocode/HoverTextActivato.c (Granny 3 Evolved, IL2CPP x64).
public partial class HoverTextActivato
{
		private void Update()
		{
			if (!SecondP.activeSelf) T.SetActive(false);
		}
		public void OnPointerEnter(PointerEventData eventData) { T.SetActive(true); }
		public void OnPointerExit(PointerEventData eventData) { T.SetActive(false); }
}
