using UnityEngine;

public class DoorRay : MonoBehaviour
{
	[Header("Main Components")]
	public float RaycastDis;

	public GameObject Ring;

	public bool buttonClicked;

	public GameObject Player;

	public PickRay PickRay;

	public PlayerStatus PlayerStatus;

	public HandleObjects OM;

	public KeyCode InteractKey;

	public ScreenEffects ScreenEffects;

	public WindowJumping WJ;

	[Header("Noisy Points Related")]
	public GameObject NoiseGranny;

	public GameObject NoiseGrandpa;

	public Transform ToiletNoiseSpawn;

	public Transform PianoNoiseSpawn;

	[Header("Highlights")]
	public GameObject firstHotAirPart1Highlight;

	public GameObject firstHotAirPart2Highlight;

	public GameObject firstHotAirPart3Highlight;

	public GameObject propaneTankHighlight;

	public GameObject gasValveHighlight;

	[Header("Sound effects")]
	public AudioSource Audio;

	[Header("Others")]
	public BGManager BG;

	public EscapeRouteChecks ERC;

	public CrowChimney CC;

	public GuillotineAnim GA;

	public Days dayStuff;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void HandleButtonClick()
	{
	}
}
