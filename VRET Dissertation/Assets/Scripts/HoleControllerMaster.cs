using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleControllerMaster : MonoBehaviour {

    private float KEY_COOLDOWN_TIME = 0.3f;
    private bool keyCoolingDown = false;
    ArrayList holeControllers = new ArrayList();

    // Use this for initialization
    void Start () {
        //Find all holes and cache the scripts
        foreach (GameObject hole in GameObject.FindGameObjectsWithTag("Hole")) {
            holeControllers.Add(hole.GetComponent<HoleController>());
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Make sure the key cooldown has timed out. We are working in Update, so one press can last many cycles, and trigger the bool many times
        if (Input.GetKey(KeyCode.Return) && !keyCoolingDown) {
            foreach(HoleController holeController in holeControllers) {
                holeController.hideObject();
            }
            keyCoolingDown = true;
            Invoke("ResetCooldown", KEY_COOLDOWN_TIME);
            
        }
    }

    private void ResetCooldown() {
        keyCoolingDown = false;
    }
}
