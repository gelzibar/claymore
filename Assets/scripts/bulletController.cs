using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bulletController : NetworkBehaviour {

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag ("player")) {
			col.gameObject.GetComponent<health> ().TakeDamage (25);
			col.gameObject.GetComponent<materialHandler> ().StartStrobe ();
			Destroy (this.gameObject);
		}
	}

}
