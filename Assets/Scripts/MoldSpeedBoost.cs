using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldSpeedBoost : Gadget
{
	private static int maxCapacityTemplate = 4;

	public MoldSpeedBoost(int amount) {
		curCooldown = 0.0f;
		maxCooldown = 0.25f;

		maxCapacity = MoldSpeedBoost.maxCapacityTemplate;
		curCapacity = amount;
		ResolveCapacityLimit ();

		name = "boost";
	}
	public MoldSpeedBoost ()
		: this(MoldSpeedBoost.maxCapacityTemplate / 2)
	{ }

	public override int ResolveInput ()
	{
		int status = 0;
		if (GetCurCapacity () > 0) {
			if (Input.GetMouseButtonDown (1)) {
				DecreaseCurCapacity (1);
				status = 1;
			}
		}
		return status;
	}

	public override void ResetAll ()
	{
	}
}
