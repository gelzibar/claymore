﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {


	private Rigidbody myRigidBody;
	public float torque;
	public float accel;

	public GameObject fltire, frtire, bltire, brtire;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();

		torque = 250.0f;
		accel = 100.0f;

	}

	void FixedUpdate() {
//		if(Input.GetKey(KeyCode.W)) {
//			myRigidBody.AddForce ((Vector3.back + Vector3.down) * 10f);
//		}

		float forward = Input.GetAxis("Vertical");
		float turn = Input.GetAxis("Horizontal");
		//myRigidBody.AddRelativeForce (Vector3.back * forward * accel);
//		tire01.GetComponent<Rigidbody>().AddForce (Vector3.back * forward * accel);
//		tire02.GetComponent<Rigidbody>().AddForce (Vector3.back * forward * accel);
//		myRigidBody.AddForce (Vector3.back * forward * accel);
		fltire.GetComponent<WheelCollider>().steerAngle = turn * torque;
		frtire.GetComponent<WheelCollider>().steerAngle = turn * torque;
		bltire.GetComponent<WheelCollider>().steerAngle = turn * torque;
		brtire.GetComponent<WheelCollider>().steerAngle = turn * torque;

//		tire01.GetComponent<Rigidbody>().AddTorque(transform.up * torque * turn);
//		tire02.GetComponent<Rigidbody>().AddTorque(transform.up * torque * turn);
		fltire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
		frtire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
		bltire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;
		brtire.GetComponent<WheelCollider>().motorTorque = accel * 250.0f;





	}
	
	// Update is called once per frame
	void Update () {
		
	}
}