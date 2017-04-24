using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class trail_maker : NetworkBehaviour {

	[SyncVar(hook = "OnEnabledChange")]
	public bool trailEnabled = false;
	[SyncVar(hook = "OnToggleChange")]
	public bool trailToggle = false;

	void OnEnabledChange(bool change) {
		trailEnabled = change;
	}
	void OnToggleChange(bool change) {
		trailToggle = change;
	}
}
