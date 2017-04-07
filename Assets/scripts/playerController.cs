using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour {


	private Rigidbody myRigidBody;
	public float torque;
	public float accel;
	public GameObject myCam;

	public GameObject fltire, frtire, bltire, brtire;

	void OnGUI()
	{
		int frames = (int)(1.0f / Time.smoothDeltaTime);
		GUI.Label(new Rect(10, 0, 100, 100), frames.ToString());        
	}

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();

		if (!isLocalPlayer) {
			myCam.SetActive (false);
		}


		torque = 500.0f;
		accel = 800.0f;

	}

	void FixedUpdate() {
//		if(Input.GetKey(KeyCode.W)) {
//			myRigidBody.AddForce ((Vector3.back + Vector3.down) * 10f);
//		}
		if (!isLocalPlayer) {
			return;
		}
			float forward = Input.GetAxis ("Vertical");
			float turn = Input.GetAxis ("Horizontal");


		//myRigidBody.AddRelativeForce (Vector3.down * accel * forward, ForceMode.Acceleration);
		myRigidBody.AddRelativeTorque (Vector3.up * turn * torque * Time.fixedDeltaTime);
		myRigidBody.AddRelativeForce (Vector3.forward * accel * forward * Time.fixedDeltaTime, ForceMode.Force);
		//myRigidBody.AddForce (Vector3.up * 1);
//		Vector3 movement = new Vector3 (gameObject.transform.position.x + turn * torque, gameObject.transform.position.y, gameObject.transform.position.z + forward * accel );
//
//
//		myRigidBody.MovePosition (movement);
//		//myRigidBody.AddRelativeForce (Vector3.back * forward * accel);
////		tire01.GetComponent<Rigidbody>().AddForce (Vector3.back * forward * accel);
////		tire02.GetComponent<Rigidbody>().AddForce (Vector3.back * forward * accel);
////		myRigidBody.AddForce (Vector3.back * forward * accel);
//		fltire.GetComponent<WheelCollider>().steerAngle = turn * torque;
//		frtire.GetComponent<WheelCollider>().steerAngle = turn * torque;
//		bltire.GetComponent<WheelCollider>().steerAngle = turn * torque;
//		brtire.GetComponent<WheelCollider>().steerAngle = turn * torque;
//
////		tire01.GetComponent<Rigidbody>().AddTorque(transform.up * torque * turn);
////		tire02.GetComponent<Rigidbody>().AddTorque(transform.up * torque * turn);
//		fltire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
//		frtire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
//		bltire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
//		brtire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;





	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
		{
			return;
		}
	}
}
