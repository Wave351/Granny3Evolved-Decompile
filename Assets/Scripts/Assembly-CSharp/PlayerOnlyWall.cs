using UnityEngine;

// An invisible wall that only stops the player. Items, enemies, Granny and Grandpa, thrown things and lasers all go
// through it, but the player can't walk through it or pick up / use things through it.
// Put it on any object with a collider (Tools > Evolved Restoration > Make Selected Player-Only Walls). It stays
// visible in the editor so it can be placed; it's hidden in the game.
public class PlayerOnlyWall : MonoBehaviour
{
	public const int Layer = 21;   // "PlayerOnly"
	public static int Mask => 1 << Layer;

	static bool layerSetUp;

	// The layer touches only the player.
	public static void SetUpLayer()
	{
		if (layerSetUp) return;
		layerSetUp = true;
		int player = LayerMask.NameToLayer("Player");
		for (int l = 0; l < 32; l++) Physics.IgnoreLayerCollision(Layer, l, l != player);
	}

	void Awake()
	{
		SetUpLayer();
		gameObject.layer = Layer;
		foreach (var r in GetComponentsInChildren<Renderer>(true)) r.enabled = false;
		if (!GetComponent<Collider>()) gameObject.AddComponent<BoxCollider>();
		foreach (var c in GetComponentsInChildren<Collider>(true)) c.isTrigger = false;
		// The player's pick-up / use ray stops at it, so nothing behind it can be reached.
		foreach (var pick in FindObjectsByType<PickRay>(FindObjectsInactive.Include)) pick.LayersIncluded |= Mask;
	}
}
