using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldGrenade : Gadget {


	private static int maxCapacityTemplate = 6;

	public MoldGrenade(int curAmount) {
		isChargeable = true;
		toggleCharge = false;
		curCharge = 0.0f;
		maxCharge = 1.0f;

		throwModifier = 1.125f;
		throwStrength = 0.0f;

		curCooldown = 0.0f;
		maxCooldown = 0.25f;

		maxCapacity = MoldGrenade.maxCapacityTemplate;
		curCapacity = curAmount;
		ResolveCapacityLimit ();

		name = "grenade";
	}
	public MoldGrenade() 
		: this(MoldGrenade.maxCapacityTemplate / 2)
	{}

	public override int ResolveInput() {
		int status = 0;
		if (GetChargeable () == true) {
			if (GetCurCapacity () > 0) {
				if (Input.GetMouseButtonDown (1) && !GetChargeComplete ()) {
					if (GetToggleCharge () == false) {
						StartCharge ();
					}
				}else if (Input.GetMouseButton (1) && !GetChargeComplete()) {
					if (GetToggleCharge() == true) {
						AddChargeTime ();
					}
				} else if (Input.GetMouseButtonUp (1) || GetChargeComplete()) {
					if (GetToggleCharge () == true) {
						CalculateThrow ();
						DecreaseCurCapacity (1);
						status = 1;
					}
				}
			}
		}
		return status;
	}

	public override void ResetAll() {
		ResetThrow ();
		EndCharge ();
	}
}
