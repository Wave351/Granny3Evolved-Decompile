using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Slientlo Never Clear: while it's picked on the difficulty slider, the main menu's 3D scene fades darker.
// A black layer sits between the 3D scene and the menu UI, so buttons and text stay readable.
public class SlientloMenuDarkness : MonoBehaviour
{
	const float MaxDarkness = .85f;  // alpha of the black layer when fully dark
	const float FadeSpeed = .6f;     // alpha per second

	Image shade;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Init()
	{
		SceneManager.sceneLoaded += (scene, mode) => Install(scene);
		Install(SceneManager.GetActiveScene());
	}

	static void Install(Scene scene)
	{
		if (scene.name != "Menu" || FindAnyObjectByType<SlientloMenuDarkness>()) return;
		// Draw just below the lowest screen-space overlay canvas, i.e. over the 3D scene but under the menu.
		int order = 0;
		bool found = false;
		foreach (var root in scene.GetRootGameObjects())
		foreach (var canvas in root.GetComponentsInChildren<Canvas>(true))
		{
			if (!canvas.isRootCanvas || canvas.renderMode != RenderMode.ScreenSpaceOverlay) continue;
			if (!found || canvas.sortingOrder < order) order = canvas.sortingOrder;
			found = true;
		}
		var go = new GameObject("SlientloMenuDarkness");
		SceneManager.MoveGameObjectToScene(go, scene);
		var c = go.AddComponent<Canvas>();
		c.renderMode = RenderMode.ScreenSpaceOverlay;
		c.sortingOrder = order - 1;
		var imageGo = new GameObject("Shade", typeof(RectTransform));
		imageGo.transform.SetParent(go.transform, false);
		var rect = (RectTransform)imageGo.transform;
		rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
		rect.offsetMin = rect.offsetMax = Vector2.zero;
		var image = imageGo.AddComponent<Image>();
		image.color = new Color(0f, 0f, 0f, 0f);
		image.raycastTarget = false;
		var dark = go.AddComponent<SlientloMenuDarkness>();
		dark.shade = image;
		// Start already dark if the menu opens with Slientlo selected.
		if (DifficultyLevel.IsSlientlo) image.color = new Color(0f, 0f, 0f, MaxDarkness);
	}

	void Update()
	{
		if (!shade) return;
		float target = DifficultyLevel.IsSlientlo ? MaxDarkness : 0f;
		var color = shade.color;
		color.a = Mathf.MoveTowards(color.a, target, FadeSpeed * Time.unscaledDeltaTime);
		shade.color = color;
		shade.enabled = color.a > .001f;
	}
}
