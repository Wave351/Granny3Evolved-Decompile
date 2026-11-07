using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using com.ootii.Actors.BoneControllers;
using UnityEngine;
using UnityEngine.AI;

// Restoration stand-in for ootii's LookAtMotor. The exported ootii.dll is an IL2CPP stub (all method bodies empty),
// so BoneController does nothing at runtime. This reads the controller's saved LookAtMotor definition (bone indexes
// and weights) and, while the BoneController is enabled, turns those bones toward the target after animation.
public class BoneLookFallback : MonoBehaviour
{
	public Transform Target;
	public float MaxYaw = 75f;
	public float MaxPitch = 40f;

	BoneController controller;
	readonly List<Transform> bones = new List<Transform>();   // head first, as ootii stores them
	readonly List<float> weights = new List<float>();
	float motorWeight = 1f, blend;
	Transform reference;

	// Called by the AI scripts in place of LookAtMotor.TargetTransform.
	public static void SetTarget(BoneController bc, Transform target)
	{
		if (!bc) return;
		var look = bc.GetComponent<BoneLookFallback>();
		if (!look) look = bc.gameObject.AddComponent<BoneLookFallback>();
		look.Target = target;
	}

	void Awake()
	{
		controller = GetComponent<BoneController>();
		var agent = GetComponentInParent<NavMeshAgent>();
		reference = agent ? agent.transform : transform.root;
		if (!controller || controller.MotorDefinitions == null) return;
		foreach (var def in controller.MotorDefinitions)
		{
			if (def == null || !def.Contains("LookAtMotor")) continue;
			var idx = Regex.Match(def, @"SerializationBoneIndexes : \[([\d,]*)\]");
			var mw = Regex.Match(def, @"_Weight : ([\d.]+)");
			if (mw.Success) motorWeight = float.Parse(mw.Groups[1].Value, CultureInfo.InvariantCulture);
			var ws = Regex.Matches(def, @"Weight : ([\d.]+)\}");
			int n = 0;
			foreach (var s in idx.Groups[1].Value.Split(','))
			{
				int i;
				if (!int.TryParse(s, out i) || controller.BoneTransforms == null || i < 0 || i >= controller.BoneTransforms.Count) { n++; continue; }
				bones.Add(controller.BoneTransforms[i]);
				weights.Add(n < ws.Count ? float.Parse(ws[n].Groups[1].Value, CultureInfo.InvariantCulture) : 1f);
				n++;
			}
			break;
		}
		// The head's facing axis in its own space, taken from the scene's rest pose (character facing reference.forward).
		if (bones.Count > 0 && bones[0] && reference) headForwardLocal = Quaternion.Inverse(bones[0].rotation) * reference.forward;
	}

	// Attach to every BoneController as soon as a scene loads, so Awake reads the head axis from the saved scene pose
	// rather than from whatever animation frame is playing when an AI first sets a target.
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Install()
	{
		AttachAll();
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) => AttachAll();
	}
	static void AttachAll()
	{
		foreach (var bc in Object.FindObjectsByType<BoneController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			if (!bc.GetComponent<BoneLookFallback>()) bc.gameObject.AddComponent<BoneLookFallback>();
	}

	Vector3 headForwardLocal = Vector3.forward;
	Vector3 smoothedLocalDir = Vector3.forward;

	// Absolute look like ootii's LookAtMotor: measure where the animated head actually faces and correct only the
	// difference, spread over the chain by weight. Poses that already face the target (e.g. Grandpa aiming the shotgun)
	// are left almost untouched instead of being twisted twice.
	void LateUpdate()
	{
		if (bones.Count == 0 || !bones[0] || !reference) return;
		bool active = controller && controller.enabled && Target;
		blend = Mathf.MoveTowards(blend, active ? 1f : 0f, Time.deltaTime * 3f);
		if (blend <= 0f) return;

		Transform head = bones[0];
		Vector3 facing = head.rotation * headForwardLocal;
		Vector3 want = facing;
		if (Target)
		{
			Vector3 local = reference.InverseTransformDirection(Target.position - head.position);
			float y = Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
			float p = Mathf.Atan2(local.y, new Vector2(local.x, local.z).magnitude) * Mathf.Rad2Deg;
			if (Mathf.Abs(y) <= 120f)                                          // target behind: leave the animation alone
			{
				y = Mathf.Clamp(y, -MaxYaw, MaxYaw); p = Mathf.Clamp(p, -MaxPitch, MaxPitch);
				want = reference.TransformDirection(Quaternion.Euler(-p, y, 0) * Vector3.forward);
			}
		}
		float k = 1f - Mathf.Pow(.9f, Time.deltaTime * 60f);                 // ootii RotationLerp 0.1 per frame
		smoothedLocalDir = Vector3.Slerp(smoothedLocalDir, reference.InverseTransformDirection(want), k);
		Vector3 goal = Vector3.Slerp(facing, reference.TransformDirection(smoothedLocalDir), motorWeight * blend);

		float remaining = 0; foreach (var w in weights) remaining += w;
		for (int i = bones.Count - 1; i >= 0; i--)                          // spine to head; the head closes the rest
		{
			if (!bones[i] || remaining <= 0) continue;
			float f = weights[i] / remaining;
			remaining -= weights[i];
			Vector3 now = head.rotation * headForwardLocal;
			bones[i].rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(now, goal), f) * bones[i].rotation;
		}
	}
}
