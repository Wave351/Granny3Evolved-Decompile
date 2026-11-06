using UnityEngine;

public class Eyes_Grandpa : MonoBehaviour
{
	public Camera CameraView;

	public float ViewRange;

	public GameObject Player;

	public bool IsChasing;

	public bool Seeing;

	public float TimerStopVision;

	public float MaxVisionConvinced;

	public AudioSource Talk;

	public AudioClip[] YappingClips;

	public float SquareMagnitude;

	public PlayerStatus PlayerStatus;

	public AI_Grandpa AI;

	public bool PlayerSpotted;

	public GameObject NoiseObjGranny;

	public AudioSource SeeS;

	public Escapes E;

	public float OriginalGrandpaPitch;

	public DoorLogic prisonerDoor;

	private void Start()
	{
	}

	public void FixedUpdate()
	{
	}
}
