using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour, IPointerDownHandler
{
	public GameObject cubeObj;
	public GameObject wallObj;
	public GameObject typoObj;
	public GameObject goalObj;
	public Image blackMask;
	Light cubeLight;
	ParticleSystem cubeParticle;
    void Start()
    {
		cubeObj.GetComponent<Rigidbody>().AddTorque(Vector3.up * 200, ForceMode.Acceleration);
		cubeObj.GetComponent<Rigidbody>().AddTorque(Vector3.right * 100, ForceMode.Acceleration);
		wallObj.GetComponent<Rigidbody>().AddTorque(Vector3.down * 50, ForceMode.Acceleration);
		// typoObj.GetComponent<Rigidbody>().AddTorque(Vector3.right * 50, ForceMode.Acceleration);
		StartCoroutine("RollTheCube");
		cubeLight = cubeObj.transform.GetChild(1).GetChild(0).GetComponent<Light>();
		cubeParticle = cubeObj.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();

    }
	bool isIncrese = true;
	int increseCount;
    void Update()
    {
		if(isIncrese)
		{
			cubeLight.intensity += 0.01f;
			cubeLight.range += 0.01f;
			typoObj.transform.localPosition += Vector3.up * 0.01f;
			// cubeParticle.transform.localScale = cubeParticle.transform.localScale + new Vector3(0, 0.05f,0);
		}
		else
		{
			cubeLight.intensity -= 0.01f;
			cubeLight.range -= 0.01f;
			typoObj.transform.localPosition -= Vector3.up * 0.01f;
			// cubeParticle.transform.localScale = cubeParticle.transform.localScale - new Vector3(0, 0.05f,0);
		}
		increseCount++;
		if(increseCount > 100)
		{
			increseCount = 0;
			if(isIncrese)
				isIncrese = false;
			else
				isIncrese = true;
		}
    }

	bool isLoading = true;
	IEnumerator RollTheCube()
	{
		isLoading = true;
		typoObj.GetComponent<Text>().color = Color.clear;
		goalObj.transform.GetChild(0).localScale = Vector3.zero;
		goalObj.transform.GetChild(1).localScale = Vector3.zero;

		blackMask.gameObject.SetActive(true);
		int length = 100;
		for (int i = 0; i < length; i++)
		{
			blackMask.color = Color.Lerp(Color.black,Color.clear, i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
		for (int i = 0; i < length; i++)
		{
			typoObj.GetComponent<Text>().color = Color.Lerp(Color.clear, Color.white, i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
		for (int i = 0; i < length; i++)
		{
			goalObj.transform.GetChild(0).localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i * 0.01f);
			goalObj.transform.GetChild(1).localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i * 0.01f);
			yield return new WaitForFixedUpdate();
		}
		isLoading = false;
	}

		IEnumerator StartGame()
		{
			isLoading = true;
			blackMask.gameObject.SetActive(true);
			int length = 100;
			for (int i = 0; i < length; i++)
			{
				blackMask.color = Color.Lerp(new Color(0,0,0,0),new Color(0,0,0,1), i * 0.01f);
				yield return new WaitForFixedUpdate();
			}
			for (int i = 0; i < length; i++)
			{
				goalObj.transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i * 0.01f);
				goalObj.transform.GetChild(1).localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i * 0.01f);
				yield return new WaitForFixedUpdate();
			}
			SceneManager.LoadScene("Level1");
		}

	public void OnPointerDown(PointerEventData eventData)
	{
		// Debug.Log("Yeah!");
		if(!isLoading)
			StartCoroutine("StartGame");
	}

}
