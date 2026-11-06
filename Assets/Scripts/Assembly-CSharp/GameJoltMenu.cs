using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameJoltMenu : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CAvailabilityGone_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameJoltMenu _003C_003E4__this;

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
		public _003CAvailabilityGone_003Ed__8(int _003C_003E1__state)
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

	public TMP_Text UsernameText;

	public bool AvailableToTrophy;

	public bool sessionSynced;

	public string[] slingshotSkins;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SkinToCloud()
	{
	}

	private string KeyGrabTrophyID(int id)
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CAvailabilityGone_003Ed__8))]
	private IEnumerator AvailabilityGone()
	{
		return null;
	}

	public void SignInStart()
	{
	}

	public void UnlockTrophy(int t)
	{
	}
}
