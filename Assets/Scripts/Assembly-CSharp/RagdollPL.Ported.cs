using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/RagdollPL.c (Granny 3 Evolved, IL2CPP x64).
public partial class RagdollPL
{
		public RagdollPL()
		{
		    LookCamVal = true; ImpactDirection = Vector3.back; NeckDrag = 1.5f; DownDrag = 4;
		    GrannyForce = 2; GrandpaForce = 5; NormalForce = 3.5f; MaxBumps = 2;
		    NeckBounceForce = 3; BodyBounceForce = 2.5f; CamMoveDuration = .35f;
		}
	    private void Start()
	    {
	        ManagerPL = GameObject.Find("PlayerManage");
	        StartCoroutine(DestroyTimer());
	        var status = ManagerPL.GetComponent<PlayerStatus>();
	        var camera = status.Player.GetComponent<MobileFPS>().playerCamera.transform;
	        if (CameraPlayer && camera) CameraPlayer.transform.SetPositionAndRotation(camera.position, camera.rotation);
	        if (status.LookAtGranny) GameObject.Find("GrannyHeadPos");
	        else if (status.LookAtGrandpa) GameObject.Find("GranpaHeadPos");
	        if (InitialPositionCam && CameraPlayer) StartCoroutine(MoveCamToInitial());
	        StartCoroutine(DestroyTimer());
	        if (NeckRigidbody)
	        {
	            float force = status.LookAtGrandpa ? GrandpaForce : status.LookAtGranny ? GrannyForce : NormalForce;
	            NeckRigidbody.AddForce(-ImpactDirection.normalized * force * NeckDrag + Vector3.down * DownDrag, ForceMode.Impulse);
	        }
	    }
		public void LookAt(Transform target)
		{
		    if (!target || !RagdollParent) return;
		    Vector3 direction = target.position - RagdollParent.transform.position; direction.y = 0;
		    if (direction.sqrMagnitude > .000001f) RagdollParent.transform.rotation = Quaternion.LookRotation(direction);
		}
		private void Update() { ManagerPL = GameObject.Find("PlayerManage"); }
		private void OnTriggerEnter(Collider other)
		{
		    if (other.CompareTag("water"))
		    {
		        Stopped = true;
		        if (WaterDes) { var timer = gameObject.AddComponent<DestroyTimer>(); timer.TimeW = 15; }
		        return;
		    }
		    bool grass = other.CompareTag("grus");
		    if ((!grass && !other.CompareTag("floor") && !other.CompareTag("Noice")) || Stopped || (OnlyOnce && TimesDropped != 0)) return;
		    if (grass) { var sound = GetComponent<AudioSource>(); if (sound && GrassFalls.Length > 0) sound.PlayOneShot(GrassFalls[UnityEngine.Random.Range(0, GrassFalls.Length)]); }
		    else PlayRandomSound();
		    if (DestroyOnSound && (!OnlyOnce || !grass && other.CompareTag("Noice")))
		    { var timer = gameObject.AddComponent<DestroyTimer>(); if (other.CompareTag("Noice") && OnlyOnce) timer.TimeW = 15; }
		    if (!OnlyOnce) ImpactBodyFloor();
		    else
		    {
		        if (DestroyOnSound && (grass || other.CompareTag("floor"))) gameObject.AddComponent<DestroyTimer>();
		        if (transform.parent) transform.parent.gameObject.layer = LayerMask.NameToLayer("Door");
		        TimesDropped = 1;
		    }
		}
		private IEnumerator DestroyTimer()
		{
		    yield return new WaitForSeconds(.1f); LookCamVal = false;
		    yield return new WaitForSeconds(7);
		    if (RagdollParent) Destroy(RagdollParent);
		}
		public void PlayRandomSound()
		{
		    var sound = GetComponent<AudioSource>();
		    if (sound && Sounds != null && Sounds.Length > 0) sound.PlayOneShot(Sounds[UnityEngine.Random.Range(0, Sounds.Length)]);
		}
		public void ImpactBodyFloor()
		{
		    if (HowManyBumps >= MaxBumps || !NeckRigidbody) return;
		    NeckRigidbody.isKinematic = false; NeckRigidbody.linearVelocity = NeckRigidbody.angularVelocity = Vector3.zero;
		    Vector3 forward = RagdollParent ? RagdollParent.transform.forward : transform.forward;
		    NeckRigidbody.AddForce((forward * .2f + Vector3.up * .4f).normalized * NeckBounceForce, ForceMode.Impulse);
		    Vector3 random = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-.5f, .5f), UnityEngine.Random.Range(-1.5f, 1.5f));
		    NeckRigidbody.AddTorque(random * NeckBounceForce * .5f, ForceMode.Impulse);
		    NeckRigidbody.AddTorque(Vector3.Cross(forward, Vector3.up) * NeckBounceForce * .75f, ForceMode.Impulse);
		    HowManyBumps++;
		}
		private IEnumerator MoveCamToInitial()
		{
		    Vector3 start = CameraPlayer.transform.position;
		    for (float t = 0; t < CamMoveDuration;)
		    {
		        t += Time.deltaTime;
		        if (!CameraPlayer || !InitialPositionCam) yield break;
		        CameraPlayer.transform.position = Vector3.Lerp(start, InitialPositionCam.position, t / CamMoveDuration);
		        yield return null;
		    }
		    if (CameraPlayer && InitialPositionCam) CameraPlayer.transform.position = InitialPositionCam.position;
		}
}
