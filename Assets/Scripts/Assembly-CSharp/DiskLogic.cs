using UnityEngine;

public partial class DiskLogic : MonoBehaviour
{
	public GameObject vaseGranny;
	public GameObject vaseGrandpa;
	public bool vaseGrannyFilled;
	public bool vaseGrandpaFilled;
	public GameObject grannyPedestalSound;
	public float grannyPedestalHeight;
	public GameObject grandpaPedestalSound;
	public float grandpaPedestalHeight;
	public Transform disk;
	public float diskRotation;
	public GameObject diskMovementSound;
	public Transform grannyPedestal;
	public Transform grandpaPedestal;
	public float pedestalMoveSpeed;
	public float diskRotateSpeed;
	public bool grannyVaseFinished;
	public bool grandpaVaseFinished;
	public GameObject vasePlaceCollider1;
	public GameObject vasePlaceCollider2;
}
