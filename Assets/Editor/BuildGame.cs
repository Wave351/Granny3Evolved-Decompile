using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Windows (Mono) build of the Granny 3 Evolved decompile.
//   Unity.exe -batchmode -quit -projectPath <project> -executeMethod BuildGame.BuildWindows -logFile <log>
public static class BuildGame
{
	public const string OutputDir = "../../Build/Granny3EvolvedDecompile";
	public const string ExeName = "Granny 3 Evolved Decompile.exe";

	[MenuItem("Tools/Evolved Restoration/Build Windows Game")]
	public static void BuildWindows()
	{
		ApplyPlayerSettings();
		var scenes = new[] { "Assets/1_Scenes/Menu.unity", "Assets/1_Scenes/Scene.unity" };
		Directory.CreateDirectory(OutputDir);
		var options = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = Path.Combine(OutputDir, ExeName),
			target = BuildTarget.StandaloneWindows64,
			options = BuildOptions.None,
		};
		BuildReport report = BuildPipeline.BuildPlayer(options);
		Debug.Log($"[BuildGame] result={report.summary.result} errors={report.summary.totalErrors} size={report.summary.totalSize} time={report.summary.totalTime}");
		if (Application.isBatchMode) EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
	}

	// Mono backend, the original game's icon, and no Unity logo (the original OmGi splash stays).
	public static void ApplyPlayerSettings()
	{
		PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
		PlayerSettings.resizableWindow = true;
		PlayerSettings.SplashScreen.showUnityLogo = false;
		var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Texture2D/AppIcon.png");   // taken from the original exe
		if (icon)
		{
			PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { icon }, IconKind.Any);
			var sizes = PlayerSettings.GetIconSizes(NamedBuildTarget.Standalone, IconKind.Any);
			var icons = new Texture2D[sizes.Length];
			for (int i = 0; i < icons.Length; i++) icons[i] = icon;
			PlayerSettings.SetIcons(NamedBuildTarget.Standalone, icons, IconKind.Any);
		}
		else Debug.LogWarning("[BuildGame] AppIcon.png not found; exe keeps the default icon");
		AssetDatabase.SaveAssets();
	}
}
