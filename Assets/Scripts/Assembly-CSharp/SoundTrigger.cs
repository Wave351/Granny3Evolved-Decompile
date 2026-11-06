using UnityEngine;

public partial class SoundTrigger : MonoBehaviour
{
	[Header("Components")]
	public AudioClip[] Sounds;
	public AudioClip Water;
	public GameObject NoiseOBJ_Granny;
	public GameObject NoiseOBJ_Grandpa;
	public GameObject SpawnBack;
	public GameObject SpawnBackSewer;
	[Header("Values")]
	public bool OnlyOnce;
	public float TimesDropped;
	public bool CanWater;
	public bool Stopped;
	public bool AlertsGrandpa;
	public bool DestroyOnSound;
	public bool WaterDes;
	public bool HouseMaterial;
	public bool canTeleportPoint;
}
