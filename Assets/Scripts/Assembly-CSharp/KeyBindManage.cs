using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindManage : MonoBehaviour
{
	[Header("KeyCodes Interactions")]
	public GameObject PrimaryB;

	public GameObject SecondaryB;

	public GameObject DropB;

	public GameObject CrouchB;

	public GameObject ShootB;

	[Header("Key Codes")]
	public KeyCode PrimaryInteraction;

	public KeyCode SecondaryInteraction;

	public KeyCode Dropping;

	public KeyCode Crouching;

	public KeyCode Shooting;

	[Header("Variables")]
	public float CurrentKeyBindTab;

	public List<KeyCode> allowedKeys;

	public bool WaitingForKey;

	public KeyCode NewKey;

	[Header("Others")]
	public GameObject KeyBindMenu;

	public Button RebindPrimaryB;

	public Button RebindSecondaryB;

	public Button RebindDropB;

	public Button RebindCrouchB;

	public Button RebindShootB;

	public GameObject PressEnterText;

	public Button[] BackButton;

	[Header("Scripts")]
	public PickRay PickRay;

	public DoorRay DoorRay;

	public CrouchHolder CrouchHolder;

	public RayRemoveTraps BearRay;

	[Header("TextsKeys")]
	public TMP_Text PrimaryText;

	public TMP_Text SecondaryText;

	public TMP_Text DropText;

	public TMP_Text CrouchText;

	public TMP_Text ShootText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartRebind(int i)
	{
	}

	public void DetectKeyPress()
	{
	}

	public void ConfirmNewKey()
	{
	}

	public void BackOrNextTab(int i)
	{
	}
}
