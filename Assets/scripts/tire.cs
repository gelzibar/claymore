using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tire : MonoBehaviour {
	public bool grounded;
	public int groundedRating;

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
	
	// Update is called once per frame
	void Update () {
		
	}

	public int ReportGroundedRating() {
		//return groundedRating;
		return 0;
	}
}
