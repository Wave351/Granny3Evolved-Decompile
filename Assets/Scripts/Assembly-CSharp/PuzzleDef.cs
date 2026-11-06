using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PuzzleDef
{
	public string puzzleName;

	public Transform spawnPoint;

	public List<string> allowedItemNames;

	public List<string> excludeItemNames;

	public int Priority;

	public List<string> requiredItemNames;

	public List<PuzzleForbiddenCombo> forbiddenCombos;

	public List<string> containedItems;
}
