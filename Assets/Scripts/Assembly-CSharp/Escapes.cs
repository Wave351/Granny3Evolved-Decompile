using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Escapes : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CWaitBalloon_003Ed__72 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Escapes _003C_003E4__this;

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
		public _003CWaitBalloon_003Ed__72(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CWaitGate_003Ed__71 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Escapes _003C_003E4__this;

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
		public _003CWaitGate_003Ed__71(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CWaitSubmarine_003Ed__73 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Escapes _003C_003E4__this;

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
		public _003CWaitSubmarine_003Ed__73(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CWaitTrain_003Ed__70 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Escapes _003C_003E4__this;

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
		public _003CWaitTrain_003Ed__70(int _003C_003E1__state)
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

	public GameObject MobileFPS;

	public PickRay Ray;

	public Image Black;

	public GameObject SpaceT;

	public GameObject HandHold;

	public GameObject UIElem;

	public GameObject Texts;

	public BGManager BG;

	public Transform GameOverTurnOff;

	public GameObject GrannyS;

	public GameObject GrandpaS;

	public GameObject TheEndT;

	public ScreenEffects ScreenEffects;

	public EnemyController EC;

	public GameObject Granny;

	public GameObject Grandpa;

	public Paused PauseLogic;

	public GameObject BloodScreens;

	public GameObject UINew;

	public GameObject TimerUISwitch;

	public PlayerStatus PS;

	public bool DiedOnce;

	public bool UsedWPOnce;

	public bool SeenOnce;

	public AI_Alligator alligator;

	public TicketControlBot ticketBot;

	public EscapeTriggerGate ETG;

	public EscapeRouteChecks ERC;

	public GameObject slendrina;

	public CribTrigger cribTrigger;

	[Header("Gate Stuff")]
	public Transform PosGrannyEndGate;

	public Transform PosGrandpaEndGate;

	public Transform posSlendrinaGateEnd;

	public GameObject escapeGateMusic;

	public GameObject cutsceneGateCam;

	[Header("Train Stuff")]
	public Transform PosGrannyEndTrain;

	public Transform PosGrandpaEndTrain;

	public Transform posSlendrinaTrainEnd;

	public Animation trainAnim;

	public Animation acceleratorAnim;

	public GameObject cutsceneCamTrain;

	public GameObject cutsceneCamTrainEscaped;

	public GameObject escapeTrainMusic;

	[Header("Balloon Stuff")]
	public Transform PosGrannyEndBalloon;

	public Transform PosGrandpaEndBalloon;

	public Transform posSlendrinaBalloonEnd;

	public GameObject cutsceneCamBalloon;

	public GameObject escapeBalloonMusic;

	public GameObject playerSilhouette;

	public Transform hotAirContainer;

	public BalloonLogic BL;

	[Header("Submarine Stuff")]
	public Transform PosGrannyEndSubmarine;

	public Transform PosGrandpaEndSubmarine;

	public Transform posSlendrinaSubmarine;

	public GameObject cutsceneCamSubmarine;

	public GameObject escapeSubmarineMusic;

	public GameObject submarineObjectTop;

	public GameObject slideDoorBottom1;

	public GameObject slideDoorBottom2;

	public GameObject slenderEnemy;

	public Transform slenderEndPosition;

	public GameObject resetFloorStuff;

	public GameObject moonlightRLT;

	public GameObject soulHuntSFX;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void EscapeGate()
	{
	}

	public void EscapeTrain()
	{
	}

	public void EscapeSubmarine()
	{
	}

	public void EscapeBalloon()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitTrain_003Ed__70))]
	private IEnumerator WaitTrain()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CWaitGate_003Ed__71))]
	private IEnumerator WaitGate()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CWaitBalloon_003Ed__72))]
	private IEnumerator WaitBalloon()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CWaitSubmarine_003Ed__73))]
	private IEnumerator WaitSubmarine()
	{
		return null;
	}

	public void ElimBody()
	{
	}

	public void EscapeCoinsEarn()
	{
	}
}
