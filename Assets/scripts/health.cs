using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class health : NetworkBehaviour {

	public RectTransform healthDisplay;

	public const int maxHealth = 125;
	public const int startHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int curHealth = startHealth;


	void Start() {
		if (!isLocalPlayer) {
			return;
		}

		healthDisplay = GameObject.Find ("Active Layer").GetComponent<RectTransform> ();
		AdjustHealthBar ();
	}

	void Update() {
		AdjustHealthBar ();
	}
	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		curHealth -= amount;
		if (curHealth <= 0)
		{
			curHealth = startHealth;
			RpcRespawn ();
		}

	}

	public void RecoverHealth(int amount) {
		if (!isServer) {
			return;
		}
		curHealth += amount;
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
		}

	}

	void OnChangeHealth(int currentHealth) {
		curHealth = currentHealth;
	}

	void AdjustHealthBar() {
		if (!isLocalPlayer) {
			return;
		}

		healthDisplay.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)curHealth / maxHealth * 290);
	}

	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			// move back to World Coord zero
			transform.position = new Vector3(0.0f, 252.5f, 0.0f);
		}
	}
}
