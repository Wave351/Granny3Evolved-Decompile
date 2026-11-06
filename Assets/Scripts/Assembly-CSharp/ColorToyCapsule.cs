using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public partial class ColorToyCapsule : MonoBehaviour
{
	public struct Tier
	{
		public string name;

		public float chance;

		public string[] items;
	}
	public GameObject TextNotEnough;
	public Animation AnimatedToyCapsule;
	public GameObject InsertCoin;
	public GameObject BackB;
	public GameObject InvB;
	public GameObject slingshotFragment;
	public Renderer fragmentRenderer;
	public TMP_Text ItemText;
	public Texture2D Slingshot_Lava;
	public Texture2D Slingshot_Ice;
	public Texture2D Slingshot_Magenta;
	public TMP_Text ColorRarityText;
	public TMP_Text LavaFragmentsText;
	public TMP_Text IceFragmentsText;
	public TMP_Text MagentaFragmentsText;
	public bool GiveCoinBack;
	public GameObject slingshotOBJDisplay;
	public GameObject menuInventory;
	public GameJoltMenu gamejoltMenuCode;
	public Tier[] tiers;
}
