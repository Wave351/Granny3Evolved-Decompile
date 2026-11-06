using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/FurnitureUnit.c (Granny 3 Evolved, IL2CPP x64).
public partial class FurnitureUnit
{
		private void Start()
		{
			InitialPosition = transform.position;
			InitialRotation = transform.rotation;
		}
		public void CleanUpFurniture()
		{
			transform.position = InitialPosition;
			transform.rotation = InitialRotation;
			if (!WithRigidbody) gameObject.layer = LayerMask.NameToLayer("Moves");
			else
			{
				GetComponent<Rigidbody>().isKinematic = true;
				var fall = GetComponent<TriggerFall>();
				if (fall) fall.Fell = false;
			}
			if (ST.OnlyOnce) ST.TimesDropped = 0;   // the drop sound may play again
		}
}
