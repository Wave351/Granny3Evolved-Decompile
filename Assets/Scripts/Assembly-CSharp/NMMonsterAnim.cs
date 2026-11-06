using UnityEngine;

public partial class NMMonsterAnim : MonoBehaviour
{
	public AudioSource audio;
	public AudioClip[] walkSounds;
	public ParticleSystem powerfulBlast;
	public Transform blastGenerate;
	public GameObject noisyGranny;
	public GameObject noisyGrandpa;
	public FallingHolder FH;
	public PlayerStatus PS;
	public bool blasting;
}
