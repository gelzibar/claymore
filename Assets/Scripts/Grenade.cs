using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grenade : Gadget {

	public float detTime;
	public float timer;
	private NetworkInstanceId ownerNetID;

	public GameObject pExplosion;
	public static float maxCD = 3.0f;


	void OnCollisionEnter() {
		GetComponent<AudioSource> ().Play ();
		GetComponent<AudioSource> ().pitch += 0.08f;
	}

	// Use this for initialization
	void Start () {
		if (!isServer) {
			return;
		}

		detTime = 3.0f;
		timer = 0.0f;

		curCooldownTimer = 0.0f;
		maxCooldownTimer = 1.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}
		if (timer > detTime) {
			GenerateExplosion ();
			Destroy (this.gameObject);
		} else {
			timer += Time.deltaTime;
		}
		
	}
	public void SetOwnerNetID(NetworkInstanceId id) {
		ownerNetID = id;
	}

	void GenerateExplosion () {
		GameObject curExplosion = Instantiate (pExplosion, transform.position, transform.rotation, GameObject.Find ("Ammo Container").transform);
		//curExplosion.GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward + Vector3.up) * 0.10f, ForceMode.Impulse);
		//curExplosion.GetComponent<Grenade> ().SetOwnerNetID (id);
		//Destroy (curBullet, 2.0f);

		NetworkServer.Spawn (curExplosion);
	}

}
