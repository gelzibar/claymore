using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class materialHandler : NetworkBehaviour {


	// Damage Effect
	public Material whiteMaterial, defaultMaterial, blackMaterial;

	[SyncVar(hook = "OnChangeStrobeToggle")]
	public bool strobeToggle;

	[SyncVar(hook = "OnChangeStrobeTime")]
	public float strobeTime;

	// Use this for initialization
	void Start () {

		if (!isLocalPlayer) {
			return;
		}

		StopStrobe ();
	}
	
	// Update is called once per frame
	void Update () {

		if (strobeToggle == true) {
			GetComponent<playerController>().SetAllMaterial(whiteMaterial);
		} else if (strobeToggle == false) {
			GetComponent<playerController>().SetAllMaterial(defaultMaterial);
		}

		if (!isLocalPlayer) {
			return;
		}
		if(strobeToggle == true) {
//			CmdCheckTime ();
		}
		
	}

	public void StartStrobe() {
		SendStrobe (true, 0);
	}

	public void StopStrobe() {

		SendStrobe (false, 0);
	}

	public void SendStrobe(bool newToggle, float newTime) {
		CmdSetStrobeToggle (newToggle);
		CmdSetStrobeTime (newTime);
	}

	public void ToggleStrobe() {
		if (strobeToggle == true) {
			StopStrobe ();
		} else if (strobeToggle == false) {
			StartStrobe ();
		}
	}



	// Will probably delete this. Caused too much trouble
	Material SwapStrobe(Material curMaterial) {
		Material newMaterial = defaultMaterial;
		if (curMaterial.Equals(defaultMaterial)) {
			newMaterial = whiteMaterial;
		} else if (curMaterial.Equals(blackMaterial)) {
			newMaterial = whiteMaterial;
		} else if (curMaterial.Equals(whiteMaterial)) {
			newMaterial = blackMaterial;
		}

		return newMaterial;
	}

	// Will probably delete this. Caused too much trouble
//	public void DamageStrobeEffect (float timeDiff) {
//		MeshRenderer myMesh = GetComponent<MeshRenderer> ();
//		//myMesh.sharedMaterial = whiteMaterial;
//		float localTime = Mathf.Round (timeDiff * 100);

//		if (localTime > 500) {
//			myMesh.sharedMaterial = defaultMaterial; 
//			if (isLocalPlayer) {
//				EndStrobe ();
//			}
//		} else if (localTime % 10 == 0) {
//			myMesh.sharedMaterial = whiteMaterial;
//			//myMesh.sharedMaterial = SwapStrobe (myMesh.sharedMaterial);
//		}
//	}
		

	void OnChangeStrobeToggle(bool newToggle) {
		strobeToggle = newToggle;
	}

	void OnChangeStrobeTime(float newTime) {
		strobeTime = newTime;
	}

	[Command]
	void CmdSetStrobeToggle (bool newToggle)
	{
		strobeToggle = newToggle;
	}

	[Command]
	void CmdSetStrobeTime (float newTime) {
		strobeTime = newTime;
	}

//	[Command]
//	void CmdCheckTime() {
//		Debug.Log (Network.time + ":" + strobeTime);
//		if (Network.time - strobeTime >= .50f) {
//			RpcStopStrobe ();
//		}
//	}

//	[ClientRpc]
//	void RpcStopStrobe () {
//		StopStrobe ();
//	}


}
