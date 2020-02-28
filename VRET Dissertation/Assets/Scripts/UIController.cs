using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private ModeController modeController;

    private const float UI_DELAY_TIME = 0.5f;
    private float timeOfLastDelay;

    //Level and high score should be read in once per level
    //method in this class called from... mode controller?
    private Text levelText;
    private Text currentScoreText;
    private Text highScoreText;

    private GameObject multichoiceButtons;
    private Text optionAText;
    private Text optionBText;
    private Text optionCText;
    private GameObject questionSummary;

    private Text menuText;
    private Text resetLevelText;
    private Text completeLevelText;

    private Vector2Int selection = new Vector2Int(0, 0);

    //Using Awake instead of Start due to MonoBehaviour execution order, need to have the modeController 
    //Reference ready to go before OnEnable is called
    void Awake () {
        modeController = GameObject.FindGameObjectWithTag("ModeController").GetComponent<ModeController>();

        //We could have these Text objects be public and assign them all through the editor...
        //But this script will appear in multiple scenes, do easier to do it programmatically 
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        currentScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();

        multichoiceButtons = GameObject.Find("Multichoice");
        optionAText = GameObject.Find("OptionAText").GetComponent<Text>();
        optionBText = GameObject.Find("OptionBText").GetComponent<Text>();
        optionCText = GameObject.Find("OptionCText").GetComponent<Text>();
        questionSummary = GameObject.Find("QuestionSummary");

        menuText = GameObject.Find("MenuText").GetComponent<Text>();
        resetLevelText = GameObject.Find("ResetLevelText").GetComponent<Text>();
        completeLevelText = GameObject.Find("CompleteLevelText").GetComponent<Text>();
    }

    //Called by MonoBehaviour when the GameObject this script is attached to is enabled
    private void OnEnable() {
        updateUISelection(new Vector2Int(0, 0));
        updateLevelAndHighScore();
        menuText.color = Color.yellow;
        multichoiceButtons.SetActive(modeController.areMultiChoiceQuestionsActive());
        questionSummary.SetActive(false);

        //Set a delay so that the last button press isn't read again straight away
        timeOfLastDelay = Time.time;
    }

    //TODO fix selection bugs - we're tied into the other scritps ok, but input still isn't right
    // Update is called once per frame
    void Update () {
        currentScoreText.text = "Current Score: " + modeController.getCurrentScore();

        if (Time.time - timeOfLastDelay > UI_DELAY_TIME) {
            OVRInput.Update(); // Call before checking the input from Touch Controllers

            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) && selection.y == 0) {
            
                if (modeController.areMultiChoiceQuestionsActive()) {
                    //Move up
                    updateUISelection(new Vector2Int(selection.x, 1));
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) ||
                OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) && selection.y == 1) {
                updateUISelection(new Vector2Int(selection.x, 0));
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) ||
                OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) && selection.x != 0) {
                updateUISelection(new Vector2Int(selection.x - 1, selection.y));
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) ||
                OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) && selection.x != 2) {
                updateUISelection(new Vector2Int(selection.x + 1, selection.y));
            }
            else if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.RawButton.X) ||
                OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.5f ||
                OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f) {

                //Debug.Log("Triggers: " + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch)
                //    + ", " + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch));
                submitUISelection();
            }
            else {
                //Debug.Log("nada");
            }
        }
    }

    private void submitUISelection() {
        //Debug.Log("Submitting UI Selection " + selection.x + ", " + selection.y);
        if (selection.y == 1) {
            modeController.selectMultiChoiceAnswer(selection.x);
        }
        else {
            switch (selection.x) {
                case 0:
                    modeController.returnToMenu();
                    break;
                case 1:
                    modeController.resetLevel();
                    break;
                case 2:
                    //Deactivates this script and enables the SUDS controller
                    modeController.toggleSUDSInput(true);
                    break;
            }
        }
        timeOfLastDelay = Time.time;
    }

    private void updateUISelection(Vector2Int newSelection) {
        //Set all text to white (saves us a lot of ifs)
        optionAText.color = optionBText.color = optionCText.color = menuText.color =
                        resetLevelText.color = completeLevelText.color = Color.white;

        selection = newSelection;

        //Set the new selection to yellow
        if (newSelection.y == 1) {
            switch (newSelection.x) {
                case 0:
                    optionAText.color = Color.yellow;
                    break;
                case 1:
                    optionBText.color = Color.yellow;
                    break;
                case 2:
                    optionCText.color = Color.yellow;
                    break;
            }
        }
        else {
            switch (newSelection.x) {
                case 0:
                    menuText.color = Color.yellow;
                    break;
                case 1:
                    resetLevelText.color = Color.yellow;
                    break;
                case 2:
                    completeLevelText.color = Color.yellow;
                    break;
            }
        }
    }

    public void updateLevelAndHighScore() {
        int level = modeController.getCurrentLevel();
        int score = modeController.getHighScoreForCurrentLevel();

        levelText.text = "Level: " + level;
        if (score != -1) {
            highScoreText.text = "High Score: " + score;
        } else {
            highScoreText.text = "High Score: N/A";
        }
    }

    public void setQuestionSummary(float score, int numberOfQuestions) {
        string summary = "You answered " + (score / (100 / numberOfQuestions)) + 
                         "/" + numberOfQuestions + " correctly";

        questionSummary.GetComponent<Text>().text = summary;
        questionSummary.SetActive(true);
    }

    public void deactivateQuestionSummary() {
        //If it's null it's not active right now anyway
        if (null != questionSummary) {
            questionSummary.SetActive(false);
        }
    }

}
