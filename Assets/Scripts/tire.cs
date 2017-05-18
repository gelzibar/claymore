using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tire : MonoBehaviour {
	public bool grounded;

	void OnCollisionEnter(Collision col) {
//		grounded = true;
//		groundedRating++;
//		Debug.Log ("Enter");
	}

	void OnCollisionExit(Collision col) {
//		grounded = false;
//		groundedRating--;
//		Debug.Log ("Exit");
	}

	// Use this for initialization
	void Start () {
//		grounded = false;
//		groundedRating = 0;
		
	}

	void FixedUpdate() {
		grounded = GroundViaSphereCast ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int ReportGroundedRating() {
		//return groundedRating;
		return 0;
	}

	public bool GroundViaSphereCast ()
	{
		RaycastHit hit;

		return Physics.SphereCast (transform.position, 0.1f, transform.up * -1, out hit, 1.1f); 

	}
}
