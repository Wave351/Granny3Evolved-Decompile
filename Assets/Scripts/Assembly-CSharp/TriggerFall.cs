using UnityEngine;

public class TriggerFall : MonoBehaviour
{
	public bool Fell;

	public bool TriggerSound;

	public AudioClip Falls;

	public Rigidbody Rigidbody;

	public bool IsBumper;

	public GameObject[] activatedObjects;

	public bool hasNecessaryComponents;

	private void Start()
	{
	}

	private void OnTriggerStay(Collider other)
	{
	}
}
