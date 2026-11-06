using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Menu_Seed : MonoBehaviour
{
	public bool IsTypingSeed;
	[Header("UI Elements")]
	public TMP_InputField SeedInput;
	public Toggle RandomToggle;
	public string SeedPrefKey;
	public GameObject SavingOrNo;
}
