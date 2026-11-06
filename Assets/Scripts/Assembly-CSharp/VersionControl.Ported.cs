using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/VersionControl.c (Granny 3 Evolved, IL2CPP x64).
public partial class VersionControl
{
		// Rebuilds the map as it was in older game versions (CURVERSION 0 = 1.0, 1 = 1.1, 2 = 1.2).
		private void Start()
		{
			int version = PlayerPrefs.GetInt("CURVERSION");
			if (version == 0)
			{
				foreach (var o in DisableOneZeroObjects) o.SetActive(false);
				foreach (var o in DisableOneOneObjects) o.SetActive(false);
				// 1.0 item spots.
				fuse_Setup.PositionMain_P1 = fusePreset1_First;
				cable_Setup.PositionMain_P1 = cablePreset1_First;
				crank_Setup.PositionMain_P1 = crankPreset1_First;
				coconut_Setup.PositionMain_P1 = coconutPreset1_First;
				plank_Setup.PositionMain_P1 = plankPreset1_First;
				wpKey_Setup.PositionMain_P1 = wpKeyPreset1_First;
				shedKey_Setup.PositionMain_P2 = shedKeyPreset2_First;
				cable_Setup.PositionMain_P2 = cablePreset2_First;
				firewood_Setup.PositionMain_P2 = firewoodPreset2_First;
				wpKey_Setup.PositionMain_P2 = wpKeyPreset2_First;
				shedKey_Setup.PositionMain_P3 = shedKeyPreset3_First;
				crank_Setup.PositionMain_P3 = crankPreset3_First;
				wpKey_Setup.PositionMain_P3 = wpKeyPreset3_First;
				shedKey_Setup.PositionMain_P4 = shedKeyPreset4_First;
				teddy_Setup.PositionMain_P4 = teddyPreset4_First;
				safeKey_Setup.PositionMain_P4 = safeKeyPreset4_First;
				wpKey_Setup.PositionMain_P4 = wpKeyPreset4_First;
				crank_Setup.PositionMain_P5 = crankPreset5_First;
				matches_Setup.PositionMain_P5 = matchesPreset5_First;
				pdKey_Setup.PositionMain_P5 = pdKeyPreset5_First;
				coconut_Setup.PositionMain_P5 = coconutPreset5_First;
				wpKey_Setup.PositionMain_P5 = wpKeyPreset5_First;
				safeKey_Setup.PositionMain_P5 = safeKeyPreset5_First;
				fuse_Setup.PositionMain_P6 = fusePreset6_First;
				pdKey_Setup.PositionMain_P6 = pdKeyPreset6_First;
				coconut_Setup.PositionMain_P6 = coconutPreset6_First;
				wpKey_Setup.PositionMain_P6 = wpKeyPreset6_First;
				teddy_Setup.PositionMain_P6 = teddyPreset6_First;
			}
			else if (version == 1)
			{
				wallsOneOne.SetActive(false);
				foreach (var o in DisableOneOneObjects) o.SetActive(false);
				trainKey_Setup.PositionMain_P2 = trainKeyPreset2_Second;
				trainKey_Setup.PositionMain_P3 = trainKeyPreset3_Second;
			}
			else if (version == 2) wallsOneOne.SetActive(false);
		}
}
