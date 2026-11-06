using System.Collections.Generic;
using UnityEngine;

public class ItemSeedData : MonoBehaviour
{
	public string itemName;

	public bool assignedToSpot;

	[Tooltip("1=escape-only, 2=escape+puzzle, 3=puzzle-only, 4=free")]
	[Range(1f, 4f)]
	public int category;

	public List<string> containedItems;
}
