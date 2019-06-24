using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleControllerMaster : MonoBehaviour {

    private float KEY_COOLDOWN_TIME = 0.3f;
    private bool keyCoolingDown = false;
    private ArrayList holeControllers = new ArrayList();
    private int numberOfObjectsStationary = 0;

    // Use this for initialization
    void Start () {
        //Find all holes and cache the scripts
        foreach (GameObject hole in GameObject.FindGameObjectsWithTag("Hole")) {
            HoleController holeController = hole.GetComponent<HoleController>();
            holeController.giveReferenceToMasterController(this); //Needed so that the script can call the master controller's "CheckAllObjectsStationary" method
            holeControllers.Add(holeController);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Make sure the key cooldown has timed out. We are working in Update, so one press can last many cycles, and trigger the bool many times
        if (Input.GetKey(KeyCode.Return) && !keyCoolingDown) {
            HideObjects();
            keyCoolingDown = true;
            Invoke("ResetCooldown", KEY_COOLDOWN_TIME);
        }
    }

    public void HideObjects() {
        Debug.Log("Hiding objects");
        foreach (HoleController holeController in holeControllers) {
            holeController.hideObject();
        }
    }

    public void ShowObjects() {
        numberOfObjectsStationary = 0;
        foreach (HoleController holeController in holeControllers) {
            holeController.showObject();
        }
    }

    public void IncrementStationaryObjects() {
        numberOfObjectsStationary++;
    }

    public bool CheckAllObjectsStationary() {
        //Each controller corresponds to one hole, and one object
        return numberOfObjectsStationary == holeControllers.Count;
    }

    private void ResetCooldown() {
        keyCoolingDown = false;
    }
}
