using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	public GameObject trackedObj;
	private Vector3 offset;
    private Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
		offset = new Vector3 (0, 25f, 25f);
        myRigidbody = GetComponent<Rigidbody>();
		
	}

	void FixedUpdate () {
        //		float turn = Input.GetAxis ("Horizontal");
        //		transform.RotateAround (Vector3.zero, Vector3.up, turn);
        //float smooth = 5.0f;
        //transform.position = Vector3.Lerp (
        //	transform.position, trackedObj.GetComponent<Rigidbody>().position,
        //	Time.deltaTime * smooth);
        mouseLook();
	
	}
	// Update is called once per frame
	void Update () {
	}


	void LockedX() {
		transform.position = new Vector3(transform.position.x, trackedObj.GetComponent<Transform>().position.y + 25f ,trackedObj.GetComponent<Transform>().position.z + 50f);
	}

    void mouseLook()
    {
        Vector2 delta = new Vector2(0, 0);
        delta.x = Input.GetAxis("Mouse Y");
        delta.y = Input.GetAxis("Mouse X");
        Debug.Log(delta.ToString());
        float lookMagnitude = 20.0f;

        transform.Rotate((Vector3.up * Time.fixedDeltaTime * delta.y * lookMagnitude) + (Vector3.right * Time.fixedDeltaTime * delta.x * lookMagnitude));
        //transform.Rotate(Vector3.right * Time.fixedDeltaTime * delta.x * lookMagnitude);


    }
}
