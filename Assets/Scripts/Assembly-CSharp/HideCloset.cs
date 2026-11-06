using UnityEngine;

public class HideCloset : MonoBehaviour
{
	public PlayerStatus PlayerStatus;

	public AI_Granny Granny;

	public AI_Grandpa Grandpa;

	public ClosetUnhid UnHidScript;

	private void OnTriggerStay(Collider other)
	{
	}
}
