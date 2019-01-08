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
		// int edgeCount = 12;
		// for (int i = 0; i < edgeCount; i++)
		// 	edgeList.Add(gameObject.transform.GetChild(0).GetChild(i).gameObject);
		
		// int surfaceCount = 6;
		for (int i = 0; i < gameObject.transform.GetChild(1).childCount; i++)
			surfaceList.Add(gameObject.transform.GetChild(1).GetChild(i).gameObject);

		// SetSurfaceDirection();
	}
	public List<GameObject> edgeListTriggerOn =  new List<GameObject>();
	public List<GameObject> surfaceListTriggerOn =  new List<GameObject>();
	GameObject leftSurface, rightSurface, forwardSurface, backSurface, upSurface, downSurface;
	GameObject center;
	private void SetSurfaceDirection()
	{
		//[2019-01-08 22:48:43] 정육면체의 6면에 맞춰서 방향을 재설정해줌.
		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x < center.transform.position.x)
				center = surfaceList[i];
		leftSurface = center;
		leftSurface.name = "LeftSurface";

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x > center.transform.position.x)
				center = surfaceList[i];
		rightSurface = center;
		rightSurface.name = "RightSurface";

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y < center.transform.position.y)
				center = surfaceList[i];
		downSurface = center;
		downSurface.name = "DownSurface";

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y > center.transform.position.y)
				center = surfaceList[i];
		upSurface = center;
		upSurface.name = "UpSurface";

		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.z < center.transform.position.z)
				center = surfaceList[i];
		backSurface = center;
		backSurface.name = "BackSurface";
		
		center = gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.z > center.transform.position.z)
				center = surfaceList[i];
		forwardSurface = center;
		forwardSurface.name = "ForwardSurface";
	}
	bool isCubeRotate = false;
	public List<GameObject> surfaceEdgeList = new List<GameObject>();
	GameObject back, rightEdge, forwardEdge, backEdge;
	IEnumerator FlipCube(string direction)
	{
		yield return new WaitUntil(() => !isCubeRotate);
		isCubeRotate = true;

		SetSurfaceDirection();
		SetSurfaceEdgeDirection();
		bool isDirectionBlock = false;
		Vector3 rotateDirection = Vector3.zero;
		Vector3 axisEdge = Vector3.zero;
		if(direction == "Left")
		{
			rotateDirection = Vector3.forward;
			axisEdge = back.transform.position;
			if(leftSurface.GetComponent<CubeEdge>().isTriggerOn)
				isDirectionBlock = true;
		}
		else if (direction == "Right")
		{
			rotateDirection = Vector3.back;
			axisEdge = rightEdge.transform.position;
			if(rightSurface.GetComponent<CubeEdge>().isTriggerOn)
				isDirectionBlock = true;
			
		}
		else if(direction =="Forward")
		{
			rotateDirection = Vector3.right;
			axisEdge = forwardEdge.transform.position;
			if(forwardSurface.GetComponent<CubeEdge>().isTriggerOn)
				isDirectionBlock = true;
		}
		else if(direction =="Back")
		{
			rotateDirection = Vector3.left;
			axisEdge = backEdge.transform.position;
			if(backSurface.GetComponent<CubeEdge>().isTriggerOn)
				isDirectionBlock = true;
		}
		else
		{
			Debug.Log("Error : Direction or Axis are Not Correct.");
		}

		if(!isDirectionBlock)
		{
			int repeatCount = 15;
			float rotateAngle = 6f;
			for (int i = 0; i < repeatCount; i++)
			{
				transform.RotateAround(axisEdge, rotateDirection, rotateAngle);
				yield return new WaitForFixedUpdate();
			}
			SetSurfaceDirection();
			SetSurfaceEdgeDirection();
		}

		isCubeRotate = false;
	}

	private void SetSurfaceEdgeDirection()
	{
		surfaceEdgeList.Clear();
		for (int i = 0; i < downSurface.transform.childCount; i++)
		{
			surfaceEdgeList.Add(downSurface.transform.GetChild(i).gameObject);
		}

		center = gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.x < center.transform.position.x)
				center = surfaceEdgeList[i];
		back = center;
		back.name = "LeftEdge";

		center = gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.x > center.transform.position.x)
				center = surfaceEdgeList[i];
		rightEdge = center;
		rightEdge.name = "RightEdge";
		
		center = gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.z < center.transform.position.z)
				center = surfaceEdgeList[i];
		backEdge = center;
		backEdge.name = "BackEdge";

		center = gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.z > center.transform.position.z)
				center = surfaceEdgeList[i];
		forwardEdge = center;
		forwardEdge.name = "ForwardEdge";
	}

	public GameObject cubeObj;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1))
		{
			SetSurfaceDirection();
			foreach (var item in edgeListTriggerOn)
				Debug.Log(item);
		}
		if(Input.GetKeyDown(KeyCode.F2))
		{
			SetSurfaceDirection();
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
				StartCoroutine("FlipCube", "Left");
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", "Right");
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", "Forward");
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if(!isCubeRotate)
				StartCoroutine("FlipCube", "Back");
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