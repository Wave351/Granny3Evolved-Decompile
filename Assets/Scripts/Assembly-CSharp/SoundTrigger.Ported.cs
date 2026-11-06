using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/SoundTrigger.c (Granny 3 Evolved, IL2CPP x64).
public partial class SoundTrigger
{
		public SoundTrigger() { canTeleportPoint = true; }
		private void Update()
		{
			if (SpawnBack == null) SpawnBack = GameObject.Find("ObjectResetPoint");
			if (SpawnBackSewer == null) SpawnBackSewer = GameObject.Find("ObjectTeleportSPFromWater");
		}
		// Dropped items make noise on the floor (Granny, optionally Grandpa, comes); items that fall out of the map
		// or into water are put back at a reset point.
		private void OnTriggerEnter(Collider other)
		{
			string n = other.gameObject.name, tag = other.gameObject.tag;
			if (n == "NoNoisyPointCollider" && !HouseMaterial) { Stopped = true; return; }
			if (tag == "water" && WaterDes)
			{
				Stopped = true;
				gameObject.AddComponent<DestroyTimer>();
				var timer = GetComponent<DestroyTimer>();
				if (timer) timer.TimeW = 15f;
				return;
			}
			if (tag == "floor" && n != "NoNoisyPointCollider")
			{
				if (Stopped) return;
				Instantiate(NoiseOBJ_Granny, transform.position, transform.rotation);
				if (!OnlyOnce)
				{
					if (DestroyOnSound) gameObject.AddComponent<DestroyTimer>();
					PlayRandomSound();
				}
				else if (TimesDropped == 0)
				{
					PlayRandomSound();
					if (DestroyOnSound) gameObject.AddComponent<DestroyTimer>();
					transform.parent.gameObject.layer = LayerMask.NameToLayer("Door");
					TimesDropped = 1;
				}
				if (AlertsGrandpa) Instantiate(NoiseOBJ_Grandpa, transform.position, transform.rotation);
				return;
			}
			if (tag == "Noice")
			{
				// Landing on a noise maker: only the drop sound.
				if (Stopped) return;
				if (!OnlyOnce)
				{
					if (DestroyOnSound) gameObject.AddComponent<DestroyTimer>();
					PlayRandomSound();
					return;
				}
				if (TimesDropped != 0) return;
				PlayRandomSound();
				if (DestroyOnSound)
				{
					gameObject.AddComponent<DestroyTimer>();
					GetComponent<DestroyTimer>().TimeW = 15f;
				}
				if (transform.parent == null) return;
				transform.parent.gameObject.layer = LayerMask.NameToLayer("Door");
				TimesDropped = 1;
				return;
			}
			if (n == "ResetFloor" && canTeleportPoint)
			{
				if (Reset(SpawnBack)) FindRay()?.FellObjectDining();
				return;
			}
			if (n != "WaterCoverEntireHouse" && n != "MoatGround" && tag != "water") return;
			if (!canTeleportPoint) return;
			if (Reset(SpawnBackSewer)) FindRay()?.FellObjectMoat();
		}
		// Moves the item (this trigger's parent) to a reset point.
		private bool Reset(GameObject point)
		{
			var parent = transform.parent;
			var body = parent.GetComponent<Rigidbody>();
			if (body != null)
			{
				body.isKinematic = true;
				body.linearVelocity = Vector3.zero;
			}
			if (point == null || parent == null) return false;
			parent.position = point.transform.position;
			if (body != null) body.isKinematic = false;
			return true;
		}
		private static PickRay FindRay()
		{
			var rayObject = GameObject.Find("PickRay");
			return rayObject != null ? rayObject.GetComponent<PickRay>() : null;
		}
		// The drop sound plays from a separate object so it survives the item being picked up.
		public void PlayRandomSound()
		{
			if (Sounds == null || Sounds.Length == 0) return;
			int i = UnityEngine.Random.Range(0, Sounds.Length);
			if (!transform.parent) return;
			var go = new GameObject("SoundDrop" + transform.parent.name);
			go.transform.position = transform.position;
			var source = go.AddComponent<AudioSource>();
			source.clip = Sounds[i];
			var mine = GetComponent<AudioSource>();
			if (!mine) return;
			source.volume = mine.volume; source.pitch = mine.pitch; source.spatialBlend = mine.spatialBlend;
			source.minDistance = mine.minDistance; source.maxDistance = mine.maxDistance;
			var destruct = go.AddComponent<SoundSelfDestruct>();
			destruct.Audio = source;
			source.Play();
			destruct.AbleToDestruct = true;
		}
}
