using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class health : NetworkBehaviour {

	public const int maxHealth = 100;
	[SyncVar]
	public int curHealth = maxHealth;

	public void TakeDamage(int amount)
	{
		if (!isServer) {
			return;
		}
		curHealth -= amount;
		if (curHealth <= 0)
		{
			curHealth = 0;
			Debug.Log("Dead!");
		}
	}
}
