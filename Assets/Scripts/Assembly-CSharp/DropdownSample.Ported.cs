using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/DropdownSample.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class DropdownSample
{
	// TextMesh Pro example: shows both dropdowns' selected values, or an error if the placeholder one is unset.
	public void OnButtonClick()
	{
	    text.text = dropdownWithPlaceholder.value < 0
	        ? "Error: Please make a selection"
	        : "Selected values:\n" + dropdownWithoutPlaceholder.value + " - " + dropdownWithPlaceholder.value;
	}
}
