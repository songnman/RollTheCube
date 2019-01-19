using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCountControl : MonoBehaviour
{
	// int moveCount;
	Text moveCountText;
	[HideInInspector] public GameObject UICube;
	[HideInInspector] public Image blackMask;
	public int maxMoveCount;
	private int moveCount = 0;
	public int MoveCount
	{
		get {return moveCount;}
		set {
			if (moveCount == value) return;
			moveCount = value;
			if (OnVariableChange != null)
				OnVariableChange(moveCount);
		}
	}
	public delegate void OnVariableChangeDelegate(int newVal);
	public event OnVariableChangeDelegate OnVariableChange;
	public static bool isLevelComplete;
	private void Start()
	{

		Screen.SetResolution(1280, 720,  true);     /*가로 뷰 */
		isLevelComplete = false;
		MoveCount = maxMoveCount;
		UICube = GameObject.Find("UICube");
		blackMask = GameObject.Find("BlackMask").GetComponent<Image>();
		moveCountText = UICube.transform.GetChild(1).GetComponent<Text>();
		moveCountText.text = moveCount.ToString();
		blackMask.gameObject.SetActive(true);
		blackMask.color = Color.black;
		// gameObject.GetComponent<MoveCountControl>().OnVariableChange += VariableChangeHandler;
		StartCoroutine("LevelStart");
	}
	public void MoveCube()
	{
		MoveCount--;
		moveCountText.text = moveCount.ToString();
	}
	public bool isLevelFailed = false;
	public IEnumerator LevelFailed()
	{
		isLevelFailed = true;
		blackMask.gameObject.SetActive(true);
		int length = 100;
		for (int i = 0; i < length; i++)
		{
			blackMask.color = Color.Lerp(new Color(0,0,0,0),new Color(0,0,0,1), i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public IEnumerator LevelStart()
	{
		blackMask.gameObject.SetActive(true);
		int length = 50;
		for (int i = 0; i < length; i++)
		{
			blackMask.color = Color.Lerp(Color.black,Color.clear, i * 0.02f);
			yield return new WaitForFixedUpdate();
		}
		blackMask.gameObject.SetActive(false);
	}
}
