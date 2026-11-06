using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/RayRemoveTraps.c (Granny 3 Evolved, IL2CPP x64).
public partial class RayRemoveTraps
{
		public RayRemoveTraps() { RaycastDis = 5; SpeedRev = .2f; }
		// Holding the interact key while looking at the bear trap that caught the player frees them.
		private void Update()
		{
			FillBar.fillAmount = ValueClock;
			RaycastHit hit;
			var falling = Player ? Player.GetComponent<FallingHolder>() : null;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, RaycastDis, LS) && !Paused.IsPaused
			    && falling && !falling.isFalling && !PlayerStatus.IsJumpscared
			    && hit.collider.gameObject.tag == "BearTrapActivated")
			{
				var trap = hit.collider.GetComponent<BearTrapLogic>();
				if (trap && trap.TrappedPlayer && !trap.ItemCaughtIt && Trapped)
				{
					TrapRemoval.SetActive(true);
					if (!Input.GetKey(KeyInteract)) { ValueClock = 0; return; }
					ValueClock += Time.deltaTime * SpeedRev;
					if (ValueClock < 1) return;
					AudioSource.PlayClipAtPoint(PlayerPrefs.GetInt("Textures") == 1 ? KillTrapNM : KillTrap, hit.collider.transform.position);
					Trapped = false;
					Destroy(hit.collider.gameObject);
					ScreenEffects.Hurt = false;
					var fps = Player ? Player.GetComponent<MobileFPS>() : null;
					if (fps) fps.AbleToMove = true;
					return;
				}
			}
			ValueClock = 0;
			TrapRemoval.SetActive(false);
		}
}
