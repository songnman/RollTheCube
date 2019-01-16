using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	// public int collisionCount = 0;
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Cube"&& moveDirection < 0)
		{
			// gameObject.GetComponent<Rigidbody>().useGravity = true;
			StartCoroutine("FallingTrap", other);
			// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
			Debug.Log("!");
		}
	}
	private void OncollisionEnter(Collider other) 
	{
		if(other.tag == "Cube" && moveDirection == -1)
		{
			StartCoroutine("FallingTrap", other);
			// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
			// collisionCount++;
			Debug.Log("!");
		}
	}
	private void OnTiriggerExit(Collision other) 
	{
		// collisionCount--;
		// Debug.Log(collisionCount);
	}

	IEnumerator FallingTrap(Collider other)
	{
		int repeat = 100;
		for (int i = 0; i < repeat; i++)
		{
			other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
			yield return new WaitForFixedUpdate();
		}
	}
	bool isFlatDown = false;
	IEnumerator MoveTrap(int direction)
	{
		int repeat = 11;
		int moveDist = 2;
		originalLocalPosition = transform.GetChild(0).transform.localPosition;
		
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
		for (int i = 0; i < repeat; i++)
		{
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(originalLocalPosition,originalLocalPosition + new Vector3(0, moveDist * direction, 0),i * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		isFlatDown = true;
		transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

		yield return new WaitWhile(()=> moveDirection < 0);

		if(divideNumList.TrueForAll(x=> GameObject.Find("Main").GetComponent<MoveCountControl>().MoveCount % x != 1))
			transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0.14f,0.14f,0.14f,1));
		for (int i = 0; i < repeat; i++)
		{
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(originalLocalPosition + new Vector3(0, moveDist * direction, 0), originalLocalPosition,i * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		isFlatDown = false;
		transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
	}
	private void Awake()
	{
		originalLocalPosition = transform.GetChild(0).transform.localPosition;
		GameObject.Find("Main").GetComponent<MoveCountControl>().OnVariableChange += VariableChangeHandler;
	}
	
	Vector3 originalLocalPosition;
	public int moveDirection = 1;
	public int divideNum = 1;
	public List<int> divideNumList = new List<int>();
	public int keepDownTime = 1;
	public GameObject buttonObj;

	private void VariableChangeHandler(int newVal)
	{
		if(divideNumList.Find(x => GameObject.Find("Main").GetComponent<MoveCountControl>().MoveCount % x == 1) != 0)
			transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);

		if(divideNumList.Find(x => GameObject.Find("Main").GetComponent<MoveCountControl>().MoveCount % x == 0) != 0)
			{
				moveDirection = -1;
				if(!isFlatDown)
					StartCoroutine("MoveTrap", moveDirection);
			}
		else
			moveDirection = 1;
		
		// else
		// {
		// if (moveDirection > 0)
		// 	moveDirection = -1;
		// else
		// 	moveDirection = 1;
		// }
		// if (moveDirection > 0)
		// 	moveDirection = -1;
		// else
		// 	moveDirection = 1;

		// Debug.Log(moveDirection);
		// if(!isFlatDown)
		// 	StartCoroutine("MoveTrap", moveDirection);
	}



}
