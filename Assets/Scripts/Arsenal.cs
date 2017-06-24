using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Arsenal : NetworkBehaviour {

	public GameObject pGrenade;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExecuteGadget(Gadget gadget, NetworkInstanceId id) {
		switch (gadget.GetName()) {
		case "grenade":
			CmdThrowGrenade (transform.Find ("turret/face").position, transform.Find ("turret/face").rotation, gadget.GetThrow (), id);
			break;
		case "boost":
			CmdActivateSpeedBoost ();
			break;
		default:
			return;
		}
	}
	public void GetRandomGadget() {
		CmdGetRandomGadget ();
	}

	[Command]
	void CmdGetRandomGadget() {
		int randVal = Random.Range (0, 2);
		TargetSetGadget (connectionToClient, randVal);

	}

	[TargetRpc]
	void TargetSetGadget(NetworkConnection target, int itemVal) {

		Gadget newGadget;
		Gadget curGadget = target.playerControllers [0].gameObject.GetComponent<playerController> ().gadget01;
		switch (itemVal) {
		case 0:
			if (curGadget != null && curGadget.GetName () == "grenade") {
				int newCapacity = curGadget.GetCurCapacity () + (curGadget.GetMaxCapacity () / 2);
				newGadget = new MoldGrenade (newCapacity);
			} else {
				newGadget = new MoldGrenade ();
			}
			break;
		case 1:
			if (curGadget != null && curGadget.GetName () == "boost") {
				int newCapacity = curGadget.GetCurCapacity () + (curGadget.GetMaxCapacity () / 2);
				newGadget = new MoldSpeedBoost (newCapacity);
			} else {
				newGadget = new MoldSpeedBoost ();
			}
			break;
		default:
			return;
		}
		target.playerControllers [0].gameObject.GetComponent<playerController> ().gadget01 = newGadget;
		Debug.Log ("End of Target Set Gadget");

	}

	[Command]
	void CmdThrowGrenade (Vector3 position, Quaternion rotation, float strength, NetworkInstanceId id) {

		GameObject curGrenade = Instantiate (pGrenade, position, rotation, GameObject.Find ("Ammo Container").transform);
		curGrenade.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward  * strength, ForceMode.Impulse);
		curGrenade.GetComponent<Rigidbody> ().angularVelocity = UnityEngine.Random.insideUnitSphere * 5.0f;
		curGrenade.GetComponent<Grenade> ().SetOwnerNetID (id);

		NetworkServer.Spawn (curGrenade);

	}

	[Command]
	public void CmdActivateSpeedBoost() {
		TargetActivateSpeedBoost (connectionToClient);
	}

	[TargetRpc]
	void TargetActivateSpeedBoost(NetworkConnection target) {
		
		target.playerControllers [0].gameObject.GetComponent<VehicleMove> ().SetSpeedMultiplier (2.0f, 2.0f);

	}
}
