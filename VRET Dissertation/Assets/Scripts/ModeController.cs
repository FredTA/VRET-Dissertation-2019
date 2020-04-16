using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionRound {
    //3 as each level with questions always has 3 questions, and a summary
    public GameObject[] questions;

    public QuestionRound(int numberOfQuestions) {
        questions = new GameObject[numberOfQuestions];
    }

}

public abstract class ModeController : MonoBehaviour {

    private Master masterScript;
    protected UIController uiController;
    private SUDSInputController sudsInputController;
    private SoundController soundController;

    private GameObject uiObject;
    private GameObject sudsInputObject;
    private GameObject soundControllerObject;

    protected float score = 0;
    private int currentLevel;
    protected bool multiChoiceQuestionsActive;
    protected int questionNum;

    protected const int NUM_OF_QUESTIONS_PER_ROUND = 3;
    protected int[,] correctAnswers;

    public virtual void Awake() {
        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();
        uiObject = GameObject.Find("UICanvas");
        sudsInputObject = GameObject.Find("SUDSCanvas");
        soundControllerObject = GameObject.Find("Controller");

        uiController = uiObject.GetComponent<UIController>();
        sudsInputController = sudsInputObject.GetComponent<SUDSInputController>();
        soundController = soundControllerObject.GetComponent<SoundController>();

        toggleSUDSInput(false);

        currentLevel = masterScript.startingLevel;
        Debug.Log("Master script found - starting level " + currentLevel);
        activateCurrentLevel(); //This needs to be before UI does its thing
    }

    //Each QRO holds it's own array of GameObjects, one for each question     
    protected QuestionRound[] questionRounds;

    //Each mode has its own num of question objects, these need to be found/stored 
    protected void loadMultiChoiceQuestions(int numOfQuestionRounds) {
        questionRounds = new QuestionRound[numOfQuestionRounds];

        //Find all the question round GameObjects in the scene, sort the array
        GameObject[] questionRoundGOs = GameObject.FindGameObjectsWithTag("QuestionRound");
        Array.Sort(questionRoundGOs, compareObjNames); //Sort in ascending order

        //Create each question round, filling it with questions
        for (int questionRndNum = 0; questionRndNum < numOfQuestionRounds; questionRndNum++) {
            questionRounds[questionRndNum] = new QuestionRound(NUM_OF_QUESTIONS_PER_ROUND);

            //Load each question round with its questions
            for (int questionNum = 0; questionNum < NUM_OF_QUESTIONS_PER_ROUND; questionNum++) {
                try {
                    //Determine the name of the question GameObject
                    String questionObjectName = getGameObjectPath(questionRoundGOs[questionRndNum]);
                    questionObjectName += "/Question " + (questionNum + 1);

                    //Now we have the name, find the GameObject and put it in the array
                    GameObject question = transform.Find(questionObjectName).gameObject;
                    questionRounds[questionRndNum].questions[questionNum] = question;
                } catch (Exception e) {
                    Debug.Log("Couldn't find Question " + (questionRndNum + 1) + ":" + 
                        (questionNum + 1) + " - " + e);
                } 
            }
        }
    }

    public int compareObjNames(GameObject x, GameObject y) {
        return x.name.CompareTo(y.name);
    }

    //Takes a GameObject and returns its path within the scene
    private string getGameObjectPath(GameObject obj) {
        string path = "/" + obj.name;
        while (obj.transform.parent != null) {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    public void completeLevel(int sudsRating, bool goToNextLevel) {
        masterScript.updateLevelData(currentLevel, (int)Math.Round(score, 0), sudsRating);
        if (goToNextLevel && currentLevel != 9) {
            currentLevel++;
            activateCurrentLevel();
        } else {
            masterScript.changeMode(SystemMode.LevelSelection);
        }
    }

    //Implementation found in SpiderModeController, WaspModeController, etc. 
    //UI controllers don't know which flavour of ModeController they're talking to 
    //Instead just storing the reference as a ModeController (this), so declare methods here
    public virtual void activateCurrentLevel() {
        soundController.playVoiceover(currentLevel);
    }
    //Each scene may have a different number of question rounds
    //level 6 may be QR 4 on one scene, but QR 2 on another
    public abstract int getQuestionRoundForLevel(int level); 

    //Resets the level, much of this functionality is common across modes 
    //However, we also override this from the derived class (for some extra, mode specific bits), and call it via super
    public virtual void resetLevel() {
        if (currentLevel != 0) {
            score = 0;
        }

        if (multiChoiceQuestionsActive) {
            questionNum = 0;
            int questionRound = getQuestionRoundForLevel(currentLevel);

            //Activate first question
            questionRounds[questionRound].questions[questionNum].SetActive(true);

            //Deactivate others
            for (int i = 1; i < NUM_OF_QUESTIONS_PER_ROUND; i++) {
                questionRounds[questionRound].questions[i].SetActive(false);
            }
            uiController.deactivateQuestionSummary();
        }

        soundController.playVoiceover(currentLevel);
    }

    public virtual void selectMultiChoiceAnswer(int selection) {
        int questionRoundNum = getQuestionRoundForLevel(currentLevel);

        Debug.Log("Answered Q " + questionRoundNum + ":" + questionNum + " with " + selection);

        //If the answer was correct, increase the score
        if (selection == correctAnswers[questionRoundNum, questionNum]) {
            score += 100f / (float)NUM_OF_QUESTIONS_PER_ROUND;
        }

        //Deactivate the question just answered
        questionRounds[questionRoundNum].questions[questionNum].SetActive(false);

        //If we're not at the last question
        if (questionNum < NUM_OF_QUESTIONS_PER_ROUND - 1) {
            //Activate the next question
            questionNum++;
            questionRounds[questionRoundNum].questions[questionNum].SetActive(true);
        }
        else {
            //Activate the question summary
            uiController.setQuestionSummary(score, NUM_OF_QUESTIONS_PER_ROUND);
        }
    }

    protected void activateQuestionForLevel(int level) {
        int questionRound = getQuestionRoundForLevel(level);
        questionRounds[questionRound].questions[0].SetActive(true);
        multiChoiceQuestionsActive = true;
    }

    protected void deactivateQuestionForLevel(int level) {
        int questionRound = getQuestionRoundForLevel(level);

        //Can't just deactivate the parent object because this prevents later activation of children
        questionRounds[questionRound].questions[0].SetActive(false);
        questionRounds[questionRound].questions[1].SetActive(false);
        questionRounds[questionRound].questions[2].SetActive(false);
        uiController.deactivateQuestionSummary();
    }

    public void toggleSUDSInput(bool sudsInputOn) {
        uiObject.SetActive(!sudsInputOn);
        sudsInputObject.SetActive(sudsInputOn);
    }

    public int getCurrentLevel() {
        return currentLevel;
    }

    public int getCurrentScore() {
        return (int)Math.Round(score, 0);
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
