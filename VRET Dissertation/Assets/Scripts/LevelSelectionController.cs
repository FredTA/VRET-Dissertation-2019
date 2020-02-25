using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Why aren't we just using a vector2? 
//So that we can expose the 2d array in the editor, 
//Meaning we don't have to assign each text object programmatically
[System.Serializable]
public class MultiDimensionalText {
    public Text[] textArray = new Text[10];
}

public class LevelSelectionController : MonoBehaviour {
    private Master masterScript;

    public GameObject[] modePanels = new GameObject[5];
    private Vector2Int levelSelection;
    public float selectedPanelOffsetZ;

    private int[] unlockedLevels = new int[5];

    public MultiDimensionalText[] levelTexts = new MultiDimensionalText[5];


    // Use this for initialization
    void Start () {
        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();
        unlockedLevels = masterScript.getUnlockedLevels();

        //Set the initial selection to the first spider level
        levelSelection = new Vector2Int(2, 0);
        levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.yellow;
        movePanel(levelSelection.x, true);

        //Grey out the text for levels that aren't unlocked
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 10; y++) {
                //string debug = "LVL " + x + ", " + y + ": ";
                if (y > unlockedLevels[x]) {
                    levelTexts[x].textArray[y].color = Color.grey;
                    levelTexts[x].textArray[y].text = "Level " + y + " - Locked"; 
                //    debug += "Locked";
                } else {
                //    debug += "Unlocked";
                }
                //Debug.Log(debug);
            }
            Debug.Log("Mode " + x + " lvl is " + unlockedLevels[x]);
        }
    }

    // Update is called once per frame
    void Update() {
        OVRInput.Update(); // Call before checking the input from Touch Controllers

        if (OVRInput.Get(OVRInput.Button.DpadLeft)) {
            Debug.Log("left button pressed");
        }
        if (OVRInput.Get(OVRInput.Button.DpadRight)) {
            Debug.Log("right button pressed");
        }
        if (OVRInput.Get(OVRInput.Button.One)) {
            Debug.Log("round button pressed");
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp)) {
            Debug.Log("Thumb up");
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown)) {
            Debug.Log("Thumb down");
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft)) {
            Debug.Log("Thumb left");
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight)) {
            Debug.Log("Thumb right");
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)) {
            Debug.Log("Thumb 2 up");
        }
        else {
            //Debug.Log("nada");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown)) {
            //If we're not already on the last level down, and if the current lvl is less than the max lvl unlocked
            Debug.Log("selection is " + levelSelection.y + " # is " + unlockedLevels[levelSelection.x]);
            if (levelSelection.y < 9 && levelSelection.y < unlockedLevels[levelSelection.x]) {
                Debug.Log("Doing ITTT");
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.black;
                levelSelection.y++;
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.yellow;
            }
            else {
                //Reject input sound
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp)) {
            if (levelSelection.y > 0) {
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.black;
                levelSelection.y--;
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.yellow;
            }
            else {
                //Reject input sound
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight)) {
            if (levelSelection.x < 4) {
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.black;
                movePanel(levelSelection.x, false);
                levelSelection.x++;
                levelSelection.y = 0;
                movePanel(levelSelection.x, true);
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.yellow;
            }
            else {
                //Reject input sound
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft)) {
            if (levelSelection.x > 0) {
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.black;
                movePanel(levelSelection.x, false);
                levelSelection.x--;
                levelSelection.y = 0;
                movePanel(levelSelection.x, true);
                levelTexts[levelSelection.x].textArray[levelSelection.y].color = Color.yellow;
            }
            else {
                //Reject input sound
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Return) || OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.RawButton.X) ||
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.5f || 
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f) {
            selectLevel();
            Debug.Log("CHOOSING SELECTION " + levelSelection.x + ": " + levelSelection.y);
            masterScript.changeMode((SystemMode)levelSelection.x);
            //Load the appropriate scene
        }
    }

    //Moves the mode panel of given number forward if true, backwards if false
    //Used for "highlighting" which panel we're on
    private void movePanel(int panelToMove, bool moveForward) {
        Vector3 panelOffset;
        if (moveForward) {
            panelOffset = new Vector3(0, 0, -selectedPanelOffsetZ);
        } else {
            panelOffset = new Vector3(0, 0, selectedPanelOffsetZ);
        }

        modePanels[panelToMove].transform.Translate(panelOffset);
    }

    private void selectLevel() {
        switch (levelSelection.x) {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
        }
    }
}
