using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ControlCube : MonoBehaviour
{

	Vector3 touchStartPosition;
	float touchStartTime;
	bool couldBeSwipe = false;
    float minSwipeDist = 5f, maxSwipeTime = .5f, comfortZone = 12f;
	float maxAngle, curAngle;
	IEnumerator ResetCubeGraphicRotation()
	{
		// GameObject cubeGraphic = transform.GetChild(1).gameObject;
		int repeat = 4;
		for(int i = 0; i < repeat; i++ )
		{
			cubeGraphic.transform.rotation = Quaternion.Lerp(cubeGraphic.transform.rotation, Quaternion.Euler(0,0,0), i * 0.25f);
			yield return new WaitForFixedUpdate();
		}

	}
	int checkXY;
	GameObject cubeGraphic;

	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) 
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && !isCubeRotate && moveCountControlSc.MoveCount > 0)
		{
			SetSurfaceDirection();
			SetSurfaceEdgeDirection();
			float xPosition = touchPosition.x - touchStartPosition.x;
			float yPosition = touchPosition.y - touchStartPosition.y;

			// int checkXY = 0;
			switch (touchPhase)
			{
				case TouchPhase.Began:
					couldBeSwipe = true;
					touchStartPosition = touchPosition;
					touchStartTime = Time.time;
					maxAngle = 10;
					curAngle = 0;
					checkXY = 0;
					// StartCoroutine("ResetCubeGraphicRotation");
					cubeGraphic.transform.localPosition = Vector3.zero;
					cubeGraphic.transform.localPosition += new Vector3(0,0.1f,0);
				break;

				case TouchPhase.Moved:
					Vector3 touchDirection = touchPosition - touchStartPosition;
					Vector3 rotateDirection;
					Vector3 axisEdge = downSurface.transform.position;
					
					rotateDirection.y = 0;
					rotateDirection.z = -touchDirection.x;
					rotateDirection.x = touchDirection.y;
					float rotateAngle = 1f;
					rotateAngle = Vector3.Distance(touchPosition, touchStartPosition) * 2;
					// Debug.Log(rotateAngle);
					// Debug.Log(xPosition +" : "+ yPosition);

					if ( xPosition * yPosition > 0)
						cubeGraphic.transform.rotation = Quaternion.Euler((touchDirection.y) * 15, 0, (touchDirection.x) * -1);
					else
						cubeGraphic.transform.rotation = Quaternion.Euler((touchDirection.y) * 1, 0, (touchDirection.x) * -15);
					// Debug.Log("-------------------------------");
					// Debug.Log(touchDirection);
					// Debug.Log(cubeGraphic.transform.rotation);
					// if(couldBeSwipe && checkXY == 0 )
					// {
					// 	cubeGraphic.transform.rotation = Quaternion.Euler((touchStartPosition.y - touchPosition.y) * -10, 0, (touchStartPosition.x - touchPosition.x) * 10);
					// 	if		( xPosition * yPosition < -0.08 )
					// 		checkXY = 1;
					// 	else if	( xPosition * yPosition > 0.08 )
					// 		checkXY = 2;
					// 	else
					// 	{
					// 		StartCoroutine("ResetCubeGraphicRotation");
					// 	}
					// }
					
					// if(checkXY != 0)
					// {
					// 	if(checkXY == 1)
					// 	{
					// 	if(touchPosition.x - touchStartPosition.x > 0)
					// 		if(cubeGraphic.transform.rotation.z > -0.2)
					// 			cubeGraphic.transform.RotateAround(axisEdge, Vector3.back, rotateAngle);
					// 	if(touchPosition.x - touchStartPosition.x < 0)
					// 		if(cubeGraphic.transform.rotation.z < 0.2)
					// 			cubeGraphic.transform.RotateAround(axisEdge, Vector3.forward, rotateAngle);
					// 	}
					// 	else if(checkXY == 2)
					// 	{
					// 	if(touchPosition.y - touchStartPosition.y > 0)
					// 		if(cubeGraphic.transform.rotation.x < 0.2)
					// 			cubeGraphic.transform.RotateAround(axisEdge, Vector3.right, rotateAngle);
					// 	if(touchPosition.y - touchStartPosition.y < 0)
					// 		if(cubeGraphic.transform.rotation.x > -0.2)
					// 			cubeGraphic.transform.RotateAround(axisEdge, Vector3.left, rotateAngle);
					// 	}
					// 	// if(checkXY == 1)
					// 	// {
					// 	// 	cubeGraphic.transform.rotation = Quaternion.Euler(0, 0, (touchStartPosition.x - touchPosition.x) * 5);
					// 	// }
					// 	// if(checkXY == 2)
					// 	// {
					// 	// 	cubeGraphic.transform.rotation = Quaternion.Euler((touchStartPosition.y - touchPosition.y) * -5, 0,0);

					// 	// }
					// 	// cubeGraphic.transform.rotation = Quaternion.Euler((touchStartPosition.y - touchPosition.y) * -5, 0, (touchStartPosition.x - touchPosition.x) * 5);
					// 	// cubeGraphic.transform.position = new Vector3(cubeGraphic.transform.position.x,-0.2f + Vector3.Distance(touchStartPosition,touchPosition) * 0.01f,cubeGraphic.transform.position.z);
					// }
				break;
				
				case TouchPhase.Ended:
					couldBeSwipe = false;
					cubeGraphic.transform.localPosition = Vector3.zero;


					// float holdTime = Time.time - touchStartTime;
					
					// cubeGraphic.transform.rotation = Quaternion.Lerp(cubeGraphic.transform.rotation, Quaternion.Euler(0,0,0),0.1f);

					if(isCubeOnLand)
					{
						if(xPosition > 0 && yPosition > 0)
							StartCoroutine("FlipCube", "Forward");
						else if(xPosition < 0 && yPosition < 0)
							StartCoroutine("FlipCube", "Back");
						else if(xPosition > 0 && yPosition < 0)
							StartCoroutine("FlipCube", "Right");
						else if(xPosition < 0 && yPosition > 0)
							StartCoroutine("FlipCube", "Left");
						else
							StartCoroutine("ResetCubeGraphicRotation");
					}
					else
					{
						StartCoroutine("ResetCubeGraphicRotation");
					}
					checkXY = 0;
				break;
			}
		}
	}
	// public List<GameObject> edgeList = new List<GameObject>();
	public List<GameObject> surfaceList = new List<GameObject>();	
	Vector3 originalCubePos;
	MoveCountControl moveCountControlSc;
	void Start()
	{
		moveCountControlSc = GameObject.Find("Main").GetComponent<MoveCountControl>();
		transform.position += new Vector3(0,10,0);
		originalCubePos = transform.position;
		cubeGraphic = transform.GetChild(1).gameObject;
		// int edgeCount = 12;
		// for (int i = 0; i < edgeCount; i++)
		// 	edgeList.Add(gameObject.transform.GetChild(0).GetChild(i).gameObject);
		
		// int surfaceCount = 6;
		for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
			surfaceList.Add(gameObject.transform.GetChild(0).GetChild(i).gameObject);

		SetSurfaceDirection();
		SetSurfaceEdgeDirection();

		// SetSurfaceDirection();
	}
	// public List<GameObject> edgeListTriggerOn =  new List<GameObject>();
	public List<GameObject> surfaceListTriggerOn =  new List<GameObject>();
	GameObject leftSurface, rightSurface, forwardSurface, backSurface, upSurface, downSurface;
	GameObject center;
	public void SetSurfaceDirection()
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
	public List<GameObject> surfaceEdgeList = new List<GameObject>();
	GameObject leftEdge, rightEdge, forwardEdge, backEdge;
	public void SetSurfaceEdgeDirection()
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
		leftEdge = center;
		leftEdge.name = "LeftEdge";

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
	public bool isCubeRotate = false;
	public bool isCubeOnLand = false;
	IEnumerator FlipCube(string direction)
	{
		// yield return new WaitUntil(() => isCubeOnLand);
		// yield return new WaitUntil(() => !isCubeRotate);
		isCubeRotate = true;
		SetSurfaceDirection();
		SetSurfaceEdgeDirection();
		bool isDirectionBlock = false;
		
		Vector3 rotateDirection = Vector3.zero;
		Vector3 axisEdge = Vector3.zero;
		if(direction == "Left")
		{
			rotateDirection = Vector3.forward;
			axisEdge = leftEdge.transform.position;
			if(leftSurface.GetComponent<CubeEdge>().triggerCount > 0)
				isDirectionBlock = true;
		}
		else if (direction == "Right")
		{
			rotateDirection = Vector3.back;
			axisEdge = rightEdge.transform.position;
			if(rightSurface.GetComponent<CubeEdge>().triggerCount > 0)
				isDirectionBlock = true;
		}
		else if(direction =="Forward")
		{
			rotateDirection = Vector3.right;
			axisEdge = forwardEdge.transform.position;
			if(forwardSurface.GetComponent<CubeEdge>().triggerCount > 0)
				isDirectionBlock = true;
		}
		else if(direction =="Back")
		{
			rotateDirection = Vector3.left;
			axisEdge = backEdge.transform.position;
			if(backSurface.GetComponent<CubeEdge>().triggerCount > 0)
				isDirectionBlock = true;
		}
		else
		{
			Debug.Log("Error : Direction or Axis are Not Correct.");
		}

		if(!isDirectionBlock)
		{
			int repeatCount = 9;
			float rotateAngle = 10f;
			Light cubeLight = transform.GetChild(1).GetChild(0).GetComponent<Light>();
			ParticleSystem cubeParticle = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = cubeParticle.main;

			moveCountControlSc.MoveCube();
			cubeLight.intensity = (5f * moveCountControlSc.MoveCount / (float)moveCountControlSc.maxMoveCount);
			// main.startSize = (1.5f * moveCountControlSc.MoveCount / moveCountControlSc.maxMoveCount);
			cubeParticle.transform.localScale = new Vector3(1,1,1) * moveCountControlSc.MoveCount / moveCountControlSc.maxMoveCount;
			for (int i = 0; i < repeatCount; i++)
			{
				transform.RotateAround(axisEdge, rotateDirection, rotateAngle);
				moveCountControlSc.UICube.transform.GetChild(0).Rotate(new Vector3(rotateAngle,0,0));
				yield return new WaitForFixedUpdate();
			}
			SetSurfaceDirection();
			SetSurfaceEdgeDirection();
			
			if(moveCountControlSc.MoveCount < 1 && !MoveCountControl.isLevelComplete && !moveCountControlSc.isLevelFailed) //[2019-01-15 03:39:37] 남은턴 계산하는부분.
				moveCountControlSc.StartCoroutine("LevelFailed");
		}
		else
		{
			int repeatCount = 10;
			for (int i = 0; i < repeatCount; i++)
			{
				cubeGraphic.transform.localPosition = Random.insideUnitSphere * 0.1f;
				yield return new WaitForFixedUpdate();

			}
			cubeGraphic.transform.localPosition = Vector3.zero;
		}
		transform.rotation = Quaternion.Euler(0,0,0);
		// yield return new WaitForSeconds(0.02f);
		isCubeRotate = false;
		yield return StartCoroutine("ResetCubeGraphicRotation");

		// StartCoroutine("ResetCubeGraphicRotation");
		// transform.GetChild(1).rotation = Quaternion.Euler(0,0,0);
	}
	void Update()
	{
		if(transform.position.y < -0.2 && transform.position.y > -0.3)
			isCubeOnLand = true;
		else
			isCubeOnLand = false;

		if(isCubeOnLand && !isCubeRotate && moveCountControlSc.MoveCount > 0)
		{
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				isCubeRotate = true;
				StartCoroutine("FlipCube", "Left");
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				isCubeRotate = true;
				StartCoroutine("FlipCube", "Right");
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				isCubeRotate = true;
				StartCoroutine("FlipCube", "Forward");
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				isCubeRotate = true;
				StartCoroutine("FlipCube", "Back");
			}
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
}