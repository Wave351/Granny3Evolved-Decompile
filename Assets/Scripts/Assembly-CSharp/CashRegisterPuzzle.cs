using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashRegisterPuzzle : MonoBehaviour
{
	public bool registerUnlocked;

	public BoxCollider[] buttonsCollision;

	public int[] buttonValues;

	public List<int> correctOrder;

	public int currentIndex;

	public TMP_Text codeText1;

	public TMP_Text codeText2;

	public Transform numberPad;

	public Transform[] locationsNumber;

	public GameObject paperUI;

	public GameObject paperObj;

	public bool inputLocked;

	private void Start()
	{
	}

	public void PressButton(int value)
	{
	}

	public void ClearButton()
	{
	}
}
