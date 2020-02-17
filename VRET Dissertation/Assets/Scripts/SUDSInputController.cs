using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUDSInputController : MonoBehaviour {

    private int sudsValue = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Do checks for suds input
	}

    public void showSUDSPrompt() {

    }

    //This method will be repeatedly called by the mode controller script 
    //If it's waiting for a suds value. -1 means no value is yet set
    public int getSUDSValue() {
        int value = sudsValue; 

        //If it's not -1, we'll be returning it to the mode cont script 
        //After we've returned it, we don't need to store it here anymore
        if (sudsValue != -1) {
            sudsValue = -1;
        }
        return value;
    }
}
