using UnityEngine;

public class ShootLogic : MonoBehaviour
{
	public GameObject Smoke;

	public GameObject SpawnImpact;

	public AudioClip ImpactS;

	public AudioClip ShootS;

	public Transform Player;

	public Transform Rotater;

	public Transform Spawner;

	public float waiting;

	public float MaxWait;

	public float Distance;

	public Transform Target;

	public bool Shooting;

	public float ShootSpeed;

	public AudioSource Shot;

	public EnemyController EC;

	public PlayerStatus Status;

	public GameObject Impactus;

	public Transform SpineGrandpa;

	public GameObject BloodEffect;

	public WindowJumping WJ;

	public LayerMask LayersShoot;

	public GameObject ExplosionGas;

	public bool CanPlayerGetShoot;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Shoot()
	{
	}
}
