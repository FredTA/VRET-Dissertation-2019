using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderModeController : ModeController {

    public GameObject laptop;
    public GameObject cartoonImage;
    public GameObject realisticImage;
    public GameObject spiderBox;
    public GameObject spider;
    private SpiderController spiderController;

    public GameObject level0Instructions;

    //4 as we have 4 levels with multi choice answers
    public MultiDimensionalGameObject[] multiChoiceQuestions = new MultiDimensionalGameObject[4];

    private static int[,] correctAnswers = new int[,] { { 0, 0, 0 }, //lvl 1
                                                        { 0, 0, 0 }, //lvl 2
                                                        { 0, 0, 0 }, //lvl 5
                                                        { 0, 0, 0 }, }; //lvl 6

	// Use this for initialization
	public override void Awake () {
        base.Awake(); 
        spiderController = spider.GetComponent<SpiderController>();
	}
	
	// Update is called once per frame
    //Handle scoring depending on what level we're on and what the user is doing
	void Update () {
        switch (getCurrentLevel()) {
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
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
        }
    }

    //We need to store previous level so we know which elements to deactivate 
    //Then activate the current level elements 
    //If prev lvl == -1 don't disable anything 

    //TODO add audio clips
    public override void activateCurrentLevel() {
        Debug.Log("Activating level " + getCurrentLevel());
        score = 0;

        switch (getCurrentLevel()) {
            case 0:
                score = 100;
                level0Instructions.SetActive(true);
                multiChoiceQuestionsActive = false;
                break;
            case 1:
                level0Instructions.SetActive(false);
                multiChoiceQuestions[0].gameObjects[0].SetActive(true);
                laptop.SetActive(true);
                multiChoiceQuestionsActive = true;
                break;
            case 2:
                multiChoiceQuestions[0].gameObjects[3].SetActive(false);
                multiChoiceQuestions[1].gameObjects[0].SetActive(true);
                cartoonImage.SetActive(false);
                realisticImage.SetActive(true);
                multiChoiceQuestionsActive = true;
                break;
            case 3:
                multiChoiceQuestions[1].gameObjects[3].SetActive(false);
                laptop.SetActive(false);
                spiderBox.SetActive(true);
                break;
            case 4:
                spiderBox.SetActive(false);
                spider.SetActive(true);
                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
        }

        //Do this last, so that the level is ready when the UI wakes up
        toggleSUDSInput(false);
    }

    public override void resetLevel() {

    }

    public override void selectMultiChoiceAnswer(int selection) {
        int level = getCurrentLevel();
        switch (level) {
            case 1:
                answerMultiChoiceQuestion(0, selection);
                break;
            case 2:
                answerMultiChoiceQuestion(1, selection);
                break;
            case 5:
                answerMultiChoiceQuestion(2, selection);
                break;
            case 6:
                answerMultiChoiceQuestion(3, selection);
                break;
            default:
                Debug.Log("Error with question numbers, SpiderModeController.selectMultiChoiceAnswer");
                break;
        }
    }

    //Take in int for which level, that links to element of array
    //TODO correct or incorrect sounds 
    //Possibly have a delay to show write or wrong (green/red)
    private void answerMultiChoiceQuestion(int questionRound, int selection) {
        Debug.Log("Answered Level " + getCurrentLevel() + " (question round " + questionRound + ") Q" + questionNumber + " with " + selection);

        if (selection == correctAnswers[questionRound, questionNumber]) {
            score += 33.3f;
        } else {
            //Todo maybe play a sound?
        }

        multiChoiceQuestions[questionRound].gameObjects[questionNumber].SetActive(false);
        multiChoiceQuestions[questionRound].gameObjects[questionNumber + 1].SetActive(true);

        questionNumber++;
        if (questionNumber == 3) {
            setQuestionSummary(questionRound);
        }
    }

    private void setQuestionSummary(int questionRound) {
        string summary = "You answered "; 

        if (score == 0) {
            summary += "0/3";
        }
        else if (score < 35) {
            summary += "1/3";
        
        } else if (score < 70) {
            summary += "2/3";
        } else {
            summary += "3/3";
        }

        summary += " correctly";

        multiChoiceQuestions[questionRound].gameObjects[3].GetComponent<Text>().text = summary;
    }

}
