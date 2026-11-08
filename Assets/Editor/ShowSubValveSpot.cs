using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The submarine valve knob's place on the hatch. The knob itself is hidden until it's put on in the game, so the click spot
// (SubValvePlaceCollider, on the hatch above the handwheel) draws it, and the knob follows that spot: move or turn the spot
// and the knob goes with it.
// Tools > Evolved Restoration > Show Submarine Valve Spot: selects the spot and points the Scene view at it.
// Tools > Evolved Restoration > Fix Submarine Valve Position: pushes the knob out of the door so all of it is in front.
public static class ShowSubValveSpot
{
	static SubValveLock Lock() => Resources.FindObjectsOfTypeAll<SubValveLock>().FirstOrDefault(l => l.gameObject.scene.IsValid());

	[MenuItem("Tools/Evolved Restoration/Show Submarine Valve Spot")]
	static void Show()
	{
		var l = Lock();
		if (!l) { EditorUtility.DisplayDialog("Submarine valve", "The valve spot wasn't found. Open the map and run Tools > Evolved Restoration > Set Up Submarine Valve.", "OK"); return; }
		if (!l.KnobFollowsSpot) FixUp(l);
		Selection.activeTransform = l.transform;
		EditorGUIUtility.PingObject(l.gameObject);
		var view = SceneView.lastActiveSceneView;
		if (view)
		{
			// From in front of the hatch (the spot's forward points out of it), a little above.
			view.LookAt(l.transform.position, Quaternion.LookRotation(-l.transform.forward + Vector3.down * .25f), 1.2f);
			view.Repaint();
		}
	}

	[MenuItem("Tools/Evolved Restoration/Fix Submarine Valve Position")]
	static void FixMenu()
	{
		var l = Lock();
		if (!l) { EditorUtility.DisplayDialog("Submarine valve", "The valve spot wasn't found.", "OK"); return; }
		FixUp(l);
	}

	// Pushes the knob out until every part of it is in front of the door, then makes it follow the click spot.
	public static void FixUp(SubValveLock l)
	{
		if (!l.Placed) return;
		var knob = l.Placed.transform;
		var mesh = knob.GetComponent<MeshFilter>()?.sharedMesh;
		if (!mesh) return;
		Undo.RecordObjects(new Object[] { knob, l }, "Fix submarine valve position");
		var outward = l.transform.forward;
		var surface = l.transform.position - outward * .12f; // the spot was made 12 cm out from the door's surface
		float min = mesh.vertices.Min(v => Vector3.Dot(knob.TransformPoint(v) - surface, outward));
		const float gap = .004f;
		if (min < gap) knob.position += outward * (gap - min);
		l.KnobLocalPosition = l.transform.InverseTransformPoint(knob.position);
		l.KnobLocalRotation = Quaternion.Inverse(l.transform.rotation) * knob.rotation;
		l.KnobFollowsSpot = true;
		EditorUtility.SetDirty(l);
		EditorUtility.SetDirty(knob);
		EditorSceneManager.MarkSceneDirty(l.gameObject.scene);
		Debug.Log($"SUBVALVE knob moved {(min < gap ? gap - min : 0f):0.000} m out of the door; it now follows {l.name}. Save the map to keep it.");
	}

	// The knob drawn where it will be, at the click spot.
	[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
	static void Draw(SubValveLock l, GizmoType type)
	{
		if (!l.Placed || !l.KnobFollowsSpot) return;
		var mesh = l.Placed.GetComponent<MeshFilter>()?.sharedMesh;
		if (!mesh) return;
		Gizmos.color = (type & GizmoType.Selected) != 0 ? new Color(.4f, 1f, .45f) : new Color(.2f, .8f, .3f, .8f);
		Gizmos.matrix = Matrix4x4.TRS(l.transform.TransformPoint(l.KnobLocalPosition), l.transform.rotation * l.KnobLocalRotation, l.Placed.transform.lossyScale);
		Gizmos.DrawMesh(mesh);
		Gizmos.matrix = Matrix4x4.identity;
		Handles.Label(l.transform.position + Vector3.up * .25f, "Submarine valve knob");
	}

	// Keeps the hidden knob itself where it's drawn while the spot is moved in the editor.
	[InitializeOnLoadMethod]
	static void Hook() => Undo.postprocessModifications += mods =>
	{
		foreach (var m in mods)
			if (m.currentValue.target is Transform t && t.GetComponent<SubValveLock>() is SubValveLock l && l.KnobFollowsSpot) l.PlaceKnob();
		return mods;
	};
}
