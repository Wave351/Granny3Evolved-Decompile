using UnityEngine;
using UnityEngine.AI;

public partial class FacilityBot : MonoBehaviour
{
	[Header("Core")]
	public Transform idlePoint;
	public NavMeshAgent agent;
	public PlayerStatus PS;
	[Header("Patrol Logic / Death / Hunt")]
	public bool travelBack;
	public bool hunting;
	public bool dead;
	public GameObject lampObject;
	public float huntingTimer;
	public float huntTimerMax;
	public float timerFadeTravel;
	public float timerFadeMaxTravel;
	public float distanceElectricCharge;
	public Transform player;
	public Transform playerHead;
	public Animation headUnitAnim;
	public Rigidbody[] partsDoor;
	public GameObject explosionDoor;
	public Renderer lightObjDoor;
	public Animation handsAnim;
	public GameObject deathTrigger;
	public ParticleSystem[] electricDischarge;
	[Header("Vision")]
	public Transform sight1;
	public Transform sight2;
	public LayerMask layersRay;
	[Header("SFX")]
	public GameObject patrolSFX;
	public AudioSource electricitySFX;
	public AudioClip robotDefeat;
	public bool fadingPatrolSFX;
}
