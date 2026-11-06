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

// Ported from Annotated-Pseudocode/AI_Slender.c (Granny 3 Evolved, IL2CPP x64).
public partial class AI_Slender
{
	    public AI_Slender()
	    {
	        NearDis = 25; FarDis = 50; Health = 100; TimerMaxIdle = 10; canAttack = true;
	        lookAtSpeed = 15; CloseDistance = 7; SpeedBorderFade = 1.5f; SpeedLookingFade = 1.5f;
	        SpeedBorderFadeGo = .5f; SpeedLookingFadeGo = 1f;
	    }
	    private void Start()
	    {
	        switch (PlayerPrefs.GetInt("Diff"))
	        {
	            case 0: DamageSpeed = 25; break;
	            case 1: DamageSpeed = 45; break;
	            case 2: DamageSpeed = 65; break;
	            case 3: DamageSpeed = 75; break;
	            case 4: DamageSpeed = 85; break;
	        }
	        BloodScreen.gameObject.SetActive(true);
	    }
	    private void SendHome()
	    {
	        SlenderTouchesPlayer = false;
	        transform.position = StartPos.position;
	        PlayerCaughtAttention = false; PathingToPlayer = false;
	    }
	    private void Update()
	    {
	        bool hidden = PS.HidingUnderThings || PS.HidingInClosets || PS.InBed1 || PS.InBed2 || PS.InBed3;
	        if (!PathingToPlayer && canAttack && BGM.InFacility && !PS.IsJumpscared && !Fall.isFalling && !Fall.isLanding
	            && !PS.Killed && PlayerTransform.gameObject.activeSelf && !hidden)
	        {
	            if (!PlayerCaughtAttention) TimerTeleport += Time.deltaTime;
	            if (TimerMaxTeleport <= TimerTeleport)
	            {
	                PathingToPlayer = true; TimerTeleport = 0; TimerIdle = 0;
	                TimerMaxTeleport = UnityEngine.Random.Range(5f, 12.5f);
	                Teleport();
	            }
	        }
	        else if ((canAttack && PS.HidingUnderThings) || !PlayerTransform.gameObject.activeSelf || PS.IsJumpscared
	            || PS.Killed || PS.HidingInClosets || PS.InBed1 || PS.InBed2 || PS.InBed3 || !BGM.InFacility)
	            SendHome();
	        else if (PathingToPlayer && canAttack)
	        {
	            if (!PlayerCaughtAttention) TimerIdle += Time.deltaTime;
	            if (TimerMaxIdle <= TimerIdle) { PathingToPlayer = false; TimerIdle = 0; SendHome(); }
	        }
	        LookRayCast.LookAt(CamPivot.transform.position);
	        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
	        if (distance <= TeleportAwayDistance && !PlayerCaughtAttention && PathingToPlayer) TimerIdle = 100;
	        RaycastHit hit;
	        SlenderTouchesPlayer = Physics.Raycast(LookRayCast.position, LookRayCast.forward, out hit, Mathf.Infinity, LayersRay)
	            && hit.collider.gameObject.CompareTag("Player");
	        IsRenderingSlender = CameraRenderSlendrina(SlenderMesh, CameraPlayer);
	        BloodScreen.color = new Color(.737f, .79f, .81f, BorderBloodValue);
	        bool looking = canAttack && SlenderTouchesPlayer && IsRenderingSlender && !PS.Killed && PathingToPlayer
	            && !PS.IsJumpscared && !Fall.isFalling && !Fall.isLanding && BGM.InFacility;
	        if (looking)
	        {
	            SlenderHunts.volume += Time.deltaTime * SpeedLookingFade;
	            Health -= Time.deltaTime * DamageSpeed;
	            if (Health <= 0) PS.NormalDeath(false);
	            if (BorderBloodValue < 1) BorderBloodValue += Time.deltaTime * SpeedBorderFade;
	            if (!PS.Player.GetComponent<MobileFPS>().CamK)
	            {
	                Transform pivot = CamPivot.transform;
	                Vector3 dir = Vector3.RotateTowards(pivot.forward, LookAtPoint.position - pivot.position, Time.deltaTime * 10, 0);
	                PlayerTransform.rotation = Quaternion.Euler(0, pivot.rotation.eulerAngles.y, 0);
	                pivot.rotation = Quaternion.LookRotation(dir);
	            }
	            if (!PlayerCaughtAttention)
	            {
	                PlayerCaughtAttention = true;
	                StartCoroutine(screenDistort());
	                if (PlayerTransform.gameObject.activeSelf) FPS.DragHead(.85f);
	                SawSource.PlayOneShot(distance <= CloseDistance ? StareNear : StareFar);
	            }
	        }
	        else
	        {
	            SlenderHunts.volume -= Time.deltaTime * SpeedLookingFadeGo;
	            if (BorderBloodValue > 0) BorderBloodValue -= Time.deltaTime * SpeedBorderFadeGo;
	            if (PlayerCaughtAttention && canAttack) { FPS.ForcefulCameraAvailability(); SendHome(); }
	            Health = 100;
	        }
	    }
	    private bool CameraRenderSlendrina(Renderer MeshE, Camera Player)
	    { return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Player), MeshE.bounds); }
	    public void EnemySpawn() { TimerTeleport = 0; }
	    public void ReturnIdle() { if (!canAttack) return; FPS.ForcefulCameraAvailability(); SendHome(); }
	    public void DisappearLogic() { transform.position = StartPos.position; }
	    public void Teleport()
	    {
	        if (!canAttack) return;
	        Transform spot = locationsTeleport[UnityEngine.Random.Range(0, locationsTeleport.Length)];
	        SlenderTransform.SetPositionAndRotation(spot.position, spot.rotation);
	    }
	    private IEnumerator screenDistort()
	    {
	        yield return new WaitForSeconds(.025f);
	        for (int i = 0; i < 5; i++)
	        {
	            BloodScreen.rectTransform.localScale = new Vector3(i % 2 == 0 ? -1f : 1f, 1f, 1f);
	            yield return new WaitForSeconds(.025f);
	        }
	        BloodScreen.rectTransform.localScale = Vector3.one;
	        if (BloodScreen.color.a > 0) StartCoroutine(screenDistort());
	    }
}
