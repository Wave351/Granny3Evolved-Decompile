using UnityEngine;

public class EnemyFootstep : MonoBehaviour
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

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SoundEffectStep()
	{
	}
}
