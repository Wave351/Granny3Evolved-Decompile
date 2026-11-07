using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Restoration fix: Granny 3 Evolved has no light probes. Moving objects (Granny, Grandpa, items) get their fill light
// only from the ambient probe that Bakery baked into each scene's LightingData. The scene settings say "flat black
// ambient", and after the Unity 6 upgrade the editor rebuilds the probe from that colour (all zeros), so every
// skinned character away from a realtime light rendered solid black. This reapplies the original baked probe
// (values copied from the AssetRipper export of LightingData.m_BakedAmbientProbeInLinear) and keeps it applied.
public class BakedAmbientRestore : MonoBehaviour
{
	private static readonly float[] SceneProbe = { 0.012784988f,0.00086856296f,0.00068147015f,0.00062273553f,-6.0653365E-05f,0.00011896137f,0.0017776577f,0.000108078224f,0.0057252515f,0.010373874f,0.0008588699f,0.00053784717f,0.00055112335f,-5.085787E-05f,0.00012112144f,0.0014319017f,6.883677E-05f,0.004587088f,0.009446755f,0.0009924683f,0.0004812735f,0.00045981468f,-5.6217927E-05f,0.00015809856f,0.0012911262f,5.7314144E-05f,0.0041028694f };
	private static readonly float[] MenuProbe = { 0.16539237f,-0.016851261f,0.011412758f,-0.0029104068f,-0.0022201322f,0.008704806f,0.009293378f,-0.0037170108f,0.021161795f,0.20739567f,0.026954452f,0.018949732f,-0.004832829f,-0.0037532873f,0.014715392f,0.013523403f,-0.0056958417f,0.03024943f,0.28723687f,0.11124085f,0.035064004f,-0.0089399535f,-0.0073009767f,0.0286298f,0.017246319f,-0.008663189f,0.035955925f };

	// The baked game-scene probe is very dark in gamma space; this lifts characters in unlit rooms so they read as
	// dim figures rather than silhouettes. 1 = original value.
	public const float SceneBrightness = 3f;

	private SphericalHarmonicsL2 probe;
	private bool active;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Install()
	{
		var host = new GameObject("BakedAmbientRestore") { hideFlags = HideFlags.HideAndDontSave };
		DontDestroyOnLoad(host);
		host.AddComponent<BakedAmbientRestore>();
	}

	private void OnEnable() { SceneManager.activeSceneChanged += OnSceneChanged; Choose(SceneManager.GetActiveScene()); }
	private void OnDisable() { SceneManager.activeSceneChanged -= OnSceneChanged; }
	private void OnSceneChanged(Scene from, Scene to) { Choose(to); }

	private void Choose(Scene scene)
	{
		float[] values = scene.name == "Menu" ? MenuProbe : scene.name == "Scene" ? SceneProbe : null;
		active = values != null;
		if (values == null) return;
		probe = new SphericalHarmonicsL2();
		for (int c = 0; c < 3; c++)
		{
			// The stored probe is linear; this project renders in gamma space, so convert each channel by its DC term.
			float dc = values[c * 9];
			float scale = QualitySettings.activeColorSpace == ColorSpace.Gamma && dc > 0 ? Mathf.LinearToGammaSpace(dc) / dc : 1f;
			if (values == SceneProbe) scale *= SceneBrightness;
			for (int k = 0; k < 9; k++) probe[c, k] = values[c * 9 + k] * scale;
		}
		Apply();
		UnityEngine.Debug.Log("[BakedAmbientRestore] applied baked ambient probe for " + scene.name + ", DC red " + probe[0, 0].ToString("F4"));
	}
	// Unity only uses an assigned probe in Custom mode; in Flat mode it keeps rebuilding it from the (black) colour.
	private void Apply()
	{
		RenderSettings.ambientMode = AmbientMode.Custom;
		RenderSettings.ambientProbe = probe;
	}

	// The editor (and some lighting changes) rebuild the ambient probe from the black flat colour; put it back.
	private void LateUpdate()
	{
		if (active && (RenderSettings.ambientMode != AmbientMode.Custom || RenderSettings.ambientProbe[0, 0] != probe[0, 0])) Apply();
	}
}
