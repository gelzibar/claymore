using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bulletController : NetworkBehaviour {
	private NetworkInstanceId ownerNetID;

	public GameObject pScorch;
	public GameObject pCircle, pCircle2;

	void OnCollisionEnter(Collision col) {
		if (!isServer) {
			return;
		}
			if (col.gameObject.CompareTag ("player")) {
				if (ownerNetID != col.gameObject.GetComponent<playerController> ().myNetID) {
					col.gameObject.GetComponent<health> ().TakeDamage (25);
				col.gameObject.GetComponent<DamageFlash> ().StartStrobe ();
					Destroy (this.gameObject);
				}
			}
		CmdCreateScorch (col.contacts[0].point, col.contacts[0].normal);

	}

	public void SetOwnerNetID(NetworkInstanceId id) {
		ownerNetID = id;
	}

	[Command]
	public void CmdCreateScorch(Vector3 position, Vector3 normal) {
		RpcCreateScorch (position, normal);
	}

	[ClientRpc]
	public void RpcCreateScorch(Vector3 position, Vector3 normal) {
		Quaternion zeroRot = new Quaternion ();
//		Instantiate (pCircle2, position + normal, zeroRot);
//		Instantiate (pCircle, position, zeroRot);

		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		Vector3 forwardPos = Vector3.Lerp (position, position + normal, 0.1f);
		GameObject curScorch = Instantiate (pScorch, forwardPos, rotation);

		}

}
