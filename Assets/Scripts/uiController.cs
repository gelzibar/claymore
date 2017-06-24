using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SupportLibrary;

public class uiController : MonoBehaviour {

	private GameObject myDropdown;
	private int prevValue;

	private GameObject myCrosshairs;
	public Vector3 turretVector;

	public playerController myPlayer;
	public skinHandler mySkinHandler;
	public GameObject respawnButton;

	public Image activeItemImage;
	public Text activeItemCount;

	public Image chargeMeter;
	public const float chargeHorOffset = 25.0f;
	public const float chargeVertOffset = -25.0f;


	// Pause Menu
	private GameObject myPauseMenu;
	private GameObject myPauseShading;
//	private bool hasAssignedButton;

	public Button serverButton;

	// Use this for initialization
	void Start () {
		myDropdown = GameObject.Find ("/Canvas/Dropdown");
		myCrosshairs = GameObject.Find ("/Canvas/Crosshairs");
		prevValue = myDropdown.GetComponent<Dropdown> ().value;

		if (Application.isEditor) {
			serverButton = GameObject.Find ("Server").GetComponent<Button> ();
			serverButton.onClick.AddListener (() => {
				GameObject.Find ("NetworkManager").GetComponent<HillNetworkManagerHUD> ().StartServer ();
			});
		}

		chargeMeter = GameObject.Find("Canvas/Charge Meter").GetComponent<Image>();

		activeItemImage = GameObject.Find ("Canvas/Items/Active Item").GetComponent<Image> ();
		activeItemCount = GameObject.Find ("Canvas/Items/ItemCount/Text").GetComponent<Text> ();
		turretVector = new Vector3 ();

		myPauseMenu = GameObject.Find ("Pause Menu");
		myPauseShading = GameObject.Find ("Pause Shading");
		SetPause (false);

		respawnButton = GameObject.Find ("Respawn");

//		hasAssignedButton = false;

//		if (hasAssignedButton == false && myPlayer != null) {
//			Debug.Log ("Assigning Button");
//			Debug.Log (myPlayer.ToString ());
//			respawnButton.GetComponent<Button>().onClick.AddListener (() => {myPlayer.GetComponent<playerController> ().Respawn ();});
//			hasAssignedButton = true;
//		}

	}
	
	// Update is called once per frame
	void Update () {
		//		if (hasAssignedButton == false && myPlayer != null) {
//			Debug.Log ("Assigning Button");
//			Debug.Log (myPlayer.ToString ());
//
//			respawnButton.GetComponent<Button>().onClick.AddListener (() => {myPlayer.GetComponent<playerController> ().Respawn ();});
//
//			hasAssignedButton = true;
//		}
		GetSelectedColor ();
		UpdateItems ();
		UpdateMeter ();
		prevValue = myDropdown.GetComponent<Dropdown> ().value;
		//myCrosshairs.transform.position = turretVector;

	}

	void UpdateItems() {
		Sprite newImage = new Sprite();
		int newCount = 0;
		if (myPlayer.gadget01 != null) {
			if (myPlayer.gadget01.GetName () == "grenade") {
				newImage = Resources.Load<Sprite> ("Sprites/bomb_icon");				
			} else if (myPlayer.gadget01.GetName () == "boost") {
				newImage = Resources.Load<Sprite> ("Sprites/boost_icon");	
			}

			newCount = myPlayer.gadget01.GetCurCapacity ();
		} else {
			newImage = Resources.Load<Sprite> ("Sprites/blank_icon");				
		}
		activeItemImage.sprite = newImage;
		activeItemCount.text = "" + newCount;
	}

	void UpdateMeter() {
		Vector3 offsetPos = new Vector3 (turretVector.x + chargeHorOffset, turretVector.y + chargeVertOffset);
		chargeMeter.transform.position = offsetPos;
		if (myPlayer.gadget01 != null) {
			if (myPlayer.gadget01.GetToggleCharge () == true) {
				chargeMeter.enabled = true;
				chargeMeter.fillAmount = myPlayer.gadget01.GetPercentCharge ();
				
			} else if (myPlayer.gadget01.GetToggleCharge () == false) {
				chargeMeter.enabled = false;
			}

			if (myPlayer.gadget01.GetName () == "boost" && myPlayer.myVehicleMove.GetSpeedToggle () == true) {
				chargeMeter.enabled = true;
				chargeMeter.fillAmount = myPlayer.myVehicleMove.GetPercentBoost ();
			} else if (myPlayer.gadget01.GetName () == "boost" && myPlayer.myVehicleMove.GetSpeedToggle () == false) {
				chargeMeter.enabled = false;
			}
		} else if (myPlayer.gadget01 == null) {
			chargeMeter.enabled = false;
		}
	}

	public void RequestRespawn() {
		myPlayer.Respawn ();
		myPlayer.ToggleLockState ();
		myPlayer.ToggleControl ();
		ToggleMenu ();
	}

	public void SetPrevValue(int newValue) {
		prevValue = newValue;
	}

	public int GetPrevValue() {
		return prevValue;
	}

	public int GetCurValue() {
		return myDropdown.GetComponent<Dropdown> ().value;
	}

	public bool CheckForChange() {
		bool localTruth = false;
		if (prevValue != myDropdown.GetComponent<Dropdown> ().value) {
			localTruth = true;
		}
		return localTruth;
	}

	public void GetSelectedColor() {
		if (prevValue != myDropdown.GetComponent<Dropdown> ().value) {
			prevValue = GetCurValue();
			mySkinHandler.SetSkinValue(GetCurValue());
			mySkinHandler.AssignMaterial ();
	
		}
	}

	public void UpdateMyCrosshairs() {
		myCrosshairs.transform.position = turretVector;
	}

	public void ToggleMenu() {
		if (myPauseMenu.activeSelf == false) {
			SetPause (true);
		} else if (myPauseMenu.activeSelf == true) {
			SetPause (false);
		}
	}

	public void SetPause(bool newValue) {
		myPauseMenu.SetActive (newValue);
		myPauseShading.SetActive (newValue);
		myDropdown.SetActive (newValue);
	}

	public void QuitGame() {
		Application.Quit ();
	}


}
