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
		else if(other.tag == "Land")
		{
			// Resources.Load("ShrinkSquare");
		}
	}
	private void OnTriggerExit(Collider other) 
	{
		if(other.tag == "Wall")
		{
			isTriggerOn = false;
			triggerCount--;
		}
	}
}
