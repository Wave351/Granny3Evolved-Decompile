using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public partial class AI_Slendrina : MonoBehaviour
{
	[Header("Main Components")]
	public NavMeshAgent Agent;
	public Transform SlendrinaTransform;
	public Renderer SlendrinaMesh;
	public Camera CameraPlayer;
	public PlayerStatus PS;
	public float NearDis;
	public float FarDis;
	public int Attemps;
	public int NavMeshAreaMask;
	public Vector3 normalExtent;
	public Vector3 easierExtent;
	public Vector3 normalCenter;
	public Vector3 easierCenter;
	[Header("Wandering")]
	public float WanderSpeed;
	public float DistanceFromPlayerSpawn;
	public bool PathingToPlayer;
	public Transform SPPlayerBack;
	[Header("Positioning")]
	public Vector3 StartPosV;
	public Transform StartPos;
	public Transform LookAtPoint;
	public Transform LookRayCast;
	public Transform LookRayCast2;
	public Transform LookRayCast3;
	public GameObject CamPivot;
	public Transform PlayerTransform;
	public MobileFPS FPS;
	public Transform HeadPL;
	[Header("Values")]
	public float Health;
	public float DamageSpeed;
	public bool PlayerCaughtAttention;
	public bool IsRenderingSlendrina;
	public bool SlendrinaTouchesPlayer;
	public LayerMask LayersRay;
	public bool Travel;
	public float TimerTeleport;
	public float TimerMaxTeleport;
	public float TimerIdle;
	public float TimerMaxIdle;
	public bool canAttack;
	public float lookAtSpeed;
	[Header("Audio")]
	public AudioSource SawSource;
	public AudioSource SlendrinaHunts;
	public AudioClip StareFar;
	public AudioClip StareNear;
	public float CloseDistance;
	public float TeleportAwayDistance;
	[Header("UI")]
	public Image BloodScreen;
	public float BorderBloodValue;
	public float SpeedBorderFade;
	public float SpeedLookingFade;
	public float SpeedBorderFadeGo;
	public float SpeedLookingFadeGo;
	[Header("Others")]
	public EnemyController EC;
	public FallingHolder Fall;
	public Animation ShakeAnim;
	public DoorLogic prisonerDoor;
	public Days Days;
	public BGManager BGM;
}
