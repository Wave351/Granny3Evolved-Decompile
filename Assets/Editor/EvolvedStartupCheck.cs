using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Observe the real startup coroutine without skipping its waits or changing scene state.
[InitializeOnLoad]
public static class EvolvedStartupCheck
{
    private const string Key = "EvolvedRestoration.StartupCheck";
    private static readonly StringBuilder Report = new StringBuilder();
    private static double started;
    private static string lastState;
    private static bool sawTitle, sawWakeup;

    static EvolvedStartupCheck() { EditorApplication.update += Observe; }

    [MenuItem("Tools/Evolved Restoration/Check Day Startup")]
    public static void Run()
    {
        if (SceneManager.GetActiveScene().name != "Scene")
        { Debug.LogError("Open the game Scene before checking day startup."); return; }
        Report.Clear(); started = 0; lastState = null; sawTitle = false; sawWakeup = false;
        SessionState.SetBool(Key, true);
        EditorApplication.isPlaying = true;
    }

    private static void Observe()
    {
        if (!SessionState.GetBool(Key, false) || !EditorApplication.isPlaying) return;
        if (started == 0) { started = EditorApplication.timeSinceStartup; Report.AppendLine("Day startup integration check"); }
        double elapsed = EditorApplication.timeSinceStartup - started;
        var days = Resources.FindObjectsOfTypeAll<Days>().FirstOrDefault(d => d.gameObject.scene.name == "Scene");
        if (!days) { if (elapsed > 30) Finish(false, "Days component not found"); return; }
        var fps = days.Player ? days.Player.GetComponent<MobileFPS>() : null;
        bool title = days.Text_Day1 && days.Text_Day1.gameObject.activeInHierarchy && days.BeganDay;
        bool wakeup = days.CamAnimRise && days.CamAnimRise.IsPlaying("PlayerRiseNy");
        bool ready = days.CurrentDay == 1 && !days.WakingUp && fps && fps.isAllowedToMove && fps.AbleToMove
            && days.Player.activeInHierarchy && days.UIElements && days.UIElements.activeInHierarchy;
        sawTitle |= title; sawWakeup |= wakeup;
        string state = "day=" + days.CurrentDay + " title=" + title + " waking=" + days.WakingUp
            + " animation=" + wakeup + " player=" + (days.Player && days.Player.activeInHierarchy)
            + " hud=" + (days.UIElements && days.UIElements.activeInHierarchy) + " controls=" + ready;
        if (state != lastState)
        {
            Report.AppendLine(elapsed.ToString("F2") + "s " + state
                + " blackAlpha=" + (days.BlackScreen ? days.BlackScreen.canvasRenderer.GetAlpha().ToString("F3") : "missing"));
            lastState = state;
            Save();
        }
        if (ready && sawTitle && sawWakeup) Finish(true, "Day 1, wake-up animation, HUD and movement gates all observed");
        else if (elapsed > 30) Finish(false, "Startup did not complete within 30 seconds");
    }

    private static void Finish(bool passed, string detail)
    {
        Report.AppendLine((passed ? "PASS: " : "FAIL: ") + detail);
        Save(); SessionState.SetBool(Key, false);
        Debug.Log("Evolved startup check: " + (passed ? "PASS" : "FAIL") + ". " + detail);
    }

    private static void Save()
    { File.WriteAllText(Path.Combine(Application.dataPath, "../Library/EvolvedStartupCheck.txt"), Report.ToString()); }
}
