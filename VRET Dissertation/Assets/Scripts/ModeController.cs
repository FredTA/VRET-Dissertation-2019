using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModeController : MonoBehaviour {

    private Master masterScript;
    private UIController uiController;
    private SUDSInputController sudsInputController;

    private GameObject uiObject;
    private GameObject sudsInputObject;

    protected int score = 0;
    private int currentLevel;
    private int previousLevel;
    protected bool multiChoiceQuestionsActive;

	public virtual void Awake() {
        Debug.Log("Hello from base.Awake");
        //Find the two scripts in the scene and cache them for use later
        uiObject = GameObject.Find("UICanvas");
        sudsInputObject = GameObject.Find("SUDSCanvas");

        uiController = uiObject.GetComponent<UIController>();
        sudsInputController = sudsInputObject.GetComponent<SUDSInputController>();

        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();
        Debug.Log("Master script found on " + masterScript.gameObject.name);

        currentLevel = masterScript.startingLevel;
        previousLevel = -1;

        toggleSUDSInput(false);
        activateCurrentLevel();

        //updateUI(masterScript.startingLevel);
	}
	
	void Update () {

	}

    //TODO some method for switching between UI and SUDS

    public void submitSUDS(int sudsRating, bool goToNextLevel) {
        if (goToNextLevel) {
            masterScript.completeLevel(currentLevel, score, sudsRating);
            previousLevel = currentLevel;
            currentLevel++;

            activateCurrentLevel();
            deactivatePreviousLevel();
        } else {
            masterScript.changeMode(SystemMode.LevelSelection);
        }
    }

    //Implementation found in SpiderModeController, WaspModeController, etc. 
    //UI controllers don't know which flavour of ModeController they're talking to 
    //Instead just storing the reference as a ModeController (this), so declare methods here
    public abstract void activateCurrentLevel();
    public abstract void deactivatePreviousLevel();
    public abstract void resetLevel();
    public abstract void selectMultiChoiceAnswer(int selection); //This one is used for the ABC questions

    public void toggleSUDSInput(bool sudsInputOn) {
        uiObject.SetActive(!sudsInputOn);
        sudsInputObject.SetActive(sudsInputOn);
    }

    public int getCurrentLevel() {
        return currentLevel;
    }

    public int getPreviousLevel() {
        return previousLevel;
    }

    public int getHighScoreForCurrentLevel() {
        return masterScript.getHighScoreForLevel(currentLevel);
    }

    public bool areMultiChoiceQuestionsActive() {
        return multiChoiceQuestionsActive;
    }

    public void returnToMenu() {
        masterScript.changeMode(SystemMode.LevelSelection);
    }

    //protected void updateUI(int level) {
    //    uiController.updateLevelAndScore(level, masterScript.getHighScoreForLevel(currentLevel));
    //}
}
