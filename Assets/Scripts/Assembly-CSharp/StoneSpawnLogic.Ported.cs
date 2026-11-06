using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/StoneSpawnLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class StoneSpawnLogic
{
		public StoneSpawnLogic() { spawnTimer = 90f; minX = -50f; maxX = 50f; minZ = -50f; maxZ = 50f; spawnedRocks = new List<GameObject>(); }
		private void Start()
		{
			SpawnStones();
			StartCoroutine(RespawnRoutine());
		}
		// Every spawnTimer seconds the old stones are cleared and new ones may appear.
		private IEnumerator RespawnRoutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(spawnTimer);
				CleanupList();
				WipeOutStones();
				SpawnStones();
			}
		}
		public void CleanupList()
		{
			for (int i = spawnedRocks.Count - 1; i >= 0; i--)
				if (spawnedRocks[i] == null) spawnedRocks.RemoveAt(i);
		}
		public void WipeOutStones()
		{
			for (int i = spawnedRocks.Count - 1; i >= 0; i--)
				if (spawnedRocks[i] != null) Destroy(spawnedRocks[i]);
			spawnedRocks.Clear();
		}
		// 85% chance of a stone somewhere in the area, then a 4% chance of one more.
		public void SpawnStones()
		{
			if (UnityEngine.Random.Range(0, 100) <= 14) return;
			SpawnOne();
			if (UnityEngine.Random.Range(0, 100) < 4) SpawnOne();
		}
		private void SpawnOne()
		{
			float x = UnityEngine.Random.Range(minX, maxX);
			float z = UnityEngine.Random.Range(minZ, maxZ);
			Vector3 p = transform.position;
			spawnedRocks.Add(Instantiate(stoneObj, new Vector3(p.x + x, p.y, p.z + z), Quaternion.identity));
		}
}
