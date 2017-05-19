using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SupportLibrary;

public class skinHandler : NetworkBehaviour {

	public Material playerMat, butterMat, blackMat, blueMat, grassMat, grayMat, 
	greenMat, orangeMat, pinkMat, purpleMat, redMat, yellowMat;
	private List<Material> matList;

	private int oldSkin;

	public uiController uiCon;

    [SyncVar(hook = "OnChangeSkinValue")]
    private int skinValue;

	// Use this for initialization
	void Start () {

		uiCon = GameObject.Find ("UIManager").GetComponent<uiController>();
		uiCon.mySkinHandler = this;
        playerMat = GetComponent<playerController>().myStandardMaterial;

		BuildMatList ();
		if (!isLocalPlayer) {
			return;
		}
		SetSkinValue(uiCon.GetCurValue());
        AssignMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		if (oldSkin != skinValue) {
			AssignMaterial ();
			oldSkin = skinValue;
		}
    }

   public void AssignMaterial() {
        switch(skinValue) {
		case (int)colorPalette.butter:
			SetNewMatAndApply (matList[(int)colorPalette.butter]);
			break;
		case (int)colorPalette.black:
			SetNewMatAndApply (matList[(int)colorPalette.black]);
			break;
		case (int)colorPalette.blue:
			SetNewMatAndApply (matList[(int)colorPalette.blue]);
			break;
		case (int)colorPalette.grass:
			SetNewMatAndApply (matList[(int)colorPalette.grass]);
			break;
		case (int)colorPalette.gray:
			SetNewMatAndApply (matList[(int)colorPalette.gray]);
			break;
		case (int)colorPalette.green:
			SetNewMatAndApply (matList[(int)colorPalette.green]);
			break;
		case (int)colorPalette.orange:
			SetNewMatAndApply (matList[(int)colorPalette.orange]);
			break;
		case (int)colorPalette.pink:
			SetNewMatAndApply (matList[(int)colorPalette.pink]);
			break;
		case (int)colorPalette.purple:
			SetNewMatAndApply (matList[(int)colorPalette.purple]);
			break;
		case (int)colorPalette.red:
			SetNewMatAndApply (matList[(int)colorPalette.red]);
			break;
		case (int)colorPalette.yellow:
			SetNewMatAndApply (matList[(int)colorPalette.yellow]);
			break;
		default:
				SetNewMatAndApply (matList[(int)colorPalette.butter]);
			break;
        }
    }

	void BuildMatList() {
		matList = new List<Material> (new Material[] {butterMat, blackMat, blueMat, grassMat, grayMat, 
			greenMat, orangeMat, pinkMat, purpleMat, redMat, yellowMat		
		});
	}

	void SetNewMatAndApply(Material mat) {
		playerController myPC = GetComponent<playerController> ();
		GetComponent<playerController>().myStandardMaterial = mat;
		myPC.SetAllMaterial(myPC.myStandardMaterial);
	}

    public void SetSkinValue(int newSkinValue) {
		oldSkin = skinValue;
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
