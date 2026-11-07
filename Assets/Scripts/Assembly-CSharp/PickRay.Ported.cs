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

// Ported from Annotated-Pseudocode/PickRay.c (Granny 3 Evolved, IL2CPP x64).
public partial class PickRay
{
		public PickRay() { RaycastDis = 5f; RaycastCheckItemDis = 1f; CanDropDis = true; DropForceItem = 5f; distanceAttack = 1.5f; ImpactS2 = 25f; Gun2RayDistance = 25f; AmmoAmount = 1f; }
		private void Start() { }
		private bool Blocked(FallingHolder falling) { return falling.isFalling || falling.isLanding || WJ.IsJumping; }
		private void Update()
		{
			var falling = Player.GetComponent<FallingHolder>();
			// Stones in the pouch and shells in the gun.
			if (StoneAmount == 0) Stone1.SetActive(false);
			else if (StoneAmount == 1) { Stone2.SetActive(false); Stone1.SetActive(true); }
			else if (StoneAmount == 2) { Stone3.SetActive(false); Stone2.SetActive(true); }
			else if (StoneAmount == 3) Stone3.SetActive(true);
			if (AmmoAmount == 0) { AmmoUI1.SetActive(false); AmmoUI2.SetActive(false); }
			else if (AmmoAmount == 1) { AmmoUI1.SetActive(true); AmmoUI2.SetActive(false); }
			else if (AmmoAmount == 2) { AmmoUI1.SetActive(true); AmmoUI2.SetActive(true); }
			UpdateGrenade(falling);
			UpdateSlingshot(falling);
			UpdateBook(falling);
			highlightBalloonSpot.SetActive(H_BOX1.activeSelf);
			UpdateBaton(falling);
			UpdateShotgun(falling);
			FirstPickupTexts();
			if (Input.GetKeyDown(MainInteract) && Ring.activeSelf && !Paused.IsPaused && !Blocked(falling) && !PlayerStatus.IsJumpscared) buttonClicked = true;
			if (Input.GetKeyDown(DropKey) && Drop1.activeSelf && !Paused.IsPaused && !falling.isLanding && !WJ.IsJumping && !PlayerStatus.IsJumpscared && CanDropDis)
				buttonClicked2 = true;
			if (Paused.IsPaused) return;
			// Key hints are hidden while landing, jumping, caught or casting the book.
			bool hints = !falling.isLanding && !WJ.IsJumping && !PlayerStatus.IsJumpscared && !bookAnim.isPlaying;
			Space.enabled = hints;
			Shoot1.enabled = hints;
			Shoot2.enabled = hints;
			Throw.enabled = hints;
			AmmoUI1.GetComponent<Image>().enabled = hints;
			AmmoUI2.GetComponent<Image>().enabled = hints;
			Crosshair.enabled = !EButton_Door.gameObject.activeSelf && !EButton_Pick.gameObject.activeSelf && !lockPickUIUsing.activeSelf;
			if (buttonClicked2 && !PlayerStatus.IsJumpscared && !bookAnim.isPlaying) CheckItemDropping();
			// Too close to a wall to drop or throw anything.
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			UnityEngine.Debug.DrawRay(transform.position, forward * RaycastCheckItemDis, Color.green);
			CanDropDis = !Physics.Raycast(transform.position, forward, RaycastCheckItemDis, GunLayers);
			UnityEngine.Debug.DrawRay(transform.position, forward * RaycastDis, Color.red);
			RaycastHit hit;
			if (!Physics.Raycast(transform.position, forward, out hit, RaycastDis, LayersIncluded) || Blocked(falling) || PlayerStatus.IsJumpscared)
			{
				T_HighlightItem.SetActive(false);
				buttonClicked = false;
				Ring.SetActive(false);
			}
			else Interact(hit);
			if (!H_GRVAS.activeSelf) vase1Filled = false;
			if (!H_PAVAS.activeSelf) vase2Filled = false;
		}
		private void UpdateGrenade(FallingHolder falling)
		{
			if (!H_GRE.activeSelf || Paused.IsPaused || PlayerStatus.IsJumpscared) { ThrowG.SetActive(false); return; }
			if (Input.GetKeyDown(ShootKey) && !Paused.IsPaused && !Blocked(falling) && CanDropDis)
			{
				H_GRE.SetActive(false);
				buttonClicked2 = false;
				Drop1.SetActive(false);
				GameObject grenade = Instantiate(LethalGrenade, PosThrowGrenade.position, PosThrowGrenade.rotation);
				grenade.GetComponent<Rigidbody>().AddForce(grenade.transform.forward * SpeedForceThrowGrenade, ForceMode.Impulse);
			}
			ThrowG.SetActive(true);
		}
		private void UpdateSlingshot(FallingHolder falling)
		{
			if (!slingShotHand.activeSelf || Paused.IsPaused || PlayerStatus.IsJumpscared)
			{
				ShootT.SetActive(false);
				if ((!slingShotHand.activeSelf || !Paused.IsPaused) && pickedSling)
				{
					slingShotAnim.transform.localPosition = new Vector3(0, .0063f, .0003f);
					slingShotAnim.transform.localEulerAngles = new Vector3(-1.994f, 0, 0);
					pickedSling = false;
				}
				return;
			}
			// Taking the slingshot from Granny leaves her without one.
			var granny = EnemyControl.Granny.GetComponent<AI_Granny>();
			if (granny.slingshotWP.activeSelf) granny.slingshotWP.SetActive(false);
			if (!pickedSling)
			{
				if (StoneAmount > 0)
				{
					slingShotAnim.Play("slingshotReload");
					GetComponent<AudioSource>().PlayOneShot(reloadSlingSFX);
				}
				pickedSling = true;
			}
			if (StoneAmount > 0)
			{
				if (Input.GetKeyDown(ShootKey) && !slingShotAnim.isPlaying && !shotWithSling && !Paused.IsPaused && !Blocked(falling) && CanDropDis)
				{
					StoneAmount -= 1f;
					GetComponent<AudioSource>().PlayOneShot(stoneShootSFX);
					GameObject stone = Instantiate(stoneShoot, PosShootStone.position, PosShootStone.rotation);
					stone.GetComponent<Rigidbody>().AddForce(stone.transform.forward * ShootSpeed, ForceMode.Impulse);
					slingShotAnim.Play("slingshotShoot");
					shotWithSling = true;
					StartCoroutine(TimerResetSling());
				}
				ShootT.SetActive(true);
				return;
			}
			ShootT.SetActive(false);
			if (Input.GetKeyDown(ShootKey)) TimedTextInitiate("I need a stone");
		}
		private void UpdateBook(FallingHolder falling)
		{
			if (!H_BOOK.activeSelf || Paused.IsPaused || PlayerStatus.IsJumpscared || hasSoul || electricBook.isPlaying) { BlastT.SetActive(false); return; }
			if (Input.GetKeyDown(ShootKey) && !bookAnim.isPlaying && !Paused.IsPaused && !Blocked(falling) && CanDropDis)
			{
				electricBook.Play();
				GetComponent<AudioSource>().PlayOneShot(electricBook.GetComponent<AudioSource>().clip);
				bookAnim.Play("BookBlast");
				Instantiate(MagicBlast, PosShootBlast.position, Quaternion.identity);
			}
			BlastT.SetActive(true);
		}
		private void UpdateBaton(FallingHolder falling)
		{
			if (!batonHand.activeSelf || Paused.IsPaused || PlayerStatus.IsJumpscared) { AttackT.SetActive(false); return; }
			var swing = batonHand.GetComponent<Animation>();
			if (Input.GetKeyDown(ShootKey) && !Paused.IsPaused && !Blocked(falling) && !swing.isPlaying) swing.Play("batonHit");
			AttackT.SetActive(true);
		}
		private void UpdateShotgun(FallingHolder falling)
		{
			if (!ShotgunPickHand.activeSelf || Paused.IsPaused || PlayerStatus.IsJumpscared)
			{
				AmmoCounterUI.SetActive(false);
				ShootB2.SetActive(false);
				return;
			}
			AmmoCounterUI.SetActive(true);
			if (AmmoAmount <= 0)
			{
				ShootB2.SetActive(false);
				if (Input.GetKeyDown(ShootKey)) GetComponent<AudioSource>().PlayOneShot(EmptyGun);
				return;
			}
			if (Input.GetKeyDown(ShootKey) && !Paused.IsPaused && !Blocked(falling))
			{
				var gun = H_DSH.GetComponent<Animation>();
				if (AmmoAmount == 1 && !gun.isPlaying)
				{
					AmmoAmount -= 1f;
					gun.Stop();
					gun.Play("ShootShootyEmpty");
					ShootGun2();
				}
				else if (AmmoAmount == 2 && !gun.isPlaying)
				{
					AmmoAmount -= 1f;
					gun.Stop();
					gun.Play("ShootShooty");
					ShootGun2();
				}
			}
			ShootB2.SetActive(true);
		}
		// The first pickup of these hints at what they hide (until the hint has been seen enough times).
		private void FirstPickupTexts()
		{
			if (!pickedCoco && H_CO.activeSelf && PlayerPrefs.GetFloat("CocoPickups") != 5f)
			{
				pickedCoco = true;
				TimedTextInitiate("There's something inside this coconut");
				PlayerPrefs.SetFloat("CocoPickups", PlayerPrefs.GetFloat("CocoPickups") + 1f);
			}
			if (!pickedMom && H_MOM.activeSelf && PlayerPrefs.GetFloat("MomPickups") != 8f)
			{
				pickedMom = true;
				TimedTextInitiate("She's heavy. There's something inside this creature");
				PlayerPrefs.SetFloat("MomPickups", PlayerPrefs.GetFloat("MomPickups") + 1f);
			}
			if (!pickedBook && H_BOOK.activeSelf && PlayerPrefs.GetFloat("Bookickups") != 8f)
			{
				pickedBook = true;
				if (PlayerPrefs.GetInt("Granny") == 1 || PlayerPrefs.GetInt("Grandpa") == 1) TimedTextInitiate("This book is evil. I could use it on the grandparents");
				else
				{
					// With nobody to use it on, the book already holds a soul.
					TimedTextInitiate("This book has a use");
					hasSoul = true;
					boxColliderOldBook.SetActive(false);
					boxColliderNewBook.SetActive(true);
				}
				// The original reads "BookPickups" but stores "Bookickups".
				PlayerPrefs.SetFloat("Bookickups", PlayerPrefs.GetFloat("BookPickups") + 1f);
			}
		}
		private void Highlight(string text)
		{
			TurnOffItemTexts();
			T_HighlightItem.SetActive(true);
			T_HighlightItem.GetComponent<TMP_Text>().text = text;
			Ring.SetActive(true);
		}
		private bool Clicked()
		{
			if (!buttonClicked) return false;
			buttonClicked = false;
			return true;
		}
		private void Say(string text)
		{
			StopSetOfTexts();
			T_NeedShit.SetActive(true);
			T_NeedShit.GetComponent<TextManager>().Add();
			T_NeedShit.GetComponent<TMP_Text>().text = text;
		}
		private void PickedUp()
		{
			TurnOffItemTexts();
			Drop1.SetActive(true);
			Ring.SetActive(false);
			GetComponent<AudioSource>().PlayOneShot(PickUp);
		}
		private void Interact(RaycastHit hit)
		{
			GameObject target = hit.collider.gameObject;
			string name = target.name;
			var puzzle = hit.collider.GetComponent<RegPuzzle>();
			if (puzzle && puzzle.enabled)
			{
				var sound = hit.collider.GetComponent<AudioSource>();
				if (sound == null || !sound.isPlaying)
				{
					TurnOffItemTexts();
					Ring.SetActive(true);
					if (Clicked()) puzzle.Interacted();
					return;
				}
			}
			// Regular items come from the inventory definitions.
			var seed = hit.collider.GetComponent<ItemSeedData>();
			if (seed)
			{
				T_HighlightItem.SetActive(false);
				var def = Inventory.GetItemDefByName(seed.itemName);
				if (def == null) return;
				T_HighlightItem.SetActive(true);
				T_HighlightItem.GetComponent<TMP_Text>().text = def.textHighlighted;
				Ring.SetActive(true);
				if (!Clicked()) return;
				CheckItemDropping();
				Inventory.PickupItem(seed.itemName);
				Destroy(target);
				T_HighlightItem.SetActive(false);
				Drop1.SetActive(true);
				Ring.SetActive(false);
				return;
			}
			if (name == "HotAirBalloonPlaced")
			{
				if (BL.floating) return;
				Highlight("Hot air balloon");
				if (!Clicked()) return;
				TurnOffItemTexts();
				BL.EnterHotAirBalloon();
				return;
			}
			if (name == "GasValve_InPlace" && !target.GetComponent<Animation>().isPlaying)
			{
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				TurnOffItemTexts();
				if (BL.ballonPart1.activeSelf && BL.ballonPart2.activeSelf)
				{
					BL.valveGasOn = !BL.valveGasOn;
					target.GetComponent<Animation>().Play(BL.valveGasOn ? "TurnOnGas" : "TurnOffGas");
					buttonClicked = false;
				}
				else Say("I need more parts assembled before I turn on the gas");
				return;
			}
			if (name == "TurnOnFlameCollision")
			{
				if (BL.fireGas.activeSelf) return;
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				if (!H_MAT.activeSelf) { Say("I need the matches"); return; }
				StopSetOfTexts();
				if (!BL.valveGasOn) { Say("I need to turn on the gas first"); return; }
				StopSetOfTexts();
				BL.fireGas.SetActive(true);
				hit.collider.GetComponent<AudioSource>().Play();
				return;
			}
			if (name == "LockpickOnTheGround")
			{
				Highlight("Lock pick");
				if (!Clicked()) return;
				target.SetActive(false);
				TurnOffItemTexts();
				LockPickUI.SetActive(true);
				doorCollisionLockPick.SetActive(true);
				GetComponent<AudioSource>().PlayOneShot(PickUp);
				return;
			}
			var mom = hit.collider.GetComponent<MomPickUp>();
			if (mom)
			{
				Highlight("Slendrina's mom");
				if (!Clicked()) return;
				CheckItemDropping();
				H_MOM.SetActive(true);
				mom.SelfDestruction();
				PickedUp();
				return;
			}
			if (name == "Vas1_InPlace" || name == "Vas2_InPlace")
			{
				bool granny = name == "Vas1_InPlace";
				Highlight(granny ? "Granny's vase" : "Grandpa's vase");
				if (!Clicked()) return;
				CheckItemDropping();
				if (granny) { H_GRVAS.SetActive(true); vase1Filled = false; }
				else { H_PAVAS.SetActive(true); vase2Filled = false; }
				target.SetActive(false);
				PickedUp();
				return;
			}
			if (name == "SlingshotHandGranny")
			{
				Highlight("Granny's Slingshot");
				if (!Clicked()) return;
				CheckItemDropping();
				target.SetActive(false);
				TurnOffItemTexts();
				slingShotHand.SetActive(true);
				PickedUp();
				return;
			}
			if (name == "ShotgunParentG" || target.tag == "shotgun")
			{
				Highlight("Double barrel shotgun");
				if (!Clicked()) return;
				PickShooty();
				CheckItemDropping();
				// Taking it from Grandpa (or his bedside) leaves him unarmed.
				if (name == "ShotgunParentG" || name == "ShotgunBed")
				{
					target.SetActive(false);
					EnemyControl.ShotgunGrandpa.SetActive(false);
				}
				else
				{
					EnemyControl.ShotgunGrandpa.SetActive(false);
					Destroy(target);
				}
				TurnOffItemTexts();
				ShotgunPickHand.SetActive(true);
				Drop1.SetActive(true);
				Ring.SetActive(false);
				return;
			}
			if (target.tag == "ammo")
			{
				Highlight("Shotgun ammo");
				if (!Clicked()) return;
				if (!ShotgunPickHand.activeSelf) { Say("I need a shotgun"); return; }
				var gun = H_DSH.GetComponent<Animation>();
				if (AmmoAmount == 0 && !gun.isPlaying) { Load(gun, "Load_0", target); return; }
				if (AmmoAmount == 1 && !gun.isPlaying) { Load(gun, "Reload", target); return; }
				if (AmmoAmount == 2) Say("It's already loaded");
				return;
			}
			if (name == "DangerousCuttingToolCollision")
			{
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				// One coconut or Slendrina's mom fits in the cutter.
				if (H_CO.activeSelf)
				{
					if (momInPlace.activeSelf) { Say("I cant fit in more"); return; }
					H_CO.SetActive(false);
					Drop1.SetActive(false);
					coconutInPlace.SetActive(true);
					placedCoconut = true;
				}
				else if (H_MOM.activeSelf)
				{
					if (coconutInPlace.activeSelf) { Say("I cant fit in more"); return; }
					H_MOM.SetActive(false);
					Drop1.SetActive(false);
					momInPlace.SetActive(true);
				}
				else { Say("This is a dangerous cutting tool"); return; }
				StopSetOfTexts();
				Ring.SetActive(false);
				return;
			}
			if (name == "VasePlacementCollider1" || name == "VasePlacementCollider2")
			{
				bool granny = name == "VasePlacementCollider1";
				GameObject placed = granny ? DL.vaseGranny : DL.vaseGrandpa;
				if (placed.activeSelf) return;
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				GameObject held = granny ? H_GRVAS : H_PAVAS;
				if (!held.activeSelf) { Say("I need the missing vase"); return; }
				Drop1.SetActive(false);
				Ring.SetActive(false);
				StopSetOfTexts();
				held.SetActive(false);
				placed.SetActive(true);
				buttonClicked = false;
				if (granny && vase1Filled) DL.vaseGrannyFilled = true;
				if (!granny && vase2Filled) DL.vaseGrandpaFilled = true;
				return;
			}
			if (name == "Accelerator_InPlace")
			{
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				if (tDoors1.closed && tDoors2.closed && ERC.TrainAvailable)
				{
					Drop1.SetActive(true);
					Ring.SetActive(false);
					StopSetOfTexts();
					Escapes.EscapeTrain();
					buttonClicked = false;
					return;
				}
				Say("I can't do it yet");
				return;
			}
			if (name == "BarrelWithWater")
			{
				var water = hit.collider.GetComponent<AudioSource>();
				if (water.isPlaying) return;
				TurnOffItemTexts();
				Ring.SetActive(true);
				if (!Clicked()) return;
				if (!H_PAVAS.activeSelf && !H_GRVAS.activeSelf) { Say("A barrel full of water"); return; }
				// Fill the held vase.
				StopSetOfTexts();
				Drop1.SetActive(true);
				Ring.SetActive(false);
				if (H_PAVAS.activeSelf) vase2Filled = true;
				else vase1Filled = true;
				water.Play();
				buttonClicked = false;
				T_NeedShit.SetActive(true);
				T_NeedShit.GetComponent<TextManager>().Add();
				T_NeedShit.GetComponent<TMP_Text>().text = "I should be careful with dropping the vase";
				return;
			}
			if (name == "Stone(Clone)")
			{
				Highlight(stoneTextHighlight);
				if (!Clicked()) return;
				TurnOffItemTexts();
				if (StoneAmount >= 3) { Say("I can't carry more"); return; }
				if (StoneAmount == 0 && slingShotHand.activeSelf)
				{
					slingShotAnim.Play("slingshotReload");
					GetComponent<AudioSource>().PlayOneShot(reloadSlingSFX);
				}
				StoneAmount += 1f;
				Destroy(target);
				Ring.SetActive(false);
				GetComponent<AudioSource>().PlayOneShot(PickUp);
				return;
			}
			TurnOffItemTexts();
			buttonClicked = false;
			Ring.SetActive(false);
		}
		private void Load(Animation gun, string clip, GameObject shells)
		{
			AmmoAmount += 1f;
			TurnOffItemTexts();
			gun.Stop();
			gun.Play(clip);
			Destroy(shells);
			buttonClicked = false;
		}
		// Picking up the shotgun shows the load state.
		public void PickShooty()
		{
			MoreAmmo.SetActive(true);
			var gun = H_DSH.GetComponent<Animation>();
			if (gun == null) return;
			if (AmmoAmount == 0) { gun.Stop(); gun.CrossFade("PickUpShootyEmpty"); }
			else if (AmmoAmount == 1 || AmmoAmount == 2) { gun.Stop(); gun.CrossFade("PickUpShooty"); }
		}
		public void ShootGun2()
		{
			Vector3 forward = RayShoot2.TransformDirection(Vector3.forward);
			ShootHead.Play("playerShootNew");
			Instantiate(MuzzleFront, DropP.position, DropP.transform.rotation);
			// The blast is loud.
			Instantiate(Noisy1, Player.transform.position, Player.transform.rotation);
			Instantiate(Noisy2, Player.transform.position, Player.transform.rotation);
			RaycastHit hit;
			if (!Physics.Raycast(RayShoot2.position, forward, out hit, Gun2RayDistance, GunLayers)) return;
			if (IsPOWER) Instantiate(ExplosionGas, hit.point, hit.collider.transform.rotation);
			AudioSource.PlayClipAtPoint(ImpactSound, hit.point);
			GameObject target = hit.collider.gameObject;
			string name = target.name;
			if (target.tag == "BearTrapActivated") hit.collider.GetComponent<BearTrapLogic>().ShootGun();
			if (target.tag == "bloodshootable") Instantiate(BloodEffect, hit.point, hit.transform.rotation);
			if (name == "SubwayCrowbarRoomWindow") WJ.DestroyWindow();
			if (name == "GrandpaHitBody")
			{
				// Grandpa asleep in bed.
				if (!EnemyControl.GrandpaKilled && !EnemyControl.Grandpa.GetComponent<AI_Grandpa>().CaughtPlayer) EnemyControl.GrandpaDeathSBed(0, false);
				Blood(hit);
			}
			if (target.tag == "granpa")
			{
				if (!EnemyControl.GrandpaKilled && !EnemyControl.Grandpa.GetComponent<AI_Grandpa>().CaughtPlayer) EnemyControl.GrandpaDeathS(0, false);
				Blood(hit);
			}
			if (target.tag == "granny")
			{
				if (!EnemyControl.GrannyKilled && !EnemyControl.Granny.GetComponent<AI_Granny>().CaughtPlayer) EnemyControl.GrannyDeathS(0, false);
				Blood(hit);
			}
			if (name == "MoatMonsterAlligator")
			{
				var alligator = hit.collider.GetComponent<AI_Alligator>();
				if (!alligator.hurt && !alligator.killingPlayer && alligator.onHunt)
				{
					alligator.HurtAlligatorWeapon();
					Instantiate(BloodEffect, hit.point, hit.transform.rotation);
				}
			}
			if (name == "CrowNest") CC.KillAssBird();
			if (name == "PlankRoofSupport") hit.collider.GetComponent<PlankRoof>().BreakUp();
			if (name == "Subway_TicketRobot" || name == "Subway_TicketRobotHead")
			{
				var robot = GameObject.Find("SubwayRobot").GetComponent<TicketControlBot>();
				if (!robot.dead) robot.SilentButDeadly();
			}
			if (name == "Facility_TicketRobot" || name == "Facility_TicketRobotHead")
			{
				var robot = GameObject.Find("FacilityRobot").GetComponent<FacilityBot>();
				if (!robot.dead) TimedTextInitiate("This robot is too strong. I might need something else");
			}
			if (name == "DoorActivatorAttached")
			{
				var robot = GameObject.Find("SubwayRobot").GetComponent<TicketControlBot>();
				if (!robot.dead) robot.DeathUponRobot();
				target.name = "DoorActivatorDetached";
			}
			if (name == "BlockShopPlank") BreakPlank(hit, "BlockShopPlankBroke", false);
			if (name == "ImpossiblePlank") BreakPlank(hit, "PlankBroke", true);
			// Propane goes up.
			var seed = target.GetComponent<ItemSeedData>();
			if (seed && seed.itemName == "PropaneTank")
			{
				Instantiate(ExplosionGas, hit.collider.transform.position, hit.collider.transform.rotation);
				Destroy(hit.collider.gameObject);
			}
			if (target.name == "PropaneTank_InPlace")
			{
				Instantiate(ExplosionGas, hit.collider.transform.position, hit.collider.transform.rotation);
				Destroy(hit.collider.gameObject);
			}
			if (hit.collider == null) return;
			// Balloon packs get lighter when shot.
			foreach (var pack in new[] { "BalloonPack3", "BalloonPack2", "BalloonPack1" })
			{
				if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Item")) continue;
				var data = hit.collider.GetComponent<ItemSeedData>();
				if (!data || data.itemName != pack) continue;
				var packBody = hit.collider.GetComponent<Rigidbody>();
				if (packBody != null) packBody.mass = 2.5f;
			}
			if (hit.collider == null) return;
			var body = hit.collider.GetComponent<Rigidbody>();
			if (body != null) body.AddForce(forward * ImpactS2, ForceMode.Impulse);
		}
		private void Blood(RaycastHit hit)
		{
			Instantiate(ImpactLord, hit.point, hit.transform.rotation);
			Instantiate(BloodEffect, hit.point, hit.transform.rotation);
		}
		// Shot or hit planks fall loose and become items.
		private void BreakPlank(RaycastHit hit, string brokenName, bool timed)
		{
			GameObject plank = hit.collider.gameObject;
			plank.GetComponent<AudioSource>().Play();
			plank.GetComponent<Rigidbody>().isKinematic = false;
			plank.layer = LayerMask.NameToLayer("Item");
			plank.name = brokenName;
			if (timed) plank.GetComponent<DestroyTimer>().enabled = true;
			Destroy(hit.collider.GetComponent<RegPuzzle>());
		}
		public void AttackBaton()
		{
			Vector3 forward = RayShoot2.TransformDirection(Vector3.forward);
			RaycastHit hit;
			if (!Physics.Raycast(RayShoot2.position, forward, out hit, distanceAttack, GunLayers)) return;
			GameObject target = hit.collider.gameObject;
			if (target.name == "SubwayCrowbarRoomWindow") WJ.DestroyWindow();
			if (target.tag == "granpa") EnemyControl.Grandpa.GetComponent<AI_Grandpa>().HitWithStone(false);
			if (target.tag == "granny") EnemyControl.Granny.GetComponent<AI_Granny>().HitWithStone(false);
			if (target.name == "CrowNest" || target.name == "ColliderBirdNest") GameObject.Find("CrowNest").GetComponent<CrowChimney>().HurtBird();
			if (target.name == "BlockShopPlank") BreakPlank(hit, "BlockShopPlankBroke", false);
			if (hit.collider.gameObject != null)
			{
				Instantiate(effectHit, hit.point, Quaternion.identity);
				AudioSource.PlayClipAtPoint(hitBaton, hit.point);
			}
		}
		private IEnumerator TimerResetSling()
		{
			yield return new WaitForSeconds(.7f);
			if (slingShotHand.activeSelf && StoneAmount > 0)
			{
				slingShotAnim.Play("slingshotReload");
				GetComponent<AudioSource>().PlayOneShot(reloadSlingSFX);
			}
			shotWithSling = false;
		}
		public void CaptureSoulBook()
		{
			hasSoul = true;
			boxColliderNewBook.SetActive(true);
			boxColliderOldBook.SetActive(false);
			StartCoroutine(TimerTextSoulStatus());
		}
		private IEnumerator TimerTextSoulStatus()
		{
			yield return new WaitForSeconds(3.5f);
			TimedTextInitiate("Half of the soul is in the book now. I must use it somehow");
		}
		private void HandleButtonClick() { buttonClicked = true; }
		private void HandleButtonClick1() { buttonClicked2 = true; }
		public void DeathAction()
		{
			CheckItemDropping();
			var text = T_NeedShit.GetComponent<TextManager>();
			if (text == null) return;
			text.Stop();
			T_HighlightItem.SetActive(false);
			ShootT.SetActive(false);
			BlastT.SetActive(false);
			AttackT.SetActive(false);
			ThrowG.SetActive(false);
			ShootB2.SetActive(false);
		}
		// The shotgun is its own drop; everything else goes through the inventory.
		public void CheckItemDropping()
		{
			if (!ShotgunPickHand.activeSelf)
			{
				buttonClicked2 = false;
				Drop1.SetActive(false);
				Inventory.DropLogic();
				return;
			}
			ShotgunPickHand.SetActive(false);
			buttonClicked2 = false;
			Drop1.SetActive(false);
			GameObject dropped = Instantiate(ShotgunDrop, DropP.position, DropP.rotation);
			var body = dropped.GetComponent<Rigidbody>();
			if (body == null) return;
			body.AddForce(dropped.transform.forward * DropForceItem, ForceMode.Impulse);
			ShotgunHandMain.localPosition = ShotgunHandMain_ZeroPos;
			ShotgunHandMain.localEulerAngles = ShotgunHandMain_ZeroRot;
			Vector3 pipes = ShotgunHandPipes.localEulerAngles;
			ShotgunHandPipes.localEulerAngles = new Vector3(9.6f, pipes.y, pipes.z);
		}
		public void FellObjectDining() { TimedTextInitiate("Item fell out of bounds. It has been teleported on the dining table"); }
		public void FellObjectMoat() { TimedTextInitiate("Item fell in the water and it is nearby the car"); }
		public void OffHoldingItemLogic() { buttonClicked2 = false; Drop1.SetActive(false); }
		public void TimedTextInitiate(string t)
		{
			var text = T_NeedShit.GetComponent<TextManager>();
			if (text == null) return;
			text.Stop();
			T_NeedShit.SetActive(true);
			text.Add();
			var label = T_NeedShit.GetComponent<TMP_Text>();
			if (label != null) label.text = t;
		}
		public void StopSetOfTexts()
		{
			var text = T_NeedShit.GetComponent<TextManager>();
			if (text != null) text.Stop();
		}
		public void TurnOffItemTexts() { T_HighlightItem.SetActive(false); }
}
