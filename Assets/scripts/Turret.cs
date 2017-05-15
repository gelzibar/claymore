using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Turret : NetworkBehaviour {

	// Aiming
	private float vertDegreeOffset;
	private float vertDegreeMax, vertDegreeMin;
	private float vertDegreeCur;
	private float startDegree;

	// Use this for initialization
	void Start () {

		// Aiming Setup
		vertDegreeOffset = 0.0f;
		vertDegreeCur = 0.0f;
		vertDegreeMax = 15.0f;
		vertDegreeMin = -8.0f;
		startDegree = 0.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		float mouseX = Input.GetAxis ("Mouse X");
		TurretRotation (mouseX);
		
	}

	public void TurretRotation(float delta) {
		float mouseY = Input.GetAxis ("Mouse Y");

		Transform barrel = transform.Find ("barrel");
		Transform bullet_source = transform.Find ("face");
		Transform pivot = transform.Find ("pivot");
		float deltaAmplifier = 1.5f;
		float verticalReducer = 2.0f;

		// Main Turret and all children
		transform.RotateAround (transform.position, transform.up, delta * deltaAmplifier);

		vertDegreeCur = mouseY / verticalReducer;
		vertDegreeOffset += vertDegreeCur;
		if (vertDegreeOffset >= vertDegreeMax) {
			vertDegreeCur = barrel.transform.localRotation.eulerAngles.x - (startDegree - vertDegreeMax);
			vertDegreeOffset = vertDegreeMax;
		} else if (vertDegreeOffset <= vertDegreeMin) {
			vertDegreeCur = barrel.transform.localRotation.eulerAngles.x - (startDegree - vertDegreeMin);
			vertDegreeOffset = vertDegreeMin;
		}

		barrel.transform.RotateAround (pivot.position, pivot.right * -1, vertDegreeCur);
		bullet_source.transform.RotateAround (pivot.position, pivot.right * -1, vertDegreeCur);

	}
}
