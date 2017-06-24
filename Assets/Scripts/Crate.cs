using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	private const float rotationSpeed = 60f;
	private const float respawn = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float curRotY = transform.rotation.y + rotationSpeed;
		transform.Rotate(0, curRotY * Time.deltaTime, 0, Space.World);
	}

	public float GetRespawn() {
		return respawn;
	}

	void OnTriggerEnter(Collider col) {
		Transform colRootTrans = col.transform.root;
		playerController playerScript = colRootTrans.gameObject.GetComponent<playerController> ();
		if (colRootTrans.CompareTag ("player")) {
			Destroy (this.gameObject);
			colRootTrans.gameObject.GetComponent<playerController> ().GetRandomGadget ();
		}
	}

}
