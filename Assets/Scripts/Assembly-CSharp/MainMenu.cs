using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CTimerLeanInToOptions_003Ed__104 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MainMenu _003C_003E4__this;

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
		public _003CTimerLeanInToOptions_003Ed__104(int _003C_003E1__state)
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
	private sealed class _003CTimerloading_003Ed__107 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CTimerloading_003Ed__107(int _003C_003E1__state)
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

	public Toggle Vsync;

	public Toggle NM;

	public Toggle Music;

	public Toggle Granny;

	public Toggle Smooth;

	public Toggle Slendrina;

	public Toggle Limping;

	public Toggle Grandpa;

	public Toggle realtimeStuff;

	public Toggle GrannyWP;

	public Toggle Seed;

	public Toggle Extras;

	public Toggle MotionBlur;

	public Toggle DepthOfField;

	public Toggle ChaseEffect;

	public Toggle ColorGrading;

	public Toggle AmbientOcclusion;

	public Toggle Bloom;

	public Toggle Occlus;

	public Toggle fog;

	public Slider Quality;

	public Slider VersionControl;

	public Slider Difficulty;

	public Slider Volume;

	public Slider FOV;

	public Slider PresetS;

	public Slider Outlines;

	public TMP_Text Outine_Text;

	public TMP_Text Diff_Text;

	public TMP_Text Ver_Text;

	public TMP_Text Quality_Text;

	public TMP_Text Preset_Text;

	public TMP_Text FOV_Text;

	public GameObject MusicG;

	public AudioSource Sounds;

	public AudioClip SwitchToggle;

	public GameObject CamHallway;

	public GameObject FirstP;

	public GameObject SecondP;

	public GameObject TipMenu;

	public bool SettingsSet;

	public GameObject[] OptionsFirstHalf;

	public Animation LogoAnim;

	public PostProcessProfile Profile;

	public AmbientOcclusion AO;

	public MotionBlur MB;

	public ChromaticAberration CA;

	public ChromaticAberration DOF;

	public ColorGrading CG;

	public Vignette V;

	public Bloom B;

	public Grain G;

	public GameObject Box_Diff1;

	public GameObject Box_Diff2;

	public GameObject Box_Diff3;

	public GameObject Box_Diff4;

	public GameObject Box_Diff5;

	public GameObject GrannyP;

	public GameObject GrandpaP;

	public GameObject Bat;

	public GameObject Slingshot;

	public GameObject SlendrinaMenu;

	public Transform GrannySP;

	public Transform GrandpaSP;

	public Transform SlendrinaSP;

	public Material NormalSky;

	public Material NMSky;

	public AudioSource BuySound;

	public TMP_Text CoinAmount;

	public GameObject TextNotEnough;

	public GameObject Cam;

	public string PlayerPrefsTest;

	public Image BlackFade;

	public GameInputManager GINPUT;

	public Sprite[] OutlineExampleCount;

	public Image OutlineImageUI;

	public GameObject[] AllOptions;

	public GameObject TextDidntUnlockAnything;

	public bool AbleToTest;

	public GameObject MenuMusic;

	public GameObject MenuMusicNM;

	public AudioSource bgAudio;

	public AudioClip bgNM;

	public AudioClip bgNormal;

	public GameObject SliderPreset;

	public GameObject SeedButton;

	public GameObject fogGround;

	public Renderer[] r;

	public Material fogNM;

	public Material fogNormal;

	public GameObject moonLight;

	public Material slingShot;

	public Texture2D slingNormal;

	public Texture2D slingNM;

	public Texture2D slingIce;

	public Texture2D slingLava;

	public Texture2D slingMagenta;

	public GameJoltMenu gamejoltMenuScript;

	public GameObject textNoticeVERControl;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SetVolume(float volume)
	{
	}

	public void OnClickSound()
	{
	}

	public void OnClickPlay()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerLeanInToOptions_003Ed__104))]
	private IEnumerator TimerLeanInToOptions()
	{
		return null;
	}

	public void BackToMain()
	{
	}

	public void PlayLoad()
	{
	}

	[IteratorStateMachine(typeof(_003CTimerloading_003Ed__107))]
	private IEnumerator Timerloading()
	{
		return null;
	}

	public void OnApplicationQuit()
	{
	}

	public void LoadGame()
	{
	}
}
