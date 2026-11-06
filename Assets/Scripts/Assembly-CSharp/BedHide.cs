using UnityEngine;
using UnityEngine.UI;

public partial class BedHide : MonoBehaviour
{
	public PlayerStatus PlayerStatus;
	public float Bed;
	public bool InBed;
	public bool NearBed;
	public Image ButtonSprite;
	public Sprite InSprite;
	public Sprite OutSprite;
	public AudioSource Audio;
	public AudioClip GetIn;
	public AudioClip GetOut;
	public Transform PlayerNewPos;
	public GameObject Player;
	public GameObject OriginalCam;
	public GameObject CamInside;
	public GameObject CamHand;
	public float TimerAgain;
	public float MaxAgain;
	public FollowHand Handheld;
	public Canvas Canvas;
	public PickRay RayP;
	public PlayerStatus StatusPL;
	public Eyes_Granny GrannyEye;
	public Eyes_Grandpa GrandpaEye;
	public AI_Slendrina AISlendrina;
}
