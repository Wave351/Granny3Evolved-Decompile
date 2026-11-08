using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Tools > Evolved Restoration > Make Selected Player-Only Walls: invisible in the game, only the player is stopped by
// them, and the player can't interact with things through them. Everything else (items, enemies, lasers) goes through.
public static class PlayerOnlyWallTools
{
	[MenuItem("Tools/Evolved Restoration/Make Selected Player-Only Walls")]
	static void MakeSelected()
	{
		int n = 0;
		foreach (var g in Selection.gameObjects) { Make(g); n++; }
		Debug.Log($"Player-only walls: made {n}.");
	}

	[MenuItem("Tools/Evolved Restoration/Make Selected Player-Only Walls", true)]
	static bool CanMake() => Selection.gameObjects.Length > 0;

	public static void Make(GameObject g)
	{
		Undo.RecordObject(g, "Player-only wall");
		g.layer = PlayerOnlyWall.Layer;
		if (!g.GetComponent<Collider>()) Undo.AddComponent<BoxCollider>(g);
		if (!g.GetComponent<PlayerOnlyWall>()) Undo.AddComponent<PlayerOnlyWall>(g);
		EditorSceneManager.MarkSceneDirty(g.scene);
	}

	// Command line: makes the map's top-level "Cube" a player-only wall and saves. -executeMethod PlayerOnlyWallTools.MakeCubeBatch
	public static void MakeCubeBatch()
	{
		EditorSceneManager.OpenScene("Assets/1_Scenes/Scene.unity");
		var cubes = EditorSceneManager.GetActiveScene().GetRootGameObjects().Where(g => g.name.ToLower() == "cube").ToList();
		foreach (var c in cubes) Make(c);
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		Debug.Log($"PLAYERONLY made {cubes.Count} cube(s) player-only walls: {string.Join(", ", cubes.Select(c => c.transform.position.ToString()))}");
	}
}
