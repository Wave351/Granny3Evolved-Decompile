using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/ChatController.c (Granny 3 Evolved, IL2CPP x64; reused from the Granny Legacy port).
public partial class ChatController
{
	// TextMesh Pro example: appends each submitted line with a [hh:mm:ss] timestamp.
	private void OnEnable() { ChatInputField.onSubmit.AddListener(AddToChatOutput); }
	private void OnDisable() { ChatInputField.onSubmit.RemoveListener(AddToChatOutput); }
	private void AddToChatOutput(string newText)
	{
	    ChatInputField.text = string.Empty;
	    var now = System.DateTime.Now;
	    string line = "[<#FFFF80>" + now.Hour.ToString("d2") + ":" + now.Minute.ToString("d2") + ":" + now.Second.ToString("d2") + "</color>] " + newText;
	    if (ChatDisplayOutput != null)
	        ChatDisplayOutput.text = ChatDisplayOutput.text == string.Empty ? line : ChatDisplayOutput.text + "\n" + line;
	    ChatInputField.ActivateInputField();
	    ChatScrollbar.value = 0;
	}
}
