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
    public SpiderController spiderController;

    private const int NUMBER_OF_QUESTION_ROUNDS = 3;

    public GameObject level0Instructions;
    public GameObject level3Instructions;
    public GameObject level4Instructions;
    public GameObject level6Instructions;
    public GameObject level7Instructions;
    public GameObject level8Instructions;
    public GameObject level9Instructions;

    public GameObject camera;
    public GameObject lookMarker;

    //Scoring consts
    private const float MIN_CAMERA_DISTANCE_TO_BOX = 0.9f;
    private const float MAX_CAMERA_DISTANCE_TO_BOX = 1.45f;
    private const float MIN_CAMERA_DISTANCE_TO_SPIDER = 2.5f;
    private const float MAX_CAMERA_DISTANCE_TO_SPIDER = 2.5f;

    public override void Awake() {
        loadMultiChoiceQuestions(NUMBER_OF_QUESTION_ROUNDS);
        correctAnswers = new int[,] { { 2, 2, 1 }, //lvl 1
                                      { 1, 2, 0 }, //lvl 2
                                      { 2, 2, 2 }}; //lvl 5
        base.Awake();
    }

    // Update is called once per frame
    //Handle scoring depending on what level we're on and what the user is doing
    void Update () {
        switch (getCurrentLevel()) {
            case 3: //Scored for getting close to box
                float distance = Vector3.Distance(spiderBox.transform.position, camera.transform.position);
                Debug.Log("DISTANCE " + distance);

                if (distance < MIN_CAMERA_DISTANCE_TO_BOX) {
                    score = 100;
                } else {
                    float newScore = 100 - ((100 * (distance - MIN_CAMERA_DISTANCE_TO_BOX) / (MAX_CAMERA_DISTANCE_TO_BOX - MIN_CAMERA_DISTANCE_TO_BOX))); 
                    if (score < newScore) {
                        score = newScore;
                    }
                }
                //dis = 1.25, min = 1, max = 1.5
                
                break;
            case 4: //scored for getting close to spider

                break;
            case 6: //Scored for looking up at the marker

                break;
            case 7: //Scored for making the spider larger
                spiderController.handleSpiderScale();
                break;
            case 8: //Scored for sitting for 2 mins

                break;
            case 9: //Scored for lowering spider from ceiling

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
        //We have to activate anything we need in each case, as we can jump in at any level
        int level = getCurrentLevel();
        switch (level) {
            case 0: //Intro level
                score = 100;
                level0Instructions.SetActive(true);
                break;
            case 1: //Cartoon spider on laptop
                level0Instructions.SetActive(false);

                activateQuestionForLevel(level);
                laptop.SetActive(true);
                break;
            case 2: //Realistic spider on laptop
                deactivateQuestionForLevel(level - 1);
                cartoonImage.SetActive(false);

                activateQuestionForLevel(level);
                realisticImage.SetActive(true);
                laptop.SetActive(true);
                break;
            case 3: //Spider in box
                deactivateQuestionForLevel(level - 1);
                laptop.SetActive(false);

                level3Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.Stationary);
                spiderBox.SetActive(true);
                break;
            case 4: //Spider out of box
                spiderBox.SetActive(false);
                level3Instructions.SetActive(false);

                level4Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.Stationary);
                break;
            case 5: //Spider walks in a pattern
                level4Instructions.SetActive(false);

                activateQuestionForLevel(level);
                spiderController.setBeviour(SpiderBehaviour.SlowWalk);
                break;
            case 6: //Spider walks randomly
                deactivateQuestionForLevel(level - 1);

                level6Instructions.SetActive(true);
                lookMarker.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.RandomWalk);
                break;
            case 7: //Spider becomes larger
                level6Instructions.SetActive(false);
                lookMarker.SetActive(false);

                level7Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.RandomWalk);
                break;
            case 8: //A group of spiders
                level7Instructions.SetActive(false);

                level8Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.GroupWalk);
                break;
            case 9: //A spider descending from the ceiling?
                level8Instructions.SetActive(false);

                level9Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.Descend);
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
