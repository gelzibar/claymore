using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gadget {

	protected bool isChargeable, toggleCharge;
	protected float curCharge, maxCharge;

	protected float throwStrength, throwModifier;

	protected float curCooldown, maxCooldown;
	protected int curCapacity, maxCapacity;
	// Some type of 'Icon' for the gadget slot
	protected string name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float GetCurCooldown() {
		return curCooldown;
	}

	public float GetMaxCooldown() {
		return maxCooldown;
	}

	public int GetCurCapacity() {
		return curCapacity;
	}

	public int GetMaxCapacity() {
		return maxCapacity;
	}

	public void DecreaseCurCapacity(int amount) {
		curCapacity -= amount;
	}

	public void IncreaseCurCapacity(int amount) {
		curCapacity += amount;

		ResolveCapacityLimit ();
	}

	public void SetCurCapacity(int amount) {
		curCapacity = amount;

		ResolveCapacityLimit ();
	}

	public void ResolveCapacityLimit() {
		if (curCapacity > maxCapacity) {
			curCapacity = maxCapacity;
		}
	}

	public bool GetChargeable() {
		return isChargeable;
	}

	public string GetName() {
		return name;
	}

	public void AddChargeTime() {
		if (toggleCharge == true) {
			curCharge += Time.deltaTime;

			if (curCharge > maxCharge) {
				curCharge = maxCharge;
			}
		}
	}

	public void StartCharge() {
		toggleCharge = true;
		curCharge = 0.0f;
	}

	public void EndCharge() {
		toggleCharge = false;
		curCharge = 0.0f;
	}

	public float GetPercentCharge() {
		return curCharge / maxCharge;
	}

	public bool GetChargeComplete() {
		bool completed = false;
		if(curCharge / maxCharge == 1.0f) {
			completed = true;
		}
		return completed;
	}

	public bool GetToggleCharge() {
		return toggleCharge;
	}

	public float GetThrow() {
		return throwStrength;
	}

	public void CalculateThrow() {
		throwStrength = (curCharge / maxCharge) * throwModifier;
	}

	public void ResetThrow() {
		throwStrength = 0.0f;
	}

	public virtual int ResolveInput() {
		Debug.Log ("Plain ol' gadget.");
		return 0;
	}

	public virtual void ResetAll() {
	}
}
