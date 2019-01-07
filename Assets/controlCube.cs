using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlCube : MonoBehaviour
{
	void Start()
	{
		
	}
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
	public float angleForFlipCube;
	bool isCubeRotate = false;
IEnumerator FlipCube(Vector3 direction)
{
	isCubeRotate = true;
	int x = 40;
	for(int i = 0; i < x; i++)
	{
		// cubeObj.GetComponent<Rigidbody>().angularVelocity = direction * 3;
		
		cubeObj.transform.RotateAround(edgeList[2].transform.localPosition,direction,3);
		yield return new WaitForFixedUpdate();
	}
	yield return new WaitForSeconds(0.5f);
	isCubeRotate = false;
	
}
public GameObject cubeObj;
	void Update()
	{
		// if(Input.GetKeyDown(KeyCode.LeftArrow))
		// {
		// 	Debug.Log("KeyDown!");
		// 	if(!isCubeRotate)
		// 		StartCoroutine("FlipCube", Vector3.forward);
		// }
		// if(Input.GetKeyDown(KeyCode.RightArrow))
		// {
		// 	Debug.Log("KeyDown!");
		// 	if(!isCubeRotate)
		// 		StartCoroutine("FlipCube", Vector3.back);
		// }
		// if(Input.GetKeyDown(KeyCode.UpArrow))
		// {
		// 	Debug.Log("KeyDown!");
		// 	if(!isCubeRotate)
		// 		StartCoroutine("FlipCube", Vector3.right);
		// }
		// if(Input.GetKeyDown(KeyCode.DownArrow))
		// {
		// 	Debug.Log("KeyDown!");
		// 	if(!isCubeRotate)
		// 		StartCoroutine("FlipCube", Vector3.left);
		// }

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