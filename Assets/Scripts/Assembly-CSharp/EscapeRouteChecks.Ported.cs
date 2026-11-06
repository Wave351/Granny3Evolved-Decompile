using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EscapeRouteChecks.c (Granny 3 Evolved, IL2CPP x64).
public partial class EscapeRouteChecks
{
		public void SetGateOpened() { OpenedGate = true; }
		public void SetAcceleratorOn() { PlacedAccelerator = true; }
		public void SetTrainKeyOn() { PlacedTrainKey = true; }
		public void SetElecSwitchOn1() { FlippedSwitch1 = true; }
		public void SetElecSwitchOn2() { FlippedSwitch2 = true; }
		public void SetElecSwitchOn3() { FlippedSwitch3 = true; }
		public void SetMaxBalloon() { maximizedBalloon = true; }
		public void PutValveOn() { valveKnobPlaced = true; }
		public void PutLeverOn() { submarineReady = true; }
		// Tracks which escapes are ready. All routes (submarine too with Extras) on Extreme unlocks EX_AllRoutesExtras.
		private void Update()
		{
			if (PlacedAccelerator && PlacedTrainKey && FlippedSwitch1 && FlippedSwitch2 && FlippedSwitch3 && !TrainAvailable) TrainAvailable = true;
			if (variableBridge.activeSelf && variableGate.activeSelf && !OpenedGate) OpenedGate = true;
			if (maximizedBalloon && valveKnobPlaced && !ballonAvailable) ballonAvailable = true;
			if (AllRoutesOpen) return;
			int extras = PlayerPrefs.GetInt("Extras");
			if (!OpenedGate) return;
			if (extras == 0)
			{
				if (TrainAvailable && ballonAvailable) AllRoutesOpen = true;
			}
			else if (TrainAvailable && ballonAvailable && submarineReady)
			{
				AllRoutesOpen = true;
				if (PlayerPrefs.GetInt("Diff") == 3) PlayerPrefs.SetInt("EX_AllRoutesExtras", 1);
			}
		}
}
