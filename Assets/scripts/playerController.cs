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
	public GameObject myLineRendererObject;
	private LineRenderer myLineRenderer;

	const int trailMaxSize = 500;
	public List<Vector3> trails;
	public List<GameObject> slickTrails;
	public GameObject pSlickTrail;
	public int trailIndex, prevTrailIndex;
	public float minDistance;
	public float zRearOffset;
	//private bool disableTrail, disableTrailToggle;
	private trail_maker sTrails;
	private GameObject exhaust;
	public List<tire> tires;
	public bool grounded;


//	void OnTriggerEnter(Collider col) {
//		Debug.Log ("Collision with " + col);
//	}

	void OnGUI()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		int frames = (int)(1.0f / Time.smoothDeltaTime);
		string torqueText = "Torque: " + torque.ToString ();
		string dragText = "Drag: " + myRigidBody.angularDrag.ToString ();
		float turn = Input.GetAxis ("Horizontal");


		GUI.Label(new Rect(300, 0, 100, 100), dragText); 
		GUI.Label(new Rect(300, 20, 100, 100), torqueText);
		GUI.Label(new Rect(300, 30, 100, 100), "Turn: " + turn.ToString());        
		GUI.Label(new Rect(10, 0, 100, 100), frames.ToString());        
	}

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
		//myLineRendererObject = (GameObject)Instantiate (myLineRendererObject, myRigidBody.position, myRigidBody.rotation, myRigidBody.transform);
//		myLineRendererObject = GetComponent<LineRenderer>();

		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
//			myLineRendererObject.gameObject.SetActive (false);
		} 

		sTrails = GetComponent<trail_maker> ();
		myLineRenderer = GetComponentInChildren<LineRenderer> ();
		exhaust = transform.Find ("exhaust").gameObject;
		sTrails.enabled = false;
		sTrails.toggle = false;

		zRearOffset = 1.0f;
		torque = 300.0f;
		accel = 400.0f;
		myRigidBody.maxAngularVelocity = 2.5f;
		grounded = false;

		trails = new List<Vector3>();
		trailIndex = 1;
		prevTrailIndex = 0;
		minDistance = 10.0f;
		slickTrails = new List<GameObject> ();
		SetInitialLine ();
		TrailsToLinePositions ();

		foreach(Transform child in transform){
			if(child.gameObject.tag == "tire"){
				tires.Add(child.GetComponent<tire>());
			}
		}


	}

	void FixedUpdate() {
//		if(Input.GetKey(KeyCode.W)) {
//			myRigidBody.AddForce ((Vector3.back + Vector3.down) * 10f);
//		}

		//grounded = ManageTires ();
		grounded = GroundViaSphereCase();
		Debug.Log ("Grounding: " + grounded);

		if(grounded == true) {
			if (!isLocalPlayer) {
				return;
			}
			float forward = Input.GetAxis ("Vertical");
			float turn = Input.GetAxis ("Horizontal");

			//Debug.Log (Vector3.up * turn * torque);
			//Debug.Log (myRigidBody.angularVelocity);
			//Debug.Log (myRigidBody.maxAngularVelocity);

			myRigidBody.AddRelativeForce (Vector3.forward * accel * forward, ForceMode.Force);
			myRigidBody.AddRelativeTorque (Vector3.up * turn * torque, ForceMode.Force);

			//myRigidBody.AddRelativeForce (Vector3.up * 50, ForceMode.Force);
		}
}
	
	// Update is called once per frame
	void Update () {
//		trails [trailIndex] = new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z);
//		Debug.Log(trailIndex + " : " + trails[trailIndex].ToString());
//		trailIndex++;
		UpdateLinePositions();
		TrailsToLinePositions ();

		if (!isLocalPlayer)
		{
			return;
		}


		if(Input.GetKeyDown(KeyCode.Space)) {

			sTrails.enabled = !sTrails.enabled;
			sTrails.toggle = true;

		}

//		if(Input.GetKey(KeyCode.M)) {
//			torque = torque - 10;
//		}
//		if(Input.GetKeyDown(KeyCode.Comma)) {
//			torque = torque - 1;
//		}
//		if(Input.GetKeyDown(KeyCode.Period)) {
//			torque = torque + 1;
//		}
//		if(Input.GetKey(KeyCode.Slash)) {
//			torque = torque + 10;
//		}

//		float newDrag = myRigidBody.angularDrag;
//
//		if(Input.GetKey(KeyCode.O)) {
//			 
//			newDrag = newDrag - 10;
//		}
//		if(Input.GetKeyDown(KeyCode.P)) {
//			newDrag = newDrag - 1;
//		}
//		if(Input.GetKeyDown(KeyCode.LeftBracket)) {
//			newDrag = newDrag + 1;
//		}
//		if(Input.GetKey(KeyCode.RightBracket)) {
//			newDrag = newDrag + 10;
//		}
//
//		myRigidBody.angularDrag = newDrag;


		if(Input.GetKeyDown(KeyCode.F1)) {
			transform.rotation = new Quaternion ();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			transform.position = new Vector3 (-23, 50, 436);
		}

//		if(Input.GetKeyDown(KeyCode.F12)) {
//			if(triggerF12 == false) {
//				myRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//				triggerF12 = true;
//			}else if (triggerF12 == true) {
//				myRigidBody.constraints = RigidbodyConstraints.None;
//				triggerF12 = true;
//			}
//
//		}
	}

	void TrailInbounds() {
		int maxIndex = trailMaxSize - 1;

		if (trailIndex > maxIndex) {
			trails.RemoveAt (0);
			trails.Add(new Vector3(exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z));

			trailIndex--;
			prevTrailIndex--;
			//trailIndex = 0;
		}
//		if (prevTrailIndex > maxIndex) {
//			prevTrailIndex = 0;
//		}
	}

