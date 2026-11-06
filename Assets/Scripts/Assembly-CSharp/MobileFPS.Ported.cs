using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/MobileFPS.c (Granny 3 Evolved, IL2CPP x64).
public partial class MobileFPS
{
		public MobileFPS()
		{
			moveSpeed = 5f;
			SpeedMove = 6f;
			SpeedMoveCrouch = 6f;
			FallSpeed = 9.81f;
			cameraRotationSpeed = 2f;
			maxXRotation = 80f;
			minXRotation = -80f;
			isAllowedToMove = true;
			AbleToMove = true;
			CamK = true;
			groundCheckRadius = 0.1f;
		}

		private void Start()
		{
			Cursor.visible = false;
		}

		private void Awake()
		{
			cameraRotationSpeed = PlayerPrefs.GetFloat("sensitivity", 2f);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void Update()
		{
			SpeedMove = InWeb ? 1f : momHand.activeSelf ? 4.5f : 6f;
			SpeedMoveCrouch = InWeb ? .3f : momHand.activeSelf ? 1.9f : 2.8f;
			if (Paused.IsPaused)
			{
				return;
			}
			int mask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Noisy"));
			bool onGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, mask);
			if (CanFade)
			{
				Transform cam = playerCamera.transform;
				float x = cam.localEulerAngles.x;
				if (x > 180f)
				{
					x -= 360f;
				}
				float t = Mathf.Clamp01(SpeedFade * Time.deltaTime);
				Vector3 euler = cam.localEulerAngles;
				cam.localEulerAngles = new Vector3(Mathf.Lerp(x, 0f, t), euler.y, euler.z);
			}
			float bobSpeed;
			if (!IsCrouched)
			{
				moveSpeed = FallingHolder.isFalling ? 0f : SpeedMove;
				bobSpeed = momHand.activeSelf ? .69f : 1f;
			}
			else
			{
				moveSpeed = FallingHolder.isFalling ? 0f : SpeedMoveCrouch;
				bobSpeed = momHand.activeSelf ? .55f : .7f;
			}
			CameraAnim["HeadBobAnimation"].speed = bobSpeed;
			CameraAnim["HeadBobAnimation2"].speed = bobSpeed;
			CameraAnim["HeadBobAnimation3"].speed = bobSpeed;
			if (!isAllowedToMove)
			{
				if (!CameraAnim.IsPlaying("PlayerRiseNy")) CameraAnim.CrossFade("idle");
			}
			else
			{
				if (!AbleToMove)
				{
					CameraAnim.CrossFade("idle");
				}
				else
				{
					MoveFromInput();
				}
				if (CamK)
				{
					GetTouchInput();
				}
			}
			if (InElev)
			{
				Vector3 elevator = ElevatorObj.transform.position;
				if (!WasOnPlatform)
				{
					LastElevPos = elevator;
					WasOnPlatform = true;
				}
				characterController.Move(elevator - LastElevPos);
				moveDirection.y = 0f;
				LastElevPos = ElevatorObj.transform.position;
				return;
			}
			if (!onGround && !characterController.isGrounded)
			{
				characterController.Move(Vector3.down * FallSpeed * Time.deltaTime);
			}
		}

		private void MoveFromInput()
		{
			Vector3 input;
			if (PlayerPrefs.GetInt("SmoothM") == 1)
			{
				input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			}
			else
			{
				input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			}
			if (input.magnitude > 1f)
			{
				input.Normalize();
			}
			moveDirection = transform.TransformDirection(input * moveSpeed);
			isMoving = moveDirection.magnitude > 0f;
			if (!isMoving)
			{
				moveDirection = Vector3.zero;
			}
			characterController.Move(moveDirection * Time.deltaTime);

			if (Input.GetAxis("Horizontal") != 0f)
			{
				string bob = HeadBobForDay();
				if (bob != null)
				{
					CameraAnim.CrossFade(bob);
				}
			}
			if (Input.GetAxis("Vertical") != 0f)
			{
				string bob = HeadBobForDay();
				if (bob != null)
				{
					CameraAnim.CrossFade(bob);
				}
			}
			else if (Input.GetAxis("Horizontal") == 0f)
			{
				CameraAnim.CrossFade("idle");
			}
		}

