using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SeedManager.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class SeedManager
{
	public SeedManager() { Seed = 12345; RandomizeSeed = true; activatePlacedItems = true; fillAllFreeSpawns = true; EscapeItemPuzzleChance = 0.7f; }
	private void Awake()
	{
	    LeaveOutChainCutter();
	    BuildLookup();
	}
	// Added: the chain cutter (Slientlo Never Clear's balloon chains) is only placed in runs that have the chains.
	// Elsewhere it's taken out of the item list, the required escape items and its own spawn area, so seeds play as before.
	private void LeaveOutChainCutter()
	{
	    if (!SubValveItem.InThisRun)
	    {
	        if (allItems != null) allItems.RemoveAll(i => i && i.GetComponent<SubValveItem>());
	        if (requiredEscapeItemNames != null) requiredEscapeItemNames.RemoveAll(n => n == SubValveItem.ItemName);
	        if (spawnAreas != null)
	            spawnAreas = spawnAreas.Where(a => a == null || a.allowedItemNames == null || !a.allowedItemNames.Contains(SubValveItem.ItemName)).ToArray();
	    }
	    if (!BlueFuse.InThisRun)
	    {
	        if (allItems != null) allItems.RemoveAll(i => i && i.GetComponent<BlueFuseItem>());
	        if (requiredEscapeItemNames != null) requiredEscapeItemNames.RemoveAll(n => n == BlueFuse.ItemName);
	        if (spawnAreas != null)
	            spawnAreas = spawnAreas.Where(a => a == null || a.allowedItemNames == null || !a.allowedItemNames.Contains(BlueFuse.ItemName)).ToArray();
	    }
	    if (BalloonChains.InThisRun) return;
	    if (allItems != null) allItems.RemoveAll(i => i && i.GetComponent<ChainCutterItem>());
	    if (requiredEscapeItemNames != null) requiredEscapeItemNames.RemoveAll(n => n == ChainCutterItem.ItemName);
	    if (spawnAreas != null)
	        spawnAreas = spawnAreas.Where(a => a == null || a.allowedItemNames == null || !a.allowedItemNames.Contains(ChainCutterItem.ItemName)).ToArray();
	}

	private void Start()
	{
	    GeneratePlacement();
	}

	public void BuildLookup()
	{
	    itemsByName = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
	    if (allItems == null) return;
	    foreach (var item in allItems)
	    {
	        if (!item) continue;
	        var data = item.GetComponent<ItemSeedData>();
	        if (!data || string.IsNullOrEmpty(data.itemName)) continue;
	        if (!itemsByName.ContainsKey(data.itemName))
	            itemsByName[data.itemName] = item;
	    }
	}

	private void SaveOriginalStates()
	{
	    if (originalStates == null)
	        originalStates = new Dictionary<GameObject, OriginalState>();
	    else
	        originalStates.Clear();

	    if (allItems == null) return;
	    foreach (var item in allItems)
	    {
	        if (!item) continue;
	        var rb = item.GetComponent<Rigidbody>();
	        var state = new OriginalState
	        {
	            pos = item.transform.position,
	            rot = item.transform.rotation,
	            active = item.activeSelf,
	            kinematic = !rb || rb.isKinematic
	        };
	        originalStates[item] = state;
	    }
	}

	public void RestoreOriginalStates()
	{
	    if (originalStates == null) return;
	    foreach (var kvp in originalStates)
	    {
	        var item = kvp.Key;
	        var state = kvp.Value;
	        if (!item) continue;
	        item.transform.position = state.pos;
	        item.transform.rotation = state.rot;
	        item.SetActive(state.active);
	        var rb = item.GetComponent<Rigidbody>();
	        if (rb) rb.isKinematic = state.kinematic;
	    }
	}

	public void PlaceItemAtTransform(GameObject item, Transform t, bool isFreeSpawn)
	{
	    if (!item || !t) return;
	    // Evolved no longer activates the item here (Legacy did when activatePlacedItems was set).
	    item.transform.position = t.position;
	    item.transform.rotation = t.rotation;
	    var rb = item.GetComponent<Rigidbody>();
	    if (rb)
	    {
	        rb.linearVelocity = Vector3.zero;
	        rb.angularVelocity = Vector3.zero;
	        rb.isKinematic = !isFreeSpawn || !Rigid;
	    }
	}

	public bool IsItemAllowedForPuzzle(GameObject item, PuzzleDef puzzle)
	{
	    if (!item) return false;
	    var d = item.GetComponent<ItemSeedData>();
	    if (!d || puzzle == null) return false;
	    if (puzzle.allowedItemNames != null && puzzle.allowedItemNames.Count > 0)
	    {
	        return puzzle.allowedItemNames.Any(x => string.Equals(x.Trim(), d.itemName.Trim(), StringComparison.OrdinalIgnoreCase));
	    }
	    if (puzzle.excludeItemNames != null && puzzle.excludeItemNames.Count > 0)
	    {
	        return !puzzle.excludeItemNames.Any(x => string.Equals(x.Trim(), d.itemName.Trim(), StringComparison.OrdinalIgnoreCase));
	    }
	    return true;
	}

	private bool IsSafeContainerPlacement(GameObject containerItem, PuzzleDef candidatePuzzle)
	{
	    if (!containerItem || candidatePuzzle == null) return true;
	    var d = containerItem.GetComponent<ItemSeedData>();
	    if (!d || d.containedItems == null || d.containedItems.Count == 0) return true;
	    if (puzzleDefs == null) return true;

	    foreach (var contained in d.containedItems)
	    {
	        foreach (var p in puzzleDefs)
	        {
	            if (p != null && p.requiredItemNames != null && p.requiredItemNames.Contains(contained))
	            {
	                if (p.spawnPoint == candidatePuzzle.spawnPoint)
	                    return false;
	            }
	        }
	    }
	    return true;
	}

	public void PlaceItemInSpecificFreeArea(GameObject item, SpawnArea area)
	{
	    if (area == null || area.freeSpots == null) return;
	    var available = area.freeSpots.Where(t => t != null && (area.usedSpots == null || !area.usedSpots.Contains(t))).ToList();
	    if (available.Count == 0) return;
	    var spot = available[rnd.Next(available.Count)];
	    PlaceItemAtTransform(item, spot, true);
	    area.currentItems++;
	    if (area.usedSpots == null) area.usedSpots = new List<Transform>();
	    area.usedSpots.Add(spot);
	}

	public void PlaceItemInFreeArea(GameObject item)
	{
	    if (!item) return;
	    var d = item.GetComponent<ItemSeedData>();
	    if (!d) return;

	    var candidates = spawnAreas
	        .Where(area => area != null && area.currentItems < area.maxItems && area.freeSpots != null && area.freeSpots.Length > 0 &&
	                       (area.allowedItemNames == null || area.allowedItemNames.Count == 0 || area.allowedItemNames.Contains(d.itemName)))
	        .ToList();
	    Shuffle(candidates);

	    foreach (var area in candidates)
	    {
	        var available = area.freeSpots.Where(t => t != null && (area.usedSpots == null || !area.usedSpots.Contains(t))).ToList();
	        if (available.Count > 0)
	        {
	            var spot = available[rnd.Next(available.Count)];
	            PlaceItemAtTransform(item, spot, true);
	            area.currentItems++;
	            if (area.usedSpots == null) area.usedSpots = new List<Transform>();
	            area.usedSpots.Add(spot);
	            return;
	        }
	    }

	    if (spawnAreas != null)
	    {
	        for (int i = 0; i < spawnAreas.Length; i++)
	        {
	            var area = spawnAreas[i];
	            if (area != null && (area.allowedItemNames == null || area.allowedItemNames.Count == 0) && area.currentItems < area.maxItems && area.freeSpots != null)
	            {
	                var available = area.freeSpots.Where(t => t != null && (area.usedSpots == null || !area.usedSpots.Contains(t))).ToList();
	                if (available.Count > 0)
	                {
	                    var spot = available[rnd.Next(available.Count)];
	                    PlaceItemAtTransform(item, spot, true);
	                    area.currentItems++;
	                    if (area.usedSpots == null) area.usedSpots = new List<Transform>();
	                    area.usedSpots.Add(spot);
	                    return;
	                }
	            }
	        }
	    }
	}

	public void Shuffle<T>(List<T> list)
	{
	    if (list == null) return;
	    for (int i = list.Count - 1; i > 0; i--)
	    {
	        int j = rnd.Next(i + 1);
	        T val = list[j];
	        list[j] = list[i];
	        list[i] = val;
	    }
	}

	public PuzzleDef GetEffectivePuzzleOfItem(string itemName, HashSet<string> visited = null)
	{
	    if (string.IsNullOrEmpty(itemName)) return null;
	    if (visited == null) visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	    if (visited.Contains(itemName)) return null;
	    visited.Add(itemName);

	    if (itemsByName == null || !itemsByName.TryGetValue(itemName, out var item) || !item) return null;

	    if (puzzleDefs != null)
	    {
	        foreach (var p in puzzleDefs)
	        {
	            if (p != null && p.spawnPoint != null)
	            {
	                if (Vector3.Distance(item.transform.position, p.spawnPoint.position) < 0.01f)
	                    return p;
	            }
	        }
	    }

	    if (allItems != null)
	    {
	        foreach (var container in allItems)
	        {
	            if (!container) continue;
	            var data = container.GetComponent<ItemSeedData>();
	            if (data != null && data.containedItems != null && data.containedItems.Any(x => string.Equals(x.Trim(), itemName.Trim(), StringComparison.OrdinalIgnoreCase)))
	            {
	                return GetEffectivePuzzleOfItem(data.itemName, visited);   // first container holding it decides, as in the original
	            }
	        }
	    }
	    return null;
	}

	public bool DetectCircularDependencies()
	{
	    var adj = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
	    if (itemsByName != null)
	    {
	        foreach (var kvp in itemsByName)
	        {
	            var list = new List<string>();
	            adj[kvp.Key] = list;
	            var eff = GetEffectivePuzzleOfItem(kvp.Key);
	            if (eff != null && eff.requiredItemNames != null)
	            {
	                foreach (var req in eff.requiredItemNames)
	                {
	                    if (!string.IsNullOrEmpty(req))
	                        list.Add(req);
	                }
	            }
	        }
	    }

	    var states = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
	    foreach (var key in adj.Keys)
	        states[key] = 0;

	    var stack = new Stack<string>();
	    bool hasCycle = false;

	    bool Dfs(string node)
	    {
	        states[node] = 1;
	        stack.Push(node);

	        if (adj.TryGetValue(node, out var neighbors) && neighbors != null)
	        {
	            foreach (var n in neighbors)
	            {
	                if (!states.TryGetValue(n, out int s)) continue;
	                if (s == 0)
	                {
	                    if (Dfs(n)) return true;
	                }
	                else if (s == 1)
	                {
	                    return true;
	                }
	            }
	        }

	        stack.Pop();
	        states[node] = 2;
	        return false;
	    }

	    foreach (var key in adj.Keys)
	    {
	        if (states[key] == 0)
	        {
	            if (Dfs(key))
	            {
	                hasCycle = true;
	                break;
	            }
	        }
	    }

	    return hasCycle;
	}

	public bool ValidatePuzzleDependencies()
	{
	    var itemPuzzles = new Dictionary<string, PuzzleDef>(StringComparer.OrdinalIgnoreCase);
	    if (itemsByName != null)
	    {
	        foreach (var kvp in itemsByName)
	        {
	            itemPuzzles[kvp.Key] = GetEffectivePuzzleOfItem(kvp.Key);
	        }
	    }

	    var puzzleItems = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
	    if (puzzleDefs != null)
	    {
	        foreach (var p in puzzleDefs)
	        {
	            if (p != null && !string.IsNullOrEmpty(p.puzzleName))
	                puzzleItems[p.puzzleName] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	        }
	    }

	    var initialFree = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	    if (itemsByName != null)
	    {
	        foreach (var itemName in itemsByName.Keys)
	        {
	            if (itemPuzzles.TryGetValue(itemName, out var p) && p != null && !string.IsNullOrEmpty(p.puzzleName))
	            {
	                if (!puzzleItems.TryGetValue(p.puzzleName, out var set))
	                {
	                    set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	                    puzzleItems[p.puzzleName] = set;
	                }
	                set.Add(itemName);
	            }
	            else
	            {
	                initialFree.Add(itemName);
	            }
	        }
	    }

	    var allRequired = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	    if (puzzleDefs != null)
	    {
	        foreach (var p in puzzleDefs)
	        {
	            if (p != null && p.requiredItemNames != null)
	            {
	                foreach (var req in p.requiredItemNames)
	                {
	                    if (!string.IsNullOrEmpty(req)) allRequired.Add(req);
	                }
	            }
	        }
	    }
	    if (requiredEscapeItemNames != null)
	    {
	        foreach (var req in requiredEscapeItemNames)
	        {
	            if (!string.IsNullOrEmpty(req)) allRequired.Add(req);
	        }
	    }

	    var accessible = new HashSet<string>(initialFree, StringComparer.OrdinalIgnoreCase);
	    var solved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	    bool changed = true;
	    while (changed)
	    {
	        changed = false;
	        if (puzzleDefs != null)
	        {
	            foreach (var p in puzzleDefs)
	            {
	                if (p == null || string.IsNullOrEmpty(p.puzzleName) || solved.Contains(p.puzzleName)) continue;
	                bool canSolve = true;
	                if (p.requiredItemNames != null && p.requiredItemNames.Count > 0)
	                {
	                    foreach (var req in p.requiredItemNames)
	                    {
	                        if (!string.IsNullOrEmpty(req) && !accessible.Contains(req))
	                        {
	                            canSolve = false;
	                            break;
	                        }
	                    }
	                }
	                if (canSolve)
	                {
	                    solved.Add(p.puzzleName);
	                    if (puzzleItems.TryGetValue(p.puzzleName, out var items))
	                    {
	                        foreach (var it in items) accessible.Add(it);
	                    }
	                    changed = true;
	                }
	            }
	        }
	    }

	    foreach (var req in allRequired)
	    {
	        if (!accessible.Contains(req) && GetEffectivePuzzleOfItem(req) != null)
	            return false;
	    }
	    return true;
	}

	public void GeneratePlacement()
	{
	    SaveOriginalStates();
	    int attempts = 0;
	    bool success = false;

	    if (PlayerPrefs.GetInt("RandomSeed") == 1 && RandomizeSeed)
	        Seed = UnityEngine.Random.Range(0, 999999999);
	    else
	        Seed = PlayerPrefs.GetInt("GameSeed");

	    while (attempts < 50 && !success)
	    {
	        attempts++;
	        rnd = new System.Random(Seed + attempts);
	        RestoreOriginalStates();

	        if (usedItemNames == null) usedItemNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
	        else usedItemNames.Clear();

	        if (usedPuzzleSpawns == null) usedPuzzleSpawns = new HashSet<Transform>();
	        else usedPuzzleSpawns.Clear();

	        if (puzzleHasItem == null) puzzleHasItem = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
	        else puzzleHasItem.Clear();

	        if (puzzleDefs != null)
	        {
	            foreach (var p in puzzleDefs)
	            {
	                if (p != null && !string.IsNullOrEmpty(p.puzzleName))
	                    puzzleHasItem[p.puzzleName] = false;
	            }
	        }

	        if (spawnAreas != null)
	        {
	            foreach (var area in spawnAreas)
	            {
	                if (area != null)
	                {
	                    area.currentItems = 0;
	                    if (area.usedSpots != null) area.usedSpots.Clear();
	                    else area.usedSpots = new List<Transform>();
	                }
	            }
	        }

	        BuildLookup();

	        var validItems = (allItems != null)
	            ? allItems.Where(x => x != null && x.GetComponent<ItemSeedData>() != null).ToList()
	            : new List<GameObject>();
	        var sortedPuzzles = (puzzleDefs != null)
	            ? puzzleDefs.Where(p => p != null && p.spawnPoint != null).OrderBy(p => p.Priority).ToList()
	            : new List<PuzzleDef>();

	        var candidatesByPuzzle = new Dictionary<PuzzleDef, List<GameObject>>();
	        foreach (var p in sortedPuzzles)
	        {
	            var cands = validItems.Where(x => !usedItemNames.Contains(x.GetComponent<ItemSeedData>().itemName))
	                                  .Where(x => IsItemAllowedForPuzzle(x, p))
	                                  .Where(x => x.GetComponent<ItemSeedData>().category != 4)
	                                  .Where(x => IsSafeContainerPlacement(x, p))
	                                  .ToList();
	            candidatesByPuzzle[p] = cands;
	        }

	        var assignedByPuzzle = new Dictionary<PuzzleDef, GameObject>();
	        foreach (var p in sortedPuzzles)
	        {
	            assignedByPuzzle[p] = null;
	            if (candidatesByPuzzle.TryGetValue(p, out var cands) && cands.Count > 0)
	            {
	                var chosen = cands[rnd.Next(cands.Count)];
	                assignedByPuzzle[p] = chosen;
	                foreach (var otherKvp in candidatesByPuzzle)
	                {
	                    if (otherKvp.Key != p)
	                        otherKvp.Value.Remove(chosen);
	                }
	            }
	        }

	        // Escape items no puzzle picked: with EscapeItemPuzzleChance they go straight into a free puzzle spot,
	        // otherwise (or if none fits) into a free spawn area. Both are placed now, before the puzzle items.
	        if (requiredEscapeItemNames != null && itemsByName != null)
	        {
	            foreach (var reqName in requiredEscapeItemNames)
	            {
	                if (!itemsByName.TryGetValue(reqName, out var reqItem) || reqItem == null) continue;
	                if (assignedByPuzzle.Values.Any(x => x == reqItem)) continue;
	                if (rnd.NextDouble() < (double)EscapeItemPuzzleChance && puzzleDefs != null && puzzleDefs.Count > 0)
	                {
	                    var compatiblePuzzles = sortedPuzzles
	                        .Where(p => p.spawnPoint != null && !usedPuzzleSpawns.Contains(p.spawnPoint) && assignedByPuzzle[p] == null && IsItemAllowedForPuzzle(reqItem, p))
	                        .ToList();
	                    if (compatiblePuzzles.Count > 0)
	                    {
	                        var targetP = compatiblePuzzles[rnd.Next(compatiblePuzzles.Count)];
	                        PlaceItemAtTransform(reqItem, targetP.spawnPoint, false);
	                        usedPuzzleSpawns.Add(targetP.spawnPoint);
	                        usedItemNames.Add(reqName);
	                        puzzleHasItem[targetP.puzzleName] = true;
	                        continue;
	                    }
	                }
	                PlaceItemInFreeArea(reqItem);
	                usedItemNames.Add(reqName);
	            }
	        }

	        foreach (var p in sortedPuzzles)
	        {
	            if (usedPuzzleSpawns.Contains(p.spawnPoint)) continue;
	            if (assignedByPuzzle.TryGetValue(p, out var item) && item != null)
	            {
	                var d = item.GetComponent<ItemSeedData>();
	                if (!usedItemNames.Contains(d.itemName))
	                {
	                    PlaceItemAtTransform(item, p.spawnPoint, false);
	                    usedPuzzleSpawns.Add(p.spawnPoint);
	                    usedItemNames.Add(d.itemName);
	                    puzzleHasItem[p.puzzleName] = true;
	                }
	                else
	                {
	                    assignedByPuzzle[p] = null;
	                }
	            }
	        }

	        foreach (var p in sortedPuzzles)
	        {
	            if (usedPuzzleSpawns.Contains(p.spawnPoint) || puzzleHasItem[p.puzzleName]) continue;
	            var cands = allItems.Where(x => x != null && x.GetComponent<ItemSeedData>() != null)
	                                .Where(x => !usedItemNames.Contains(x.GetComponent<ItemSeedData>().itemName))
	                                .Where(x => IsItemAllowedForPuzzle(x, p))
	                                .Where(x => x.GetComponent<ItemSeedData>().category != 4)
	                                .ToList();
	            if (cands.Count == 0)
	            {
	                cands = allItems.Where(x => x != null && x.GetComponent<ItemSeedData>() != null)
	                                .Where(x => !usedItemNames.Contains(x.GetComponent<ItemSeedData>().itemName))
	                                .Where(x => x.GetComponent<ItemSeedData>().category != 4)
	                                .ToList();
	            }
	            if (cands.Count > 0)
	            {
	                var item = cands[rnd.Next(cands.Count)];
	                var d = item.GetComponent<ItemSeedData>();
	                PlaceItemAtTransform(item, p.spawnPoint, false);
	                usedPuzzleSpawns.Add(p.spawnPoint);
	                usedItemNames.Add(d.itemName);
	                puzzleHasItem[p.puzzleName] = true;
	            }
	        }

	        if (spawnAreas != null && itemsByName != null)
	        {
	            foreach (var area in spawnAreas)
	            {
	                if (area != null && !string.IsNullOrEmpty(area.mandatoryItemName))
	                {
	                    if (itemsByName.TryGetValue(area.mandatoryItemName, out var item) && item != null)
	                    {
	                        if (!usedItemNames.Contains(area.mandatoryItemName))
	                        {
	                            PlaceItemInSpecificFreeArea(item, area);
	                            usedItemNames.Add(area.mandatoryItemName);
	                        }
	                    }
	                }
	            }
	        }

	        if (fillAllFreeSpawns && allItems != null)
	        {
	            var remainingItems = allItems.Where(x => x != null && x.GetComponent<ItemSeedData>() != null)
	                                         .Where(x => !usedItemNames.Contains(x.GetComponent<ItemSeedData>().itemName))
	                                         .ToList();
	            Shuffle(remainingItems);
	            foreach (var item in remainingItems)
	            {
	                PlaceItemInFreeArea(item);
	            }
	        }

	        foreach (var kvp in puzzleHasItem)
	        {
	            Debug.Log(kvp.Key + ": " + kvp.Value);
	        }

	        if (!DetectCircularDependencies() && ValidatePuzzleDependencies())
	        {
	            success = true;
	        }
	    }
	}
}
