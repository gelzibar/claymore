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

	// Use this for initialization
	void Start () {
		myDropdown = GameObject.Find ("/Canvas/Dropdown");
		myCrosshairs = GameObject.Find ("/Canvas/Crosshairs");
		prevValue = myDropdown.GetComponent<Dropdown> ().value;

		turretVector = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		GetSelectedColor ();

		prevValue = myDropdown.GetComponent<Dropdown> ().value;
		//myCrosshairs.transform.position = turretVector;

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
			
		}
	}

	public void UpdateMyCrosshairs() {
		myCrosshairs.transform.position = turretVector;
	}

	public void QuitGame() {
		Application.Quit ();
	}
}
