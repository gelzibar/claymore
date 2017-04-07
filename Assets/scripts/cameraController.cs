
        myRigidbody = GetComponent<Rigidbody>();
	void FixedUpdate () {
        //		float turn = Input.GetAxis ("Horizontal");
        //		transform.RotateAround (Vector3.zero, Vector3.up, turn);
        //float smooth = 5.0f;
        //transform.position = Vector3.Lerp (
        //	transform.position, trackedObj.GetComponent<Rigidbody>().position,
        //	Time.deltaTime * smooth);
        mouseLook();

	void LockedY() {
		transform.position = new Vector3(trackedObj.GetComponent<Transform>().localPosition.x, 6f, trackedObj.GetComponent<Transform>().localPosition.z - 10f);
	}

	void Follow() {
//		transform.position = new Vector3(trackedObj.GetComponent<Transform>().localPosition.x, 6f, trackedObj.GetComponent<Transform>().localPosition.z - 10f);

		Vector3 conversion = transform.TransformPoint (new Vector3 (trackedObj.GetComponent<Transform> ().localPosition.x, trackedObj.GetComponent<Transform> ().localPosition.y + 6f, trackedObj.GetComponent<Transform> ().localPosition.z - 10f));
		transform.position = conversion;
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