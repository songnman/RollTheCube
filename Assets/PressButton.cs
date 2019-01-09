using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
	public int collisionCount = 0;

	private void OnCollisionEnter(Collision other) 
	{
		// Debug.Log("Collision");
		 if(other.gameObject.tag == "Button")
		 	Debug.Log("Button On");
		 	// other.transform.position = new Vector3(transform.position.x,other.transform.position.y,transform.position.z);

	}

}
