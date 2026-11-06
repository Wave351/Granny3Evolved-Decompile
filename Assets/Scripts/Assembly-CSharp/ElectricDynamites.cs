using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public partial class ElectricDynamites : MonoBehaviour
{
	[Serializable]
	public class Spawns
	{
		public Transform transform;

		public string locationName;
	}
	[Header("Main Logic")]
	public GameObject elecDynamites;
	public GameObject dynamiteBox;
	public float normalSpawnTimer;
	public float restrictedSpawnTimer;
	public int lastSpawnI;
	[Header("Spawns")]
	public Spawns[] spawnsElectricDynamite;
	public GameObject currentTarget;
	[Header("Tracker")]
	public GameObject trackerHand;
	public TMP_Text locationText;
}
