using UnityEngine;

public class ClosetUnhid : MonoBehaviour
{
	public PlayerStatus PlayerStatus;

	public AI_Granny Granny;

	public AI_Grandpa Grandpa;

	public bool Hid;

	private void OnTriggerStay(Collider other)
	{
	}
}
