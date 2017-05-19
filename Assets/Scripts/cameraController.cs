using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	public GameObject trackedObj;

	private playerController myPlayerController;
	private bool isControlEnabled;

	// Use this for initialization
	void Start () {
		myPlayerController = trackedObj.GetComponent<playerController> ();
		isControlEnabled = true;
		
	}

	void FixedUpdate () {
		
			//mouseLook();
	
	}
	// Update is called once per frame
	void Update () {
		isControlEnabled = myPlayerController.GetIsControlEnabled ();

		if(isControlEnabled) {
			float mouseX = Input.GetAxis ("Mouse X");
			float deltaAmplifier = 1.5f;

			transform.RotateAround (trackedObj.transform.position, trackedObj.transform.up, mouseX * deltaAmplifier);

			//Vector3 conversion = transform.TransformPoint (new Vector3 (trackedObj.GetComponent<Transform> ().localPosition.x, trackedObj.GetComponent<Transform> ().localPosition.y + 6f, trackedObj.GetComponent<Transform> ().localPosition.z - 10f));
		//transform.position = conversion;
		}
	}

    void mouseLook()
    {
//        Vector2 delta = new Vector2(0, 0);
//        delta.x = Input.GetAxis("Mouse Y");
//        delta.y = Input.GetAxis("Mouse X");
//        Debug.Log(delta.ToString());
//        float lookMagnitude = 20.0f;
//
//        transform.Rotate((Vector3.up * Time.fixedDeltaTime * delta.y * lookMagnitude) + (Vector3.right * Time.fixedDeltaTime * delta.x * lookMagnitude));
        //transform.Rotate(Vector3.right * Time.fixedDeltaTime * delta.x * lookMagnitude);


    }
}