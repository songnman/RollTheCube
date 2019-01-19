using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class touchScreen : MonoBehaviour
{
	bool isMouseDown = false;
	public Vector2 oriDownPos;
	public Vector2 curDownPos;
	public Vector2 diffBetweenPos;
	GameObject cubeObj, cubeGraphic;
	ControlCube cubeContorlSc;
	private void Start()
	{
		cubeObj = GameObject. FindWithTag("Cube");
		cubeGraphic = cubeObj.transform.GetChild(1).gameObject;
		cubeContorlSc = cubeObj.GetComponent<ControlCube>();
	}
	
	private void OnMouseDown()
	{
		isMouseDown = true;
		oriDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		curDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		diffBetweenPos = curDownPos - oriDownPos;
		cubeContorlSc.SetSurfaceDirection();
		cubeContorlSc.SetSurfaceEdgeDirection();

	}
	private void OnMouseDrag()
	{
		curDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		diffBetweenPos = curDownPos - oriDownPos;
		cubeGraphic.transform.rotation = Quaternion.Euler(diffBetweenPos.y * 10, 0, diffBetweenPos.x * -10);
		
	}
	private void OnMouseUp()
	{
		if(diffBetweenPos.x > 0 && diffBetweenPos.y > 0)
			cubeContorlSc.StartCoroutine("FlipCube", "Forward");
		else if(diffBetweenPos.x < 0 && diffBetweenPos.y < 0)
			cubeContorlSc.StartCoroutine("FlipCube", "Back");
		else if(diffBetweenPos.x > 0 && diffBetweenPos.y < 0)
			cubeContorlSc.StartCoroutine("FlipCube", "Right");
		else if(diffBetweenPos.x < 0 && diffBetweenPos.y > 0)
			cubeContorlSc.StartCoroutine("FlipCube", "Left");
		isMouseDown = false;
	}
}
