using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Paused : MonoBehaviour
{
	public GameObject pauseMenu;
	public Slider SensSlider;
	public static bool IsPaused;
	public Slider Volume;
	public string PlayerPrefsTest;
	public PlayerStatus PlayerStatus;
	public AudioSource ClickSound;
	public PlayerStatus PS;
	public AudioSource[] AudiosAffected;
	public GameObject KeyBindMenu;
	public GameObject ResMenu;
	public GameObject EscMenu;
	public GameObject MenuData;
	public HandleObjects OM;
	public TMP_Text Data_Diff;
	public TMP_Text Data_Granny;
	public TMP_Text Data_Grandpa;
	public TMP_Text Data_Extras;
	public TMP_Text Data_Preset;
	public TMP_Text Data_S;
	public GameObject SeedSysMain;
	public GameObject SeedSysExtras;
	public GameObject SeedSys11;
	public GameObject SeedSys10;
}
