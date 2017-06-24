using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerController : NetworkBehaviour
{

	// Related Objects
//	private VehicleMove myVehicleMove;
	public GameObject myCam;
	public GameObject pBullet, pMuzzle;
	private float primaryCD, primaryTimer;
	private float specialCD, specialTimer;
	private bool primaryToggle, specialToggle;
	public NetworkInstanceId myNetID;
	public uiController myUI;
	public VehicleMove myVehicleMove;
	private Arsenal myArsenal;

	public Gadget gadget01;

    // Personalization
    public Material myStandardMaterial;

	// Aiming Script
//	private Turret sTurret;

	private bool isControlEnabled;

	public GameObject pScorch;

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

		gadget01 = new MoldGrenade();

		primaryCD = 0.60f;
		primaryTimer = 0;
		primaryToggle = false;
		specialCD = gadget01.GetMaxCooldown();
		specialTimer = 0.0f;
		specialToggle = false;
		myNetID = GetComponent<NetworkIdentity> ().netId;

		// Aiming Setup
//		sTurret = transform.Find("turret").GetComponent<Turret>();

		// Movement Setup
//		myVehicleMove = GetComponent<VehicleMove>();



		// UI Setup
		myUI = GameObject.Find("UIManager").GetComponent<uiController>();
		if (myUI.myPlayer == null) {
			myUI.myPlayer = this;
		}

		myVehicleMove = GetComponent<VehicleMove> ();
		myArsenal = GetComponent<Arsenal> ();

		isControlEnabled = true;

	}

	void OnGUI ()
	{
		if (!isLocalPlayer) {
			return;
		}
		int frames = (int)(1.0f / Time.smoothDeltaTime);

		GUI.Label (new Rect (10, 0, 100, 100), frames.ToString ());        
		GetComponent<VehicleMove> ().ChildGUI ();
	}

	void FixedUpdate ()
	{
		if (!isLocalPlayer) {
			return;
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}
        GetPlayerInputStandard();

		if (gadget01 != null && gadget01.GetCurCapacity() <= 0) {
			gadget01 = null;
		}

		if (isControlEnabled) {
			SetCrosshairs ();
		}


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
		if (isControlEnabled == true) {
			if (primaryToggle == false) {
				if (Input.GetMouseButtonDown (0)) {
					primaryToggle = true;
					CmdFireBullet2 (transform.Find ("turret/face").position, transform.Find ("turret/face").forward, myNetID);
					//transform.FindChild ("turret/face/Muzzle Flash").gameObject.GetComponent<ParticleSystem> ().Play ();
					CmdMuzzleFlash(transform.FindChild ("turret/face").position);
					GetComponent<AudioSource> ().Play ();

				}
			} else if (primaryToggle == true && primaryTimer >= primaryCD) {
				primaryToggle = false;
				primaryTimer = 0;
			} else {
				primaryTimer += Time.deltaTime;
			}

			if (specialToggle == false) {

				if (gadget01 != null) {
					if (gadget01.ResolveInput () == 1) {
						specialToggle = true;
//						GetComponent<AudioSource> ().Play ();
						myArsenal.ExecuteGadget (gadget01, myNetID);
						gadget01.ResetAll ();
					}
				}
//					}
//				}

//				if (Input.GetMouseButtonDown (1)) {
//					myArsenal.CmdActivateSpeedBoost ();
//				}
//				if (Input.GetMouseButtonDown (1)) {
//					myVehicleMove.SetSpeedMultiplier (2.0f, 2.0f);
//				}else if (Input.GetMouseButtonUp(1)) {
//					myVehicleMove.ResetSpeedMultiplier ();
//				}
			} else if (specialToggle == true && specialTimer >= specialCD	) {
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
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            transform.position = new Vector3(0.0f, 252.0f, 0.0f);
//        }
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			ToggleLockState ();
			myUI.ToggleMenu ();
			ToggleControl ();

		}
		if (Input.GetKeyDown (KeyCode.X)) {
//			gadget01 = new MoldGrenade();
		}
		if (Input.GetKeyDown (KeyCode.C)) {
//			gadget01 = new MoldSpeedBoost();
		}

    }

	public void ToggleControl() {
		if (isControlEnabled == false) {
			isControlEnabled = true;
		} else if (isControlEnabled == true) {
			isControlEnabled = false;
		}
	}

	public void ToggleLockState() {
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

	public void Respawn() {
		transform.position = new Vector3(0.0f, 252.0f, 0.0f);
	}

	void AssignNetworkTransformChild() {
		
		foreach(NetworkTransformChild aChild in GetComponents (typeof(NetworkTransformChild))) {
			if (aChild.target == null) {
				aChild.target = transform.Find ("turret/barrel");
			}
		}
	}

	public bool GetIsControlEnabled() {
		return isControlEnabled;
	}

	public void SetGadgetOneCapacity(int amount) {
		gadget01.SetCurCapacity (amount);
	}

	public void SetGadgetOneCapacityToFull() {
		if (gadget01 == null) {
			gadget01 = new MoldGrenade ();
		} else if (gadget01 != null) {
			gadget01.SetCurCapacity (gadget01.GetMaxCapacity ());
		}
	}

	public string GetGadgetName() {
		return gadget01.GetName ();
	}

	public void GetRandomGadget() {
		myArsenal.GetRandomGadget ();
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
	void CmdFireBullet2 (Vector3 position, Vector3 rotation, NetworkInstanceId id)
	{

		RaycastHit hit;

//		Physics.Raycast(
		//Physics.Raycast (position, 0.1f, rotation.eulerAngles, out hit, 2000.0f);
		if (Physics.Raycast (position, rotation, out hit, 2000.0f)) {
			if (hit.transform.root.gameObject.tag == "player") {
				hit.transform.root.gameObject.GetComponent<health> ().TakeDamage (25);
				hit.transform.root.gameObject.GetComponent<DamageFlash> ().StartStrobe ();
			} else {
				RpcCreateScorch (hit.point, hit.normal);
			}
		}

//		Transform bullet_source = transform.Find ("turret/face");
//		Ray downRay = new Ray(bullet_source.position, bullet_source.forward);
//		int layerMask = 1 << 8;
//		layerMask = ~layerMask;
//		if (Physics.Raycast (downRay, out hit, 5000.0f, layerMask)) {
//			forwardMulti = hit.distance;
//		} else {
//			forwardMulti = 1000.0f;
//		}
//		GameObject curBullet = Instantiate (pBullet, position, rotation, GameObject.Find ("Ammo Container").transform);
//		curBullet.GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * 0.05f, ForceMode.Impulse);
//		curBullet.GetComponent<bulletController> ().SetOwnerNetID (id);
//		Destroy (curBullet, 2.0f);
//
//		NetworkServer.Spawn (curBullet);
	}

	[ClientRpc]
	public void RpcCreateScorch(Vector3 position, Vector3 normal) {
		Quaternion zeroRot = new Quaternion ();
		//		Instantiate (pCircle2, position + normal, zeroRot);
		//		Instantiate (pCircle, position, zeroRot);

		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		Vector3 forwardPos = Vector3.Lerp (position, position + normal, 0.1f);
		Transform scorchContainer = GameObject.Find ("Decal Container").transform;
		GameObject curScorch = Instantiate (pScorch, forwardPos, rotation, scorchContainer);
	}

	[Command]
	void CmdMuzzleFlash(Vector3 position) {
//		Quaternion rotate = new Quaternion ();
//		GameObject curFlash = Instantiate (pMuzzle, position, rotate);
//
//		NetworkServer.Spawn (curFlash);
		RpcMuzzleFlash(position);
	}

	[ClientRpc]
	void RpcMuzzleFlash(Vector3 position) {
//		if(isLocalPlayer)
//			return;
		Quaternion rotate = new Quaternion ();
		GameObject curFlash = Instantiate (pMuzzle, position, rotate);
		Destroy (curFlash, 2.0f);
	}
		
}
 