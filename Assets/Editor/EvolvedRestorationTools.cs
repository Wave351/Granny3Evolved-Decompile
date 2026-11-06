using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Small editor commands for verifying the restored project without an MCP bridge.
public static class EvolvedRestorationTools
{
    [MenuItem("Tools/Evolved Restoration/Apply High Quality")]
    public static void ApplyHighQuality()
    {
        int high = Array.IndexOf(QualitySettings.names, "High");
        if (high < 0) throw new InvalidOperationException("High quality preset is missing.");
        QualitySettings.SetQualityLevel(high, true);
        PlayerPrefs.SetInt("Quality", high);
        PlayerPrefs.Save();
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/QualitySettings.asset"))
            EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        Debug.Log("Evolved restoration: quality set to " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }

    [MenuItem("Tools/Evolved Restoration/Write Menu Diagnostics")]
    public static void WriteMenuDiagnostics()
    {
        var text = new StringBuilder();
        text.AppendLine("Scene: " + SceneManager.GetActiveScene().path);
        text.AppendLine("Playing: " + EditorApplication.isPlaying);
        text.AppendLine("Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
        text.AppendLine("Saved quality: " + PlayerPrefs.GetInt("Quality", -1));
        foreach (var camera in Resources.FindObjectsOfTypeAll<Camera>())
            if (camera.gameObject.scene.IsValid())
                text.AppendLine("Camera: " + camera.name + " active=" + camera.gameObject.activeInHierarchy
                    + " enabled=" + camera.enabled + " mask=" + camera.cullingMask + " depth=" + camera.depth);
        foreach (var menu in Resources.FindObjectsOfTypeAll<MainMenu>())
            if (menu.gameObject.scene.IsValid())
            {
                text.AppendLine("MainMenu: active=" + menu.gameObject.activeInHierarchy + " enabled=" + menu.enabled);
                if (menu.BlackFade) text.AppendLine("Fade: color=" + menu.BlackFade.color
                    + " rendererAlpha=" + menu.BlackFade.canvasRenderer.GetAlpha());
                if (menu.Quality_Text) text.AppendLine("Quality label: " + menu.Quality_Text.text);
                if (menu.Diff_Text) text.AppendLine("Difficulty label: " + menu.Diff_Text.text);
            }
        File.WriteAllText(Path.Combine(Application.dataPath, "../Library/EvolvedMenuDiagnostics.txt"), text.ToString());
        Debug.Log("Evolved restoration: menu diagnostics written to Library/EvolvedMenuDiagnostics.txt");
    }
}
