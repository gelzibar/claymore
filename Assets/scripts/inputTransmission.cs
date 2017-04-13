using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class inputTransmission : NetworkBehaviour {


	[SyncVar(hook = "OnSpaceChange")]
	public bool space_key = false;

	void OnSpaceChange(bool change) {
		space_key = change;
	}
}
