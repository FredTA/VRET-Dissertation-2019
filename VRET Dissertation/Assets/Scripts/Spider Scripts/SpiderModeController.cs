using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderModeController : ModeController {

    public GameObject laptop;
    public GameObject cartoonImage;
    public GameObject realisticImage;
    public GameObject spiderBox;
    public GameObject spider;

    private SpiderController spiderController;

	// Use this for initialization
	public override void Awake () {
        Debug.Log("Calling base.awake...");
        base.Awake(); 
        spiderController = spider.GetComponent<SpiderController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //We need to store previous level so we know which elements to deactivate 
    //Then activate the current level elements 
    //If prev lvl == -1 don't disable anything 

    public override void activateCurrentLevel() {
        toggleSUDSInput(false);
        score = 0;

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
            case 10:

                break;
        }
    }

    public override void deactivatePreviousLevel() {
        switch (getPreviousLevel()) {
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

    public override void resetLevel() {

    }

    public override void selectMultiChoiceAnswer(int selection) {

    }

}
