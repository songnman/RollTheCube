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
        
    }

    // Update is called once per frame

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
			yield return new WaitForFixedUpdate();
		}
		gameObject.GetComponent<BoxCollider>().size = new Vector3(0,0,0);
		gameObject.GetComponent<BoxCollider>().center = new Vector3(0,-2,0);
		// gameObject.GetComponent<BoxCollider>().enabled = false;
	}
	void Update()
    {
        if(!isDoorOpen && buttonObj.GetComponent<PressButton>().isButtonActive)
			StartCoroutine("OpenDoor");
    }
}
