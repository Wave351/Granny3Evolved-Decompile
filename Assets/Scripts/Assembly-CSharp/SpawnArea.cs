using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnArea
{
	public string areaName;

	public Transform[] freeSpots;

	public int maxItems;

	[NonSerialized]
	public int currentItems;

	[NonSerialized]
	public List<Transform> usedSpots;

	public List<string> allowedItemNames;

	public string mandatoryItemName;
}
