using UnityEngine;
using UnityEngine.UI;

public partial class BGManager : MonoBehaviour
{
	public GameObject MusicG;
	public Toggle Music;
	public AudioSource Outside;
	public AudioSource House;
	public AudioSource Facility;
	public AudioSource HouseMusic;
	public AudioSource ChaseNormal;
	public bool CanFade;
	public bool InHouse;
	public bool InOutside;
	public bool InFacility;
	public float Speed;
	public float MaxHouse;
	public float MaxOutside;
	public float MaxFacility;
	public float MaxHouseMusic;
	public bool GrannyHunting;
	public float MaxGranny;
	public float FadeBackBG;
	public float SpeedChaseFade;
}
