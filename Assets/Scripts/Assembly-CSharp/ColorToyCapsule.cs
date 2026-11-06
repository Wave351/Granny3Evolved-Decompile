using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ColorToyCapsule : MonoBehaviour
{
	public struct Tier
	{
		public string name;

		public float chance;

		public string[] items;
	}

	[CompilerGenerated]
	private sealed class _003CTimerToyCapsule_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ColorToyCapsule _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CTimerToyCapsule_003Ed__30(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public GameObject TextNotEnough;

	public Animation AnimatedToyCapsule;

	public GameObject InsertCoin;

	public GameObject BackB;

	public GameObject InvB;

	public GameObject slingshotFragment;

	public Renderer fragmentRenderer;

	public TMP_Text ItemText;

	public Texture2D Slingshot_Lava;

	public Texture2D Slingshot_Ice;

	public Texture2D Slingshot_Magenta;

	public TMP_Text ColorRarityText;

	public TMP_Text LavaFragmentsText;

	public TMP_Text IceFragmentsText;

	public TMP_Text MagentaFragmentsText;

	public bool GiveCoinBack;

	public GameObject slingshotOBJDisplay;

	public GameObject menuInventory;

	public GameJoltMenu gamejoltMenuCode;

	public Tier[] tiers;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void UpdateRarityUI()
	{
	}

	public void AttemptInsertCoin()
	{
	}

	public bool IsTierFullyUnlocked(Tier tier)
	{
		return false;
	}

	public float AdjustedChance(Tier tier)
	{
		return 0f;
	}

	public float ActiveChance()
	{
		return 0f;
	}

	public void UnlockCertainSkin()
	{
	}

	public void ApplySlingshotSkin(string item)
	{
	}

	[IteratorStateMachine(typeof(_003CTimerToyCapsule_003Ed__30))]
	private IEnumerator TimerToyCapsule()
	{
		return null;
	}
}
