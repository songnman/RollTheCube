using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
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

	}

	IEnumerator Trap(Collider other)
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
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0,10,0),ForceMode.Acceleration);
			yield return new WaitForFixedUpdate();
		}
		// yield return new WaitForSeconds(1);
		for (int i = 0; i < 100; i++)
		{
			other.GetComponent<Rigidbody>().AddTorque(Vector3.up * 1000);
			yield return new WaitForFixedUpdate();
		}
	}
}
