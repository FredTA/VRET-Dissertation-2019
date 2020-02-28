using System;
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

	// Use this for initialization
	public void Awake () {
        base.Awake(4); //4 questions rounds for this mode
        correctAnswers = new int[,] { { 2, 2, 1 }, //lvl 1
                                      { 1, 2, 0 }, //lvl 2
                                      { 0, 0, 0 }, //lvl 5
                                      { 0, 0, 0 }, }; //lvl 6

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
        multiChoiceQuestionsActive = false;
        questionNumber = 0;

        //TODO ensure everything is activated in every case
        int level = getCurrentLevel();
        switch (level) {
            case 0:
                score = 100;
                level0Instructions.SetActive(true);
                break;
            case 1:
                level0Instructions.SetActive(false);

                multiChoiceQuestions[0].questions[0].SetActive(true);
                multiChoiceQuestionsActive = true;
                laptop.SetActive(true);
                break;
            case 2:
                deactivateQuestion(getQuestionRoundForLevel(level - 1));
                cartoonImage.SetActive(false);

                multiChoiceQuestionsActive = true;
                multiChoiceQuestions[1].questions[0].SetActive(true);
                realisticImage.SetActive(true);
                break;
            case 3:
                deactivateQuestion(getQuestionRoundForLevel(level - 1));
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
        base.resetLevel();
        //TODO There'll be some spider specific bits in here, like changing the spider path, etc
    }

    public override int getQuestionRoundForLevel(int level) {
        switch (level) {
            case 1:
                return 0;
                break;
            case 2:
                return 1;
                break;
            case 5:
                return 2;
                break;
            case 6:
                return 3;
                break;
            default:
                Debug.Log("Trying to get question round, but this level has no questions");
                return -1;
                break;
        }
    }

}
