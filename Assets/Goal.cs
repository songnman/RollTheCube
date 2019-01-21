using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
	private void Start()
	{
		originalPos = gameObject.transform.position;
		// gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0,1000,0),ForceMode.Acceleration);
	}
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Cube")
		{
			MoveCountControl.isLevelComplete = true;
			StartCoroutine("RollTheCube", other);
			// other.transform.position = other.transform.position - new Vector3(0,0.1f,0);
			Debug.Log("!");
		}
	}
	private void OnTiriggerExit(Collision other) 
	{

	}
	
	IEnumerator RollTheCube(Collider other)
	{
		Light cubeLight = other.transform.GetChild(1).GetChild(0).GetComponent<Light>();
		ParticleSystem cubeParticle = other.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = cubeParticle.main;
		Image blackMask = GameObject.Find("Main").GetComponent<MoveCountControl>().blackMask;

		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.transform.GetChild(0).gameObject.SetActive(false);
		gameObject.transform.GetChild(1).gameObject.SetActive(false);
		gameObject.transform.GetChild(2).gameObject.SetActive(true);

		// other.GetComponent<ControlCube>().isCubeRotate = true;
		other.GetComponent<Rigidbody>().useGravity = false;
		other.GetComponent<Rigidbody>().freezeRotation = false;
		
		for (int i = 0; i < 10; i++)
		{
			other.transform.position = other.transform.position + new Vector3(0, 0.1f, 0);
			yield return new WaitForFixedUpdate();
		}
		// cubeLight.intensity = 5f;
		// cubeLight.range = 10f;
		// main.startSize = 1.5f;
		for (int i = 0; i < 50; i++)
		{
			// main.startSize = i * 0.5f;
			cubeLight.intensity += 0.2f;
			cubeLight.range += 0.3f;
			cubeParticle.transform.localScale = new Vector3(1, 0.3f * i,1);
			// cubeParticle.transform.localRotation = Quaternion.Euler(0,  0, 0);
			other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Acceleration);
			other.GetComponent<Rigidbody>().AddTorque(Vector3.up * 500, ForceMode.Acceleration);
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitUntil(() => other.transform.position.y > 15);
		
		blackMask.gameObject.SetActive(true);
		int length = 50;
		for (int i = 0; i < length; i++)
		{
			blackMask.color = Color.Lerp(Color.clear, Color.black, i * 0.02f);
			yield return new WaitForFixedUpdate();
		}
		for (int i = 0; i < 50; i++)
		{
			gameObject.transform.GetChild(2).localScale = Vector3.Lerp(Vector3.one, new Vector3(0,.5f,0), i * 0.02f);
			yield return new WaitForFixedUpdate();
		}
		gameObject.transform.GetChild(2).gameObject.SetActive(false);
		yield return new WaitForSeconds(1f);
		// blackMask.gameObject.SetActive(false);

		LoadNextLevel();


		// gameObject.SetActive(false);
	}

	private static void LoadNextLevel()
	{
		if (SceneManager.GetActiveScene().name == "Level1")
			SceneManager.LoadScene("Level2");
		else if (SceneManager.GetActiveScene().name == "Level2")
			SceneManager.LoadScene("Level3");
		else if (SceneManager.GetActiveScene().name == "Level3")
			SceneManager.LoadScene("Level4");
		else if (SceneManager.GetActiveScene().name == "Level4")
			SceneManager.LoadScene("Level5");
		else if (SceneManager.GetActiveScene().name == "Level5")
			SceneManager.LoadScene("Level6");
		else if (SceneManager.GetActiveScene().name == "Level6")
			SceneManager.LoadScene("Level7");
		else if (SceneManager.GetActiveScene().name == "Level7")
			SceneManager.LoadScene("Level8");
		else if (SceneManager.GetActiveScene().name == "Level8")
			SceneManager.LoadScene("Level9");
		else
			Application.Quit();
	}

	float curTime;
	Vector3 originalPos;
	public bool isUptime = false;
	private void Update() 
	{
		curTime += Time.deltaTime;
		if(!isUptime)
			gameObject.transform.position -= new Vector3(0,0.001f,0);
		else 
			gameObject.transform.position += new Vector3(0,0.001f,0);

		if(gameObject.transform.position.y <= -0.2)
			isUptime = true;
		else if(gameObject.transform.position.y >= 0f)
			isUptime = false;
		// else 
		// 	transform.position = originalPos;

	}
}
