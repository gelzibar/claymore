using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class health : NetworkBehaviour {

	public const int maxHealth = 100;
	[SyncVar(hook = "OnChangeHealth")]
	public int curHealth = maxHealth;

	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		curHealth -= amount;
		if (curHealth <= 0)
		{
			curHealth = maxHealth;
			RpcRespawn ();
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
