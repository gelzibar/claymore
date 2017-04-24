using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SupportLibrary;

public class uiController : MonoBehaviour {

	private GameObject myDropdown;
	private int prevValue;

	// Use this for initialization
	void Start () {
		myDropdown = GameObject.Find ("/Canvas/Dropdown");
		prevValue = myDropdown.GetComponent<Dropdown> ().value;


	}
	
	// Update is called once per frame
	void Update () {
		GetSelectedColor ();

		prevValue = myDropdown.GetComponent<Dropdown> ().value;
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
}
