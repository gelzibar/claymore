using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerController : NetworkBehaviour
{

	// Related Objects
	private VehicleMove myVehicleMove;
	public GameObject myCam;
	public GameObject pBullet, pGrenade;
	private float primaryCD, primaryTimer;
	private float specialCD, specialTimer;
	private bool primaryToggle, specialToggle;
	public NetworkInstanceId myNetID;

    // Personalization
    public Material myStandardMaterial;

	// Aiming Script
	private Turret sTurret;

	void Awake() {
		AssignNetworkTransformChild ();
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.CompareTag ("safe")) {
			int healAmount = 100 - GetComponent<health> ().curHealth;
			gameObject.GetComponent<health> ().RecoverHealth(healAmount);

		}
			
	}

	// Use this for initialization
	void Start ()
	{
        // Only assign active camera to the local player
		if (!isLocalPlayer) {
			
			myCam.SetActive (false);
		}
		Cursor.lockState = CursorLockMode.Locked;

		primaryCD = 0.60f;
		primaryTimer = 0;
		primaryToggle = false;
		specialCD = Grenade.maxCD;
		specialTimer = 0.0f;
		specialToggle = false;
		myNetID = GetComponent<NetworkIdentity> ().netId;

		// Aiming Setup
		sTurret = transform.Find("turret").GetComponent<Turret>();

		// Movement Setup
		myVehicleMove = GetComponent<VehicleMove>();

		//UI Disable
		//GameObject.Find("Title").GetComponent<Image>().enabled = false;
//		GameObject.Find("Dropdown").GetComponent<Dropdown>().enabled = false;
//		GameObject.Find("Dropdown").GetComponent<Image>().enabled = false;
	}

	void OnGUI ()
	{
		if (!isLocalPlayer) {
			return;
		}
		int frames = (int)(1.0f / Time.smoothDeltaTime);

		if (specialToggle == true) {
			GUI.Label (new Rect (300, 0, 200, 100), "Grenade CD: " + specialTimer.ToString ("F1") + " of " + specialCD); 
		} else if (specialToggle == false) {
			GUI.Label (new Rect (300, 0, 200, 100), "Grenade Ready"); 
		}
		GUI.Label (new Rect (10, 0, 100, 100), frames.ToString ());        
		GetComponent<VehicleMove> ().ChildGUI ();
	}

	void FixedUpdate ()
	{
		if (!isLocalPlayer) {
			return;
		}

        GetPlayerInputFixed();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Disable Butter Trails functionality.
		// UpdateLinePositions ();
		// TrailsToLinePositions ();

		if (!isLocalPlayer) {
			return;
		}
        GetPlayerInputStandard();
		SetCrosshairs ();


	}

	void SetCrosshairs() {
		Vector3 tempVec = new Vector3();
		float forwardMulti = 0.0f;
		RaycastHit hit;
		Transform bullet_source = transform.Find ("turret/face");
		Ray downRay = new Ray(bullet_source.position, bullet_source.forward);
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		if (Physics.Raycast (downRay, out hit, 5000.0f, layerMask)) {
			forwardMulti = hit.distance;
		} else {
			forwardMulti = 1000.0f;
		}
		uiController myUICon = GameObject.Find ("UIManager").GetComponent<uiController>();
		tempVec = bullet_source.position + (bullet_source.forward * forwardMulti);
		myUICon.turretVector = myCam.GetComponent<Camera>().WorldToScreenPoint(tempVec);
		myUICon.UpdateMyCrosshairs ();
	}

    void GetPlayerInputFixed() {

    }

