using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/AI_Slendrina.c (Granny 3 Evolved, IL2CPP x64).
public partial class AI_Slendrina
{
		public AI_Slendrina()
		{
		    normalExtent = new Vector3(.01f, .035693f, .001f);
	        easierExtent = new Vector3(.01f, -.02f, .001f);
	        normalCenter = new Vector3(-.000118719f, .0350507f, -.000797565f);
	        easierCenter = new Vector3(-.000118719f, .046f, -.000797565f);
	        canAttack = true; lookAtSpeed = 15f;
	        NearDis = 25; FarDis = 50; Attemps = 30; NavMeshAreaMask = 1; Health = 100; TimerMaxIdle = 10; CloseDistance = 7;
		    SpeedBorderFade = 1.5f; SpeedLookingFade = 1.5f; SpeedBorderFadeGo = .5f; SpeedLookingFadeGo = 1;
		}
		private void Start()
		{
		    if (!Travel) Agent.enabled = false;
		    switch (PlayerPrefs.GetInt("Diff"))
		    {
		        case 0: DamageSpeed = 55; break;
		        case 1: DamageSpeed = 75; break;
		        case 2: DamageSpeed = 85; break;
		        case 3: DamageSpeed = 105; break;
		        case 4: DamageSpeed = 125; break;

		    }
		}
		private void SendHome()
		{
		    GetComponent<Rigidbody>().isKinematic = true;
		    SlendrinaTouchesPlayer = false;
		    Agent.enabled = false;
		    transform.position = StartPos.position;
		    PlayerCaughtAttention = false;
		    PathingToPlayer = false;
		}
	    private void Update()
	    {
	        bool hidden = PS.HidingUnderThings || PS.HidingInClosets || PS.InBed1 || PS.InBed2 || PS.InBed3;
	        bool blocked = BGM.InFacility || Days.WakingUp || PS.IsJumpscared || Fall.isFalling || Fall.isLanding
	            || prisonerDoor.Locked || PS.Killed || !PlayerTransform.gameObject.activeSelf || Travel || hidden;
	        if (!PathingToPlayer && canAttack && !blocked)
	        {
	            if (!PlayerCaughtAttention) TimerTeleport += Time.deltaTime;
	            if (TimerMaxTeleport <= TimerTeleport)
	            {
	                PathingToPlayer = true; TimerTeleport = 0; TimerIdle = 0;
	                TimerMaxTeleport = UnityEngine.Random.Range(11f, 20f);
	                Teleport();
	            }
	        }
	        else if ((canAttack && PS.HidingUnderThings) || Days.WakingUp || !PlayerTransform.gameObject.activeSelf
	            || PS.IsJumpscared || PS.Killed || PS.HidingInClosets || PS.InBed1 || PS.InBed2 || PS.InBed3 || BGM.InFacility)
	            SendHome();
	        else if (PathingToPlayer && canAttack)
	        {
	            if (!PlayerCaughtAttention) TimerIdle += Time.deltaTime;
	            if (TimerMaxIdle <= TimerIdle) { PathingToPlayer = false; TimerIdle = 0; SendHome(); }
	        }
		    LookRayCast.LookAt(HeadPL.position);
		    LookRayCast2.LookAt(HeadPL.position);
		    LookRayCast3.LookAt(HeadPL.position);
		    if (Vector3.Distance(transform.position, PlayerTransform.position) <= TeleportAwayDistance && !PlayerCaughtAttention && PathingToPlayer) TimerIdle = 100;
		    RaycastHit hit;
		    bool seen = Physics.Raycast(LookRayCast.position, LookRayCast.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayersRay)
		        || Physics.Raycast(LookRayCast2.position, LookRayCast2.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayersRay)
		        || Physics.Raycast(LookRayCast3.position, LookRayCast3.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayersRay);
		    SlendrinaTouchesPlayer = seen && hit.collider.gameObject.CompareTag("Player");
		    IsRenderingSlendrina = CameraRenderSlendrina(SlendrinaMesh, CameraPlayer);
		    BloodScreen.color = new Color(1f, 0f, 0f, BorderBloodValue);
		    Vector3 toPlayer = PlayerTransform.position - transform.position; toPlayer.y = 0;
		    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toPlayer), Time.deltaTime * lookAtSpeed);
		    var skinned = SlendrinaMesh.GetComponent<SkinnedMeshRenderer>();
		    bool looking = canAttack && !BGM.InFacility && SlendrinaTouchesPlayer && IsRenderingSlendrina && !PS.Killed && PathingToPlayer && !PS.IsJumpscared && !Fall.isFalling && !Fall.isLanding;
		    if (!looking)
		    {
		        if (ShakeAnim.IsPlaying("LookAtSlendrina")) ShakeAnim.Play("IdleShake");
		        SlendrinaHunts.volume -= Time.deltaTime * SpeedLookingFadeGo;
		        if (0 < BorderBloodValue) BorderBloodValue -= Time.deltaTime * SpeedBorderFadeGo;
		        if (PlayerCaughtAttention && canAttack) SendHome();
		        Health = 100;
		        // Back to the normal detection bounds.
		        if (skinned && (skinned.bounds.center != normalCenter || skinned.bounds.extents != normalExtent))
		            skinned.localBounds = new Bounds { center = normalCenter, extents = normalExtent };
		    }
		    else
		    {
		        SlendrinaHunts.volume += Time.deltaTime * SpeedLookingFade;
		        Health -= Time.deltaTime * DamageSpeed;
		        ShakeAnim.CrossFade("LookAtSlendrina");
		        if (Health <= 0) PS.NormalDeath(false);
		        if (BorderBloodValue < 1) BorderBloodValue += Time.deltaTime * SpeedBorderFade;
		        Agent.speed = 0;
		        if (!FPS.CamK)
		        {
		            // The camera is pulled toward her face.
		            var pivot = CamPivot.transform;
		            Vector3 dir = Vector3.RotateTowards(pivot.forward, LookAtPoint.position - pivot.position, Time.deltaTime * 10, 0);
		            PlayerTransform.rotation = Quaternion.Euler(0, pivot.rotation.eulerAngles.y, 0);
		            pivot.rotation = Quaternion.LookRotation(dir);
		        }
		        if (!PlayerCaughtAttention)
		        {
		            PlayerCaughtAttention = true;
		            if (PlayerTransform.gameObject.activeSelf) FPS.DragHead(.35f);
		            float d = Vector3.Distance(transform.position, PlayerTransform.position);
		            SawSource.PlayOneShot(d <= CloseDistance ? StareNear : StareFar);
		        }
		        // With the keyboard camera she is easier to keep in view.
		        if (FPS.CamK && skinned) skinned.localBounds = new Bounds { center = easierCenter, extents = easierExtent };
		    }
		    if (IsRenderingSlendrina && !SlendrinaTouchesPlayer) Agent.speed = 0;
		    if (PathingToPlayer && Travel) Agent.SetDestination(PlayerTransform.position);
		}
		private bool CameraRenderSlendrina(Renderer MeshE, Camera Player)
		{
		    return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Player), MeshE.bounds);
		}
		public void EnemySpawn()
		{
		    Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		    TimerTeleport = 0;
		    if (ShakeAnim.transform.eulerAngles != Vector3.zero) ShakeAnim.transform.localEulerAngles = Vector3.zero;
		}
		public void ReturnIdle()
		{
		    var body = GetComponent<Rigidbody>();
		    if (!canAttack || !body) return;
		    SendHome();
		}
		public void KeepDistanceManual(float desiredDistance)
		{
		    Vector3 target = PlayerTransform.position - PlayerTransform.forward * desiredDistance;
	        target.y = PlayerTransform.position.y;
		    NavMeshHit hit;
		    if (NavMesh.SamplePosition(target, out hit, .8f, NavMesh.AllAreas)) transform.position = hit.position;
		}
		// She appears at the spot behind the player, but only if it really is behind them.
		public void Teleport()
		{
	        if (!canAttack) return;
		    if (Travel) { Agent.enabled = false; Agent.enabled = true; return; }
		    NavMeshHit hit;
		    if (NavMesh.SamplePosition(SPPlayerBack.position, out hit, 1, NavMesh.AllAreas)
		        && Vector3.Dot((hit.position - PlayerTransform.position).normalized, PlayerTransform.forward) < 0)
		    {
		        Agent.enabled = false;
		        SlendrinaTransform.position = hit.position;
		        Agent.Warp(hit.position);
		        Agent.enabled = true;
		        return;
		    }
		    PathingToPlayer = false;
		}

	    public void DisappearLogic()
	    {
	        Agent.enabled = false;
	        transform.position = StartPos.position;
	    }
}
