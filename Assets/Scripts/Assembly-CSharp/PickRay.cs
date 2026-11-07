using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class PickRay : MonoBehaviour
{
	[Header("General")]
	public GameObject Button;
	public GameObject Ring;
	public GameObject Drop1;
	public GameObject Player;
	[Header("Raycasting & Drop")]
	public float RaycastDis;
	public float RaycastCheckItemDis;
	public bool CanDropDis;
	public float DropForceItem;
	public Transform DropP;
	[Header("Systems")]
	public PlayerStatus PlayerStatus;
	public WindowJumping WJ;
	public Escapes Escapes;
	[Header("Slingshot")]
	public GameObject ShootT;
	public Transform PosShootStone;
	public float ShootSpeed;
	public float StoneAmount;
	public GameObject Stone1;
	public GameObject Stone2;
	public GameObject Stone3;
	public GameObject stoneShoot;
	public AudioClip stoneShootSFX;
	public AudioClip reloadSlingSFX;
	[Header("Grenade")]
	public GameObject ThrowG;
	public GameObject LethalGrenade;
	public Transform PosThrowGrenade;
	public float SpeedForceThrowGrenade;
	[Header("Book")]
	public GameObject BlastT;
	public GameObject MagicBlast;
	public ParticleSystem electricBook;
	public Animation bookAnim;
	public Transform PosShootBlast;
	public Transform PosShootBlastCrouch;
	public GameObject boxColliderOldBook;
	public GameObject boxColliderNewBook;
	public bool hasSoul;
	public float SpeedForceBlast;
	[Header("Baton")]
	public GameObject batonHand;
	public GameObject AttackT;
	public float distanceAttack;
	public AudioClip hitBaton;
	public GameObject effectHit;
	[Header("Shotgun/s")]
	public GameObject ShootB2;
	public float ImpactS2;
	public GameObject MuzzleFront;
	public float Gun2RayDistance;
	public Transform RayShoot2;
	public float AmmoAmount;
	public Transform ShotgunHandMain;
	public Transform ShotgunHandPipes;
	public Vector3 ShotgunHandMain_ZeroPos;
	public Vector3 ShotgunHandMain_ZeroRot;
	public AudioClip EmptyGun;
	public Animation ShootHead;
	public GameObject BloodEffect;
	public GameObject ExplosionGas;
	public bool IsPOWER;
	public GameObject MoreAmmo;
	[Header("UI Others")]
	public TMP_Text Space;
	public TMP_Text Shoot1;
	public TMP_Text Shoot2;
	public TMP_Text Throw;
	public Image Crosshair;
	public Image EButton_Door;
	public Image EButton_Pick;
	public GameObject lockPickUIUsing;
	public GameObject AmmoCounterUI;
	public GameObject AmmoUI1;
	public GameObject AmmoUI2;
	[Header("Objects Prefabs & Others")]
	public GameObject slingShotHand;
	public Animation slingShotAnim;
	public GameObject ShotgunPickHand;
	public GameObject H_DSH;
	public GameObject H_GRE;
	public GameObject H_GRVAS;
	public GameObject H_PAVAS;
	public GameObject H_CO;
	public GameObject H_MOM;
	public GameObject H_BOX1;
	public GameObject H_MAT;
	public GameObject H_BOOK;
	public GameObject ShotgunDrop;
	public GameObject ImpactLord;
	[Header("Text Items")]
	public GameObject T_HighlightItem;
	public GameObject T_NeedShit;
	public string stoneTextHighlight;
	[Header("Sound effects")]
	public AudioClip PickUp;
	public AudioClip ImpactSound;
	[Header("Other Important Components")]
	public bool buttonClicked;
	public bool buttonClicked2;
	public EnemyController EnemyControl;
	public EscapeRouteChecks ERC;
	public DiskLogic DL;
	public Inventory Inventory;
	public CrowChimney CC;
	public BalloonLogic BL;
	public Days dayStuff;
	public LayerMask GunLayers;
	public LayerMask DropDistanceCheckLayers;
	public GameObject Noisy1;
	public GameObject Noisy2;
	public LayerMask LayersIncluded;
	public GameObject LockPickUI;
	public GameObject doorCollisionLockPick;
	public GameObject coconutInPlace;
	public GameObject momInPlace;
	public bool vase1Filled;
	public bool vase2Filled;
	public bool placedCoconut;
	public bool pickedCoco;
	public bool pickedMom;
	public bool pickedBook;
	public bool pickedSling;
	public bool shotWithSling;
	public TrainDoor tDoors1;
	public TrainDoor tDoors2;
	public GameObject highlightBalloonSpot;
	[Header("KeyCodes")]
	public KeyCode MainInteract;
	public KeyCode DropKey;
	public KeyCode ShootKey;
}
