using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	MoveCountControl moveCountControlSc;
    void Start()
    {
        moveCountControlSc = GameObject.Find("Main").GetComponent<MoveCountControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnTriggerEnter(Collider other) 
	{
		if(!moveCountControlSc.isLevelFailed)
			moveCountControlSc.StartCoroutine("LevelFailed");
	}
}
