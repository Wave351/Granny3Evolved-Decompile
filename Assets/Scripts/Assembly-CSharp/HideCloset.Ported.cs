using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/HideCloset.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class HideCloset
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag != "Player")
		{
			return;
		}
		if (!UnHidScript.Hid)
		{
			UnHidScript.Hid = true;
			if (!Granny.EnemyVision.Seeing && Granny.EnemyVision.TimerStopVision > 0.7f)
			{
				Granny.UnSeenPlayer = true;
			}
			if (!Grandpa.EnemyVision.Seeing && Grandpa.EnemyVision.TimerStopVision > 0.7f)
			{
				Grandpa.UnSeenPlayer = true;
			}
			if (Granny.EnemyVision.Seeing || Granny.IsAngry)
			{
				Granny.CanOpenClosets = true;
			}
			if (Grandpa.EnemyVision.Seeing || Grandpa.IsAngry)
			{
				Grandpa.CanOpenClosets = true;
			}
			PlayerStatus.HidingInClosets = true;
		}
		if (!UnHidScript.Hid)
		{
			return;
		}
		if (Granny.EnemyVision.Seeing || Granny.IsAngry)
		{
			Granny.CanOpenClosets = true;
		}
		else if (!PlayerStatus.HidingInClosets || !Granny.CanOpenClosets)
		{
			Granny.CanOpenClosets = false;
		}
		if (Grandpa.EnemyVision.Seeing || Grandpa.IsAngry)
		{
			Grandpa.CanOpenClosets = true;
		}
		else if (!PlayerStatus.HidingInClosets || !Grandpa.CanOpenClosets)
		{
			Grandpa.CanOpenClosets = false;
		}
	}
}
