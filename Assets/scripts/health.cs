using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class health : NetworkBehaviour {

	public const int maxHealth = 125;
	public const int startHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int curHealth = startHealth;

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

	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			// move back to World Coord zero
			transform.position = new Vector3(-32f, 2f, 11.5f);
		}
	}
}
