using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	private void Start()
	{
		originalPos = gameObject.transform.position;
		// gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0,1000,0),ForceMode.Acceleration);
	}
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Cube")
		{
			StartCoroutine("RollTheCube", other);
			// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);

			Debug.Log("!");
		}
	}
	private void OnTiriggerExit(Collision other) 
	{

	}

	IEnumerator RollTheCube(Collider other)
	{
		other.GetComponent<ControlCube>().isCubeRotate = true;
		other.GetComponent<Rigidbody>().useGravity = false;
		other.GetComponent<Rigidbody>().freezeRotation = false;
		for (int i = 0; i < 5; i++)
		{
			other.transform.position = other.transform.position + new Vector3(0,0.1f,0);
			yield return new WaitForFixedUpdate();
		}
		for (int i = 0; i < 10; i++)
		{
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0,30,0),ForceMode.Acceleration);
			yield return new WaitForFixedUpdate();
		}
		gameObject.SetActive(false);
		for (int i = 0; i < 200; i++)
		{
			other.GetComponent<Rigidbody>().AddTorque(Vector3.up * 100);
			yield return new WaitForFixedUpdate();
		}
	}
	float curTime;
	Vector3 originalPos;
	public bool isUptime = false;
	private void Update() 
	{
		curTime += Time.deltaTime;
		if(!isUptime)
			gameObject.transform.position -= new Vector3(0,0.001f,0);
		else 
			gameObject.transform.position += new Vector3(0,0.001f,0);

		if(gameObject.transform.position.y <= -0.2)
			isUptime = true;
		else if(gameObject.transform.position.y >= 0f)
			isUptime = false;
		// else 
		// 	transform.position = originalPos;

	}
}
