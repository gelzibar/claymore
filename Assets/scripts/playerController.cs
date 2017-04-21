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


	// Trail
	const int trailMaxSize = 10;
	public GameObject myLineRendererObject;
	private LineRenderer myLineRenderer;
	public List<Vector3> trails;
	public List<GameObject> slickTrails;
	public GameObject pSlickTrail;
	private int trailIndex, prevTrailIndex;
	private float minDistance;
	private trail_maker sTrails;
	private GameObject exhaust;

	// Sound
	public GameObject sound_trigger;

	// Use this for initialization
	void Start ()
	{
        // Only assign active camera to the local player
		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
		}

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

        sTrails = GetComponent<trail_maker> ();
		myLineRenderer = GetComponentInChildren<LineRenderer> ();
		exhaust = transform.Find ("exhaust").gameObject;
		sTrails.enabled = false;
		sTrails.toggle = false;

		trails = new List<Vector3> ();
		trailIndex = 1;
		prevTrailIndex = 0;
		minDistance = 10.0f;
		slickTrails = new List<GameObject> ();
		SetInitialLine ();
		TrailsToLinePositions ();

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
        if (Input.GetMouseButtonDown(0))
        {
            CmdFireBullet(transform.Find("turret_face").position, myRigidbody.rotation);
			AudioSource audio = sound_trigger.GetComponent<AudioSource> ();
			audio.Play ();

        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sTrails.enabled = !sTrails.enabled;
            //sTrails.toggle = true;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.rotation = new Quaternion();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = new Vector3(-32f, 2f, 11.5f);
        }
    }

	void TrailInbounds ()
	{
		int maxIndex = trailMaxSize - 1;

		if (trailIndex > maxIndex) {
			trails.RemoveAt (0);
			trails.Add (new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z));

			trailIndex--;
			prevTrailIndex--;
			//trailIndex = 0;
		}
	}

	bool CompareLinePositions ()
	{
		bool returnValue = false;
		float curDistance = Vector3.Distance (myRigidbody.position, trails [prevTrailIndex]);

		if (curDistance > minDistance) {
			returnValue = true;
		}

		return returnValue;
	}

	void UpdateLinePositions ()
	{
		Vector3 curPosition = new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z);

		if (sTrails.enabled == false) {
			if (sTrails.toggle == true) {
				trails.Clear ();
				SetInitialLine ();
				trailIndex = 1;
				prevTrailIndex = 0;

				sTrails.toggle = false;
			}
			trails [trailIndex] = curPosition;

			if (CompareLinePositions ()) {
				// Create trail/line objects for trailIndex.
				if (trails.Capacity < trailMaxSize) {
					trails.Add (curPosition);
					LineRendererAdd (curPosition);
				} else if (trails.Count < trails.Capacity && trails.Count < trailMaxSize) {
					trails.Add (curPosition);
					LineRendererAdd (curPosition);
				}

				trailIndex++;
				prevTrailIndex++;
				TrailInbounds ();
				trails [prevTrailIndex] = curPosition;

				slickTrails.Add (Instantiate (pSlickTrail, curPosition, myRigidbody.rotation));

			}
		}
	}

	void SetInitialLine ()
	{
		Vector3 initialLine = new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z);
		trails.Add (initialLine);
		trails.Add (initialLine);
		myLineRenderer.numPositions = trails.Count;
	}

	void TrailsToLinePositions ()
	{
		for (int i = 0; i < myLineRenderer.numPositions; i++) {
			myLineRenderer.SetPosition (i, trails [i]);
		}
	}

	void LineRendererAdd (Vector3 vector)
	{
		myLineRenderer.numPositions++;
		myLineRenderer.SetPosition (myLineRenderer.numPositions - 1, vector);
	}

	bool GroundViaSphereCast (Vector3 origin)
	{
		RaycastHit hit;

		return Physics.SphereCast (origin, 0.1f, Vector3.down, out hit, 1.4f); 

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
 