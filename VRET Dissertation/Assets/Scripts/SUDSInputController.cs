using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SUDSInputController : MonoBehaviour {

    private Text[] inputNumbers = new Text[10];
    private Text nextLevelText;
    private Text backToMenuText;

    private int sudsSelection;
    private int menuSelection;

    private ModeController modeController;


	// Use this for initialization
	void Awake () {
        modeController = GameObject.FindGameObjectWithTag("ModeController").GetComponent<ModeController>();

        //We could have these Text objects be public and assign them all through the editor...
        //But this script will appear in multiple scenes, do easier to do it programmatically 
        nextLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        backToMenuText = GameObject.Find("MenuText").GetComponent<Text>();

        GameObject[] sudsInputObjects = GameObject.FindGameObjectsWithTag("SUDSInputNumbers");
        Array.Sort(sudsInputObjects, modeController.compareObjNames); //So that 1 appears first and 10 last

        for (int i = 0; i < 10; i++) {
            inputNumbers[i] = sudsInputObjects[i].GetComponent<Text>();
        }
    }

    private void OnEnable() {
        sudsSelection = 4;
        menuSelection = 0;

        for (int i = 0; i < 10; i++) {
            inputNumbers[i].color = Color.white;
        }
        nextLevelText.color = Color.white;
        backToMenuText.color = Color.white;

        inputNumbers[sudsSelection].color = Color.yellow;
    }

    // Update is called once per frame
    void Update () {
        OVRInput.Update(); // Call before checking the input from Touch Controllers

        if ((OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)) && menuSelection > 0) {
            updateMenuSelection(menuSelection - 1);
        }
        else if ((OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown)) && menuSelection < 2) {
            updateMenuSelection(menuSelection + 1);
        }
        else if ((OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft)) && sudsSelection > 0) {
            updateSUDSSelection(sudsSelection - 1);
        }
        else if ((OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight)) && sudsSelection < 9) {
            updateSUDSSelection(sudsSelection + 1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.RawButton.X) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.5f ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f) {
            submitSUDS();
        }
        else {
            //Debug.Log("nada");
        }

    }

    private void submitSUDS() {
        if (menuSelection == 1) {
            //Must add 1 to the SUDS selection, as array starts at 0, but suds starts at 1
            modeController.completeLevel(sudsSelection + 1, true); //submit scores and go to next level
        }
        else if (menuSelection == 2) {
            modeController.completeLevel(sudsSelection + 1, false); //Submit scores and go back to menu
        }
    }

    private void updateSUDSSelection(int newSelection) {
        inputNumbers[sudsSelection].color = Color.white;
        sudsSelection = newSelection;
        inputNumbers[sudsSelection].color = Color.yellow;
        Debug.Log("SELECTION " + sudsSelection);
    }

    private void updateMenuSelection(int newSelection) {
        //Change colour of text we are moving away from 
        switch (menuSelection) {
            case 0:
                inputNumbers[sudsSelection].color = Color.green;
                break;
            case 1:
                nextLevelText.color = Color.white;
                break;
            case 2:
                backToMenuText.color = Color.white;
                break;
        }

        menuSelection = newSelection;

        //Change colour of text we are moving on to 
        switch (menuSelection) {
            case 0:
                inputNumbers[sudsSelection].color = Color.yellow;
                break;
            case 1:
                nextLevelText.color = Color.yellow;
                break;
            case 2:
                backToMenuText.color = Color.yellow;
                break;
        }
    }
}
