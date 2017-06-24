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

	private float speedMultiplier, speedDuration;
	private bool speedToggle;
	private const float speedDefault = 1.0f;

	float forward;
	float turn;
	float roll;
	float groundedAdjust;

	bool isBraking;
	bool jumpTrigger;

	private playerController myPlayerController;
	bool isControlEnabled;

	// Use this for initialization
	void Start () {
		// Physics and movement definitions
		myRigidbody = GetComponent<Rigidbody>();
		torque = 8.0f;
		accel = 3.0f;
		decel = accel * 3.5f;
		maxVelocity = 450f;
		minVelocity = 100f;
		//myRigidbody.maxAngularVelocity = 2.5f;
		grounded = false;
		groundedRating = 0;

		speedMultiplier = 1.0f;
		speedDuration = 0.0f;
		speedToggle = false;

		forward = 0.0f;
		turn = 0.0f;
		roll = 0.0f;
		groundedAdjust = 0.0f;

		isBraking = false;
		jumpTrigger = false;
		isControlEnabled = true;

		myPlayerController = transform.root.GetComponent<playerController> ();

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

	}

	void FixedUpdate() {
		if (isControlEnabled == true) {
			Move ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		isControlEnabled = myPlayerController.GetIsControlEnabled ();
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

		if (isControlEnabled == true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				jumpTrigger = true;
			}
		}


		isBraking = false;

		if (speedDuration > 0.0f && speedToggle == true) {
			speedDuration -= Time.deltaTime;
		} else if (speedDuration <= 0.0f && speedToggle == true) {
			ResetSpeedMultiplier ();
		}
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
		myRigidbody.AddRelativeForce (speed * speedMultiplier, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.up * turn * torque * groundedAdjust, ForceMode.Acceleration);

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
		float torqueMulti = 30.0f;
		float torqueAdjust = torque * torqueMulti;

		myRigidbody.AddRelativeTorque (Vector3.right * forward * torqueAdjust * inAirFactor, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.up * turn * torqueAdjust * inAirFactor, ForceMode.Force);
		myRigidbody.AddRelativeTorque (Vector3.forward * roll * torqueAdjust * inAirFactor, ForceMode.Force);

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

		float slowReverse = 0.50f;

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

	public void SetSpeedMultiplier(float amount, float duration) {
		speedMultiplier = amount;
		speedDuration = duration;
		speedToggle = true;
	}

	public void ResetSpeedMultiplier() {
		speedMultiplier = speedDefault;
		speedDuration = 0.0f;
		speedToggle = false;
	}

	public bool GetSpeedToggle() {
		return speedToggle;
	}

	public float GetPercentBoost() {
		return speedDuration / 2.0f;
	}
}
