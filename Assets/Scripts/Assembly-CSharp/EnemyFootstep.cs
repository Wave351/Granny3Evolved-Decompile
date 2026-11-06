using UnityEngine;

public partial class EnemyFootstep : MonoBehaviour
{
	public AudioSource FeetSource;
	public AudioClip[] House;
	public AudioClip[] Grass;
	public AudioClip[] Water;
	public float CurrentGround;
	public float RayDistance;
	public bool InWater;
	public Transform SPRayGround;
	public LayerMask LS;
}
