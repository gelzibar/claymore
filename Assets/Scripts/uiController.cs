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

	// Pause Menu
	private GameObject myPauseMenu;
	private GameObject myPauseShading;
	private bool hasAssignedButton;

	// Use this for initialization
	void Start () {
		myDropdown = GameObject.Find ("/Canvas/Dropdown");
		myCrosshairs = GameObject.Find ("/Canvas/Crosshairs");
		prevValue = myDropdown.GetComponent<Dropdown> ().value;

		turretVector = new Vector3 ();

		myPauseMenu = GameObject.Find ("Pause Menu");
		myPauseShading = GameObject.Find ("Pause Shading");
		SetPause (false);

		respawnButton = GameObject.Find ("Respawn");

		hasAssignedButton = false;

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

		prevValue = myDropdown.GetComponent<Dropdown> ().value;
		//myCrosshairs.transform.position = turretVector;

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
