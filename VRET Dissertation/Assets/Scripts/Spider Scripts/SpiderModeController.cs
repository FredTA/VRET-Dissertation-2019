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

    private const int NUMBER_OF_QUESTION_ROUNDS = 4;

    public GameObject level0Instructions;

	// Use this for initialization
	public override void Awake () {
        loadMultiChoiceQuestions(NUMBER_OF_QUESTION_ROUNDS);
        correctAnswers = new int[,] { { 2, 2, 1 }, //lvl 1
                                      { 1, 2, 0 }, //lvl 2
                                      { 0, 0, 0 }, //lvl 5
                                      { 0, 0, 0 }, }; //lvl 6
        
        spiderController = spider.GetComponent<SpiderController>();

        base.Awake(); //4 questions rounds for this mode
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

                activateQuestionForLevel(level);
                laptop.SetActive(true);
                break;
            case 2:
                deactivateQuestionForLevel(level - 1);
                cartoonImage.SetActive(false);

                activateQuestionForLevel(level);
                realisticImage.SetActive(true);
                laptop.SetActive(true);
                break;
            case 3:
                deactivateQuestionForLevel(level - 1);
                laptop.SetActive(false);

                spider.SetActive(true);
                spiderBox.SetActive(true);
                break;
            case 4:
                spiderBox.SetActive(false);

                spider.SetActive(true);
                break;
            case 5:
                activateQuestionForLevel(level);
                spider.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.SlowWalk);
                break;
            case 6:
                activateQuestionForLevel(level);
                spider.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.SlowWalk);
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

    public override void selectMultiChoiceAnswer(int selection) {
        base.selectMultiChoiceAnswer(selection); 

        //On these two levels, the spider switches it's walking path
        if (getCurrentLevel() == 5 || getCurrentLevel() == 6) {
            spiderController.changeWalkingMode();
        }
        
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
