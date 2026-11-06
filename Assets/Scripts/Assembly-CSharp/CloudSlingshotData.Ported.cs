using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/CloudSlingshotData.c (Granny 3 Evolved, IL2CPP x64).
public partial class CloudSlingshotData
{
		public CloudSlingshotData()
		{
			unlockedSkins = new List<string>();
			fragmentKeys = new List<string>();
			fragmentCounts = new List<int>();
		}
}
