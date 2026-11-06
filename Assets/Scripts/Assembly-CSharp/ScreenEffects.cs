using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public partial class ScreenEffects : MonoBehaviour
{
	public Image BloodScreen1;
	public Image BloodBitenScreen;
	public Image BloodScreenHit;
	public Image BloodScreenGameover;
	public Image BloodScreenDay4;
	public bool Hurt;
	public float Blood;
	public float SpeedFadeUp;
	public float SpeedFadeDown;
	public PostProcessProfile Profile;
	public ChromaticAberration CA;
	public Vignette V;
	public DepthOfField DOF;
	public Grain G;
	public bool NormalSee;
	public float SpeedGo;
	public float SpeedAppear;
	public float Scare1;
	public float Scare2;
	public float Max1;
	public float Max2;
	public float grainValue;
	public float speedGainGrain;
	public float speedLoseGrain;
	public AI_Slender SlenderAI;
	public Animation Camera;
	public GameObject Player;
}