//	void PopulateTrails() {
//		for(int i = 0; i < trailMaxSize; i++) {
//			trails.Add (new Vector3 (0, 0, 0));
//		}
//		SetInitialLine ();
//	}

	bool CompareLinePositions() {
		bool returnValue = false;
		float curDistance = Vector3.Distance (myRigidBody.position, trails [prevTrailIndex]);

		if (curDistance > minDistance) {
			returnValue = true;
		}

		return returnValue;
	}

	void UpdateLinePositions() {
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

				slickTrails.Add (Instantiate(pSlickTrail, curPosition, myRigidBody.rotation));

				//trails [trailIndex] = curPosition;


				//		Vector3 curPosition = new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z);
				//		if(trails.Count < trails.Capacity) {
				//			trails.Add(curPosition);
				//			LineRendererAdd (curPosition);
				//		}else if(trails.Capacity < trailMaxSize)
				//		{
				//			//trails.Add(curPosition);
				//			//LineRendererAdd (curPosition);
				//		}else{
				//			trails[trailIndex] = curPosition;
				//		}

				//if (CompareLinePositions ()) {

				//}
			}
		}
	}

	void SetInitialLine() {
		Vector3 initialLine = new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z);
		trails.Add(initialLine);
		trails.Add(initialLine);
		//configureCollisionTrail ();
		myLineRenderer.numPositions = trails.Count;
	}

	void TrailsToLinePositions() {
		for(int i = 0; i < myLineRenderer.numPositions; i++){
//			myLineRenderer.GetPosition(i) = trails [i];
			myLineRenderer.SetPosition (i, trails [i]);
		}
	}

	void LineRendererAdd(Vector3 vector) {
		myLineRenderer.numPositions++;
		myLineRenderer.SetPosition (myLineRenderer.numPositions - 1, vector);
	}

	void ConfigureCollisionTrail(Vector3 vecOne, Vector3 vecTwo ) {
	}

	bool ManageTires() {
		int curGroundedRating = 0;
		bool groundingStatus = false;
		foreach (tire child in tires) {
			curGroundedRating += child.ReportGroundedRating ();
		}

		if (curGroundedRating > 0) {
			groundingStatus = true;
		}

		return groundingStatus;
	}

	bool GroundViaSphereCase() {
		RaycastHit hit;

		return Physics.SphereCast (myRigidBody.position, 0.1f, Vector3.down,out hit, 1.4f); 

		//origin: radius: direction hitInfo maxDistance
	}
		
}
