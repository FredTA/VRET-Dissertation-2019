using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionRoundObject {
    //3 as each level with questions always has 3 questions, and a summary
    public GameObject[] questions;

    public QuestionRoundObject(int numberOfQuestions) {
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
    protected int questionNumber;

    protected const int NUMBER_OF_QUESTIONS_PER_ROUND = 3;
    protected QuestionRoundObject[] multiChoiceQuestions;
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

    //Each mode has multichoice question objects to find, The  only difference is the number of rounds
    protected void loadMultiChoiceQuestions(int numberOfQuestionRounds) {
        multiChoiceQuestions = new QuestionRoundObject[numberOfQuestionRounds]; 

        for (int questionRound = 0; questionRound < numberOfQuestionRounds; questionRound++) {
            multiChoiceQuestions[questionRound] = new QuestionRoundObject(NUMBER_OF_QUESTIONS_PER_ROUND);

            //Find all the question round objects and sort the array
            GameObject[] questionRounds = GameObject.FindGameObjectsWithTag("QuestionRound");
            Array.Sort(questionRounds, compareObjNames); //So that 1 appears first and 10 last

            for (int questionNumber = 0; questionNumber < NUMBER_OF_QUESTIONS_PER_ROUND; questionNumber++) {

                //We might not have finished building the scene yet, try block so we can test without having added all GameObjects
                try {
                    String questionObjectName = getGameObjectPath(questionRounds[questionRound]) + "/Question " + (questionNumber + 1);
                    GameObject question = transform.Find(questionObjectName).gameObject;
                    multiChoiceQuestions[questionRound].questions[questionNumber] = question;
                } catch (Exception e) {
                    Debug.Log("Couldn't find Question " + (questionRound + 1) + ":" + (questionNumber + 1) + " - " + e);
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
            questionNumber = 0;
            int questionRound = getQuestionRoundForLevel(currentLevel);

            //Activate first question
            multiChoiceQuestions[questionRound].questions[questionNumber].SetActive(true);

            //Deactivate others
            for (int i = 1; i < NUMBER_OF_QUESTIONS_PER_ROUND; i++) {
                multiChoiceQuestions[questionRound].questions[i].SetActive(false);
            }
            uiController.deactivateQuestionSummary();
        }

        soundController.playVoiceover(currentLevel);
    }

    public virtual void selectMultiChoiceAnswer(int selection) {
        int questionRound = getQuestionRoundForLevel(currentLevel);

        Debug.Log("Answered Q " + questionRound + ":" + questionNumber + " with " + selection);

        if (selection == correctAnswers[questionRound, questionNumber]) {
            score += 100f / (float)NUMBER_OF_QUESTIONS_PER_ROUND;
        }
        else {
            //TODO maybe play a sound?
        }

        multiChoiceQuestions[questionRound].questions[questionNumber].SetActive(false);

        //If we're not at the last question
        if (questionNumber < NUMBER_OF_QUESTIONS_PER_ROUND - 1) {
            questionNumber++;
            multiChoiceQuestions[questionRound].questions[questionNumber].SetActive(true);
        }
        else {
            uiController.setQuestionSummary(score, NUMBER_OF_QUESTIONS_PER_ROUND);
        }
    }

    protected void activateQuestionForLevel(int level) {
        int questionRound = getQuestionRoundForLevel(level);
        multiChoiceQuestions[questionRound].questions[0].SetActive(true);
        multiChoiceQuestionsActive = true;
    }

    protected void deactivateQuestionForLevel(int level) {
        int questionRound = getQuestionRoundForLevel(level);

        //Can't just deactivate the parent object because this prevents later activation of children
        multiChoiceQuestions[questionRound].questions[0].SetActive(false);
        multiChoiceQuestions[questionRound].questions[1].SetActive(false);
        multiChoiceQuestions[questionRound].questions[2].SetActive(false);
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
