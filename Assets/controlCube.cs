using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ControlCube : MonoBehaviour
{

	Vector3 touchStartPosition;

	float touchStartTime;
	bool couldBeSwipe = false;
	float minSwipeDist = 5f, maxSwipeTime = .5f;
	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) 
	{
		// Rigidbody rb = cubeObj.GetComponent<Rigidbody>();
		if((Input.touchCount > 0 || touchFingerId > 0)) 
		{
			Vector3 touchPos = touchPosition;
			switch (touchPhase)
			{
				case TouchPhase.Began:
					touchStartPosition = touchPosition;
					couldBeSwipe = true;
					touchStartTime = Time.time;
					break;

				case TouchPhase.Moved:
					
					if(couldBeSwipe)
					{
						// rb.angularVelocity = touchStartPosition - touchPosition;
					}

					// if(Mathf.Abs(touchPosition.x - touchStartPosition.x) > comfortZone || Time.time - touchStartTime > maxSwipeTime)
					// 	couldBeSwipe = false;
					
				// if(Vector2.Distance(touchStartPosition,touchPos) > 4 && !interActionSc.isFever)
				// 	Instantiate(particlePrefab, touchPos, Quaternion.identity);
				// Debug.Log(Vector2.Distance(touchStartPosition,touchPos));
				break;
				
				// [2018-11-08 04:04:02]  Stationary 판정이 강해서 계속 couldBeSwipe Bool이 false를 유지해버린다.
				// case TouchPhase.Stationary:
				//     couldBeSwipe = false;
				//     break;

				case TouchPhase.Ended:
					// rb.angularVelocity = new Vector3(0,0,0);
					float swipeTime = Time.time - touchStartTime;
					float swipeDist = (touchPosition - touchStartPosition).magnitude;
					
					if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
					{
						var swipeDirection = Mathf.Sign(touchStartPosition.x - touchPosition.x);
						Debug.Log(swipeDirection);
						Debug.Log("Swipe On !");
						if(swipeDirection < 0)
							Debug.Log("Direction : 0");
					}
					// if(Vector2.Distance(touchStartPosition,touchPos) > 4 && !interActionSc.isFever)
					//     FeverStart();
					break;
			}
		}
	}
	public List<GameObject> edgeList = new List<GameObject>();
	public List<GameObject> surfaceList = new List<GameObject>();	
	void Start()
	{
		int edgeCount = 12;
		for (int i = 0; i < edgeCount; i++)
			edgeList.Add(gameObject.transform.GetChild(0).GetChild(i).gameObject);
		
		int surfaceCount = 6;
		for (int i = 0; i < surfaceCount; i++)
			surfaceList.Add(gameObject.transform.GetChild(1).GetChild(i).gameObject);

		GetTriggerOnList();
	}
	public List<GameObject> edgeListTriggerOn =  new List<GameObject>();
	public List<GameObject> surfaceListTriggerOn =  new List<GameObject>();
	GameObject leftSurface, rightSurface, forwardSurface, backSurface, upSurface, downSurface;
	GameObject center;
	private void GetTriggerOnList()
	{
		edgeListTriggerOn.Clear();
		surfaceListTriggerOn.Clear();
		edgeListTriggerOn = new List<GameObject>();
		surfaceListTriggerOn = new List<GameObject>();
		foreach (var item in edgeList)
		{
			if (item.GetComponent<CubeEdge>().isTriggerOn)
			{
				edgeListTriggerOn.Add(item);
			}
		}
		foreach (var item in surfaceList)
		{
			if (item.GetComponent<CubeEdge>().isTriggerOn)
			{
				surfaceListTriggerOn.Add(item);
			}
		}
		
		//[2019-01-08 22:48:43] 정육면체의 6면에 맞춰서 방향을 재설정해줌.
		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x < center.transform.position.x)
				center = surfaceList[i];
		leftSurface = center;

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x > center.transform.position.x)
				center = surfaceList[i];
		rightSurface = center;

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y < center.transform.position.y)
				center = surfaceList[i];
		downSurface = center;

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y > center.transform.position.y)
				center = surfaceList[i];
		upSurface = center;

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.z < center.transform.position.z)
				center = surfaceList[i];
		backSurface = center;

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.z > center.transform.position.z)
				center = surfaceList[i];
		forwardSurface = center;


	}

	public float angleForFlipCube;
	bool isCubeRotate = false;
