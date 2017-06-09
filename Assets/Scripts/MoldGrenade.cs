using System.Collections;
using System.Collections.Generic;

public class MoldGrenade : Gadget {



	public MoldGrenade() {
		isChargeable = true;
		toggleCharge = false;
		curCharge = 0.0f;
		maxCharge = 1.0f;

		throwModifier = 1.125f;
		throwStrength = 0.0f;

		curCooldown = 0.0f;
		maxCooldown = 0.25f;

		maxCapacity = 15;
		curCapacity = maxCapacity;

		name = "grenade";
	}
}
