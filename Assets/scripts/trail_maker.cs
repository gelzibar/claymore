using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class trail_maker : NetworkBehaviour {

	[SyncVar(hook = "OnEnabledChange")]
	public bool enabled = false;
	[SyncVar(hook = "OnToggleChange")]
	public bool toggle = false;

	void OnEnabledChange(bool change) {
		enabled = change;
	}
	void OnToggleChange(bool change) {
		toggle = change;
	}
}
