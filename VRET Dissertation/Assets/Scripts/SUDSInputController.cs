﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SUDSInputController : MonoBehaviour {

    private int sudsValue = -1;
    private Text[] inputNumbers = new Text[10];
    private Text nextLevelText;
    private Text backToMenuText;

    private int sudsSelection = 4;
    private int menuSelection = 0;

    private ModeController modeController;


	// Use this for initialization
	void Start () {
        modeController = GameObject.FindGameObjectWithTag("ModeController").GetComponent<ModeController>();

        //We could have these Text objects be public and assign them all through the editor...
        //But this script will appear in multiple scenes, do easier to do it programmatically 
        nextLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        backToMenuText = GameObject.Find("MenuText").GetComponent<Text>();

        GameObject[] sudsInputObjects = GameObject.FindGameObjectsWithTag("SUDSInputNumbers");
        Array.Sort(sudsInputObjects, CompareObNames); //So that 1 appears first and 10 last
        
        for (int i = 0; i < 10; i++) {
            inputNumbers[i] = sudsInputObjects[i].GetComponent<Text>();
        }
    }

    int CompareObNames(GameObject x, GameObject y) {
        return x.name.CompareTo(y.name);
    }

    // Update is called once per frame
    void Update () {
        OVRInput.Update(); // Call before checking the input from Touch Controllers

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) && menuSelection > 0) {
            updateMenuSelection(menuSelection + 1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) && menuSelection < 2) {
            updateMenuSelection(menuSelection + 1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) && sudsSelection != 0) {
            updateSUDSSelection(sudsSelection + 1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) && sudsSelection != 9) {
            updateSUDSSelection(sudsSelection - 1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.RawButton.X) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.5f ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f) {

            if (menuSelection == 1) {
                modeController.submitSUDS(sudsSelection, true); //submit scores and go to next level
            } else if (menuSelection == 2) { 
                modeController.submitSUDS(sudsSelection, false); //Submit scores and go back to menu
            }
        }
        else {
            //Debug.Log("nada");
        }

    }

    private void updateSUDSSelection(int newSelection) {
        inputNumbers[sudsSelection].color = Color.white;
        sudsSelection = newSelection;
        inputNumbers[sudsSelection].color = Color.yellow;
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

    public void showSUDSPrompt() {

    }

    //This method will be repeatedly called by the mode controller script 
    //If it's waiting for a suds value. -1 means no value is yet set
    public int getSUDSValue() {
        int value = sudsValue; 

        //If it's not -1, we'll be returning it to the mode cont script 
        //After we've returned it, we don't need to store it here anymore
        if (sudsValue != -1) {
            sudsValue = -1;
        }
        return value;
    }
}