		// With "Limping" on, the head bob gets heavier as the days go on; null = keep the current clip.
		private string HeadBobForDay()
		{
			if (PlayerPrefs.GetInt("Limping") != 1)
			{
				return "HeadBobAnimation";
			}
			float day = SystemDay.CurrentDay;
			if (day == 1f || day == 2f || day == 6f)
			{
				return "HeadBobAnimation";
			}
			if (day == 3f)
			{
				return "HeadBobAnimation2";
			}
			if (day == 4f || day == 5f)
			{
				return "HeadBobAnimation3";
			}
			return null;
		}

		private void GetTouchInput()
		{
			float mouseX = Input.GetAxis("Mouse X");
			rotationX -= Input.GetAxis("Mouse Y") * cameraRotationSpeed;
			if (rotationX < minXRotation)
			{
				rotationX = minXRotation;
			}
			else if (rotationX > maxXRotation)
			{
				rotationX = maxXRotation;
			}
			playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
			transform.rotation = transform.rotation * Quaternion.Euler(0f, mouseX * cameraRotationSpeed, 0f);
		}

		private void HandlePlayerMovement(bool isGrounded)
		{
			Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			if (input.magnitude > 1f)
			{
				input.Normalize();
			}
			moveDirection = transform.TransformDirection(input * moveSpeed);
			isMoving = moveDirection.magnitude > 0f;
		}

		private void HandlePlayerMovementS(bool isGrounded)
		{
			Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			if (input.magnitude > 1f)
			{
				input.Normalize();
			}
			moveDirection = transform.TransformDirection(input * moveSpeed);
			isMoving = moveDirection.magnitude > 0f;
		}

		public void ApplyFinalMovements()
		{
			if (!isMoving)
			{
				moveDirection = Vector3.zero;
			}
			characterController.Move(moveDirection * Time.deltaTime);
		}

		public void HandleVerticalMovement()
		{
			if (!characterController.isGrounded)
			{
				characterController.Move(Vector3.down * FallSpeed * Time.deltaTime);
			}
		}

		public void AdjustSpeed(float newSpeed)
		{
			cameraRotationSpeed = newSpeed;
		}

		public void ReserRotation()
		{
			CanFade = true;
		}

		public void DragHead(float timerToBack)
		{
			StartCoroutine(DragHeadTimer(timerToBack));
		}

		private IEnumerator DragHeadTimer(float timerBack)
		{
			CamK = false;
			yield return new WaitForSeconds(timerBack);
	        ForcefulCameraAvailability();
	    }

	    public void ForcefulCameraAvailability()
	    {
	        rotationX = playerCamera.localEulerAngles.x;
	        if (rotationX > 180f) rotationX -= 360f;
	        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
	        CamK = true;
	    }

		public void ResetRotationSlendrina()
		{
			CameraAnim.enabled = false;
			rotationX = playerCamera.localRotation.x;
			playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
			CameraAnim.enabled = true;
		}

		public void ResetRotation()
		{
			CameraAnim.enabled = false;
			rotationX = 0f;
			playerCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
			CameraAnim.enabled = true;
		}

		public void OnControllerColliderHit(ControllerColliderHit OBJ)
		{
			Rigidbody body = OBJ.collider.GetComponent<Rigidbody>();
			if (body != null && !body.isKinematic && OBJ.moveDirection.y >= -0.2f)
			{
				body.linearVelocity = new Vector3(OBJ.moveDirection.x * 3.6f, 0f, OBJ.moveDirection.z * 3.6f);
			}
		}
}
