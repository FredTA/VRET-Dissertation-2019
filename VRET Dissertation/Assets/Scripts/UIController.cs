using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private ModeController modeController;

    //Level and high score should be read in once per level
    //method in this class called from... mode controller?
    public Text levelText;
    public Text currentScoreText;
    public Text highScoreText;

    public Text menuText;
    public Text prevLevelText;
    public Text resetLevelText;
    public Text completeLevelText;

    private int horizontalSelection = 0;
    private bool onBottomRow = true;

    // Use this for initialization
    void Start () {
        modeController = GameObject.FindGameObjectWithTag("ModeController").GetComponent<ModeController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) && !onBottomRow) {
            
            if (modeController.multiChoiceQuestionsActive) {
                //Move up
                onBottomRow = false;
                //TODO figure out how exactly to handle the two rows..
            }
        }
        //else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) ||
        //    OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) && menuSelection < 2) {
        //    updateMenuSelection(menuSelection + 1);
        //}
        //else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) ||
        //    OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) && sudsSelection != 0) {
        //    updateSUDSSelection(sudsSelection + 1);
        //}
        //else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight) ||
        //    OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) && sudsSelection != 9) {
        //    updateSUDSSelection(sudsSelection - 1);
        //}
        //else if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.RawButton.X) ||
        //    OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.5f ||
        //    OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f) {

        //    if (menuSelection == 1) {
        //        modeController.submitSUDS(sudsSelection, true); //submit scores and go to next level
        //    }
        //    else if (menuSelection == 2) {
        //        modeController.submitSUDS(sudsSelection, false); //Submit scores and go back to menu
        //    }
        //}
        else {
            //Debug.Log("nada");
        }
    }

    private void updateUISelection() {

    }

    public void updateLevelAndScore(int level, int score) {
        levelText.text = "Level: " + level;
        if (score != -1) {
            highScoreText.text = "High Score: " + score;
        } else {
            highScoreText.text = "N/A";
        }
        
    }
}
