using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public partial class Escapes : MonoBehaviour
{
	public GameObject MobileFPS;
	public PickRay Ray;
	public Image Black;
	public GameObject SpaceT;
	public GameObject HandHold;
	public GameObject UIElem;
	public GameObject Texts;
	public BGManager BG;
	public Transform GameOverTurnOff;
	public GameObject GrannyS;
	public GameObject GrandpaS;
	public GameObject TheEndT;
	public ScreenEffects ScreenEffects;
	public EnemyController EC;
	public GameObject Granny;
	public GameObject Grandpa;
	public Paused PauseLogic;
	public GameObject BloodScreens;
	public GameObject UINew;
	public GameObject TimerUISwitch;
	public PlayerStatus PS;
	public bool DiedOnce;
	public bool UsedWPOnce;
	public bool SeenOnce;
	public AI_Alligator alligator;
	public TicketControlBot ticketBot;
	public EscapeTriggerGate ETG;
	public EscapeRouteChecks ERC;
	public GameObject slendrina;
	public CribTrigger cribTrigger;
	[Header("Gate Stuff")]
	public Transform PosGrannyEndGate;
	public Transform PosGrandpaEndGate;
	public Transform posSlendrinaGateEnd;
	public GameObject escapeGateMusic;
	public GameObject cutsceneGateCam;
	[Header("Train Stuff")]
	public Transform PosGrannyEndTrain;
	public Transform PosGrandpaEndTrain;
	public Transform posSlendrinaTrainEnd;
	public Animation trainAnim;
	public Animation acceleratorAnim;
	public GameObject cutsceneCamTrain;
	public GameObject cutsceneCamTrainEscaped;
	public GameObject escapeTrainMusic;
	[Header("Balloon Stuff")]
	public Transform PosGrannyEndBalloon;
	public Transform PosGrandpaEndBalloon;
	public Transform posSlendrinaBalloonEnd;
	public GameObject cutsceneCamBalloon;
	public GameObject escapeBalloonMusic;
	public GameObject playerSilhouette;
	public Transform hotAirContainer;
	public BalloonLogic BL;
	[Header("Submarine Stuff")]
	public Transform PosGrannyEndSubmarine;
	public Transform PosGrandpaEndSubmarine;
	public Transform posSlendrinaSubmarine;
	public GameObject cutsceneCamSubmarine;
	public GameObject escapeSubmarineMusic;
	public GameObject submarineObjectTop;
	public GameObject slideDoorBottom1;
	public GameObject slideDoorBottom2;
	public GameObject slenderEnemy;
	public Transform slenderEndPosition;
	public GameObject resetFloorStuff;
	public GameObject moonlightRLT;
	public GameObject soulHuntSFX;
}
