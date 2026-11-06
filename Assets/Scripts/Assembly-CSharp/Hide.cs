using UnityEngine;

public class Hide : MonoBehaviour
{
	public PlayerStatus PlayerStatus;

	public bool RegularHide;

	public bool UnderStuffHide;

	public bool ClosetHiding;

	public AI_Granny Granny;

	public AI_Grandpa Grandpa;

	private void OnTriggerEnter(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}
}