IEnumerator FlipCube(Vector3 direction)
{
	yield return new WaitUntil(()=>!isCubeRotate);

	int x = 15;
	float y = 6f;
	isCubeRotate = true;
	GetTriggerOnList();

	center = gameObject.transform.GetChild(0).gameObject;
	if (direction == Vector3.forward && !leftSurface.GetComponent<CubeEdge>().isTriggerOn)
	{
		for (int i = 0; i < edgeListTriggerOn.Count; i++)
			if (edgeListTriggerOn[i].transform.position.x < center.transform.position.x && edgeListTriggerOn[i].transform.position.y < 0)
				center = edgeListTriggerOn[i];
		
		for (int i = 0; i < x; i++)
		{
			transform.RotateAround(center.transform.position, direction, y);
			yield return new WaitForFixedUpdate();
		}
	}
	else if (direction == Vector3.back && !rightSurface.GetComponent<CubeEdge>().isTriggerOn)
	{
		for (int i = 0; i < edgeListTriggerOn.Count; i++)
			if (edgeListTriggerOn[i].transform.position.x > center.transform.position.x && edgeListTriggerOn[i].transform.position.y < 0)
				center = edgeListTriggerOn[i];
		
		for (int i = 0; i < x; i++)
		{
			transform.RotateAround(center.transform.position, direction, y);
			yield return new WaitForFixedUpdate();
		}
	}
	else if (direction == Vector3.right && !forwardSurface.GetComponent<CubeEdge>().isTriggerOn)
	{
		for (int i = 0; i < edgeListTriggerOn.Count; i++)
			if (edgeListTriggerOn[i].transform.position.z > center.transform.position.z && edgeListTriggerOn[i].transform.position.y < 0)
				center = edgeListTriggerOn[i];
		
		for (int i = 0; i < x; i++)
		{
			transform.RotateAround(center.transform.position, direction, y);
			yield return new WaitForFixedUpdate();
		}
	}
	else if (direction == Vector3.left && !backSurface.GetComponent<CubeEdge>().isTriggerOn)
	{
		for (int i = 0; i < edgeListTriggerOn.Count; i++)
			if (edgeListTriggerOn[i].transform.position.z < center.transform.position.z && edgeListTriggerOn[i].transform.position.y < 0)
				center = edgeListTriggerOn[i];
		
		for (int i = 0; i < x; i++)
		{
			transform.RotateAround(center.transform.position, direction, y);
			yield return new WaitForFixedUpdate();
		}
	}
	// transform.position = new Vector3(transform.position.x,0,transform.position.z);
	isCubeRotate = false;
	// gameObject.GetComponent<Rigidbody>().freezeRotation = true;
}

	public GameObject cubeObj;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1))
		{
			GetTriggerOnList();
			foreach (var item in edgeListTriggerOn)
				Debug.Log(item);
		}
		if(Input.GetKeyDown(KeyCode.F2))
		{
			GetTriggerOnList();
			foreach (var item in surfaceListTriggerOn)
				Debug.Log(item);

			Debug.Log("Left : " + leftSurface);
			Debug.Log("Right : " + rightSurface);
			Debug.Log("Down : " + downSurface);
			Debug.Log("Up : " + upSurface);
			Debug.Log("Back : " + backSurface);
			Debug.Log("Forward : " + forwardSurface);

		}
		if(Input.GetKeyDown(KeyCode.F3))
		{
			SceneManager.LoadScene("SampleScene");
		}
		if(Input.GetKeyDown(KeyCode.F4))
		{
			transform.position = Vector3.zero;
		}



		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", Vector3.forward);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", Vector3.back);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", Vector3.right);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", Vector3.left);
		}

		foreach (Touch touch in Input.touches) 
		{
			HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase);
		}

		// Simulate touch events from mouse events
		if ( Input.touchCount == 0 ) 
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
			}
			else if (Input.GetMouseButton(0)) 
			{
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
			}
			else if (Input.GetMouseButtonUp(0)) 
			{
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
			}
			
		}
		
	}

	// private void FixedUpdate() 
	// {
	// 	if (Input.GetAxisRaw ("Horizontal") != 0) 
	// 	{
	// 		cubeObj.GetComponent<Rigidbody>().angularVelocity = Vector3.right * Input.GetAxisRaw ("Horizontal") * 2;
	// 		// cubeObj.transform.Translate(Vector3.right * Input.GetAxisRaw ("Horizontal") * 0.1f);
	// 	} 
	// 	else if (Input.GetAxisRaw ("Vertical") != 0) 
	// 	{
	// 		cubeObj.GetComponent<Rigidbody>().angularVelocity = Vector3.forward * Input.GetAxisRaw ("Vertical") * 2;
	// 		// cubeObj.transform.Translate(Vector3.forward * Input.GetAxisRaw ("Vertical") * 0.1f);
	// 	}
	// }

}