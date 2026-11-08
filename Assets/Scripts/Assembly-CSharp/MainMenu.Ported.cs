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
using UnityEngine.Rendering.PostProcessing;

// Ported from Annotated-Pseudocode/MainMenu.c (Granny 3 Evolved, IL2CPP x64).
public partial class MainMenu
{
	    private bool preferencesDirty;
	    private bool loading;
	    private int appliedQuality = -1;
	    private (Toggle control, string key)[] savedToggles;
	    private (Slider control, string key)[] savedSliders;
	    private void Start()
	    {
	        // Keep the scene preview transparent, then recreate the original 0.4-second fade in Play mode.
	        if (BlackFade)
	        {
	            var color = BlackFade.color; color.a = 1f; BlackFade.color = color;
	            BlackFade.canvasRenderer.SetAlpha(1f);
	            BlackFade.CrossFadeAlpha(0f, .4f, false);
	        }
	        Cursor.visible = true; Cursor.lockState = CursorLockMode.Confined;
	        SetObject(Cam, true);
	        Place(GrannyP, GrannySP); Place(GrandpaP, GrandpaSP); Place(SlendrinaMenu, SlendrinaSP);
	        if (Profile)
	        {
	            Profile.TryGetSettings(out AO); Profile.TryGetSettings(out MB); Profile.TryGetSettings(out CA);
	            Profile.TryGetSettings(out DOF); Profile.TryGetSettings(out CG); Profile.TryGetSettings(out V);
	            Profile.TryGetSettings(out B); Profile.TryGetSettings(out G);
	        }
	        if (V) V.intensity.value = 0;
	        if (CA) CA.intensity.value = 0;
	        if (G) G.intensity.value = 0;
	        if (B) B.enabled.value = false;
	        if (PlayerPrefs.GetInt("NewPlayerJoin") == 0)
	        {
	            string[] on = { "NewPlayerJoin", "Diff", "Granny", "Grandpa", "Music", "FOV", "RealtimeLights", "Limping", "RandomSeed", "OcclusionCullingOP", "SlendrinaEnemy" };
	            foreach (string key in on) Store(key, 1);
	            Store("FlashL", 0); Store("Quality", 3);
	            PlayerPrefs.SetString("PrimaryInt_KEY", "E"); PlayerPrefs.SetString("SecondaryInt_KEY", "F");
	            PlayerPrefs.SetString("Dropping_KEY", "Space"); PlayerPrefs.SetString("Crouch_KEY", "C"); PlayerPrefs.SetString("WP_KEY", "Mouse0");
	        }
	        if (PlayerPrefs.GetInt("NewPlayerJoinVC") == 0) { Store("NewPlayerJoinVC", 1); Store("CURVERSION", 2); }
	        if (string.IsNullOrEmpty(PlayerPrefsTest)) PlayerPrefsTest = "Volume";
	        float volume = PlayerPrefs.GetFloat(PlayerPrefsTest, 1f);
	        if (Volume) { Volume.SetValueWithoutNotify(volume); Volume.onValueChanged.AddListener(SetVolume); }
	        SetVolume(volume);
	        savedToggles = new (Toggle, string)[] {
	            (NM, "Textures"), (Smooth, "SmoothM"), (Limping, "Limping"), (Slendrina, "SlendrinaEnemy"),
	            (Vsync, "V"), (realtimeStuff, "RealtimeLights"), (Music, "Music"), (MotionBlur, "EFFECT_MB"),
	            (DepthOfField, "EFFECT_DOF"), (ChaseEffect, "EFFECT_CC"), (ColorGrading, "EFFECT_CG"),
	            (AmbientOcclusion, "EFFECT_AMB"), (fog, "FogEnabled"), (Occlus, "OcclusionCullingOP"),
	            (Seed, "FlashL"), (Extras, "Extras"), (GrannyWP, "GrannyWP"), (Granny, "Granny"), (Grandpa, "Grandpa") };
	        savedSliders = new (Slider, string)[] {
	            (Difficulty, "Diff"), (PresetS, "PresetChoosin"), (FOV, "FOV"), (Outlines, "Outlines"),
	            (Quality, "Quality"), (VersionControl, "CURVERSION") };
	        UpdateDifficultyLimit();
	        SetUpSlientloDifficulty();
	        foreach (var setting in savedToggles) if (setting.control) setting.control.SetIsOnWithoutNotify(PlayerPrefs.GetInt(setting.key) == 1);
	        foreach (var setting in savedSliders) if (setting.control) setting.control.SetValueWithoutNotify(PlayerPrefs.GetInt(setting.key));
	        UpdateMenuSettings();
	        FlushPreferences();
	    }
	    private GameObject slientloBox;
	    private const string SlientloDescription =
	        "* Granny and Grandpa are much faster\n" +
	        "* They open doors faster too\n" +
	        "* Grandpa shoots faster, Slendrina drains faster\n" +
	        "* Lasers and extra creaking floors\n" +
	        "* Every escape has extra locks and puzzles\n" +
	        "* The atmosphere is pitch dark";
	    // Slientlo Never Clear is the only difficulty, so the slider bar is hidden but its title stays, and a sixth
	    // description box (a copy of Impossible's) explains it.
	    private void SetUpSlientloDifficulty()
	    {
	        if (Difficulty)
	        {
	            Difficulty.interactable = false;
	            foreach (Transform part in Difficulty.transform)
	                if (!Diff_Text || part != Diff_Text.transform) part.gameObject.SetActive(false);
	        }
	        if (!Box_Diff5 || slientloBox) return;
	        slientloBox = Instantiate(Box_Diff5, Box_Diff5.transform.parent);
	        slientloBox.name = "SlientloHolder";
	        foreach (var text in slientloBox.GetComponentsInChildren<TMP_Text>(true))
	        {
	            if (text.name == "DiffT") text.text = "Difficulty: Slientlo Never Clear";
	            else if (text.name == "DescriptionDiff")
	            {
	                text.text = SlientloDescription;
	                text.fontSize = 15f;
	                text.enableAutoSizing = false;
	                text.alignment = TextAlignmentOptions.Top;
	                var rect = text.rectTransform;
	                rect.sizeDelta = new Vector2(480f, 160f);
	                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -33f); // just under the title, inside the box
	            }
	        }
	    }
	    private static void Place(GameObject obj, Transform spot)
	    { if (obj && spot) obj.transform.SetPositionAndRotation(spot.position, spot.rotation); }
	    private static void SetObject(GameObject obj, bool active)
	    { if (obj && obj.activeSelf != active) obj.SetActive(active); }
	    private void Store(string key, int value)
	    { if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) != value) { PlayerPrefs.SetInt(key, value); preferencesDirty = true; } }
	    private void FlushPreferences()
	    { if (preferencesDirty) { PlayerPrefs.Save(); preferencesDirty = false; } }
	    private static int Value(Slider slider, int fallback = 0) { return slider ? Mathf.RoundToInt(slider.value) : fallback; }
	    private static bool On(Toggle toggle) { return toggle && toggle.isOn; }
	    private void UpdateDifficultyLimit()
	    { if (Difficulty && PlayerPrefs.GetInt("IM_BOUGHT") == 1 && Difficulty.maxValue < 5) Difficulty.maxValue = 5; }
	    private void Update()
	    {
	        if (savedToggles == null || savedSliders == null) return;
	        UpdateDifficultyLimit(); UpdateMenuSettings(); FlushPreferences();
	        if (Input.GetKeyDown(KeyCode.Escape) && SettingsSet && SecondP && SecondP.activeSelf)
	        {
	            OnClickSound(); SetObject(SecondP, false); SetObject(TipMenu, false); SetObject(FirstP, true);
	            SettingsSet = false; BackToMain();
	        }
	        if (AbleToTest && Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.J)
	            && Input.GetKey((KeyCode)0x122) && Input.GetKey((KeyCode)0x12f))
	            PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") + 15f);
	    }
	    private void UpdateMenuSettings()
	    {
	        // Forced: game version 1.2, Extras on and Nightmare on (shown but locked).
	        if (VersionControl) { VersionControl.SetValueWithoutNotify(2); VersionControl.interactable = false; }
	        if (Extras) { Extras.SetIsOnWithoutNotify(true); }
	        if (NM) { NM.SetIsOnWithoutNotify(true); NM.interactable = false; }
	        // Slientlo Never Clear is the only difficulty; its slider bar is hidden (SetUpSlientloDifficulty) but the title shows.
	        if (Difficulty) { Difficulty.maxValue = 5; Difficulty.SetValueWithoutNotify(5); }
	        if (CoinAmount) CoinAmount.text = PlayerPrefs.GetFloat("Coins").ToString();
	        foreach (var setting in savedToggles) if (setting.control) Store(setting.key, setting.control.isOn ? 1 : 0);
	        foreach (var setting in savedSliders) if (setting.control) Store(setting.key, Value(setting.control));
	        int outline = Value(Outlines);
	        string[] colors = { "off", "yellow", "orange", "red", "magenta", "purple", "blue", "cyan", "green", "lime", "black" };
	        if (Outine_Text && outline >= 0 && outline < colors.Length) Outine_Text.text = "Outlines (" + colors[outline] + ")";
	        if (OutlineImageUI && OutlineExampleCount != null && outline >= 0 && outline < OutlineExampleCount.Length)
	            OutlineImageUI.sprite = OutlineExampleCount[outline];
	        int preset = Value(PresetS);
	        if (Preset_Text) Preset_Text.text = preset == 0 ? "Item Preset Chooser (in order)" : preset == 1 ? "Item Preset Chooser (random)" : "Item Preset Chooser (" + (preset - 1) + ")";
	        int fov = Value(FOV);
	        if (FOV_Text) FOV_Text.text = fov == 1 ? "Field of view (65f - Default)" : "Field of view (" + (60 + fov * 5) + "f)";
	        bool seeded = On(Seed);
	        SetObject(SeedButton, seeded); SetObject(SliderPreset, !seeded);
	        SetObject(SlendrinaMenu, On(Slendrina));
	        bool nightmare = On(NM);
	        RenderSettings.skybox = nightmare ? NMSky : NormalSky;
	        SetObject(MenuMusicNM, nightmare); SetObject(MenuMusic, !nightmare);
	        if (r != null) foreach (var renderer in r) if (renderer) renderer.sharedMaterial = nightmare ? fogNM : fogNormal;
	        if (bgAudio)
	        {
	            var clip = nightmare ? bgNM : bgNormal;
	            if (bgAudio.clip != clip) bgAudio.clip = clip;
	            if (!bgAudio.isPlaying) bgAudio.Play();
	        }
	        int difficulty = Value(Difficulty);
	        string[] names = { "Easy", "Normal", "Hard", "Extreme", "Impossible", "Slientlo Never Clear" };
	        if (Diff_Text && difficulty >= 0 && difficulty < names.Length)
	            Diff_Text.text = difficulty == DifficultyLevel.Slientlo ? names[difficulty] : "Difficulty - " + names[difficulty];
	        GameObject[] boxes = { Box_Diff1, Box_Diff2, Box_Diff3, Box_Diff4, Box_Diff5, slientloBox };
	        for (int i = 0; i < boxes.Length; i++) SetObject(boxes[i], i == difficulty);
	        bool forceEnemies = difficulty >= 3;
	        if (Granny) { Granny.interactable = !forceEnemies; if (forceEnemies) Granny.SetIsOnWithoutNotify(true); }
	        if (Grandpa) { Grandpa.interactable = !forceEnemies; if (forceEnemies) Grandpa.SetIsOnWithoutNotify(true); }
	        SetObject(GrannyP, On(Granny)); SetObject(GrandpaP, On(Grandpa));
	        Store("Granny", On(Granny) ? 1 : 0); Store("Grandpa", On(Grandpa) ? 1 : 0);
	        int quality = Mathf.Clamp(Value(Quality, 3), 0, QualitySettings.names.Length - 1);
	        string[] qualityNames = { "very low", "low", "medium", "high" };
	        if (Quality_Text && quality < qualityNames.Length) Quality_Text.text = "Texture quality (" + qualityNames[quality] + ")";
	        if (quality != appliedQuality || QualitySettings.GetQualityLevel() != quality)
	        { QualitySettings.SetQualityLevel(quality, true); appliedQuality = quality; }
	        QualitySettings.vSyncCount = On(Vsync) ? 1 : 0;
	        if (PlayerPrefs.GetInt("VerControlUNLOCKED") == 1) SetObject(textNoticeVERControl, true);
	        SetObject(moonLight, On(realtimeStuff));
	        SetObject(Bat, !On(GrannyWP)); SetObject(Slingshot, On(GrannyWP));
	        int version = Value(VersionControl);
	        if (Extras)
	        {
	            Extras.interactable = false; // forced on (see the top of this method)
	        }
	        if (Ver_Text) Ver_Text.text = "Game Version (1." + version + ")";
	        SetObject(MusicG, On(Music));
	        if (DOF) DOF.enabled.value = On(DepthOfField);
	        if (CA) CA.enabled.value = On(ChaseEffect);
	        if (AO) AO.enabled.value = On(AmbientOcclusion);
	        if (MB) MB.enabled.value = On(MotionBlur);
	        if (CG) CG.enabled.value = On(ColorGrading);
	        if (slingShot)
	        {
	            Texture2D texture = PlayerPrefs.GetInt("Slingshot_MagentaON") == 1 ? slingMagenta
	                : PlayerPrefs.GetInt("Slingshot_LavaON") == 1 ? slingLava
	                : PlayerPrefs.GetInt("Slingshot_IceON") == 1 ? slingIce : nightmare ? slingNM : slingNormal;
	            if (slingShot.mainTexture != texture) slingShot.mainTexture = texture;
	        }
	        SetObject(fogGround, On(fog));
	    }
	    private void SetVolume(float volume)
	    { AudioListener.volume = volume; PlayerPrefs.SetFloat(PlayerPrefsTest, volume); }
	    public void OnClickSound() { if (Sounds) Sounds.PlayOneShot(SwitchToggle); }
	    public void OnClickPlay()
	    {
	        StartCoroutine(TimerLeanInToOptions());
	        if (LogoAnim) LogoAnim.Play("LogoFadeAway");
	        if (OptionsFirstHalf != null) foreach (var option in OptionsFirstHalf) SetObject(option, false);
	    }
	    private IEnumerator TimerLeanInToOptions()
	    {
	        yield return new WaitForSeconds(LogoAnim && LogoAnim["LogoFadeAway"] != null ? LogoAnim["LogoFadeAway"].length : 0f);
	        SettingsSet = true; SetObject(FirstP, false); SetObject(SecondP, true);
	        if (OptionsFirstHalf != null) foreach (var option in OptionsFirstHalf) SetObject(option, true);
	    }
	    public void BackToMain() { if (LogoAnim) LogoAnim.Play("LogoDefault"); }
	    public void PlayLoad()
	    {
	        if (loading) return;
	        loading = true; Cursor.visible = false; SettingsSet = false; FlushPreferences();
	        StartCoroutine(Timerloading());
	    }
	    private IEnumerator Timerloading() { yield return new WaitForSeconds(3.5f); SceneManager.LoadScene("Scene"); }
	    // The Quit button calls this. The original only called Application.Quit; settings are saved first here.
	    public void OnApplicationQuit() { FlushPreferences(); PlayerPrefs.Save(); Application.Quit(); }
	    public void LoadGame()
	    {
	        if (GINPUT && (!GINPUT.gameObject.activeSelf || !GINPUT.enabled)) Application.Quit();
	    }
}
