using UnityEngine;

public class TriggerLethalWater : MonoBehaviour
{
	public AI_Alligator alligator;

	public AudioClip water;

	public AudioClip playerWater;

	public EnemyController EC;

	public Transform teleportStonePoint;

	public GameObject momInCellarBody;

	private void OnTriggerEnter(Collider other)
	{
	}
}
