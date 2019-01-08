using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEdge : MonoBehaviour
{
	public bool isTriggerOn = false;
	public Vector3 worldPositon;
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Wall")
			isTriggerOn = true;
		worldPositon = transform.position;
	}
	private void OnTriggerExit(Collider other) 
	{
		isTriggerOn = false;
		// Debug.Log("Out : " + name);
	}
}
