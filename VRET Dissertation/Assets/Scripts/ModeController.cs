using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiDimensionalGameObject {
    //3 as each level with questions always has 3 questions, and a summary
    //element #0 is question 1 etc, element #3 is the summary
    public GameObject[] gameObjects = new GameObject[4];
}

public abstract class ModeController : MonoBehaviour {

    private Master masterScript;
    private UIController uiController;
    private SUDSInputController sudsInputController;

    private GameObject uiObject;
    private GameObject sudsInputObject;

    protected float score = 0;
    private int currentLevel;
    protected bool multiChoiceQuestionsActive;
    protected int questionNumber;

	public virtual void Awake() {
        uiObject = GameObject.Find("UICanvas");
        sudsInputObject = GameObject.Find("SUDSCanvas");

        uiController = uiObject.GetComponent<UIController>();
        sudsInputController = sudsInputObject.GetComponent<SUDSInputController>();

        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();

        currentLevel = masterScript.startingLevel;
        Debug.Log("Master script found - starting level " + currentLevel);

        toggleSUDSInput(false);
        activateCurrentLevel();
	}

    public void submitSUDS(int sudsRating, bool goToNextLevel) {
        if (goToNextLevel) {
            masterScript.completeLevel(currentLevel, (int)score, sudsRating);
            currentLevel++;
            activateCurrentLevel();
        } else {
            masterScript.changeMode(SystemMode.LevelSelection);
        }
    }

    //Implementation found in SpiderModeController, WaspModeController, etc. 
    //UI controllers don't know which flavour of ModeController they're talking to 
    //Instead just storing the reference as a ModeController (this), so declare methods here
    public abstract void activateCurrentLevel();
    public abstract void resetLevel();
    public abstract void selectMultiChoiceAnswer(int selection); //This one is used for the ABC questions

    public void toggleSUDSInput(bool sudsInputOn) {
        uiObject.SetActive(!sudsInputOn);
        sudsInputObject.SetActive(sudsInputOn);
    }

    public int getCurrentLevel() {
        return currentLevel;
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
}
