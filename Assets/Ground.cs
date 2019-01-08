using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	public int collisionCount = 0;
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Cube")
		{
			StartCoroutine("Trap", other);
			// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
			Debug.Log("!");
		}
	}
	private void OnTiriggerExit(Collision other) 
	{
		collisionCount--;
		Debug.Log(collisionCount);
	}

	IEnumerator Trap(Collider other)
	{
		for (int i = 0; i < 10; i++)
		{
			other.transform.position = other.transform.position - new Vector3(0,0.05f,0);
			yield return null;
		}
	}
}
