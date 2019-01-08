using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEdge : MonoBehaviour
{
	public bool isTriggerOn = false;
	public Vector3 worldPositon;
	private void OnTriggerEnter(Collider other) 
	{
		
		isTriggerOn = true;
		// Debug.Log("In : " + name);
		worldPositon = transform.position;
	}
	private void OnTriggerExit(Collider other) 
	{
		isTriggerOn = false;
		// Debug.Log("Out : " + name);
	}
}
