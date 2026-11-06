using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/VaseTop.c (Granny 3 Evolved, IL2CPP x64).
public partial class VaseTop
{
		public VaseTop() { otherVaseName = "VaseGrannyTop"; }
		// When the vase top lands in the matching vase it becomes a heavy, collectable item ("VaseDown").
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name != otherVaseName || didIt) return;
			other.gameObject.AddComponent<ItemSeedData>();
			var data = other.gameObject.GetComponent<ItemSeedData>();
			if (data == null) return;
			data.itemName = nameVase;
			other.gameObject.name = "VaseDown";
			var body = other.gameObject.GetComponent<Rigidbody>();
			if (body == null) return;
			body.isKinematic = false;
			body.mass = 10f;
			if (other.gameObject.name == "VaseGrannyTop") body.mass = 50f;   // never true after the rename above; kept as in the original
			other.gameObject.layer = LayerMask.NameToLayer("Item");
			foreach (var vt in vts) { if (vt == null) break; vt.didIt = true; }
		}
}
