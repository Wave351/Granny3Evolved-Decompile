using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Slientlo Never Clear: Granny and Grandpa's clothes get the Slientlo picture (heads and skin stay normal).
// Nightmare mode uses a black-and-white copy painted onto the Nightmare textures.
// The textures live in Resources/Slientlo.
public static class SlientloSkins
{
	// Original texture name -> replacement in Resources/Slientlo (the NM version adds "NM").
	private static readonly Dictionary<string, string> Replacements = new Dictionary<string, string>
	{
		{ "GrannyTexturePainted5", "Slientlo/GrannyTexture_Slientlo" },
		{ "GrannyTexturePainted5_NM", "Slientlo/GrannyTexture_Slientlo" },
		{ "TextureAtlasGrandpaPainted2_Ragdoll", "Slientlo/TextureAtlasGrandpa_Slientlo" },
		{ "TextureAtlasGrandpaPainted2_Ragdoll_NM", "Slientlo/TextureAtlasGrandpa_Slientlo" },
	};
	private static readonly Dictionary<string, Texture> Loaded = new Dictionary<string, Texture>();

	private static int cachedFrame = -1;
	private static bool cachedActive;

	// Read once per frame so dozens of Texture_Changing components don't each hit PlayerPrefs.
	public static bool Active
	{
		get
		{
			if (cachedFrame != Time.frameCount) { cachedFrame = Time.frameCount; cachedActive = DifficultyLevel.IsSlientlo; }
			return cachedActive;
		}
	}

	private static bool Nightmare => PlayerPrefs.GetInt("Textures") == 1;

	// The Slientlo texture to use instead of this one, or null if it isn't a Granny or Grandpa texture.
	public static Texture For(Texture original, bool nightmare)
	{
		if (original == null || !Replacements.TryGetValue(original.name, out string path)) return null;
		if (nightmare) path += "NM";
		if (!Loaded.TryGetValue(path, out Texture tex))
		{
			tex = Resources.Load<Texture>(path);
			Loaded[path] = tex;
		}
		return tex;
	}

	// Renderers whose material isn't driven by Texture_Changing (like GrandpaHitBody) get their own
	// material copy with the Slientlo texture, so the shared material asset is left alone.
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Init()
	{
		SceneManager.sceneLoaded += (scene, mode) => ApplyToScene(scene);
		ApplyToScene(SceneManager.GetActiveScene());
	}

	private static void ApplyToScene(Scene scene)
	{
		if (!DifficultyLevel.IsSlientlo || !scene.isLoaded) return;
		var driven = new HashSet<Material>();
		foreach (var changer in Object.FindObjectsByType<Texture_Changing>(FindObjectsInactive.Include))
			if (changer.Material) driven.Add(changer.Material);
		bool nightmare = Nightmare;
		foreach (var root in scene.GetRootGameObjects())
		foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
		{
			var shared = renderer.sharedMaterials;
			bool changed = false;
			for (int i = 0; i < shared.Length; i++)
			{
				var mat = shared[i];
				if (!mat || driven.Contains(mat) || !mat.HasProperty("_MainTex")) continue;
				var tex = For(mat.mainTexture, nightmare);
				if (!tex) continue;
				shared[i] = new Material(mat) { mainTexture = tex };
				changed = true;
			}
			if (changed) renderer.sharedMaterials = shared;
		}
	}
}
