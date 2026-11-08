using UnityEngine;

// Keeps a laser's flare facing whichever camera is drawing it (the game camera or the Scene view).
[ExecuteAlways]
public class LaserFlareBillboard : MonoBehaviour
{
	void OnWillRenderObject()
	{
		var cam = Camera.current;
		if (cam) transform.rotation = cam.transform.rotation;
	}
}
