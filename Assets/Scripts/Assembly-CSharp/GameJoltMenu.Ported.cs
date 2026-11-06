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

// Ported from Annotated-Pseudocode/GameJoltMenu.c (Granny 3 Evolved, IL2CPP x64).
public partial class GameJoltMenu
{
	    public GameJoltMenu() { AvailableToTrophy = true; slingshotSkins = new[] { "Lava", "Ice", "Magenta" }; }
	    private void Start() { StartCoroutine(AvailabilityGone()); }
	    private void Update()
	    {
	        var api = GameJolt.API.GameJoltAPI.Instance;
	        if (api == null || api.CurrentUser == null)
	        {
	            UsernameText.text = "Not signed in";
	            sessionSynced = false;
	        }
	        else
	        {
	            UsernameText.text = "Signed in as: " + api.CurrentUser.Name;
	            if (!sessionSynced)
	            {
	                sessionSynced = true;
	                GameJolt.API.Trophies.Get(trophies =>
	                {
	                    if (trophies == null) return;
	                    bool changed = false;
	                    foreach (var trophy in trophies)
	                    {
	                        if (!trophy.Unlocked) continue;
	                        string key = KeyGrabTrophyID(trophy.ID);
	                        if (!string.IsNullOrEmpty(key) && PlayerPrefs.GetInt(key) != 1)
	                        { PlayerPrefs.SetInt(key, 1); changed = true; }
	                    }
	                    if (changed) PlayerPrefs.Save();
	                });
	                GameJolt.API.DataStore.Get("slingshot_progression", false, SyncSlingshot);
	            }
	        }
	        if (!AvailableToTrophy || api == null || api.CurrentUser == null) return;
	        foreach (var entry in trophyPrefs)
	            if (PlayerPrefs.GetInt(entry.pref) == 1) GameJolt.API.Trophies.Unlock(entry.id);
	    }
	    private static readonly (string pref, int id)[] trophyPrefs = {
	        ("HARD_Escape", 0x4aa5b),
	        ("HARD_Both", 0x4aa5c),
	        ("AllRoutes", 0x4aa5d),
	        ("EXTREME_Balloon", 0x4aa5e),
	        ("EXTREME_Gate", 0x4aa5f),
	        ("EXTREME_Train", 0x4aa60),
	        ("EXTREME_NoDying", 0x4aa61),
	        ("IMP_Escape", 0x4aa62),
	        ("IMP_Seed", 0x4aa63),
	        ("IMP_NoDying", 0x4aa64),
	        ("IMP_AllRoutes", 0x4aa65),
	        ("EXTREME_Escape", 0x4c295),
	        ("EXTREME_Submarine", 0x4c296),
	        ("EXTREME_Slingy", 0x4c297),
	        ("EX_Kirky", 0x4c298),
	        ("EX_NoKilling", 0x4c299),
	        ("EX_NoAlarm", 0x4c29a),
	        ("EX_AllRoutesExtras", 0x4c29b),
	        ("IMP_Gate", 0x4c29c),
	        ("IMP_Train", 0x4c29d),
	        ("IMP_Balloon", 0x4c29e),
	        ("IMP_Submarine", 0x4c29f)
	    };
	    private string KeyGrabTrophyID(int id)
	    {
	        foreach (var entry in trophyPrefs) if (entry.id == id) return entry.pref;
	        return null;
	    }
	    public void SkinToCloud()
	    {
	        var api = GameJolt.API.GameJoltAPI.Instance;
	        if (api == null || api.CurrentUser == null) return;
	        var data = new CloudSlingshotData();
	        foreach (string skin in slingshotSkins)
	        {
	            string key = "Slingshot_" + skin;
	            string fragmentKey = key + "_Fragments";
	            if (PlayerPrefs.GetInt(key) == 1) data.unlockedSkins.Add(key);
	            int fragments = PlayerPrefs.GetInt(fragmentKey);
	            if (fragments > 0) { data.fragmentKeys.Add(fragmentKey); data.fragmentCounts.Add(fragments); }
	        }
	        GameJolt.API.DataStore.Set("slingshot_progression", JsonUtility.ToJson(data), false, success => { });
	    }
	    private void SyncSlingshot(string json)
	    {
	        var data = new CloudSlingshotData();
	        if (!string.IsNullOrEmpty(json))
	        {
	            data = JsonUtility.FromJson<CloudSlingshotData>(json);
	            if (data == null || data.unlockedSkins == null) return;
	            bool changed = false;
	            foreach (string key in data.unlockedSkins)
	                if (PlayerPrefs.GetInt(key) != 1) { PlayerPrefs.SetInt(key, 1); changed = true; }
	            if (data.fragmentKeys == null || data.fragmentCounts == null) return;
	            for (int i = 0; i < data.fragmentKeys.Count && i < data.fragmentCounts.Count; i++)
	                if (PlayerPrefs.GetInt(data.fragmentKeys[i]) < data.fragmentCounts[i])
	                { PlayerPrefs.SetInt(data.fragmentKeys[i], data.fragmentCounts[i]); changed = true; }
	            if (changed) PlayerPrefs.Save();
	        }
	        bool upload = false;
	        foreach (string skin in slingshotSkins)
	        {
	            string key = "Slingshot_" + skin;
	            string fragmentKey = key + "_Fragments";
	            if (PlayerPrefs.GetInt(key) == 1 && !data.unlockedSkins.Contains(key))
	            { data.unlockedSkins.Add(key); upload = true; }
	            int fragments = PlayerPrefs.GetInt(fragmentKey);
	            if (fragments <= 0) continue;
	            int index = data.fragmentKeys.IndexOf(fragmentKey);
	            if (index == -1)
	            { data.fragmentKeys.Add(fragmentKey); data.fragmentCounts.Add(fragments); upload = true; }
	            else if (data.fragmentCounts[index] < fragments)
	            { data.fragmentCounts[index] = fragments; upload = true; }
	        }
	        if (upload) GameJolt.API.DataStore.Set("slingshot_progression", JsonUtility.ToJson(data), false, success => { });
	    }
	    private IEnumerator AvailabilityGone() { yield return new WaitForSeconds(6f); AvailableToTrophy = false; }
	    public void SignInStart() { GameJolt.UI.GameJoltUI.Instance.ShowSignIn(); }
	    public void UnlockTrophy(int t) { GameJolt.API.Trophies.Unlock(t); }
}
