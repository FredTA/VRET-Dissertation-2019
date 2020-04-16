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
    public GameObject descendingSpider;
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
    private Renderer lookMarkerRenderer;

    //Scoring consts
    private const float MIN_CAMERA_DISTANCE_TO_BOX = 0.9f;
    private const float MAX_CAMERA_DISTANCE_TO_TARGET = 1.45f;
    private const float MIN_CAMERA_DISTANCE_TO_SPIDER = 0.75f;
    private const float LEVEL_8_WAIT_TIME = 120;
    private const float LEVEL_9_WAIT_TIME = 45;
    private const float LOOK_MARKER_TIME = 30;

    private float maximumSpiderElevation;
    private float MINIMUM_SPIDER_ELEVATION = 1f;
    private const float spiderDescentSpeed = 0.0006f;

    private float timer;

    public override void Awake() {
        loadMultiChoiceQuestions(NUMBER_OF_QUESTION_ROUNDS);
        correctAnswers = new int[,] { { 2, 2, 1 }, //lvl 1+
                                      { 1, 2, 0 }, //lvl 2
                                      { 2, 2, 2 }}; //lvl 5

        lookMarkerRenderer = lookMarker.GetComponent<Renderer>();
        maximumSpiderElevation = descendingSpider.transform.position.y;
        base.Awake();
    }

    public override void selectMultiChoiceAnswer(int selection) {
        base.selectMultiChoiceAnswer(selection);

        //On this level, the spider switches it's walking path
        if (getCurrentLevel() == 5) {
            spiderController.changeWalkingMode();
        }
    }

    //Update is called once per frame
    void Update () {
        handleCurrentLevel();
    }

    //Handle scoring depending on what level we're on and what the user is doing
    private void handleCurrentLevel() {
        switch (getCurrentLevel()) {
            case 3: //Scored for getting close to box
                handleCameraDistanceScoring(MIN_CAMERA_DISTANCE_TO_BOX);
                break;
            case 4: //scored for getting close to spider
                handleCameraDistanceScoring(MIN_CAMERA_DISTANCE_TO_SPIDER);
                break;
            case 6: //Scored for looking up at the marker
                if (lookMarkerRenderer.isVisible && score < 100) {
                    score += ((100 / LOOK_MARKER_TIME) * Time.deltaTime);
                }
                break;
            case 7: //Scored for making the spider larger
                //similar to the maths in the cam distance, a percentage of scale between max and min
                float scalePercentage;
                spiderController.handleSpiderScale(out scalePercentage);
                if (scalePercentage > score) {
                    score = scalePercentage;
                }
                break;
            case 8: //Scored for sitting for 2 mins
                if (Time.time - timer > LEVEL_8_WAIT_TIME) {
                    score = 100;
                }
                else {
                    score = (100 * (Time.time - timer) / LEVEL_8_WAIT_TIME);
                }
                break;
            case 9: //Scored for lowering spider from ceiling
                if (descendingSpider.transform.position.y > MINIMUM_SPIDER_ELEVATION) {
                    descendingSpider.transform.position -= transform.up * spiderDescentSpeed;
                }

                if (Time.time - timer > LEVEL_9_WAIT_TIME) {
                    score = 100;
                }
                else {
                    score = (100 * (Time.time - timer) / LEVEL_9_WAIT_TIME);
                }
                break;
        }
    }

    //Assign a score based on how close the camera is to the spider or box
    private void handleCameraDistanceScoring(float minDistance) {
        float distance = Vector3.Distance(spiderBox.transform.position, camera.transform.position);

        if (distance < MIN_CAMERA_DISTANCE_TO_BOX) {
            score = 100;
        } else {
            float newScore = 100 - ((100 * (distance - minDistance) / (MAX_CAMERA_DISTANCE_TO_TARGET - minDistance)));
            if (score < newScore) {
                score = newScore;
            }
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
        questionNum = 0;
        base.activateCurrentLevel(); //Plays the voiceover

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

                timer = Time.time;
                level8Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.GroupWalk);
                break;
            case 9: //A spider descending from the ceiling?
                level8Instructions.SetActive(false);

                timer = Time.time;
                descendingSpider.SetActive(true);
                level9Instructions.SetActive(true);
                spiderController.setBeviour(SpiderBehaviour.Inactive);
                break;
        }

        //Do this last, so that the level is ready when the UI wakes up
        toggleSUDSInput(false);
    }

    public override void resetLevel() {
        base.resetLevel();

        switch (getCurrentLevel()) {
            case 5:
                spiderController.resetWalkingMode();
                break;
            case 7:
                spiderController.resetSpiderScale();
                break;
            case 8: 
                timer = Time.time;
                break;
            case 9:
                descendingSpider.transform.position = new Vector3(descendingSpider.transform.position.x, maximumSpiderElevation, descendingSpider.transform.position.z);
                break;
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
