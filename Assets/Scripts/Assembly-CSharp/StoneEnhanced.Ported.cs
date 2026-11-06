using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Ported from Annotated-Pseudocode/StoneEnhanced.c (Granny 3 Evolved, IL2CPP x64).
public partial class StoneEnhanced
{
		// A slingshot stone with the equipped skin's look and impact effect.
		private void Start()
		{
			if (PlayerPrefs.GetInt("Slingshot_MagentaON") == 1) Skin(stoneMagenta, stoneMagentaTexture);
			if (PlayerPrefs.GetInt("Slingshot_LavaON") == 1) Skin(stoneLava, stoneLavaTexture);
			if (PlayerPrefs.GetInt("Slingshot_IceON") == 1) Skin(stoneIcy, stoneIcyTexture);
			var player = GameObject.Find("Player");
			var cam = GameObject.Find("Main Camera");
			if (grannyShot) return;
			// The player's own stones never hit the player.
			var own = GetComponent<Collider>();
			if (!own || player == null) return;
			Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
			Physics.IgnoreCollision(cam.GetComponent<Collider>(), GetComponent<Collider>());
		}
		private void Skin(Mesh mesh, Texture texture)
		{
			mf.mesh = mesh;
			mf.GetComponent<Renderer>().material.mainTexture = texture;
		}
		private GameObject SkinImpact()
		{
			if (PlayerPrefs.GetInt("Slingshot_MagentaON") == 1) return impactPower;
			if (PlayerPrefs.GetInt("Slingshot_LavaON") == 1) return impactLava;
			if (PlayerPrefs.GetInt("Slingshot_IceON") == 1) return impactIce;
			return null;
		}
		private void OnCollisionEnter(Collision other)
		{
			if (!onCollision) return;
			Instantiate(impactSmoke, transform.position, Quaternion.identity);
			var skin = SkinImpact();
			if (skin != null)
			{
				Instantiate(skin, transform.position, Quaternion.identity);
				onCollision = false;
				Destroy(gameObject);
				return;
			}
			if (other.gameObject.tag == "Player" || other.gameObject.name == "CheckResetFloor")
				Instantiate(impactSmoke, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		// Granny's stones knock the player down (unless falling, landing or jumping through the window).
		private void OnTriggerEnter(Collider other)
		{
			if (!onCollision || !grannyShot) return;
			var manage = GameObject.Find("PlayerManage");
			if (other.gameObject.tag != "Player" && other.gameObject.name != "CheckResetFloor") return;
			var window = GameObject.Find("WindowLogic");
			var status = manage ? manage.GetComponent<PlayerStatus>() : null;
			if (status == null) return;
			if (!status.Killed)
			{
				var falling = status.Player ? status.Player.GetComponent<FallingHolder>() : null;
				if (falling == null) return;
				var jumping = window ? window.GetComponent<WindowJumping>() : null;
				if (!falling.isFalling && !falling.isLanding)
				{
					if (jumping == null) return;
					if (!jumping.IsJumping) falling.HandleLanding(true);
				}
			}
			var skin = SkinImpact();
			if (skin == null)
			{
				Instantiate(impactSmokeNoHits, transform.position, Quaternion.identity);
				Destroy(gameObject);
				return;
			}
			Instantiate(skin, transform.position, Quaternion.identity);
			onCollision = false;
			Destroy(gameObject);
		}
}
