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

// Ported from Annotated-Pseudocode/Days.c (Granny 3 Evolved, IL2CPP x64).
public partial class Days
{
	    private void Start()
	    {
	        StartCoroutine(StartDays());
	        ConfigureRun();
	    }
	    private static void Active(GameObject obj, bool active) { if (obj) obj.SetActive(active); }
	    private static void ActiveAll(GameObject[] objects, bool active)
	    { if (objects != null) foreach (var obj in objects) Active(obj, active); }
	    private void ConfigureRun()
	    {
	        ActiveAll(DeactivatedOnStart, false);
	        if (ShadowsOnObjs != null) foreach (var renderer in ShadowsOnObjs)
	            if (renderer) renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	        int difficulty = DifficultyLevel.Rules;
	        Active(ImpossibleFeature, false);
	        switch (difficulty)
	        {
	            case 0: Active(elevatorVariableEasy, true); Active(NormalFeature, false); break;
	            case 1: Active(NormalFeature, true); break;
	            case 2: Active(NormalFeature, true); Active(HardPlusFeature, true); break;
	            case 3: Active(NormalFeature, true); Active(HardPlusFeature, true); Active(ammoOffExtreme, false); break;
	            case 4:
	                Active(NormalFeature, false); Active(HardPlusFeature, true); Active(ImpossibleFeature, true);
	                ActiveAll(AmmoDisableImpossible, false);
	                if (lockPickedDoorsImpossible != null) foreach (var door in lockPickedDoorsImpossible)
	                    if (door) { door.Locked = true; door.LockpickAvailable = true; Active(door.lockPickCollider, true); }
	                Active(ammoOffExtreme, false); Active(ammoOffImpossible, false);
	                if (offFlameColliderImpossible) offFlameColliderImpossible.enabled = false;
	                Active(padlockLockBalloon, true);
	                if (electricSwitches != null) foreach (var obj in electricSwitches)
	                {
	                    if (!obj) continue;
	                    var interactive = obj.GetComponent<Interactive>(); if (interactive) interactive.enabled = false;
	                    var puzzle = obj.GetComponent<RegPuzzle>(); if (puzzle) puzzle.enabled = true;
	                }
	                break;
	        }
	        bool nightmare = PlayerPrefs.GetInt("Textures") == 1;
	        bool dark = difficulty == 4 || difficulty == 5 || PlayerPrefs.GetInt("Dark") == 1 || PlayerPrefs.GetInt("FlashLightIsOn") == 1;
	        RenderSettings.skybox = dark ? null : nightmare ? NM : Normal;
	        RenderSettings.fog = dark;
	        if (dark) { RenderSettings.fogStartDistance = 2.51f; RenderSettings.fogEndDistance = 11.1f; }
	        if (nightmare)
	        {
	            if (IGSource) IGSource.clip = BGNM;
	            if (OutSource) { OutSource.clip = bgOutsideNM; OutSource.Play(); }
	            if (HuntSource) HuntSource.clip = HuntDefaultNM;
	            if (enemySee) enemySee.clip = enemySeeNM;
	            Active(NMStuff, true);
	            if (LightsAffected != null) foreach (var light in LightsAffected) if (light) light.color = Color.red;
	            if (lockPickUI != null) foreach (var image in lockPickUI) if (image) image.sprite = nmLockPickUISprite;
	            if (guillotineMesh) guillotineMesh.sharedMaterial = guillotineNM;
	            if (paper) paper.sprite = paperNM;
	            if (ticket) ticket.sprite = ticketNM;
	            if (fogs != null) foreach (var renderer in fogs) if (renderer) renderer.sharedMaterial = NMFog;
	            if (alligatorWater)
	            {
	                alligatorWater.material.mainTexture = alligatorNM;
	                alligatorWater.material.SetTexture("_EmissionMap", alligatorNM);
	                alligatorWater.material.SetColor("_EmissionColor", alligatorRedColor);
	            }
	            if (underwaterEffects) { var main = underwaterEffects.main; main.startColor = bloodyWaterColor; }
	            if (waterSubmarineEntrance) waterSubmarineEntrance.sharedMaterial = newNMMaterialWater;
	        }
	        if (water) water.SetTexture("_EmissionMap", nightmare ? waterNM : waterNo);
	        if (crow) crow.SetTexture("_EmissionMap", nightmare ? crowNM : crowNo);
	        if (buttons != null) foreach (var material in buttons) if (material) material.color = nightmare ? NMColor : normalColor;
	        ApplyTexture(hotAirBalloonMat, nightmare ? NMHotAir : normalHotAir);
	        ApplyTexture(underwaterSub, nightmare ? NMSub : normalSub);
	        if (PlayerPrefs.GetInt("RealtimeLights") == 0 && LightsAffected != null)
	            foreach (var light in LightsAffected) if (light) light.gameObject.SetActive(false);
	        if (PlayerPrefs.GetInt("FogEnabled") == 1) Active(groundFog, true);
	        Sprite stone = null;
	        GameObject grannyPrison = null, grandpaPrison = null;
	        Texture2D slingshot = nightmare ? slingNM : slingNormal;
	        string stoneText = null;
	        if (PlayerPrefs.GetInt("Slingshot_MagentaON") == 1)
	        { stone = stonePower; slingshot = slingMagenta; stoneText = "Powerful stone"; grannyPrison = PowerPrison_Granny; grandpaPrison = PowerPrison_Grandpa; }
	        else if (PlayerPrefs.GetInt("Slingshot_LavaON") == 1)
	        { stone = stoneLava; slingshot = slingLava; stoneText = "Lava stone"; grannyPrison = LavaPrison_Granny; grandpaPrison = LavaPrison_Grandpa; }
	        else if (PlayerPrefs.GetInt("Slingshot_IceON") == 1)
	        { stone = stoneIce; slingshot = slingIce; stoneText = "Icy stone"; grannyPrison = IcePrison_Granny; grandpaPrison = IcePrison_Grandpa; }
	        if (slingShot) slingShot.mainTexture = slingshot;
	        if (stoneText != null)
	        {
	            if (stoneImages != null) foreach (var image in stoneImages) if (image) image.sprite = stone;
	            if (RayPick) RayPick.stoneTextHighlight = stoneText;
	            if (AIGranny) AIGranny.dizzyEffect = grannyPrison;
	            if (AIGrandpa) AIGrandpa.dizzyEffect = grandpaPrison;
	        }
	    }
	    private static void ApplyTexture(Material material, Texture texture)
	    { if (material) { material.mainTexture = texture; material.SetTexture("_EmissionMap", texture); } }
	    private Image[] DayImages() { return new[] { Text_Day1, Text_Day2, Text_Day3, Text_Day4, Text_Day5Final }; }
	    private void Update()
	    {
	        ValueFade = Mathf.Clamp01(ValueFade + (BeganDay ? 2f : -2f) * Time.deltaTime);
	        foreach (var image in DayImages()) if (image) image.color = new Color(1, 1, 1, ValueFade);
	        if (WakingUp && !Paused.IsPaused && Input.GetKeyDown(KeyCode.B)) Time.timeScale = 15f;
	    }
	    public void StartNew()
	    {
	        if (Player)
	        {
	            var fps = Player.GetComponent<MobileFPS>(); if (fps) fps.SpeedFade = 1.5f;
	        }
	        if (CurrentDay == (ExtraDay ? 3f : 2f)) // only 2 days
	        {
	            if (GOvers) GOvers.GameOverCall();
	            if (!ExtraDay) Active(Player, false);
	            return;
	        }
	        CurrentDay += 1; WakingUp = true;
	        StartCoroutine(StartDays());
	    }
	    public IEnumerator StartDays()
	    {
	        yield return new WaitForSeconds(2f);
	        Cursor.visible = false;
	        if (CurrentDay == 0) CurrentDay = 1;
	        int day = Mathf.RoundToInt(CurrentDay);
	        if (day < 1 || day > 5) yield break;
	        if (day > 1) ResetDay(day);
	        yield return new WaitForSeconds(3f);
	        if (DayTextPopup) DayTextPopup.Play();
	        // Only 2 days (3 with the extra day): the last one shows the "Last day" banner.
	        Image dayImage = day == (ExtraDay ? 3 : 2) ? Text_Day5Final : DayImages()[day - 1];
	        if (dayImage) dayImage.gameObject.SetActive(true);
	        BeganDay = true;
	        yield return new WaitForSeconds(2f);
	        if (day == 1) Active(NMStuff2, false);
	        BeganDay = false;
	        if (IGSource) IGSource.gameObject.SetActive(true);
	        if (CamAnimRise) CamAnimRise.Play("PlayerRiseNy");
	        if (BlackScreen) BlackScreen.CrossFadeAlpha(0, 2.5f, false);
	        yield return new WaitForSeconds(3.1f);
	        if (OutSource) OutSource.gameObject.SetActive(true);
	        if (dayImage) dayImage.gameObject.SetActive(false);
	        Active(Player, true); Active(UIElements, true); Active(PauseManager, true);
	        if (day > 1) Active(Texts, true);
	        if (day == 2 && RayPick && RayPick.T_NeedShit)
	        {
	            var hint = RayPick.T_NeedShit; hint.SetActive(true);
	            var timer = hint.GetComponent<TextManager>(); if (timer) timer.Add();
	            var label = hint.GetComponent<TMP_Text>(); if (label) label.text = "I have to escape this place";
	        }
	        if (Player)
	        {
	            var fps = Player.GetComponent<MobileFPS>();
	            if (fps)
	            {
	                fps.isAllowedToMove = true; fps.AbleToMove = true;
	                if (day > 1) { fps.IsCrouched = false; fps.CamK = true; }
	            }
	            if (day > 1)
	            {
	                var crouch = Player.GetComponent<CrouchHolder>();
	                if (crouch) { crouch.Starter = true; crouch.Disabled = false; }
	            }
	        }
	        DisableCaught(); SpawnEnemies(day);
	        Time.timeScale = 1;
	        if (EC) EC.SlendrinaSpawn();
	        WakingUp = false;
	    }
	    private void ResetDay(int day)
	    {
	        if (day == 4) { Active(BedBlood, true); Active(Day4Blood, true); }
	        ActiveAll(BCams, false); Active(UIElements, false);
	        if (EC)
	        {
	            if (EC.SpawnCheck) { EC.SpawnCheck.Floor2 = false; EC.SpawnCheck.Floor3 = false; EC.SpawnCheck.Cellar = true; EC.SpawnCheck.Floor1 = false; }
	            EC.GrannyKilled = false;
	            if (EC.GrannyGoneText) EC.GrannyGoneText.gameObject.SetActive(false);
	            if (EC.GrandpaGoneText) EC.GrandpaGoneText.gameObject.SetActive(false);
	        }
	        KillRagdolls(); DoorsClose();
	        if (Furni) Furni.CleanUp();
	        NoiseObjectsKilled();
	        if (RemoveRay) RemoveRay.Trapped = false;
	        CamFOVReset();
	        if (ScreenEffects) { ScreenEffects.Hurt = false; ScreenEffects.Blood = 0; if (ScreenEffects.BloodScreenHit) ScreenEffects.BloodScreenHit.gameObject.SetActive(false); }
	        if (EC) EC.EnemyRearrange();
	        if (BGManager) { BGManager.InHouse = true; BGManager.InOutside = false; BGManager.InFacility = false; if (BGManager.HouseMusic) BGManager.HouseMusic.gameObject.SetActive(true); }
	    }
	    private void SpawnEnemies(int day)
	    {
	        if (!EC) return;
	        int difficulty = DifficultyLevel.Rules;
	        if (day == 1 && (difficulty == 3 || difficulty == 4))
	        { PlayerPrefs.SetInt("Granny", 1); PlayerPrefs.SetInt("Grandpa", 1); EC.SpawnGrandpa(); EC.SpawnGranny(); }
	        else if (PlayerPrefs.GetInt("Granny") == 1)
	        { EC.SpawnGranny(); if (day == 1) PlayerPrefs.SetInt("Granny", 1); }
	        if (PlayerPrefs.GetInt("Grandpa") == 1)
	        { EC.SpawnGrandpa(); if (day == 1) PlayerPrefs.SetInt("Grandpa", 1); }
	        Active(EC.ShotgunGrandpa, true);
	    }
	    public void DoorsClose()
	    {
	        if (Doors != null) foreach (var door in Doors)
	        {
	            if (!door) continue;
	            if (!door.Closed && !door.Locked) door.CloseAction();
	            if (door.LockpickAvailable) { door.Locked = true; Active(door.lockPickCollider, true); }
	        }
	        if (AnimatedOnes != null) foreach (var animated in AnimatedOnes)
	            if (animated && !animated.Closed && !animated.Locked) animated.CloseAction();
	    }
	    public void NoiseObjectsKilled()
	    {
	        foreach (string tag in new[] { "noiseobject", "noiseobject_grandpa", "BearTrapActivated" })
	            foreach (var obj in GameObject.FindGameObjectsWithTag(tag)) Destroy(obj);
	    }
	    public void KillRagdolls()
	    {
	        foreach (var obj in GameObject.FindGameObjectsWithTag("shotgun")) Destroy(obj);
	        if (PlayerPrefs.GetInt("GrannyWP") == 1)
	        {
	            var slingshot = GameObject.Find("Slingshot(Clone)"); if (slingshot) Destroy(slingshot);
	            if (EC && EC.Granny) Active(EC.Granny.GetComponent<AI_Granny>().slingshotWP, true);
	        }
	        foreach (string name in new[] { "GrannyRagdoll(Clone)", "GrandpaRagdoll(Clone)", "GrandpaFreezeDoll(Clone)", "GrannyFreezeDoll(Clone)", "GrandpaCrystalBody(Clone)", "GrannyCrystalBodyClone)" })
	        { var obj = GameObject.Find(name); if (obj) Destroy(obj); }
	    }
	    public void DisableCaught()
	    {
	        if (!EC) return;
	        if (EC.Granny) EC.Granny.GetComponent<AI_Granny>().CaughtPlayer = false;
	        if (EC.Grandpa) EC.Grandpa.GetComponent<AI_Grandpa>().CaughtPlayer = false;
	    }
	    private void CamFOVReset()
	    { if (CamN) { var camera = CamN.GetComponent<Camera>(); if (camera) camera.fieldOfView = 60 + PlayerPrefs.GetInt("FOV") * 5; } }
	    public void PlayerBackInTheProperSpot()
	    {
	        if (GroundC) GroundC.transform.localPosition = new Vector3(0, -.614f, 0);
	        if (ShakeCam)
	        { ShakeCam.transform.localPosition = new Vector3(0, 1.235f, 0); var reset = ShakeCam.GetComponent<ShakeReset>(); if (reset) reset.ResetLogic(); }
	        if (!Player) return;
	        var controller = Player.GetComponent<CharacterController>();
	        bool wasEnabled = controller && controller.enabled;
	        if (controller) controller.enabled = false;
	        if (PositionPlayerBack) Player.transform.SetPositionAndRotation(PositionPlayerBack.position, PositionPlayerBack.rotation);
	        if (controller) { controller.center = new Vector3(0, .47f, 0); controller.height = 2.45f; controller.enabled = wasEnabled; }
	        var crouch = Player.GetComponent<CrouchHolder>();
	        if (crouch) { crouch.IsCrouched = false; crouch.isCrouching = false; crouch.Starter = false; crouch.Disabled = true; }
	        var fps = Player.GetComponent<MobileFPS>();
	        if (fps) { fps.InWeb = false; fps.ResetRotation(); fps.isAllowedToMove = false; fps.IsCrouched = false; fps.AbleToMove = false; fps.CamK = false; }
	    }
}
