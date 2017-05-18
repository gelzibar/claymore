using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bulletController : NetworkBehaviour {
	private NetworkInstanceId ownerNetID;

	void OnCollisionEnter(Collision col) {
		if (!isServer) {
			return;
		}
			if (col.gameObject.CompareTag ("player")) {
				if (ownerNetID != col.gameObject.GetComponent<playerController> ().myNetID) {
					col.gameObject.GetComponent<health> ().TakeDamage (25);
					col.gameObject.GetComponent<materialHandler> ().StartStrobe ();
					Destroy (this.gameObject);
				}
			}
	}

	public void SetOwnerNetID(NetworkInstanceId id) {
		ownerNetID = id;
	}

}
