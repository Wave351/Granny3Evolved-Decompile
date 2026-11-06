using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class CribTrigger : MonoBehaviour
{
	public Material TeddyM;
	public Material SlendrinaBodyM;
	public Material SlendrinaHairM;
	public Color StartSlendrinaBody;
	public Color StartSlendrinaHair;
	public Color StartTeddy;
	public Transform slendrinaPos;
	public GameObject Slendrina;
	public GameObject GlowEffect;
	public bool TeddyFade;
	public float SpeedFadeSlendrina;
	public float SpeedFadeSlendrinaH;
	public float maxHair;
	public bool slendrinaActive;
	public bool SlendrinaFades;
	public ParticleSystem Smoke;
	public GameObject TeddyObj;
	public Transform PlayerP;
	public Animation SlendrinaAnim;
	public GameObject ItemDropPoint;
	public Animation ShakeAnim;
	public PickRay rayPick;
}
