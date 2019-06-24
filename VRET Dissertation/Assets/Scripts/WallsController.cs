using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour {

    private bool activated = false;
    private bool deactivating = false;
    private bool activating = false;

    public GameObject windowControllerObject;

    private WindowController windowController;
    private HoleControllerMaster holeController; //Attached to this gameobject
    private TextDisplayController textDisplayController;

	// Use this for initialization
	void Start () {
        windowController = windowControllerObject.GetComponent<WindowController>();
        //Attached to this gameobject
        holeController = gameObject.GetComponent<HoleControllerMaster>();
        textDisplayController = gameObject.GetComponent<TextDisplayController>();
    }

	// Update is called once per frame
	void Update () {
        if (activated && holeController.CheckAllObjectsStationary()) {

            if (deactivating && windowController.isWindowOpen()) {
                //If the controller is deactivating, and all objects are stationary, it must be finished deactivating
                activated = false;
                deactivating = false;
                Debug.Log("Walls Controller deactivated");
            }
            //If the objects are all hidden below and the window is closed 
            else if (holeController.CheckAllObjectsStationary() && windowController.isWindowClosed()) {

                if (Input.GetKey(KeyCode.DownArrow)) {
                    Debug.Log("Moving walls in");
                }
            }
            
        }
        else if (activating && holeController.CheckAllObjectsStationary()) {
            activated = true;
            activating = false;
            textDisplayController.HighLightWallsText();
        }

    }

    public bool controllerIsActive() {
        return activated;
    }

    public void activate() {
        activating = true;
        Debug.Log("Activating walls controller");
        holeController.HideObjects();
        windowController.CloseWindow();
    }

    public void deactivate() {
        deactivating = true;
        holeController.ShowObjects();
        windowController.OpenWindow();
    }
}
