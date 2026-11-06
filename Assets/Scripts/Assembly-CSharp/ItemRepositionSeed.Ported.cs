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

// Ported from Annotated-Pseudocode/ItemRepositionSeed.c (Granny 3 Evolved, IL2CPP x64).
public partial class ItemRepositionSeed
{
	    public Dictionary<string, (Transform item, string spotName)> itemMap;
	    public void Awake()
	    {
	        itemMap = new Dictionary<string, (Transform item, string spotName)> {
	            { "PDKey", (PDKey, "SpotReposition_PDKey") },
	            { "ShedKey", (ShedKey, "SpotReposition_ShedKey") },
	            { "Cable", (GenCable, "SpotReposition_Cable") },
	            { "BridgeCrank", (BridgeCrank, "SpotReposition_BridgeCrank") },
	            { "Coin", (Coin, "SpotReposition_Coin") },
	            { "ElecSwitch", (ElecSwitch, "SpotReposition_ElecSwitch") },
	            { "Accelerator", (Accelerator, "SpotReposition_Accelerator") },
	            { "TrainKey", (TrainKey, "SpotReposition_TrainKey") },
	            { "Firewood", (Firewood, "SpotReposition_Firewood") },
	            { "Matches", (Matches, "SpotReposition_Matches") },
	            { "Coconut", (Coconut, "SpotReposition_Coconut") },
	            { "Plank", (Plank, "SpotReposition_Plank") },
	            { "Teddy", (Teddy, "SpotReposition_Teddy") },
	            { "SafeKey", (SafeKey, "SpotReposition_SafeKey") },
	            { "WPKey", (WPKey, "SpotReposition_WPKey") },
	            { "Fuse", (GateFuse, "SpotReposition_GateFuse") },
	            { "Valve", (Valve, "SpotReposition_Valve") },
	            { "PropaneTank", (Propane, "SpotReposition_PropaneTank") },
	            { "BalloonPack1", (BalloonPart1, "SpotReposition_BalloonPack") },
	            { "BalloonPack2", (BalloonPart2, "SpotReposition_BalloonPack") },
	            { "BalloonPack3", (BalloonPart3, "SpotReposition_BalloonPack") },
	            { "SubLever", (SubLever, "SpotReposition_SubLever") },
	            { "Castle", (Castle, "SpotReposition_CastleFig") },
	            { "Handwheel", (Handwheel, "SpotReposition_Handwheel") },
	            { "IntenseCoin", (IntenseCoin, "SpotReposition_IntenseCoin") },
	            { "Book", (Book, "SpotReposition_Book") },
	            { "Painting", (Painting, "SpotReposition_Painting") },
	            { "SecRelay", (SecurityRelay, "SpotReposition_Relay") }
	        };
	    }
	    public void OnTriggerEnter(Collider other)
	    {
	        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;
	        var data = other.GetComponent<ItemSeedData>();
	        if (data == null) return;
	        if (itemMap.TryGetValue(data.itemName, out var entry))
	        {
	            var spot = FindChildByName(transform, entry.spotName);
	            if (spot != null && entry.item != null)
	            {
	                entry.item.SetPositionAndRotation(spot.position, spot.rotation);
	                RepositionedItem = true;
	            }
	        }
	        if (Rigid) other.GetComponent<Rigidbody>().isKinematic = false;
	    }
	    public Transform FindChildByName(Transform parent, string name)
	    {
	        foreach (Transform child in parent) if (child.name == name) return child;
	        return null;
	    }
	    private IEnumerator Start()
	    {
	        yield return new WaitForSeconds(2f);
	        RepositionedItem = true;
	    }
}
