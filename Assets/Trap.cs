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
	// private void OncollisionEnter(Collider other) 
	// {
	// 	if(other.tag == "Cube" && moveDirection == -1)
	// 	{
	// 		StartCoroutine("FallingTrap", other);
	// 		// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
	// 		// collisionCount++;
	// 		Debug.Log("!");
	// 	}
	// }
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
	public bool isFlatDown = false;
	IEnumerator MoveTrap(int direction)
	{
		int repeat = 11;
		int moveDist = 2;
		originalLocalPosition = transform.GetChild(0).transform.localPosition;
		
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
		for (int i = 0; i < repeat; i++)
		{
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(originalLocalPosition,originalLocalPosition + new Vector3(0, moveDist * direction, 0),i * 0.1f);
			transform.GetChild(0).transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		isFlatDown = true;
		transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

		// yield return new WaitWhile(()=> moveDirection < 0);
		yield return new WaitWhile(()=> keepDownTime > 0);

		if(keepUpTime > 1)
			transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0.14f,0.14f,0.14f,1));
		for (int i = 0; i < repeat; i++)
		{
			transform.GetChild(0).transform.localPosition = Vector3.Lerp(originalLocalPosition + new Vector3(0, moveDist * direction, 0), originalLocalPosition,i * 0.1f);
			transform.GetChild(0).transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i * 0.1f);
			yield return new WaitForFixedUpdate();
		}
		isFlatDown = false;
		transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
	}
	private void Awake()
	{	
		int childCount = transform.GetChild(1).childCount;
		for (int i = 0; i < childCount; i++)
		{
			lightList.Add(transform.GetChild(1).GetChild(i).gameObject);
		}
		if(startKeepUpTime != -1)
			keepUpTime = startKeepUpTime;
		else
			keepUpTime = oriKeepUpTime;

		keepDownTime = oriKeepDownTime;
		originalLocalPosition = transform.GetChild(0).transform.localPosition;
		GameObject.Find("Main").GetComponent<MoveCountControl>().OnVariableChange += VariableChangeHandler;
	}
	List<GameObject> lightList = new List<GameObject>();
	Vector3 originalLocalPosition;
	public int moveDirection = 1;
	// public int divideNum = 1;
	// public List<int> divideNumList = new List<int>();
	public int startKeepUpTime;
	int keepUpTime = 1;
	public int oriKeepUpTime;
	int keepDownTime = 1;
	public int oriKeepDownTime;
	public GameObject buttonObj;

	private void LightOff()
	{
		lightList.ForEach(x => {x.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);});
	}
	private void LightOn()
	{
		if (keepUpTime == 1 )
		{
				lightList.ForEach(x => {x.transform.localScale = Vector3.zero;});
		}
		else if (keepUpTime < 1 ) // [2019-01-17 13:34:33] 발판이 내려간 시간만큼 녹색불로 표시한다.
		{
			// lightList.ForEach(x => {x.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);});
			// lightList.ForEach(x => {x.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);});
			lightList.ForEach(x => {x.transform.localScale = Vector3.zero;});
			
			if(keepDownTime < 10)
			{
				for (int i = 0; i < keepDownTime; i++)
				{
					lightList[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
					lightList[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
				}
			}
			else
			{
				lightList.ForEach(x => {x.transform.localScale = Vector3.zero;});
			}
		}
		else if (keepUpTime < 10) // [2019-01-17 13:34:54]  발판이 올라가있을 시간을 빨간불로 표시한다.
		{
			// lightList.ForEach(x => {x.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);});
			// lightList.ForEach(x => {x.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);});
			lightList.ForEach(x => {x.transform.localScale = Vector3.zero;});
			

			for (int i = 0; i < keepUpTime - 1; i++)
			{
				lightList[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				lightList[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
			}
		}
		else
		{
			lightList.ForEach(x => {x.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);});
		}

	}
		private void VariableChangeHandler(int newVal)
	{

		if(GameObject.Find("Main").GetComponent<MoveCountControl>().MoveCount != GameObject.Find("Main").GetComponent<MoveCountControl>().maxMoveCount)
		{
			if(isFlatDown)
			{
				keepDownTime--;
				if(keepDownTime == 0)
					keepUpTime = oriKeepUpTime;
			}
			else
			{
				keepUpTime--;
				if(keepUpTime == 0)
					keepDownTime = oriKeepDownTime;
			}
		}
		
		LightOn();

		if(keepUpTime == 1)
			transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);

		if(keepUpTime == 0)
			{
				moveDirection = -1;
				if(!isFlatDown)
					StartCoroutine("MoveTrap", moveDirection);
			}
		else if(keepDownTime == 0)
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
