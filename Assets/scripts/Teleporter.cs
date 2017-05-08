using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
	
	public Transform teleportExit;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("player")) {
			col.gameObject.transform.parent.gameObject.transform.position = teleportExit.position;
			col.gameObject.transform.parent.gameObject.transform.rotation = teleportExit.rotation;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
