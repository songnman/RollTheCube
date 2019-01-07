using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	int collisionCount = 0;
	private void OnCollisionEnter(Collision other) 
	{
		collisionCount++;
		Debug.Log(collisionCount);
	}
	private void OnCollisionExit(Collision other) 
	{
		collisionCount--;
		Debug.Log(collisionCount);
	}
}
