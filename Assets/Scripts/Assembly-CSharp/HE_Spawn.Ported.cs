using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/HE_Spawn.c (Granny 3 Evolved, IL2CPP x64).
public partial class HE_Spawn
{
		public void EnterF1() { Floor1 = true; Floor2 = false; Floor3 = false; Cellar = false; }
		public void EnterF2() { Floor1 = false; Floor2 = true; Floor3 = false; Cellar = false; }
		public void EnterF3() { Floor1 = false; Floor2 = false; Floor3 = true; Cellar = false; }
		public void EnterCellar() { Floor1 = false; Floor2 = false; Floor3 = false; Cellar = true; }
}
