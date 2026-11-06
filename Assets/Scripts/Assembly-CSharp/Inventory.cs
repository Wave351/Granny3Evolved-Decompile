using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[Header("Components")]
	public List<ItemDefs> ItemDefs;

	public AudioSource Audio;

	public float mainDropSpeed;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void PickupItem(string itemName)
	{
	}

	public ItemDefs GetItemDefByName(string name)
	{
		return null;
	}

	public void DropLogic()
	{
	}

	public ItemDefs GetItem(string itemName)
	{
		return null;
	}
}
