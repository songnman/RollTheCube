using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject buttonObj;
	bool isDoorOpen = false;
    // Start is called before the first frame update
    void Start()
    {
		if(buttonObj != null)
		{
			buttonObj.GetComponent<PressButton>().OnVariableChange += VariableChangeHandler;
			transform.GetChild(0).GetComponent<MeshRenderer>().material = buttonObj.transform.GetChild(0).GetComponent<MeshRenderer>().material;
			transform.GetChild(1).GetComponent<MeshRenderer>().material = buttonObj.transform.GetChild(0).GetComponent<MeshRenderer>().material;
		}
		else
		{
			Debug.Log("Button Object is not Assigned.");
		}
    }

	// private void Update() 
	// {
	// 	transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor ("_EmissionColor",);
	// }
	IEnumerator	OpenDoor()
	{
		isDoorOpen = true;
		GameObject doorBelow = transform.GetChild(0).gameObject;
		GameObject doorUpper = transform.GetChild(1).gameObject;
		
		int length = 100;
		for (int i = 0; i < length; i++)
		{
			doorUpper.transform.position += new Vector3(0,0.01f,0);
			doorBelow.transform.position -= new Vector3(0,0.005f,0);
			doorBelow.transform.localScale = Vector3.Lerp(doorBelow.transform.localScale, Vector3.zero, i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
		gameObject.GetComponent<BoxCollider>().size = new Vector3(0,0,0);
		gameObject.GetComponent<BoxCollider>().center = new Vector3(0,20,0);
		// gameObject.GetComponent<BoxCollider>().enabled = false;
		for (int i = 0; i < length; i++)
		{
			doorUpper.transform.localScale = Vector3.Lerp(doorUpper.transform.localScale, Vector3.zero, i * 0.01f);
			// doorBelow.transform.localScale = Vector3.Lerp(doorBelow.transform.localScale, Vector3.zero, i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
	}
	private void VariableChangeHandler(bool newVal)
	{
		// Debug.Log("Value Changed");
		if(!isDoorOpen && buttonObj.GetComponent<PressButton>().IsButtonActive)
			StartCoroutine("OpenDoor");
	}
}
