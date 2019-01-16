using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
	public int collisionCount = 0;
	// public bool isButtonActive = false;
	private bool isButtonActive = false;
	public bool IsButtonActive
	{
		get {return isButtonActive;}
		set {
			if (isButtonActive == value) return;
				isButtonActive = value;
			if (OnVariableChange != null)
				OnVariableChange(isButtonActive);
		}
	}
	public delegate void OnVariableChangeDelegate(bool newVal);
	public event OnVariableChangeDelegate OnVariableChange;

	private void OnCollisionEnter(Collision other) 
	{
		// Debug.Log("Collision");
		 if(other.gameObject.tag == "Button")
		 	IsButtonActive = true;
		 	// other.transform.position = new Vector3(transform.position.x,other.transform.position.y,transform.position.z);
	}
	private void OnCollisionExit(Collision other) 
	{
		if(other.gameObject.tag == "Button")
		 	IsButtonActive = false;
	}

}
