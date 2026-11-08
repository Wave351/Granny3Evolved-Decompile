using UnityEngine;

// Slientlo Never Clear: the train's electric switch box is shut and padlocked. On the padlock (a copy of the game's own,
// set up by Tools > Evolved Restoration > Set Up Switch Box Lock). Its RegPuzzle needs the padlock key and calls Unlock:
// the padlock drops with its sound, and the box door falls off (the game's own way, as when it was clicked off).
// On other difficulties the box is as the original game has it and this padlock is gone.
public class SwitchBoxPadlock : MonoBehaviour
{
	public Rigidbody Door;
	public Interactive DoorInteractive;
	[Tooltip("The door's shut position and rotation in the box (world space).")]
	public Vector3 ClosedPosition;
	public Quaternion ClosedRotation = Quaternion.identity;
	[Tooltip("The switch slot behind the door, blocked until the box is open.")]
	public Collider Slot;
	public Rigidbody Padlock;
	public AudioSource PadlockSound;
	public RegPuzzle Puzzle;

	public bool Unlocked { get; private set; }

	void Awake()
	{
		if (!DifficultyLevel.IsSlientlo) { gameObject.SetActive(false); return; }
		if (Door)
		{
			Door.isKinematic = true;
			Door.transform.SetPositionAndRotation(ClosedPosition, ClosedRotation);
		}
		if (DoorInteractive) DoorInteractive.enabled = false;   // it can't be knocked off while it's locked
		if (Slot) Slot.enabled = false;
	}

	public void Unlock()
	{
		if (Unlocked) return;
		Unlocked = true;
		if (Puzzle) Puzzle.enabled = false;
		if (Padlock)
		{
			Padlock.isKinematic = false;
			Padlock.AddForce(-Door.transform.right * .4f, ForceMode.VelocityChange);
		}
		if (PadlockSound) PadlockSound.Play();
		if (Door)
		{
			// Falls out of the box towards the player, as the game's own door does once it's knocked loose.
			Door.isKinematic = false;
			Door.AddForce(-Door.transform.right * 1.2f, ForceMode.VelocityChange);
			Door.AddTorque(Door.transform.forward * .8f, ForceMode.VelocityChange);
		}
		if (Slot) Slot.enabled = true;
	}
}
