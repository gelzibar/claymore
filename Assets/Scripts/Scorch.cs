using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorch : MonoBehaviour {


	private float aliveTimer, maxTimer;
	// Use this for initialization
	void Start () {
		aliveTimer = 0.0f;
		maxTimer = 10.0f;		
	}
	
	// Update is called once per frame
	void Update () {
		if (aliveTimer > maxTimer) {
			Destroy (this.gameObject);
		} else {
			aliveTimer += Time.deltaTime;
		}
	}
}
