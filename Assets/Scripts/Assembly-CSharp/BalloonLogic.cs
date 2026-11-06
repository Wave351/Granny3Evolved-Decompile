using UnityEngine;

public class BalloonLogic : MonoBehaviour
{
	public PlayerStatus PS;

	public MobileFPS FPS;

	public Transform temporaryParentPlayerBalloon;

	public Transform exteriorPositionTeleport;

	public GameObject deterioratedBalloon;

	public Transform playerStuff;

	public GameObject flameThrowerTop;

	public GameObject gasSFX;

	public GameObject valveGas;

	public bool valveGasOn;

	public bool insideBalloon;

	public BoxCollider fieryCollision;

	public GameObject fireGas;

	public GameObject propaneTank;

	public SkinnedMeshRenderer b1;

	public SkinnedMeshRenderer b2;

	public int blendInt;

	public float inflateSpeedBalloon;

	public float timerInflateOff;

	public GameObject ballonPart1;

	public GameObject ballonPart2;

	public bool floating;

	public Escapes E;

	public AI_Granny AI1;

	public AI_Grandpa AI2;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BalloonContainPlayer()
	{
	}

	public void EnterHotAirBalloon()
	{
	}
}
