using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEdge : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) 
	{
		Debug.Log("In : " + name);
	}
	private void OnTriggerExit(Collider other) 
	{
		Debug.Log("Out : " + name);
	}
}
