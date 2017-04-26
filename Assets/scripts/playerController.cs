using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour
{

	// Physics and movement
	private Rigidbody myRigidbody;
	private float torque;
	private float accel, decel;
	private float curVelocity;
	private float minVelocity, maxVelocity;
	private List<tire> tires;
	private bool grounded;
	private int groundedRating;

	// Related Objects
	public GameObject myCam;
	public GameObject pBullet;
	private float primaryCD, primaryTimer;
	private bool primaryToggle;

    // Personalization
    public Material myStandardMaterial;

	// Use this for initialization
	void Start ()
	{
        // Only assign active camera to the local player
		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
		}

		//GameObject.Find ("Dropdown").SetActive (false);

        // Physics and movement definitions
        myRigidbody = GetComponent<Rigidbody>();
        torque = 300.0f;
        accel = 3.0f;
		decel = accel * 2;
		maxVelocity = 1000;
		minVelocity = 100;
        myRigidbody.maxAngularVelocity = 2.5f;
        grounded = false;
		groundedRating = 0;


		tires = new List<tire> ();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "tire")
            {
                tires.Add(child.GetComponent<tire>());
            }
        }

		primaryCD = 0.60f;
		primaryTimer = 0;
		primaryToggle = false;

	}

	void OnGUI ()
	{
		if (!isLocalPlayer) {
			return;
		}
		int frames = (int)(1.0f / Time.smoothDeltaTime);


		GUI.Label (new Rect (300, 0, 100, 100), "Health: " + GetComponent<health> ().curHealth); 
		GUI.Label (new Rect (10, 0, 100, 100), frames.ToString ());        
	}

	void FixedUpdate ()
	{
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


		if (!isLocalPlayer) {
			return;
		}

//		bool[] groundedTires = new bool[4];
//		Debug.Log (tires.Count);
//		for (int i = 0; i < tires.Count; i++) {
//			groundedTires[i] = tires[i].GroundViaSphereCast();
//		}
//
//		grounded = groundedTires [0] || groundedTires [1] || groundedTires [2] || groundedTires [3];

        GetPlayerInputFixed();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Disable Butter Trails functionality.
		// UpdateLinePositions ();
		// TrailsToLinePositions ();

		if (!isLocalPlayer) {
			return;
		}
        GetPlayerInputStandard();

	}

    void GetPlayerInputFixed() {
        float forward = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
		float roll = Input.GetAxis ("Roll");

		float groundedAdjust = groundedRating * 0.5f;
        float jumpFactor = 0.5f;
		float inAirFactor = 0.1f;

		Debug.Log (curVelocity + ":" + myRigidbody.velocity);
		if (grounded == true) {
//			if (Input.GetKey (KeyCode.W)) {
//				ForwardVelocity ();
//			} else if (Input.GetKey (KeyCode.S)) {
//				if (curVelocity > 0) {
//					ReduceCurVelocity ();
//				}
//				//BackwardVecloity ();
//			} else {
//				ReduceCurVelocity ();
//			}

			DetermineCurVelocity (forward);
			Vector3 speed = Vector3.forward * curVelocity * groundedAdjust;
			myRigidbody.AddRelativeForce (speed, ForceMode.Force);
			myRigidbody.AddRelativeTorque (Vector3.up * turn * torque * groundedAdjust, ForceMode.Force);

			if (Input.GetKeyDown (KeyCode.Space)) {
				//Debug.Log ("Pressed.");
				myRigidbody.AddRelativeForce (Vector3.up * 400.0f * jumpFactor, ForceMode.Impulse);
			}
			if(Input.GetKey(KeyCode.LeftShift)) {
				ApplyBrake();
			}
		} else if (grounded == false) {
			myRigidbody.AddRelativeTorque (Vector3.right * forward * torque * inAirFactor, ForceMode.Force);
			myRigidbody.AddRelativeTorque (Vector3.up * turn * torque * inAirFactor, ForceMode.Force);
			myRigidbody.AddRelativeTorque (Vector3.forward * roll * torque * inAirFactor, ForceMode.Force);

			ReduceCurVelocity ();
		}
    }

	void DetermineCurVelocity(float input) {

		if (input != 0) {
			IdentifyContextualInput (input);
		} else {
			ReduceCurVelocity ();
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
		float tempV = Mathf.Abs (curVelocity);
		int signed = VelocitySign ();

		if (tempV > decel) {
			curVelocity = (tempV - decel) * signed;
		} else {
			curVelocity = 0;
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

	void ReduceCurVelocity() {
		float tempV = Mathf.Abs (curVelocity);
		int signed = VelocitySign ();

		curVelocity = (tempV - accel) * signed;
	}

	int VelocitySign() {
		int signed = 1;

		if (curVelocity < 0) {
			signed = -1;			
		}

		return signed;
	}

	void ForwardVelocity() {
		
		ChangeVelocity (1);
	}
	void BackwardVecloity() {
//		float inverseMin = minVelocity * -1;
//		float inverseMax = maxVelocity * -1;
//
//		if (curVelocity > inverseMin) {
//			curVelocity = inverseMin;
//		}
//
//		if (curVelocity > (maxVelocity * 1 / 3 * -1)) {
//			curVelocity--;
//		}
		ChangeVelocity(-1);
	}

	void ChangeVelocity(int direction) {
		if (curVelocity < minVelocity) {
			curVelocity = minVelocity;
		}

		if (curVelocity < maxVelocity) {
			curVelocity++;
		}

		curVelocity = curVelocity;
	
	}

	bool MinThreshold(float left, float right) {
		return (left < right);
	}

    void GetPlayerInputStandard() {
        // Shooting Mechanics
		if (primaryToggle == false) {
			if (Input.GetMouseButtonDown (0)) {
				primaryToggle = true;
				CmdFireBullet (transform.Find ("turret_face").position, myRigidbody.rotation);

			}
		} else if (primaryToggle == true && primaryTimer >= primaryCD) {
			primaryToggle = false;
			primaryTimer = 0;
		} else {
			primaryTimer += Time.deltaTime;
		}


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sTrails.enabled = !sTrails.enabled;
            //sTrails.toggle = true;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            float offsetX = transform.rotation.eulerAngles.x * -1;
            float offsetZ = transform.rotation.eulerAngles.z * -1;
            transform.Rotate(new Vector3(offsetX, 0, offsetZ ));

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = new Vector3(-32f, 2f, 11.5f);
        }
    }


	bool GroundViaSphereCast (Vector3 origin)
	{
		RaycastHit hit;

		return Physics.SphereCast (origin, 0.1f, Vector3.down, out hit, 1.4f); 

	}

	public void SetAllMaterial(Material newMaterial) {
		GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("FL_Tire").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("FR_Tire").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("BL_Tire").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("BR_Tire").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("L_Cylinder").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("R_Cylinder").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find ("Face").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
	}

	[Command]
	void CmdFireBullet (Vector3 position, Quaternion rotation)
	{
		GameObject curBullet = Instantiate (pBullet, position, rotation, GameObject.Find ("Ammo Container").transform);
		curBullet.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * 0.05f, ForceMode.Impulse);
		Destroy (curBullet, 2.0f);

		NetworkServer.Spawn (curBullet);
	}


		
}
 