using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ClosetUnhid.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class ClosetUnhid
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag != "Player" || !Hid)
		{
			return;
		}
		Hid = false;
		PlayerStatus.HidingInClosets = false;
		Granny.CanOpenClosets = false;
		Granny.UnSeenPlayer = false;
		Grandpa.CanOpenClosets = false;
		Grandpa.UnSeenPlayer = false;
	}
}
