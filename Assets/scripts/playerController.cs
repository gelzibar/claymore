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

	const int trailMaxSize = 10;
	public List<Vector3> trails;
	public int trailIndex, prevTrailIndex;
	public float minDistance;

	void OnGUI()
	{
		int frames = (int)(1.0f / Time.smoothDeltaTime);
		GUI.Label(new Rect(10, 0, 100, 100), frames.ToString());        
	}

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
		myLineRendererObject = (GameObject)Instantiate (myLineRendererObject, myRigidBody.position, myRigidBody.rotation, myRigidBody.transform);

		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
			myLineRendererObject.gameObject.SetActive (false);
		} 

		myLineRenderer = myLineRendererObject.GetComponent<LineRenderer> ();
			


		torque = 500.0f;
		accel = 800.0f;

		trails = new List<Vector3>();
		trailIndex = 1;
		prevTrailIndex = 0;
		minDistance = 5.0f;
		SetInitialLine ();
		TrailsToLinePositions ();

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
//		trails [trailIndex] = new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z);
//		Debug.Log(trailIndex + " : " + trails[trailIndex].ToString());
//		trailIndex++;
		UpdateLinePositions();
		TrailsToLinePositions ();
	}

	void TrailInbounds() {
		int maxIndex = trailMaxSize - 1;

		if (trailIndex > maxIndex) {
			trails.RemoveAt (0);
			trails.Add(new Vector3(myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z));

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
		Vector3 curPosition = new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z);

		trails[trailIndex] = curPosition;

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
			trails [prevTrailIndex] = new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z);

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

	void SetInitialLine() {
		trails.Add(new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z));
		trails.Add(new Vector3 (myRigidBody.position.x, myRigidBody.position.y, myRigidBody.position.z));
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
		
}
