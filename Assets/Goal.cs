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
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.transform.GetChild(0).gameObject.SetActive(false);

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
		// yield return new WaitForSeconds(0.2f);
		// yield return new WaitUntil( () => other.transform.position.y > 0.8);
		for (int i = 0; i < 100; i++)
		{
			other.GetComponent<Rigidbody>().AddTorque(Vector3.up * 500,ForceMode.Acceleration);
			yield return new WaitForFixedUpdate();
		}

		// gameObject.SetActive(false);
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
