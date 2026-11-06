using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public partial class Outline : MonoBehaviour
{
	public enum Mode
	{
		OutlineAll = 0,
		OutlineVisible = 1,
		OutlineHidden = 2,
		OutlineAndSilhouette = 3,
		SilhouetteOnly = 4
	}
	[Serializable]
	private class ListVector3
	{
		public List<Vector3> data;
	}
	private static HashSet<Mesh> registeredMeshes;
	[SerializeField]
	private Mode outlineMode;
	[SerializeField]
	private Color outlineColor;
	[Range(0f, 10f)]
	[SerializeField]
	private float outlineWidth;
	[Header("Optional")]
	[SerializeField]
	[Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
	private bool precomputeOutline;
	[SerializeField]
	[HideInInspector]
	private List<Mesh> bakeKeys;
	[SerializeField]
	[HideInInspector]
	private List<ListVector3> bakeValues;
	private Renderer[] renderers;
	private Material outlineMaskMaterial;
	private Material outlineFillMaterial;
	private bool needsUpdate;
}
