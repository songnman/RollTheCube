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
		// if(other.tag == "Land")
		// {
		// 	transform.parent.parent.GetComponent<ControlCube>().isCubeOnLand = true;
		// }
		// worldPositon = transform.position;
		// Debug.Log(gameObject.name + " to " + other);
	}
	private void OnTriggerExit(Collider other) 
	{
		if(other.tag == "Wall")
		{
			isTriggerOn = false;
			triggerCount--;
			// Debug.Log("Out : " + name);
		}
		// if(other.tag == "Land")
		// {
		// 	transform.parent.parent.GetComponent<ControlCube>().isCubeOnLand = false;
		// }
		// worldPositon = transform.position;
		// Debug.Log(gameObject.name + " to " + other);

		
	}
}
