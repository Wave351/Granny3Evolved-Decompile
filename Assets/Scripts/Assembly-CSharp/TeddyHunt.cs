using UnityEngine;

public partial class TeddyHunt : MonoBehaviour
{
	public AudioSource audioSource;
	public bool fading;
	public float fadeSpeed;
	public bool SupposedToFade;
	public bool NeedPlay;
	public float TimerGone;
	public bool AlreadyHunted;
	public GameObject TeddyH;
	public bool AlwaysAngry1;
	public bool AlwaysAngry2;
	public Renderer GrannyEyeM;
	public Renderer GrandpaEyeM;
	public AI_Granny Granny;
	public AI_Grandpa Grandpa;
	public Color GrannyEyesNormal;
	public Color GrannyEyesAngry;
	public Color GrandpaEyesNormal;
	public Color GrandpaEyesAngry;
	public Color GrannyEyesSetDark;
	public Color GrandpaEyesSetDark;
	public AudioSource soulHunt;
	public PickRay pickRay;
}
