using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour
{

	// Physics and movement
	private Rigidbody myRigidBody;
	private float torque;
	private float accel;
	private List<tire> tires;
	private bool grounded;

	// Related Objects
	public GameObject myCam;
	public GameObject pBullet;


	// Trail
	const int trailMaxSize = 500;
	public GameObject myLineRendererObject;
	private LineRenderer myLineRenderer;
	public List<Vector3> trails;
	public List<GameObject> slickTrails;
	public GameObject pSlickTrail;
	private int trailIndex, prevTrailIndex;
	private float minDistance;
	//private bool disableTrail, disableTrailToggle;
	private trail_maker sTrails;
	private GameObject exhaust;

	// Use this for initialization
	void Start ()
	{
		myRigidBody = GetComponent<Rigidbody> ();

		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
		} 

		sTrails = GetComponent<trail_maker> ();
		myLineRenderer = GetComponentInChildren<LineRenderer> ();
		exhaust = transform.Find ("exhaust").gameObject;
		sTrails.enabled = false;
		sTrails.toggle = false;

		torque = 300.0f;
		accel = 400.0f;
		myRigidBody.maxAngularVelocity = 2.5f;
		grounded = false;

		trails = new List<Vector3> ();
		trailIndex = 1;
		prevTrailIndex = 0;
		minDistance = 10.0f;
		slickTrails = new List<GameObject> ();
		SetInitialLine ();
		TrailsToLinePositions ();

		foreach (Transform child in transform) {
			if (child.gameObject.tag == "tire") {
				tires.Add (child.GetComponent<tire> ());
			}
		}


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

		grounded = GroundViaSphereCase ();

		if (grounded == true) {
			if (!isLocalPlayer) {
				return;
			}
			float forward = Input.GetAxis ("Vertical");
			float turn = Input.GetAxis ("Horizontal");

			myRigidBody.AddRelativeForce (Vector3.forward * accel * forward, ForceMode.Force);
			myRigidBody.AddRelativeTorque (Vector3.up * turn * torque, ForceMode.Force);

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateLinePositions ();
		TrailsToLinePositions ();

		if (!isLocalPlayer) {
			return;
		}

		// Shooting Mechanics
		if (Input.GetMouseButtonDown (0)) {
			CmdFireBullet (transform.Find ("turret_face").position, myRigidBody.rotation);

		}


		if (Input.GetKeyDown (KeyCode.Space)) {

			sTrails.enabled = !sTrails.enabled;
			sTrails.toggle = true;

		}

		if (Input.GetKeyDown (KeyCode.F1)) {
			transform.rotation = new Quaternion ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			transform.position = new Vector3 (-32f, 2f, 11.5f);
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
		float curDistance = Vector3.Distance (myRigidBody.position, trails [prevTrailIndex]);

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

				slickTrails.Add (Instantiate (pSlickTrail, curPosition, myRigidBody.rotation));

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

	bool GroundViaSphereCase ()
	{
		RaycastHit hit;

		return Physics.SphereCast (myRigidBody.position, 0.1f, Vector3.down, out hit, 1.4f); 

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
