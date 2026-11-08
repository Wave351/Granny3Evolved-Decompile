using UnityEngine;

// Added to a balloon chain once it's cut: a metal clank whenever it hits something hard enough while falling, and the
// first landing makes a noise Granny and Grandpa can hear, like a dropped item.
public class FallenChain : MonoBehaviour
{
	public AudioClip LandClip;
	public GameObject NoiseObj;

	const float MinSpeed = 1.2f;    // slower bumps make no sound
	const float Gap = .25f;         // seconds between clanks
	const int MaxClanks = 4;

	int clanks;
	float last = -1f;
	bool heard;

	void OnCollisionEnter(Collision hit)
	{
		if (clanks >= MaxClanks || Time.time - last < Gap || hit.relativeVelocity.magnitude < MinSpeed) return;
		clanks++;
		last = Time.time;
		Vector3 at = hit.contactCount > 0 ? hit.GetContact(0).point : transform.position;
		if (LandClip) AudioSource.PlayClipAtPoint(LandClip, at, Mathf.Clamp01(hit.relativeVelocity.magnitude / 5f) * .9f + .1f);
		if (!heard && NoiseObj) { heard = true; Instantiate(NoiseObj, at, Quaternion.identity); }
	}
}
