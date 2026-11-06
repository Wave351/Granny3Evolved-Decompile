using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/EnvMapAnimator.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class EnvMapAnimator
{
	// TextMesh Pro example: spins the text material's environment-map reflection every frame.
	private void Awake()
	{
	    m_textMeshPro = GetComponent<TMP_Text>();
	    m_material = m_textMeshPro.fontSharedMaterial;
	}
	private IEnumerator Start()
	{
	    Matrix4x4 matrix = new Matrix4x4();
	    while (true)
	    {
	        matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * RotationSpeeds.x, Time.time * RotationSpeeds.y, Time.time * RotationSpeeds.z), Vector3.one);
	        m_material.SetMatrix("_EnvMatrix", matrix);
	        yield return null;
	    }
	}
}
