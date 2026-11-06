using System.Collections.Generic;
using UnityEngine;

public partial class Inventory : MonoBehaviour
{
	[Header("Components")]
	public List<ItemDefs> ItemDefs;
	public AudioSource Audio;
	public float mainDropSpeed;
}
