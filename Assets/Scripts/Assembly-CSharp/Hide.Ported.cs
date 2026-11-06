using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/Hide.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class Hide
{
	public Hide() { RegularHide = true; }
	private void OnTriggerEnter(Collider other)
	{
	    if (!other.gameObject.CompareTag("Player")) return;
	    if (RegularHide) PlayerStatus.HidingFromHouseEnemy = true;
	    const float lostSight = .7f;
	    if (UnderStuffHide)
	    {
	        PlayerStatus.HidingUnderThings = true;
	        Granny.PlayerPos = PlayerStatus.Player.transform.position;
	        Grandpa.PlayerPos = PlayerStatus.Player.transform.position;
	        if (!Granny.EnemyVision.Seeing && lostSight < Granny.EnemyVision.TimerStopVision) Granny.UnSeenPlayer = true;
	        if (!Grandpa.EnemyVision.Seeing && lostSight < Grandpa.EnemyVision.TimerStopVision) Grandpa.UnSeenPlayer = true;
	    }
	    if (ClosetHiding)
	    {
	        if (!Granny.EnemyVision.Seeing && lostSight < Granny.EnemyVision.TimerStopVision) Granny.UnSeenPlayer = true;
	        if (Granny.EnemyVision.Seeing || Granny.IsAngry) Granny.CanOpenClosets = true;
	        if (!Grandpa.EnemyVision.Seeing && lostSight < Grandpa.EnemyVision.TimerStopVision) Grandpa.UnSeenPlayer = true;
	        if (Grandpa.EnemyVision.Seeing || Grandpa.IsAngry) Grandpa.CanOpenClosets = true;
	        PlayerStatus.HidingInClosets = true;
	    }
	}
	private void OnTriggerExit(Collider other)
	{
	    if (!other.gameObject.CompareTag("Player")) return;
	    if (RegularHide) PlayerStatus.HidingFromHouseEnemy = false;
	    if (UnderStuffHide) PlayerStatus.HidingUnderThings = false;
	    if (ClosetHiding) PlayerStatus.HidingInClosets = false;
	    Granny.CanOpenClosets = false; Granny.UnSeenPlayer = false;
	    Grandpa.CanOpenClosets = false; Grandpa.UnSeenPlayer = false;
	}
}
