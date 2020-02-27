using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModeController : MonoBehaviour {

    private Master masterScript;
    private UIController uiController;
    private SUDSInputController sudsInputController;

    protected int score;
    protected int currentLevel;
    public bool multiChoiceQuestionsActive;

	void Start () {
        //Find the two scripts in the scene and cache them for use later
        sudsInputController = GameObject.Find("SUDSInputObject").GetComponent<SUDSInputController>();
        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();

        updateUI(masterScript.startingLevel);
	}
	
	void Update () {

	}

    //TODO some method for switching between UI and SUDS

    public void submitSUDS(int sudsRating, bool goToNextLevel) {
        if (goToNextLevel) {
            masterScript.completeLevel(currentLevel, score, sudsRating);
            currentLevel++;
            score = 0;
        } else {
            masterScript.changeMode(SystemMode.LevelSelection);
        }
    }

    //Implementation found in SpiderModeController, WaspModeController, etc. 
    //UI controllers don't know which flavour of ModeController they're talking to 
    //Instead just storing the reference as a ModeController (this), so declare methods here
    public abstract void nextLevel();
    public abstract void resetLevel();
    public abstract void previousLevel();
    public abstract void selectMultiChoiceAnswer(); //This one is used for the ABC questions

    protected void updateUI(int level) {
        uiController.updateLevelAndScore(level, masterScript.getHighScoreForLevel(currentLevel));
    }
}
