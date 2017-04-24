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
	private float accel;
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
        accel = 400.0f;
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
//		bool[] groundedTires = new bool[4];
//		Debug.Log (tires.Count);
//		for (int i = 0; i < tires.Count; i++) {
//			groundedTires[i] = tires[i].GroundViaSphereCast();
//		}
//
//		grounded = groundedTires [0] || groundedTires [1] || groundedTires [2] || groundedTires [3];

		if (grounded == true) {
			if (!isLocalPlayer) {
				return;
			}
            GetPlayerInputFixed();
		}
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

		float groundedAdjust = groundedRating * 0.5f;

		myRigidbody.AddRelativeForce(Vector3.forward * accel * forward * groundedAdjust, ForceMode.Force);
		myRigidbody.AddRelativeTorque(Vector3.up * turn * torque  * groundedAdjust, ForceMode.Force);
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
 