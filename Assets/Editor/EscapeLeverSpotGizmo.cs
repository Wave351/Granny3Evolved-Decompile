using System.Linq;
using UnityEditor;
using UnityEngine;

// Placing the two escape levers in Unity: each spot under "== Added Hazards == / Escape Lever Spots" draws the wall lever
// where it will be in the game. Move a spot near a wall and use Tools > Evolved Restoration > Stick Selected Lever Spots To
// Wall: it finds the nearest wall in any direction and turns the lever to face out from it.
public static class EscapeLeverSpotGizmo
{
	[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
	static void Draw(EscapeLevers levers, GizmoType type)
	{
		if (levers.Spots == null) return;
		for (int i = 0; i < levers.Spots.Length; i++)
		{
			var spot = levers.Spots[i];
			if (!spot) continue;
			// Same layout as EscapeLevers.BuildWallLever: +X into the wall, the base on the wall, stick pointing down and out.
			var wall = spot.position + spot.right * .06f;
			Gizmos.color = Selection.Contains(spot.gameObject) ? Color.yellow : new Color(1f, .6f, .1f);
			Gizmos.matrix = Matrix4x4.TRS(wall, spot.rotation, Vector3.one);
			Gizmos.DrawCube(new Vector3(-.02f, 0f, 0f), new Vector3(.04f, .32f, .18f));
			var hinge = new Vector3(-.065f, 0f, 0f);
			var tip = hinge + Quaternion.Euler(0f, 0f, 40f) * new Vector3(-.23f, 0f, 0f);
			Gizmos.DrawLine(hinge, tip);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(tip, .035f);
			Gizmos.matrix = Matrix4x4.identity;
			Handles.Label(spot.position - spot.right * .3f + Vector3.up * .25f, "Escape lever " + (i + 1));
		}
	}

	[MenuItem("Tools/Evolved Restoration/Stick Selected Lever Spots To Wall")]
	static void Stick()
	{
		var spots = Selection.transforms.Where(t => t.name.StartsWith("Escape Lever Spot")).ToArray();
		if (spots.Length == 0) { EditorUtility.DisplayDialog("Escape levers", "Select one or both Escape Lever Spot objects first (under == Added Hazards == / Escape Lever Spots).", "OK"); return; }
		foreach (var s in spots)
		{
			Undo.RecordObject(s, "Stick lever spot to wall");
			if (!EscapeLeversSetup.StickToNearestWall(s))
				Debug.LogWarning($"{s.name}: no wall within 2 m. Move it closer to a wall and try again.");
		}
	}
}
