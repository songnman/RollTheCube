using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
	public int collisionCount = 0;
	public bool isButtonActive = false;
	private void OnCollisionEnter(Collision other) 
	{
		// Debug.Log("Collision");
		 if(other.gameObject.tag == "Button")
		 	isButtonActive = true;
		 	// other.transform.position = new Vector3(transform.position.x,other.transform.position.y,transform.position.z);
	}
	private void OnCollisionExit(Collision other) 
	{
		if(other.gameObject.tag == "Button")
		 	isButtonActive = false;
	}

}
