using UnityEngine;

public partial class EscapeRouteChecks : MonoBehaviour
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
}
