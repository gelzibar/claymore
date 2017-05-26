using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrateAltar : NetworkBehaviour {


	public GameObject pPowerup;
	private GameObject attachedPowerup;
	private float respawn;
	private float timer;

	public Material defaultMaterial;

	// Use this for initialization
	void Start () {
		if (!isServer) {
			return;
		}
		respawn = pPowerup.GetComponent<Crate>().GetRespawn ();

		timer = 0;


		//Set Color to white for all
		transform.Find("pAltar").GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
		//NewPowerup ();

	}

	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		if (attachedPowerup == null) {
			timer += Time.deltaTime;

			if (timer >= respawn) {
				NewPowerup ();
				timer = 0;
			}
		}

	}

	void DestroyPowerup() {
		if (!isServer) {
			return;
		}
		Destroy (transform.FindChild (attachedPowerup.name).gameObject);
	}

	void NewPowerup() {
		if (!isServer) {
			return;
		}
		Vector3 position = new Vector3 (transform.position.x, transform.position.y + pPowerup.transform.position.y, transform.position.z);
		GameObject powerup = Instantiate (pPowerup, position, pPowerup.transform.rotation, transform);
		attachedPowerup = powerup;
		NetworkServer.Spawn (powerup);
		RpcPowerupProperties (powerup, gameObject);

	}

	[ClientRpc]
	void RpcPowerupProperties(GameObject powerup, GameObject parent) {
		powerup.transform.parent = parent.transform;

	}
}
