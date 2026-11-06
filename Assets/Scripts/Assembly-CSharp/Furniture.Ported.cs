using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Furniture.c (Granny 3 Evolved, IL2CPP x64).
public partial class Furniture
{
		public void CleanUp()
		{
			foreach (var unit in FurnitureUnits) unit.CleanUpFurniture();
		}
}
