using UnityEngine;

public partial class TriggerFall : MonoBehaviour
{
	public bool Fell;
	public bool TriggerSound;
	public AudioClip Falls;
	public Rigidbody Rigidbody;
	public bool IsBumper;
	public GameObject[] activatedObjects;
	public bool hasNecessaryComponents;
}
