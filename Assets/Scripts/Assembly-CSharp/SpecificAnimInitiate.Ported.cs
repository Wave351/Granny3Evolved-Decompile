using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SpecificAnimInitiate.c (Granny 3 Evolved, IL2CPP x64).
public partial class SpecificAnimInitiate
{
		private void Start()
		{
			if (Anim == null) Anim = GetComponent<Animation>();
		}
		public void PlayAnimation(string name) { Anim.Play(name); }
}
