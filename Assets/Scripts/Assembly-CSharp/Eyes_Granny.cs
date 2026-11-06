using UnityEngine;

public class Eyes_Granny : MonoBehaviour
{
	public Camera CameraView;

	public float ViewRange;

	public GameObject Player;

	public bool IsChasing;

	public bool Seeing;

	public float TimerStopVision;

	public float MaxVisionConvinced;

	public float SquareMagnitude;

	public PlayerStatus PlayerStatus;

	public AI_Granny AI;

	public bool PlayerSpotted;

	public AudioSource SeeS;

	public Escapes E;

	public DoorLogic prisonerDoor;

	private void Start()
	{
	}

	public void FixedUpdate()
	{
	}
}
