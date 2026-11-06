using UnityEngine;

public class StoneEnhanced : MonoBehaviour
{
	public MeshFilter mf;

	public Mesh stoneLava;

	public Mesh stoneIcy;

	public Mesh stoneMagenta;

	public Texture stoneLavaTexture;

	public Texture stoneIcyTexture;

	public Texture stoneMagentaTexture;

	public GameObject impactLava;

	public GameObject impactIce;

	public GameObject impactPower;

	public GameObject impactSmoke;

	public GameObject impactSmokeNoHits;

	public bool onCollision;

	public bool grannyShot;

	private void Start()
	{
	}

	private void OnCollisionEnter(Collision other)
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}
}
