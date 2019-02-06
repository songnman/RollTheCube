using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ControlCube : MonoBehaviour
{

	public Vector3 touchStartPosition;
	public Vector3 touchInputPosition;
	public Vector3 mouseInputPosition;

	
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
	GameObject cubeObj, cubeGraphic;
	public float xPosition = 0;
	public float yPosition = 0;
	float compareXY = 0;


	private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase) 
	{
		if((Input.touchCount > 0 || touchFingerId > 0) && !isCubeRotate && moveCountControlSc.MoveCount > 0)
		{
			SetSurfaceDirection();
			SetSurfaceEdgeDirection();

			// int checkXY = 0;
			switch (touchPhase)
			{
				case TouchPhase.Began:
					couldBeSwipe = true;
					touchStartPosition = touchPosition;
					touchStartTime = Time.time;
					xPosition = touchPosition.x - touchStartPosition.x;
					yPosition = touchPosition.y - touchStartPosition.y;
					compareXY = Mathf.Abs(Mathf.Max(xPosition,yPosition) * Mathf.Min(xPosition,yPosition));

					maxAngle = 10;
					curAngle = 0;
					checkXY = 0;
					// StartCoroutine("ResetCubeGraphicRotation");
					cubeGraphic.transform.localPosition = Vector3.zero;
					cubeGraphic.transform.localPosition += new Vector3(0,0.1f,0);
				break;

				case TouchPhase.Moved:
					Vector3 touchDirection = touchPosition - touchStartPosition;
					// Vector3 rotateDirection;
					Vector3 axisEdge = downSurface.transform.position;

					xPosition = touchPosition.x - touchStartPosition.x;
					yPosition = touchPosition.y - touchStartPosition.y;
					compareXY = Mathf.Abs(Mathf.Max(xPosition,yPosition) * Mathf.Min(xPosition,yPosition));
					touchInputPosition = touchPosition;
					mouseInputPosition = Input.mousePosition;

					// rotateDirection.y = 0;
					// rotateDirection.z = -touchDirection.x;
					// rotateDirection.x = touchDirection.y;
					// float rotateAngle = 1f;
					// rotateAngle = Vector3.Distance(touchPosition, touchStartPosition) * 2;
					
					if (xPosition * yPosition > 0)
					{
						// if( Mathf.Abs(cubeGraphic.transform.rotation.x) < 0.2f)
							cubeGraphic.transform.localRotation = Quaternion.Euler((touchDirection.y) * 0.05f, 0, (touchDirection.x) * 0.00f);
							cubeGraphic.transform.localPosition = new Vector3((touchDirection.y) * 0.00f, 0.1f, (touchDirection.x) * 0.0005f);
					}
					else
					{
						// if(Mathf.Abs(cubeGraphic.transform.rotation.z) < 0.2)
							cubeGraphic.transform.localRotation = Quaternion.Euler((touchDirection.y) * 0.00f, 0, (touchDirection.x) * -0.05f);
							cubeGraphic.transform.localPosition = new Vector3((touchDirection.y) * -0.0005f, 0.1f, (touchDirection.x) * 0.00f);
					}
				break;
				
				case TouchPhase.Ended:
					couldBeSwipe = false;
					cubeGraphic.transform.localPosition = Vector3.zero;

					// float holdTime = Time.time - touchStartTime;
					
					// cubeGraphic.transform.rotation = Quaternion.Lerp(cubeGraphic.transform.rotation, Quaternion.Euler(0,0,0),0.1f);
					// Debug.Log(xPosition);
					// Debug.Log(yPosition);
					if(isCubeOnLand && compareXY > 200)
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
		cubeObj = GameObject.FindWithTag("Cube");
		moveCountControlSc = GameObject.Find("Main").GetComponent<MoveCountControl>();
		cubeObj.transform.position += new Vector3(0,10,0);
		originalCubePos = cubeObj.transform.position;
		cubeGraphic = cubeObj.transform.GetChild(1).gameObject;
		// int edgeCount = 12;
		// for (int i = 0; i < edgeCount; i++)
		// 	edgeList.Add(gameObject.transform.GetChild(0).GetChild(i).gameObject);
		
		// int surfaceCount = 6;
		for (int i = 0; i < cubeObj.gameObject.transform.GetChild(0).childCount; i++)
			surfaceList.Add(cubeObj.gameObject.transform.GetChild(0).GetChild(i).gameObject);

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
		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x < center.transform.position.x)
				center = surfaceList[i];
		leftSurface = center;
		leftSurface.name = "LeftSurface";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.x > center.transform.position.x)
				center = surfaceList[i];
		rightSurface = center;
		rightSurface.name = "RightSurface";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y < center.transform.position.y)
				center = surfaceList[i];
		downSurface = center;
		downSurface.name = "DownSurface";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.y > center.transform.position.y)
				center = surfaceList[i];
		upSurface = center;
		upSurface.name = "UpSurface";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for(int i=0; i < surfaceList.Count; i++)
			if(surfaceList[i].transform.position.z < center.transform.position.z)
				center = surfaceList[i];
		backSurface = center;
		backSurface.name = "BackSurface";
		
		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
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

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.x < center.transform.position.x)
				center = surfaceEdgeList[i];
		leftEdge = center;
		leftEdge.name = "LeftEdge";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.x > center.transform.position.x)
				center = surfaceEdgeList[i];
		rightEdge = center;
		rightEdge.name = "RightEdge";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.z < center.transform.position.z)
				center = surfaceEdgeList[i];
		backEdge = center;
		backEdge.name = "BackEdge";

		center = cubeObj.gameObject.transform.GetChild(0).gameObject;
		for (int i = 0; i < surfaceEdgeList.Count; i++)
			if (surfaceEdgeList[i].transform.position.z > center.transform.position.z)
				center = surfaceEdgeList[i];
		forwardEdge = center;
		forwardEdge.name = "ForwardEdge";
	}
	public bool isCubeRotate = false;
	public bool isCubeOnLand = false;
	public GameObject particleObj;
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
			Light cubeLight = cubeObj.transform.GetChild(1).GetChild(0).GetComponent<Light>();
			ParticleSystem cubeParticle = cubeObj.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = cubeParticle.main;

			moveCountControlSc.MoveCube();
			cubeLight.intensity = (5f * moveCountControlSc.MoveCount / (float)moveCountControlSc.maxMoveCount);
			// main.startSize = (1.5f * moveCountControlSc.MoveCount / moveCountControlSc.maxMoveCount);
			cubeParticle.transform.localScale = new Vector3(1,1,1) * moveCountControlSc.MoveCount / moveCountControlSc.maxMoveCount;
			for (int i = 0; i < repeatCount; i++)
			{
				cubeObj.transform.RotateAround(axisEdge, rotateDirection, rotateAngle);
				moveCountControlSc.UICube.transform.GetChild(0).Rotate(new Vector3(rotateAngle,0,0));
				yield return new WaitForFixedUpdate();
			}
			SetSurfaceDirection();
			SetSurfaceEdgeDirection();
			Instantiate(Resources.Load("Particles/ShrinkSquare"),downSurface.transform.position + new Vector3(0,0.001f,0),Quaternion.identity);
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
		cubeObj.transform.rotation = Quaternion.Euler(0,0,0);
		// yield return new WaitForSeconds(0.02f);
		isCubeRotate = false;
		yield return StartCoroutine("ResetCubeGraphicRotation");

		// StartCoroutine("ResetCubeGraphicRotation");
		// transform.GetChild(1).rotation = Quaternion.Euler(0,0,0);
	}
	void Update()
	{
		if(cubeObj.transform.position.y < -0.2 && cubeObj.transform.position.y > -0.3)
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
			HandleTouch(touch.fingerId, touch.position, touch.phase);
		}

		// Simulate touch events from mouse events
		if ( Input.touchCount == 0 ) 
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Began);
			}
			else if (Input.GetMouseButton(0)) 
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
			}
			else if (Input.GetMouseButtonUp(0)) 
			{
				HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
			}
		}
	}
}