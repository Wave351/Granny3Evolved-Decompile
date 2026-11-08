using UnityEngine;
using UnityEngine.AI;

// Inside PlankPlaced (the 3rd floor plank once it's put down): while the plank is there, Granny and Grandpa can walk across
// it. The far side's nav mesh is the "Plank Far Side NavMesh" surface. Set up by Tools > Evolved Restoration > Set Up Plank Crossing.
public class PlankNavLink : MonoBehaviour
{
	public Vector3 Start, End; // world positions on the nav mesh at each end of the plank
	public float Width = .4f;

	NavMeshLinkInstance link;

	void OnEnable()
	{
		link = NavMesh.AddLink(new NavMeshLinkData
		{
			startPosition = Start,
			endPosition = End,
			width = Width,
			bidirectional = true,
			costModifier = -1f,
			area = 0,
			agentTypeID = 0,
		});
		if (link.valid) link.owner = this;
	}

	void OnDisable()
	{
		if (link.valid) NavMesh.RemoveLink(link);
	}
}
