using UnityEngine;

public class EscapeRouteChecks : MonoBehaviour
{
	public bool AllRoutesOpen;

	[Header("Gate Escape")]
	public bool OpenedGate;

	public GameObject variableGate;

	public GameObject variableBridge;

	[Header("Train Escape")]
	public bool PlacedAccelerator;

	public bool PlacedTrainKey;

	public bool FlippedSwitch1;

	public bool FlippedSwitch2;

	public bool FlippedSwitch3;

	public bool TrainAvailable;

	[Header("Hot Air Balloon Escape")]
	public bool maximizedBalloon;

	public bool valveKnobPlaced;

	public bool ballonAvailable;

	[Header("Submarine Escape")]
	public bool submarineReady;

	public void SetGateOpened()
	{
	}

	public void SetAcceleratorOn()
	{
	}

	public void SetTrainKeyOn()
	{
	}

	public void SetElecSwitchOn1()
	{
	}

	public void SetElecSwitchOn2()
	{
	}

	public void SetElecSwitchOn3()
	{
	}

	public void SetMaxBalloon()
	{
	}

	public void PutValveOn()
	{
	}

	public void PutLeverOn()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
