using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class healthPickupController : MonoBehaviour {


	private const float rotationSpeed = 60f;
	private int healAmount;
	private float respawn;

	// Use this for initialization
	void Start () {
		DetermineProperties ();
		
	}
	
	// Update is called once per frame
	void Update () {
		float curRotY = transform.rotation.y + rotationSpeed;
		transform.Rotate(0, curRotY * Time.deltaTime, 0, Space.World);
		
	}

	public float GetRespawn() {
		if (respawn == 0) {
			DetermineProperties ();
		}
		return respawn;
	}

	void DetermineProperties() {
		if (gameObject.name == "pSmall_Health(Clone)" || gameObject.name == "pSmall_Health") {
			healAmount = 5;
			respawn = 10f;
		} else if (gameObject.name == "pMedium_Health(Clone)" || gameObject.name == "pMedium_Health") {
			healAmount = 20;
			respawn = 20f;
		} else if (gameObject.name == "pLarge_Health(Clone)" || gameObject.name == "pLarge_Health") {
			healAmount = 50;
			respawn = 30f;
		}

	}

	void OnTriggerEnter(Collider col) {
		GameObject colGameObject = col.transform.parent.gameObject;
		if (col.gameObject.CompareTag ("player")) {
			Debug.Log (healAmount);
			colGameObject.gameObject.GetComponent<health> ().RecoverHealth(healAmount);
			Destroy (this.gameObject);
		}
	}
}
