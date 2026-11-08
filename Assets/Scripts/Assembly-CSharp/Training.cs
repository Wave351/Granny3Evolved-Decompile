using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Training (like Granny Legacy's): an option on the options screen. In a run, Granny, Grandpa, Slendrina, Slender and the
// alligator can't kill the player, and G freezes or unfreezes every enemy: those plus the nightmare monsters, rats, the
// spectral crow, the chimney crow and the facility robots. Other dangers (traps, falls, electricity) still kill.
// The enemies' own scripts call Hold() first each frame; while frozen they skip their step and stand still mid-pose.
public static class Training
{
	public const string PrefKey = "Training";
	public const KeyCode FreezeKey = KeyCode.G;

	public static bool On => PlayerPrefs.GetInt(PrefKey, 0) == 1;
	public static bool Frozen { get; private set; }

	static readonly HashSet<NavMeshAgent> held = new HashSet<NavMeshAgent>();
	static readonly HashSet<Animation> stopped = new HashSet<Animation>();

	// Called at the top of an enemy's Update/FixedUpdate. True means "frozen: skip this step".
	public static bool Hold(NavMeshAgent agent, Animation anim)
	{
		if (!On || !Frozen) return false;
		if (agent && agent.enabled && agent.isOnNavMesh)
		{
			agent.isStopped = true;
			agent.velocity = Vector3.zero;
			held.Add(agent);
		}
		if (anim && anim.enabled) { anim.enabled = false; stopped.Add(anim); }
		return true;
	}

	// True when the player should be spared a death caused by an enemy.
	public static bool Spare() => On;

	static void Toggle(PickRay pick)
	{
		Frozen = !Frozen;
		if (Frozen)
		{
			// Everything else that roams (the nightmare rats have no script of their own): every nav agent stops, and the
			// animations under the enemy and nightmare groups pause.
			foreach (var a in Object.FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
				if (a.enabled && a.isOnNavMesh) { a.isStopped = true; a.velocity = Vector3.zero; held.Add(a); }
			foreach (var a in Object.FindObjectsByType<Animation>(FindObjectsSortMode.None))
				if (a.enabled && UnderEnemies(a.transform)) { a.enabled = false; stopped.Add(a); }
		}
		else
		{
			foreach (var a in held) if (a && a.enabled && a.isOnNavMesh) a.isStopped = false;
			foreach (var a in stopped) if (a) a.enabled = true;
			held.Clear(); stopped.Clear();
		}
		if (pick) pick.TimedTextInitiate(Frozen ? "Enemies frozen (G)" : "Enemies unfrozen (G)");
	}

	static bool UnderEnemies(Transform t)
	{
		for (; t; t = t.parent) if (t.name == "Entities" || t.name == "Nightmare-Stuff") return true;
		return false;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Boot()
	{
		var go = new GameObject("Training");
		Object.DontDestroyOnLoad(go);
		go.AddComponent<Runner>();
	}

	// Watches for G in a run, resets the freeze for each run, and adds the option to the menu.
	class Runner : MonoBehaviour
	{
		PickRay pick;
		MainMenu menu;
		Toggle option;

		void OnEnable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded += Loaded;
		void OnDisable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded -= Loaded;
		void Loaded(UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.LoadSceneMode m)
		{
			Frozen = false; held.Clear(); stopped.Clear(); pick = null; menu = null; option = null;
		}

		void Update()
		{
			if (!menu) { menu = FindAnyObjectByType<MainMenu>(); if (menu) AddOption(); }
			if (!On) return;
			if (!pick) pick = FindAnyObjectByType<PickRay>();
			if (pick && Input.GetKeyDown(FreezeKey)) Toggle(pick);
		}

		// A copy of the Music option, in the General column on the row below it, labelled Training.
		// (It used to copy the Nightmare option, but that one is an unlockable that starts hidden, so the copy never showed.)
		void AddOption()
		{
			if (!menu.Music) return;
			var template = menu.Music.gameObject;
			var copy = Instantiate(template, template.transform.parent);
			copy.name = "Option_Training";
			copy.SetActive(true);
			var rt = copy.GetComponent<RectTransform>();
			var src = template.GetComponent<RectTransform>();
			// The empty middle slot of the Music / Smooth movement row, under the Granny option.
			float x = menu.Granny ? menu.Granny.GetComponent<RectTransform>().anchoredPosition.x : src.anchoredPosition.x + 171f;
			rt.anchoredPosition = new Vector2(x, src.anchoredPosition.y);
			option = copy.GetComponent<Toggle>();
			option.onValueChanged = new Toggle.ToggleEvent();
			option.interactable = true;
			option.SetIsOnWithoutNotify(On);
			option.onValueChanged.AddListener(v => { PlayerPrefs.SetInt(PrefKey, v ? 1 : 0); PlayerPrefs.Save(); menu.OnClickSound(); });
			var label = copy.GetComponentInChildren<TMP_Text>(true);
			if (!label) return;
			label.text = "Training";
			label.rectTransform.sizeDelta = new Vector2(Mathf.Max(label.rectTransform.sizeDelta.x, 200f), label.rectTransform.sizeDelta.y);
			// A small hint under the checkbox.
			var hint = Instantiate(label, copy.transform);
			hint.name = "TextHint";
			hint.text = "G freezes enemies\nEnemies can't kill you";
			hint.fontSize = 22f;
			hint.enableAutoSizing = false;
			hint.alignment = TextAlignmentOptions.Top;
			hint.rectTransform.sizeDelta = new Vector2(300f, 60f);
			hint.rectTransform.anchoredPosition = new Vector2(0f, -33f);
		}
	}
}
