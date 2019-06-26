using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour {

    public float closedVertical;
    public float shutterSpeed;
    public float maximumLampPropIntensity;
    public float maximumLampPropRange;

    public Light lampProp;
    public Light sun;
    public Light ceilingSun;
    public Light lampInside;
    public Light lampInsideCeiling;

    public AudioSource windowClickSound;

    private bool windowClosed;
    private bool windowNotInPosition;
    private float openVertical;
    private bool keyCoolingDown = false;
    private float KEY_COOLDOWN_TIME = 0.3f;
    private float maximumSunIntensity;
    private float maximumCeilingSunIntensity;
    private float maximumLampInsideIntensity;
    private float maximumLampInsideCeilingIntensity;
    private float minimumLampIntensity;
    private float minimumLampRange;

    //Calculated from shutter speed and transform offsets
    private float sunIntensityChangeSpeed;
    private float ceilingSunIntensityChangeSpeed;
    private float lampPropIntensityChangeSpeed;
    private float lampPropRangeChangeSpeed;
    private float lampInsideIntensityChangeSpeed;
    private float lampInsideCeilingIntensityChangeSpeed;

    // Use this for initialization
    void Start () {
        openVertical = transform.position.y;

        //cache maximum intensities of various light sources
        maximumSunIntensity = sun.intensity;
        maximumCeilingSunIntensity = ceilingSun.intensity;
        maximumLampInsideIntensity = lampInside.intensity;
        maximumLampInsideCeilingIntensity = lampInsideCeiling.intensity;
        minimumLampIntensity = lampProp.intensity;
        minimumLampRange = lampProp.range;

        //The two inside lamp lights should start inactive
        lampInside.intensity = 0; ;
        lampInsideCeiling.intensity = 0;

        //The two lamps should be enabled as they are disabled in the editor (for ease of development)
        lampInside.enabled = true;
        lampInsideCeiling.enabled = true;

        //Calculate how fast these intensities should change
        CalculateLightChangeSpeeds();
	}

    //Calculate the speed at which to change the intensity and range of the lights in the scene
    void CalculateLightChangeSpeeds() {
        float shutterTravelDistance = openVertical - closedVertical;
        float numberOfSteps = shutterTravelDistance / shutterSpeed;

        sunIntensityChangeSpeed = maximumSunIntensity / numberOfSteps;
        ceilingSunIntensityChangeSpeed = maximumCeilingSunIntensity / numberOfSteps;
        lampInsideIntensityChangeSpeed = maximumLampInsideIntensity / numberOfSteps;
        lampInsideCeilingIntensityChangeSpeed = maximumLampInsideCeilingIntensity / numberOfSteps;

        float lampIntensityChangeAmount = maximumLampPropIntensity - minimumLampIntensity;
        lampPropIntensityChangeSpeed = lampIntensityChangeAmount / numberOfSteps;

        float lampRangeChangeAmount = maximumLampPropRange - minimumLampRange;
        lampPropRangeChangeSpeed = lampRangeChangeAmount / numberOfSteps;
    }

    // Update is called once per frame
    void Update() {

        //Make sure the key cooldown has timed out. We are working in Update, so one press can last many cycles, and trigger the bool many times
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Home) && !keyCoolingDown) {
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
                    windowClickSound.Play();
                    windowNotInPosition = false; 
                }
            }
            else {
                if (transform.position.y < openVertical) {
                    OpenWindowStep();
                }
                else {
                    windowClickSound.Play();
                    windowNotInPosition = false; 
                }
            }
        }

    }

    public bool isWindowClosed() {
        return !windowNotInPosition && windowClosed;
    }

    public bool isWindowOpen() {
        return !windowNotInPosition && !windowClosed;
    }

    private void CloseWindowStep() {
        transform.Translate(-shutterSpeed * Time.deltaTime, 0, 0);
        sun.intensity -= sunIntensityChangeSpeed * Time.deltaTime;
        ceilingSun.intensity -= ceilingSunIntensityChangeSpeed * Time.deltaTime;
        lampProp.intensity += lampPropIntensityChangeSpeed * Time.deltaTime;
        lampProp.range += lampPropRangeChangeSpeed * Time.deltaTime;
        lampInside.intensity += lampInsideIntensityChangeSpeed * Time.deltaTime;
        lampInsideCeiling.intensity += lampInsideCeilingIntensityChangeSpeed * Time.deltaTime;
    }

    private void OpenWindowStep() {
        transform.Translate(shutterSpeed * Time.deltaTime, 0, 0);
        sun.intensity += sunIntensityChangeSpeed * Time.deltaTime;
        ceilingSun.intensity += ceilingSunIntensityChangeSpeed * Time.deltaTime;
        lampProp.intensity -= lampPropIntensityChangeSpeed * Time.deltaTime;
        lampProp.range -= lampPropRangeChangeSpeed * Time.deltaTime;
        lampInside.intensity -= lampInsideIntensityChangeSpeed * Time.deltaTime;
        lampInsideCeiling.intensity -= lampInsideCeilingIntensityChangeSpeed * Time.deltaTime;
    }

    public void ToggleWindow() {
        windowNotInPosition = true;
        windowClosed = !windowClosed;
    }

    public void OpenWindow() {
        windowClosed = false;
        windowNotInPosition = true;
    }

    public void CloseWindow() {
        windowClosed = true;
        windowNotInPosition = true;
    }

    private void ResetCooldown() {
        keyCoolingDown = false;
    }
}