//	void ForwardVelocity() {
//		
//		ChangeVelocity (1);
//	}
//	void BackwardVecloity() {
////		float inverseMin = minVelocity * -1;
////		float inverseMax = maxVelocity * -1;
////
////		if (curVelocity > inverseMin) {
////			curVelocity = inverseMin;
////		}
////
////		if (curVelocity > (maxVelocity * 1 / 3 * -1)) {
////			curVelocity--;
////		}
//		ChangeVelocity(-1);
//	}
//
//	void ChangeVelocity(int direction) {
//		if (curVelocity < minVelocity) {
//			curVelocity = minVelocity;
//		}
//
//		if (curVelocity < maxVelocity) {
//			curVelocity++;
//		}
//	}

    void GetPlayerInputStandard() {
		if (primaryToggle == false) {
			if (Input.GetMouseButtonDown (0)) {
				primaryToggle = true;
				CmdFireBullet (transform.Find ("turret/face").position, transform.Find ("turret/face").rotation, myNetID);

			}
		} else if (primaryToggle == true && primaryTimer >= primaryCD) {
			primaryToggle = false;
			primaryTimer = 0;
		} else {
			primaryTimer += Time.deltaTime;
		}

		if (specialToggle == false) {
			if (Input.GetMouseButtonDown (1)) {
				specialToggle = true;
//				GetComponent<AudioSource> ().pitch = 1.75f;
				GetComponent<AudioSource> ().Play ();
				//GetComponent<AudioSource> ().pitch = 1.0f;
				CmdFireSpecial (transform.Find ("turret/face").position, transform.Find ("turret/face").rotation, myNetID);
			}
		} else if (specialToggle == true && specialTimer >= specialCD) {
			specialToggle = false;
			specialTimer = 0.0f;
		} else {
			specialTimer += Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.F12)) {
			ToggleLockState ();
		}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //sTrails.enabled = !sTrails.enabled;
            //sTrails.toggle = true;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            float offsetX = transform.rotation.eulerAngles.x * -1;
            float offsetZ = transform.rotation.eulerAngles.z * -1;
            transform.Rotate(new Vector3(offsetX, 0, offsetZ ));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.position = new Vector3(0.0f, 252.0f, 0.0f);
        }
    }

	void ToggleLockState() {
		if (Cursor.lockState == CursorLockMode.None) {
			Cursor.lockState = CursorLockMode.Locked;
		} else if (Cursor.lockState == CursorLockMode.Locked) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	bool GroundViaSphereCast (Vector3 origin)
	{
		RaycastHit hit;

		return Physics.SphereCast (origin, 0.1f, Vector3.down, out hit, 1.4f); 

	}

	public void SetAllMaterial(Material newMaterial) {
		transform.Find ("tank_body").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		//transform.Find ("turret").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find("turret").Find("barrel").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find("turret").Find ("cap").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
		transform.Find("turret/pivot").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
//		transform.Find("Front Indicator").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;
//		transform.Find("Front Indicator (1)").GetComponent<MeshRenderer> ().sharedMaterial = newMaterial;

		if (!isLocalPlayer) {
			return;
		}
		GameObject.Find ("Active Layer").GetComponent<Image> ().color = newMaterial.color;
	}

	void AssignNetworkTransformChild() {
		
		foreach(NetworkTransformChild aChild in GetComponents (typeof(NetworkTransformChild))) {
			if (aChild.target == null) {
				aChild.target = transform.Find ("turret/barrel");
			}
		}
	}

	[Command]
	void CmdFireBullet (Vector3 position, Quaternion rotation, NetworkInstanceId id)
	{
		GameObject curBullet = Instantiate (pBullet, position, rotation, GameObject.Find ("Ammo Container").transform);
		curBullet.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * 0.05f, ForceMode.Impulse);
		curBullet.GetComponent<bulletController> ().SetOwnerNetID (id);
		Destroy (curBullet, 2.0f);

		NetworkServer.Spawn (curBullet);
	}

	[Command]
	void CmdFireSpecial (Vector3 position, Quaternion rotation, NetworkInstanceId id) {
//		Quaternion tempQuat = rotation;
//		tempQuat.eulerAngles = new Vector3(tempQuat.eulerAngles.x - 30.0f , tempQuat.eulerAngles.y, tempQuat.eulerAngles.z);

		GameObject curGrenade = Instantiate (pGrenade, position, rotation, GameObject.Find ("Ammo Container").transform);
		curGrenade.GetComponent<Rigidbody> ().AddRelativeForce ((Vector3.forward + (Vector3.up / 4)) * 0.30f, ForceMode.Impulse);
		curGrenade.GetComponent<Rigidbody> ().angularVelocity = UnityEngine.Random.insideUnitSphere * 5.0f;
		curGrenade.GetComponent<Grenade> ().SetOwnerNetID (id);
		//Destroy (curBullet, 2.0f);

		NetworkServer.Spawn (curGrenade);

//		Vector3 offset = new Vector3 (position.x + 1, position.y, position.z);
//		GameObject curGrenade = Instantiate (pGrenade, offset, rotation, GameObject.Find ("Ammo Container").transform);
//		curGrenade.GetComponent<Rigidbody> ().AddRelativeForce (((Vector3.right / 5) + Vector3.forward + (Vector3.up / 4)) * 0.30f, ForceMode.Impulse);
//		curGrenade.GetComponent<Grenade> ().SetOwnerNetID (id);
//
//		NetworkServer.Spawn (curGrenade);
//
//		offset = new Vector3 (position.x -1, position.y, position.z);
//		curGrenade = Instantiate (pGrenade, offset, rotation, GameObject.Find ("Ammo Container").transform);
//		curGrenade.GetComponent<Rigidbody> ().AddRelativeForce (((Vector3.left / 5) + Vector3.forward + (Vector3.up / 4)) * 0.30f, ForceMode.Impulse);
//		curGrenade.GetComponent<Grenade> ().SetOwnerNetID (id);
//
//		NetworkServer.Spawn (curGrenade);
	}
		
}
 