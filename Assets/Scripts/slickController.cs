using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class slickController : NetworkBehaviour {

	private bool hasTriggered;
	private bool toDestroy;

	private GameObject mySlick;


	public GameObject pSlick;
	// Use this for initialization
	void Start () {
		hasTriggered = false;
		toDestroy = false;
		
	}

	// Update is called once per frame
	void Update () {

		if (toDestroy == true) {
			Destroy (this.gameObject);
		}
		
	}

	void OnCollisionEnter(Collision col) {
		if (hasTriggered == false) {
			mySlick = Instantiate (pSlick);
			mySlick.transform.position = col.contacts [0].point;
			 
			toDestroy = true;
			hasTriggered = true;
		}
	}
}
