using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class materialHandler : NetworkBehaviour {

	// Damage Effect
	public Material whiteMaterial, blackMaterial;
    public float strobeLocalTime;

	playerController myPC;

    [SyncVar(hook = "OnChangeStrobeToggle")]
	public bool strobeToggle;

	// Use this for initialization
	void Start () {

		myPC = GetComponent<playerController> ();
		if (!isLocalPlayer) {
			return;
		}

		StopStrobe ();
	}
	
	// Update is called once per frame
	void Update () {

		if (strobeToggle == true) {
			myPC.SetAllMaterial(whiteMaterial);
		} else if (strobeToggle == false) {
			myPC.SetAllMaterial(myPC.myStandardMaterial);
		}

		if (!isLocalPlayer) {
			return;
		}
		if(strobeToggle == true) {
            if (strobeLocalTime >= 0.5f) {
                StopStrobe();
                strobeLocalTime = 0;
            }
            strobeLocalTime += Time.deltaTime;
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
        strobeLocalTime = newTime;
	}

	public void ToggleStrobe() {
		if (strobeToggle == true) {
			StopStrobe ();
		} else {
			StartStrobe ();
		}
	}



	// Will probably delete this. Caused too much trouble
	//Material SwapStrobe(Material curMaterial) {
  //      Material newMaterial = defaultMaterial;
  //      if (curMaterial.Equals(defaultMaterial)) {
  //          newMaterial = whiteMaterial;
		//} else if (curMaterial.Equals(blackMaterial)) {
		//	newMaterial = whiteMaterial;
		//} else if (curMaterial.Equals(whiteMaterial)) {
		//	newMaterial = blackMaterial;
		//}

		//return newMaterial;
	//}

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

	[Command]
	void CmdSetStrobeToggle (bool newToggle)
	{
		strobeToggle = newToggle;
	}

}
