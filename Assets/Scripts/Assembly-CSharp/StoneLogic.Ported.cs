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

// Ported from Annotated-Pseudocode/StoneLogic.c (Granny 3 Evolved, IL2CPP x64).
public partial class StoneLogic
{
	    private void Start()
	    {
	        EnemyControl = GameObject.Find("EnemyController"); PS = GameObject.Find("PlayerManage");
	        StartCoroutine(TimerEndOffAbility()); smoke.transform.parent = null;
	    }
	    private int SlingshotType()
	    {
	        if (PlayerPrefs.GetInt("Slingshot_MagentaON") == 1) return 3;
	        if (PlayerPrefs.GetInt("Slingshot_LavaON") == 1) return 1;
	        if (PlayerPrefs.GetInt("Slingshot_IceON") == 1) return 2;
	        return 0;
	    }
	    private void OnTriggerEnter(Collider hit)
	    {
	        if (!canHurtGeneral) { Destroy(gameObject); return; }
	        GameObject target = hit.gameObject;
	        if (target.CompareTag("BearTrapActivated")) hit.GetComponent<BearTrapLogic>().ShootGun();
	        if (target.name == "SubwayCrowbarRoomWindow")
	        {
	            var window = GameObject.Find("WindowLogic").GetComponent<WindowJumping>();
	            window.windowOld.SetActive(false); window.windowBroken.SetActive(true);
	        }
	        if (target.name == "GrandpaHitBody")
	        {
	            canHurtGeneral = false;
	            var ec = EnemyControl.GetComponent<EnemyController>();
	            if (!ec.GrandpaKilled && !ec.Grandpa.GetComponent<AI_Grandpa>().CaughtPlayer) ec.GrandpaDeathSBed(SlingshotType(), true);
	        }
	        if (target.CompareTag("granpa"))
	        {
	            canHurtGeneral = false;
	            var ec = EnemyControl.GetComponent<EnemyController>();
	            var ai = ec.Grandpa.GetComponent<AI_Grandpa>();
	            if (target.name != "head") ai.HitWithStone(true);
	            else if (!ec.GrandpaKilled && !ai.CaughtPlayer) ec.GrandpaDeathS(SlingshotType(), true);
	        }
	        if (target.CompareTag("granny") && !canHurtPlayer)
	        {
	            canHurtGeneral = false;
	            var ec = EnemyControl.GetComponent<EnemyController>();
	            var ai = ec.Granny.GetComponent<AI_Granny>();
	            if (target.name != "Bip01 Head") ai.HitWithStone(true);
	            else if (!ec.GrannyKilled && !ai.CaughtPlayer) ec.GrannyDeathS(SlingshotType(), true);
	        }
	        if (target.name == "Subway_TicketRobot" || target.name == "Subway_TicketRobotHead")
	        {
	            var bot = GameObject.Find("SubwayRobot").GetComponent<TicketControlBot>();
	            if (!bot.dead) bot.SilentButDeadly();
	        }
	        if (target.name == "Facility_TicketRobot" || target.name == "Facility_TicketRobotHead")
	        {
	            var bot = GameObject.Find("FacilityRobot").GetComponent<FacilityBot>();
	            if (!bot.dead) GameObject.Find("PickRay").GetComponent<PickRay>().TimedTextInitiate("This robot is too strong. I might need something else");
	        }
	        if (target.name == "DoorActivatorAttached")
	        {
	            var bot = GameObject.Find("SubwayRobot").GetComponent<TicketControlBot>();
	            if (!bot.dead) bot.DeathUponRobot();
	            target.name = "DoorActivatorDetached";
	        }
	        if (target.name == "CrowNest" || target.name == "ColliderBirdNest")
	            GameObject.Find("CrowNest").GetComponent<CrowChimney>().HurtBird();
	        var body = hit.GetComponent<Rigidbody>();
	        if (target.name == "VaseGrannyTop" && body) body.mass = 13f;
	        if (target.name == "VaseGrandpaTop" && body) body.mass = 12f;
	        if (target.name == "BlockShopPlank" || target.name == "ImpossiblePlank")
	        {
	            bool impossible = target.name == "ImpossiblePlank";
	            target.GetComponent<AudioSource>().Play(); body.isKinematic = false;
	            target.layer = LayerMask.NameToLayer("Item");
	            target.name = impossible ? "PlankBroke" : "BlockShopPlankBroke";
	            if (impossible) hit.GetComponent<DestroyTimer>().enabled = true;
	            Destroy(hit.GetComponent<RegPuzzle>());
	        }
	        if (target.name == "PlankRoofSupport") target.GetComponent<PlankRoof>().BreakUp();
	        if (target.layer == LayerMask.NameToLayer("Item"))
	        {
	            if (target.name != "VaseGrandpaTop" && target.name != "VaseGrannyTop" && body) body.mass = 34f;
	            var data = target.GetComponent<ItemSeedData>();
	            if (data && body && (data.itemName == "PropaneTank" || data.itemName == "BalloonPack1"
	                || data.itemName == "BalloonPack2" || data.itemName == "BalloonPack3")) body.mass = 42f;
	        }
	        if ((target.CompareTag("Player") || target.name == "Main Camera") && canHurtPlayer)
	        {
	            var status = PS.GetComponent<PlayerStatus>();
	            var fall = status.Player.GetComponent<FallingHolder>();
	            var window = GameObject.Find("WindowLogic").GetComponent<WindowJumping>();
	            if (!status.Killed && !fall.isFalling && !fall.isLanding && !window.IsJumping) fall.HandleLanding(true);
	        }
	        // The original uses the struck object's world position as its impulse vector.
	        if (body) body.AddForce(hit.transform.position * 8f, ForceMode.Impulse);
	        Destroy(gameObject);
	    }
	    private IEnumerator TimerEndOffAbility()
	    {
	        yield return new WaitForSeconds(.15f);
	        canHurtGeneral = false;
	        yield return new WaitForSeconds(7f);
	        Destroy(gameObject);
	    }
}
