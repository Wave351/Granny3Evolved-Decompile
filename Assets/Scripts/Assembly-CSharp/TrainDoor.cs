using UnityEngine;

public class TrainDoor : MonoBehaviour
{
	public Interactive ontEventLogic;

	public Animation animDoors;

	public AudioSource audioDoors;

	public AudioClip openSound;

	public AudioClip closeSound;

	public bool closed;

	public bool second;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerStay(Collider other)
	{
	}

	public void AnimationCall()
	{
	}
}
