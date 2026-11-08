using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Added for the decompile (not in the original game):
//  * F11 or Alt+Enter toggles fullscreen (borderless at the monitor's native resolution) <-> a 1280x720 window.
//  * The game always starts borderless fullscreen at the monitor's native resolution.
//  * The menus and HUD are laid out for 16:9, so a dragged window snaps back to 16:9 once the user lets go.
//    Fullscreen and maximized windows are left alone.
public class WindowAspectLock : MonoBehaviour
{
	const float Aspect = 16f / 9f;
	int lastW, lastH;
	float settle, cooldown;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
	[DllImport("user32.dll")] static extern IntPtr GetActiveWindow();
	[DllImport("user32.dll")] static extern bool IsZoomed(IntPtr hWnd);
	static IntPtr hwnd;
	static bool Maximized()
	{
		try
		{
			var h = GetActiveWindow();
			if (h != IntPtr.Zero) hwnd = h;
			return hwnd != IntPtr.Zero && IsZoomed(hwnd);
		}
		catch { return false; }
	}
#else
	static bool Maximized() => false;
#endif

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Install()
	{
		if (Application.isEditor) return;                                    // the Game view has its own aspect setting
		var go = new GameObject("WindowAspectLock");
		DontDestroyOnLoad(go);
		go.AddComponent<WindowAspectLock>();
		// Always start borderless fullscreen, even if a windowed size was saved last time (F11 still gives a window).
		if (Screen.fullScreenMode != FullScreenMode.FullScreenWindow) GoFullscreen();
	}

	static void GoFullscreen() =>
		Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);

	void Update()
	{
		bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr);
		if (Input.GetKeyDown(KeyCode.F11) || (alt && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))))
		{
			if (Screen.fullScreenMode == FullScreenMode.Windowed && !Maximized()) GoFullscreen();
			else Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
			cooldown = 1f; lastW = lastH = 0;
			return;
		}
		if (cooldown > 0) { cooldown -= Time.unscaledDeltaTime; lastW = lastH = 0; return; }   // let a mode switch finish

		if (Screen.fullScreenMode != FullScreenMode.Windowed || Maximized()) { lastW = lastH = 0; return; }
		int w = Screen.width, h = Screen.height;
		if (w != lastW || h != lastH) { lastW = w; lastH = h; settle = .35f; return; }   // still being dragged
		if (settle <= 0) return;
		settle -= Time.unscaledDeltaTime;
		if (settle > 0) return;
		if (Mathf.Abs((float)w / h - Aspect) < .01f) return;
		// Keep whichever edge the user changed most, and fit the other one to 16:9.
		int newW = w, newH = Mathf.RoundToInt(w / Aspect);
		if (newH > Display.main.systemHeight - 80) { newH = h; newW = Mathf.RoundToInt(h * Aspect); }
		Screen.SetResolution(newW, newH, FullScreenMode.Windowed);
		lastW = newW; lastH = newH;
	}
}
