using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour {
	
	public float detTime;
	public float timer;
	private NetworkInstanceId ownerNetID;
	private List<NetworkInstanceId> damagedNetID;

	void OnTriggerEnter(Collider col) {
		if (!isServer) {
			return;
		}

		if (col.transform.parent != null && col.transform.parent.CompareTag ("player")) {
			bool alreadyDamaged = false;
			foreach (NetworkInstanceId childID in damagedNetID) {
					if (childID == col.transform.parent.GetComponent<NetworkIdentity> ().netId) {
					alreadyDamaged = true;
				}
			}
			if (alreadyDamaged == false) {
				col.transform.parent.GetComponent<health> ().TakeDamage (25);
				col.transform.parent.GetComponent<materialHandler> ().StartStrobe ();
				//col.transform.parent
				damagedNetID.Add(col.transform.parent.GetComponent<NetworkIdentity> ().netId);
				//Destroy (this.gameObject);
			}
		}
	}

//	void OnTriggerStay(Collider col) {
//		if (!isServer) {
//			return;
//		}
//		if (col.gameObject.CompareTag ("player")) {
//			col.gameObject.GetComponent<health> ().TakeDamage (25);
//			col.gameObject.GetComponent<materialHandler> ().StartStrobe ();
//			//Destroy (this.gameObject);
//		}
//	}

	// Use this for initialization
	void Start () {
		if (!isServer) {
			return;
		}

		detTime = 0.1f;
		timer = 0.0f;

		damagedNetID = new List<NetworkInstanceId> ();


	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}
		if (timer > detTime) {
			Destroy (this.gameObject, 3.0f);
			GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<SphereCollider> ().enabled = false;
		} else {
			timer += Time.deltaTime;
		}

	}
}
