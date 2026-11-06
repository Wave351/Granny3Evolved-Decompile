using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class StoneSpawnLogic : MonoBehaviour
{
	public GameObject stoneObj;
	public float spawnTimer;
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;
	public List<GameObject> spawnedRocks;
}
