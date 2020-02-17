using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModeController : MonoBehaviour {

    SUDSInputController sudsInputController;
    Master masterScript;

    protected int score;
    protected int currentLevel;
    protected bool waitingForSUDSInput;

	void Start () {
        //Find the two scripts in the scene and cache them for use later
        sudsInputController = GameObject.Find("SUDSInputObject").GetComponent<SUDSInputController>();
        masterScript = GameObject.Find("ScenePersistentObject").GetComponent<Master>();
	}
	
	void Update () {
		if (waitingForSUDSInput) {
            int sudsValue = sudsInputController.getSUDSValue();
            if (sudsValue != -1) {
                waitingForSUDSInput = false;
                masterScript.completeLevel(currentLevel, score, sudsValue);
                currentLevel++;
                score = 0;
            }
           
        }
	}

    protected void endLevel() {
        //TODO get suds input 
        waitingForSUDSInput = true;
        sudsInputController.showSUDSPrompt();
    }
}
