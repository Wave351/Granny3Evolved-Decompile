using UnityEngine;
using UnityEngine.UI;

public partial class RayRemoveTraps : MonoBehaviour
{
	public float RaycastDis;
	public GameObject TrapRemoval;
	public AudioClip KillTrap;
	public AudioClip KillTrapNM;
	public bool Trapped;
	public float ValueClock;
	public float SpeedRev;
	public Image FillBar;
	public ScreenEffects ScreenEffects;
	public GameObject Player;
	public PlayerStatus PlayerStatus;
	public LayerMask LS;
	public KeyCode KeyInteract;
}
