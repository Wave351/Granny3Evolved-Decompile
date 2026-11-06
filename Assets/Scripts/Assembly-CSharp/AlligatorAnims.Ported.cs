using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/AlligatorAnims.c (Granny 3 Evolved, IL2CPP x64).
public partial class AlligatorAnims
{
		public void RandomWaterPatrolAlligator() { audio.PlayOneShot(waterSteps[UnityEngine.Random.Range(0, waterSteps.Length)]); }
		public void SoundAttack() { attackSound.Play(); }
}
