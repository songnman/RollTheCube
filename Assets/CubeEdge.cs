using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEdge : MonoBehaviour
{
	public bool isTriggerOn = false;
	private void OnTriggerEnter(Collider other) 
	{
		
		isTriggerOn = true;
		// Debug.Log("In : " + name);
	}
	private void OnTriggerExit(Collider other) 
	{
		isTriggerOn = false;
		// Debug.Log("Out : " + name);
	}
}
