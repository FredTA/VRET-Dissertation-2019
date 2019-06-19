using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour {

    public float closedVertical;
    public float shutterSpeed;
    public float minimumSunIntensity;
    public float maximumLampIntensity;
    public float maximumLampRange;

    public Light lamp;
    public Light sun;

    private bool windowClosed;
    private bool windowNotInPosition;
    private float openVertical;
    private bool keyCoolingDown = false;
    private float KEY_COOLDOWN_TIME = 0.3f;
    private float maximumSunIntensity;
    private float minimumLampIntensity;
    private float minimumLampRange;

    //Calculated from shutter speed and transform offsets
    private float sunIntensityChangeSpeed;
    private float lampIntensityChangeSpeed;
    private float lampRangeChangeSpeed;

	// Use this for initialization
	void Start () {
        openVertical = transform.position.y;
        maximumSunIntensity = sun.intensity;
        minimumLampIntensity = lamp.intensity;
        minimumLampRange = lamp.range;
        CalculateLightChangeSpeeds();
	}

    //Calculate the speed at which to change the intensity and range of the lights in the scene
    void CalculateLightChangeSpeeds() {
        float shutterTravelDistance = openVertical - closedVertical;
        float numberOfSteps = shutterTravelDistance / shutterSpeed;

        float sunIntensityChangeAmount = maximumSunIntensity - minimumSunIntensity;
        sunIntensityChangeSpeed = sunIntensityChangeAmount / numberOfSteps;

        float lampIntensityChangeAmount = maximumLampIntensity - minimumLampIntensity;
        lampIntensityChangeSpeed = lampIntensityChangeAmount / numberOfSteps;

        float lampRangeChangeAmount = maximumLampRange - minimumLampRange;
        lampRangeChangeSpeed = lampRangeChangeAmount / numberOfSteps;
    }

    // Update is called once per frame
    void Update() {

        //Make sure the key cooldown has timed out. We are working in Update, so one press can last many cycles, and trigger the bool many times
        if (Input.GetKey(KeyCode.W) && !keyCoolingDown) {
            Debug.Log("Toggling window!");
            keyCoolingDown = true;
            Invoke("ResetCooldown", KEY_COOLDOWN_TIME);
            ToggleWindow();
        }

        if (windowNotInPosition) {
            if (windowClosed) { 
                if (transform.position.y > closedVertical) {
                    CloseWindowStep();
                }
                else {
                    windowNotInPosition = true;
                }
            }
            else {
                if (transform.position.y < openVertical) {
                    OpenWindowStep();
                }
                else {
                    windowNotInPosition = true;
                }
            }
        }

    }

    private void CloseWindowStep() {
        transform.Translate(-shutterSpeed * Time.deltaTime, 0, 0);
        sun.intensity -= sunIntensityChangeSpeed * Time.deltaTime;
        lamp.intensity += lampIntensityChangeSpeed * Time.deltaTime;
        lamp.range += lampRangeChangeSpeed * Time.deltaTime;
    }

    private void OpenWindowStep() {
        transform.Translate(shutterSpeed * Time.deltaTime, 0, 0);
        sun.intensity += sunIntensityChangeSpeed * Time.deltaTime;
        lamp.intensity -= lampIntensityChangeSpeed * Time.deltaTime;
        lamp.range -= lampRangeChangeSpeed * Time.deltaTime;
    }

    public void ToggleWindow() {
        windowNotInPosition = true;
        windowClosed = !windowClosed;
    }

    public void OpenWindow() {
        windowClosed = false; 
    }

    public void CloseWindow() {
        windowClosed = true;
    }

    private void ResetCooldown() {
        keyCoolingDown = false;
    }
}
