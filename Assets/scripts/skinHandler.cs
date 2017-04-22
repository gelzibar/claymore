using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class skinHandler : NetworkBehaviour {

    public Material butterMat, blueMat, playerMat;

    [SyncVar(hook = "OnChangeSkinValue")]
    private int skinValue;

	// Use this for initialization
	void Start () {
        playerMat = GetComponent<playerController>().myStandardMaterial;
        SetSkinValue(0);
        AssignMaterial();
	}
	
	// Update is called once per frame
	void Update () {
    }

   public void AssignMaterial() {
        switch(skinValue) {
            case 0:
                Debug.Log("Executing Case 0");
                GetComponent<playerController>().myStandardMaterial = butterMat;
                break;
            case 1:
                Debug.Log("Executing Case 1");
                GetComponent<playerController>().myStandardMaterial = blueMat;
                break;
        }
    }

    public void SetSkinValue(int newSkinValue) {
        CmdSetSkinValue(newSkinValue);
    }
    
    void OnChangeSkinValue(int newSkinValue) {
        skinValue = newSkinValue;
    }
    [Command]
    void CmdSetSkinValue(int newSkinValue) {
        skinValue = newSkinValue;
    }
}
