using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Outline.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class Outline
{
	// QuickOutline v1.1 (Chris Nolet, MIT licence) as compiled into Legacy. The only Legacy change is that OnEnable skips the
	// rainbow glow renderers ("RainbowGlowRegular(Clone)", "RainbowGlowExpand(Clone)").
	public Mode OutlineMode { get { return outlineMode; } set { outlineMode = value; needsUpdate = true; } }
	public Color OutlineColor { get { return outlineColor; } set { outlineColor = value; needsUpdate = true; } }
	public float OutlineWidth { get { return outlineWidth; } set { outlineWidth = value; needsUpdate = true; } }
	public Outline()
	{
	    outlineMode = Mode.OutlineAll;
	    outlineColor = Color.white;
	    outlineWidth = 2;
	    bakeKeys = new List<Mesh>();
	    bakeValues = new List<ListVector3>();
	}
	static Outline() { registeredMeshes = new HashSet<Mesh>(); }
	private void Awake()
	{
	    renderers = GetComponentsInChildren<Renderer>();
	    outlineMaskMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineMask"));
	    outlineFillMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineFill"));
	    outlineMaskMaterial.name = "OutlineMask (Instance)";
	    outlineFillMaterial.name = "OutlineFill (Instance)";
	    LoadSmoothNormals();
	    needsUpdate = true;
	}
	private void OnEnable()
	{
	    foreach (var renderer in renderers)
	    {
	        string name = renderer.gameObject.name;
	        if (name == "RainbowGlowRegular(Clone)" || name == "RainbowGlowExpand(Clone)") continue;
	        var materials = new List<Material>(renderer.sharedMaterials);
	        materials.Add(outlineMaskMaterial);
	        materials.Add(outlineFillMaterial);
	        renderer.materials = materials.ToArray();   // Evolved sets .materials (Legacy set .sharedMaterials)
	    }
	}
	private void OnValidate()
	{
	    needsUpdate = true;
	    if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
	    {
	        bakeKeys.Clear();
	        bakeValues.Clear();
	    }
	    if (precomputeOutline && bakeKeys.Count == 0) Bake();
	}
	private void Update()
	{
	    if (!needsUpdate) return;
	    needsUpdate = false;
	    UpdateMaterialProperties();
	}
	private void OnDisable()
	{
	    foreach (var renderer in renderers)
	    {
	        if (!renderer) continue; // child renderers (e.g. finished particle effects) can already be destroyed
	        var materials = new List<Material>(renderer.sharedMaterials);
	        materials.Remove(outlineMaskMaterial);
	        materials.Remove(outlineFillMaterial);
	        renderer.materials = materials.ToArray();   // Evolved sets .materials (Legacy set .sharedMaterials)
	    }
	}
	private void OnDestroy()
	{
	    Destroy(outlineMaskMaterial);
	    Destroy(outlineFillMaterial);
	}
	private void Bake()
	{
	    var bakedMeshes = new HashSet<Mesh>();
	    foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
	    {
	        if (!bakedMeshes.Add(meshFilter.sharedMesh)) continue;
	        var smoothNormals = SmoothNormals(meshFilter.sharedMesh);
	        bakeKeys.Add(meshFilter.sharedMesh);
	        bakeValues.Add(new ListVector3 { data = smoothNormals });
	    }
	}
	private void LoadSmoothNormals()
	{
	    foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
	    {
	        if (!registeredMeshes.Add(meshFilter.sharedMesh)) continue;
	        var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
	        var smoothNormals = index >= 0 ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);
	        meshFilter.sharedMesh.SetUVs(3, smoothNormals);
	        var renderer = meshFilter.GetComponent<Renderer>();
	        if (renderer != null) CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
	    }
	    foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
	    {
	        if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)) continue;
	        skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
	        CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
	    }
	}
	private List<Vector3> SmoothNormals(Mesh mesh)
	{
	    // Vertices at the same position share an averaged normal, so the outline has no gaps at hard edges.
	    var groups = new Dictionary<Vector3, List<KeyValuePair<int, Vector3>>>();
	    var vertices = mesh.vertices;
	    for (int i = 0; i < vertices.Length; i++)
	    {
	        List<KeyValuePair<int, Vector3>> group;
	        if (!groups.TryGetValue(vertices[i], out group)) groups[vertices[i]] = group = new List<KeyValuePair<int, Vector3>>();
	        group.Add(new KeyValuePair<int, Vector3>(i, vertices[i]));
	    }
	    var smoothNormals = new List<Vector3>(mesh.normals);
	    foreach (var group in groups.Values)
	    {
	        if (group.Count == 1) continue;
	        var smoothNormal = Vector3.zero;
	        foreach (var pair in group) smoothNormal += smoothNormals[pair.Key];
	        smoothNormal.Normalize();
	        foreach (var pair in group) smoothNormals[pair.Key] = smoothNormal;
	    }
	    return smoothNormals;
	}
	private void CombineSubmeshes(Mesh mesh, Material[] materials)
	{
	    if (mesh.subMeshCount == 1) return;
	    if (mesh.subMeshCount > materials.Length) return;
	    mesh.subMeshCount++;
	    mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
	}
	private void UpdateMaterialProperties()
	{
	    outlineFillMaterial.SetColor("_OutlineColor", outlineColor);
	    switch (outlineMode)
	    {
	        case Mode.OutlineAll:
	            outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	            outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	            outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	            break;
	        case Mode.OutlineVisible:
	            outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	            outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
	            outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	            break;
	        case Mode.OutlineHidden:
	            outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	            outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
	            outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	            break;
	        case Mode.OutlineAndSilhouette:
	            outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
	            outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
	            outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
	            break;
	        case Mode.SilhouetteOnly:
	            outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
	            outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
	            outlineFillMaterial.SetFloat("_OutlineWidth", 0);
	            break;
	    }
	}
}
