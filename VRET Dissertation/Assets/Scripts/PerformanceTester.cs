using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceTester : MonoBehaviour {

    double numberOfFrames = 0;
    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        numberOfFrames++;
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            Debug.Log("AVERAGE FRAMERATE " + CalculateFPS());
        }
    }

    private float CalculateFPS() {
        float elapsedTime = Time.time - startTime;
        return (float)(numberOfFrames / elapsedTime);
    }
}
