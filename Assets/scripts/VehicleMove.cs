using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VehicleMove : NetworkBehaviour {

	// Physics and movement
	private Rigidbody myRigidbody;
	private float torque;
	private float accel, decel;
	private float curVelocity;
	private float minVelocity, maxVelocity;
	private List<tire> tires;
	private bool grounded;
	private int groundedRating;

	float forward;
	float turn;
	float roll;
	float groundedAdjust;

	bool isBraking;
	bool jumpTrigger;

	// Use this for initialization
	void Start () {
		// Physics and movement definitions
		myRigidbody = GetComponent<Rigidbody>();
		torque = 300.0f;
		accel = 3.0f;
		decel = accel * 3.5f;
		maxVelocity = 750;
		minVelocity = 100;
		myRigidbody.maxAngularVelocity = 2.5f;
		grounded = false;
		groundedRating = 0;

		forward = 0.0f;
		turn = 0.0f;
		roll = 0.0f;
		groundedAdjust = 0.0f;

		isBraking = false;
		jumpTrigger = false;

		tires = new List<tire> ();
		foreach (Transform child in transform)
		{
			if (child.gameObject.tag == "tire")
			{
				tires.Add(child.GetComponent<tire>());
			}
		}
	}

	public void ChildGUI() {
//		GUI.Label (new Rect (400, 0, 100, 100), "curVelocity: " + curVelocity); 

	}

	void FixedUpdate() {
		Move ();
	}
	
	// Update is called once per frame
	void Update () {
		grounded = false;
		groundedRating = 0;
		foreach (tire tire in tires) {
			if (grounded == false) {
				grounded = tire.grounded;
			}
			if (tire.grounded == true) {
				groundedRating++;
			}


		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			jumpTrigger = true;
		}


		isBraking = false;
	}

	void Move() {
		if (!isLocalPlayer) {
			return;
		}
		forward = Input.GetAxis("Vertical");
		turn = Input.GetAxis("Horizontal");
		roll = Input.GetAxis ("Roll");

		groundedAdjust = groundedRating * 0.5f;

		if (grounded == true) {
			GroundedMovement ();
		} else if (grounded == false) {
			AirborneMovement ();
		}
	}

	void GroundedMovement() {
		float jumpFactor = 0.5f;

		DetermineCurVelocity (forward);
		Vector3 speed = Vector3.forward * curVelocity * groundedAdjust;
		myRigidbody.AddRelativeForce (speed, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.up * turn * torque * groundedAdjust, ForceMode.Force);

		if (jumpTrigger == true) {
			myRigidbody.AddRelativeForce (Vector3.up * 400.0f * jumpFactor, ForceMode.Impulse);
			jumpTrigger = false;
		}
		if(Input.GetKey(KeyCode.LeftShift)) {
			ApplyBrake();
		}
	}

	void AirborneMovement() {
		float inAirFactor = 0.1f;

		myRigidbody.AddRelativeTorque (Vector3.right * forward * torque * inAirFactor, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.up * turn * torque * inAirFactor, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.forward * roll * torque * inAirFactor, ForceMode.Force);

		Vector3 speed = Vector3.forward * curVelocity * groundedAdjust;

		myRigidbody.AddRelativeForce (speed, ForceMode.Force);

		ReduceCurVelocity (decel);
	}

	void DetermineCurVelocity(float input) {

		if (input != 0) {
			IdentifyContextualInput (input);
		} else {
			ReduceCurVelocity (accel);
		}

		CheckVelocityThreshold (input);
	}

	void IdentifyContextualInput(float input) {
		int inputSign = 1;
		if (input < 0) {
			inputSign = -1;
		}

		if (curVelocity * inputSign >= 0) {
			curVelocity += (accel * inputSign);
		} else if (curVelocity * inputSign < 0) {
			ApplyBrake ();
		}
	}

	void ApplyBrake() {
		if (isBraking == false) {
			isBraking = true;
			float tempV = Mathf.Abs (curVelocity);
			int signed = VelocitySign ();

			if (tempV > decel) {
				curVelocity = (tempV - decel) * signed;
			} else {
				curVelocity = 0;
			}
		}
	}

	void CheckVelocityThreshold(float input) {
		float curV = Mathf.Abs (curVelocity);
		float minV = Mathf.Abs (minVelocity);
		float maxV = Mathf.Abs (maxVelocity);
		int signed = VelocitySign ();
		bool braking = false;

		float slowReverse = 0.25f;

		if (input <= 0 && curVelocity < 0) {
			minV *= slowReverse;
			maxV *= slowReverse;
		}


		if (curVelocity > 0 && input < 0 || curVelocity < 0 && input > 0) {
			braking = true;	
		}

		if (curV < minV && input != 0 && braking == false) {
			curVelocity = minV * signed;
		} else if (curV <= minV && input == 0 || curV <= minV && braking == true) {
			curVelocity = 0;
		}

		if (curV > maxV) {
			curVelocity = maxV * signed;
		}
	}

	void ReduceCurVelocity(float delta) {
		float tempV = Mathf.Abs (curVelocity);
		int signed = VelocitySign ();

		curVelocity = (tempV - delta) * signed;
	}

	int VelocitySign() {
		int signed = 1;

		if (curVelocity < 0) {
			signed = -1;			
		}

		return signed;
	}
}
