using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
	
	public Transform teleportExit;

	void OnTriggerEnter(Collider col) {
		if (col.transform.root.CompareTag ("player")) {
			col.transform.root.transform.position = teleportExit.position;
			col.transform.root.transform.rotation = teleportExit.rotation;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
