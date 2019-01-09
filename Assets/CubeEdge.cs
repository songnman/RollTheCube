using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEdge : MonoBehaviour
{
	public bool isTriggerOn = false;
	public int triggerCount = 0;
	public Vector3 worldPositon;
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Wall")
		{
			isTriggerOn = true;
			triggerCount++;
		}
		worldPositon = transform.position;
		Debug.Log(gameObject.name + " to " + other);
	}
	private void OnTriggerExit(Collider other) 
	{
		isTriggerOn = false;
		triggerCount--;
		// Debug.Log("Out : " + name);
	}
}
