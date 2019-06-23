using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour {

    private bool activated = false;
    private bool deactivating = false;

    public GameObject windowControllerObject;

    private WindowController windowController;
    private HoleControllerMaster holeController;

	// Use this for initialization
	void Start () {
        windowController = windowControllerObject.GetComponent<WindowController>();
        holeController = gameObject.GetComponent<HoleControllerMaster>(); //Attached to this gameobject
	}

	// Update is called once per frame
	void Update () {
        if (activated && holeController.CheckAllObjectsStationary()) {

            if (deactivating && windowController.isWindowOpen()) {
                //If the controller is deactivating, and all objects are stationary, it must be finished deactivating
                activated = false;
                deactivating = false;
            }
            //If the objects are all hidden below and the window is closed 
            else if (holeController.CheckAllObjectsStationary() && windowController.isWindowClosed()) {

                if (Input.GetKey(KeyCode.DownArrow)) {
                    Debug.Log("Moving walls in");
                }
            }
            
        }

    }

    public bool controllerIsActive() {
        return activated;
    }

    public void activate() {
        holeController.HideObjects();
        windowController.CloseWindow();
        //Set text to please wait (while room reconfigures)
    }

    public void deactivate() {
        deactivating = true;
        holeController.ShowObjects();
        windowController.OpenWindow();
        //Set text to please wait (while room reconfigures)
    }
}
