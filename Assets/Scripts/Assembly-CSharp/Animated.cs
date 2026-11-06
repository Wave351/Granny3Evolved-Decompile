using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class Animated : MonoBehaviour
{
	public float OpeningTimer;
	public float ClosingTimer;
	public AudioSource Audio;
	public AudioClip CloseSound;
	public AudioClip OpenSound;
	public AudioClip LockedSound;
	public bool Locked;
	public bool Closed;
	public Animation AnimatedObject;
	public AnimationClip OpenAnimation;
	public AnimationClip CloseAnimation;
	public AnimationClip LockedAnim;
	public bool CanBeLayer;
	public bool GoThru;
	public bool EnabledAnim;
}
