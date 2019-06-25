using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Scripts holding the other controllers can be activated and deactivated 
public class RoomController : MonoBehaviour {

    private RoomMode currentRoomMode;

    public GameObject heightsControllerObject;
    public AudioSource backgroundMusic;

    private SpiderController spiderController;
    private Elevator heightsController;
    private WallsController wallsController;
    private TextDisplayController textDisplayController;

    private bool changingMode = false;
    private RoomMode pendingRoomMode;

    private enum RoomMode {
        Spider,
        Heights,
        Walls,
    }

    // Use this for initialization
    void Start() {
        //Cache references to controllers, which are attached to the same object this script is attached to
        spiderController = gameObject.GetComponent<SpiderController>();
        wallsController = gameObject.GetComponent<WallsController>();
        textDisplayController = gameObject.GetComponent<TextDisplayController>();

        //Script whose gameobject is not the same as this script's
        heightsController = heightsControllerObject.GetComponent<Elevator>();

        currentRoomMode = RoomMode.Spider;
        spiderController.activate();
        textDisplayController.HighLightSpiderText();

        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update() {
        //If the room is being in the process of being reconfigured
        if (changingMode) {
            switch (currentRoomMode) {
                //Check if the controller has finished deactivating, if it has then complete the mode change
                case RoomMode.Spider:
                    if (!spiderController.controllerIsActive()) {
                        CompleteModeChange();
                    }
                    break;
                case RoomMode.Heights:
                    if (!heightsController.controllerIsActive()) {
                        CompleteModeChange();
                    }
                    break;
                case RoomMode.Walls:
                    if (!wallsController.controllerIsActive()) {
                        CompleteModeChange();
                    }
                    break;
            }
        }
        //If the user has pressed the key to activate spider mode
        else if (Input.GetKey(KeyCode.Alpha1)) {
            //First check spider mode isn't already active
            if (currentRoomMode != RoomMode.Spider) {
                textDisplayController.ActivateChangingModeText();
                changingMode = true;
                pendingRoomMode = RoomMode.Spider;

                //Deactivate the controller that is currently activated
                switch (currentRoomMode) {
                    case RoomMode.Heights:
                        heightsController.deactivate();
                        break;
                    case RoomMode.Walls:
                        wallsController.deactivate();
                        break;
                }
            }
        }
        //If the user has pressed the key to activate heights mode
        else if (Input.GetKey(KeyCode.Alpha2)) {
            //First check heights mode isn't already active
            if (currentRoomMode != RoomMode.Heights) {
                textDisplayController.ActivateChangingModeText();
                changingMode = true;
                pendingRoomMode = RoomMode.Heights;

                //Deactivate the controller that is currently activated
                switch (currentRoomMode) {
                    case RoomMode.Spider:
                        spiderController.deactivate();
                        break;
                    case RoomMode.Walls:
                        wallsController.deactivate();
                        break;
                }
            }
        }
        else if (Input.GetKey(KeyCode.Alpha3)) {
            Debug.Log("3 pressed");
            //First check walls mode isn't already active
            if (currentRoomMode != RoomMode.Walls) {
                textDisplayController.ActivateChangingModeText();
                changingMode = true;
                pendingRoomMode = RoomMode.Walls;

                //Deactivate the controller that is currently activated
                switch (currentRoomMode) {
                    case RoomMode.Heights:
                        heightsController.deactivate();
                        break;
                    case RoomMode.Spider:
                        spiderController.deactivate();
                        break;
                }
            }
        }
    }

    /**
    private bool CheckRoomSetupIsComplete() {
        //Return true if the controller being activated has finished it's setup
        switch (pendingRoomMode) {
            case RoomMode.Spider:
                return true;
                break;
            case RoomMode.Heights:
                return true;
                break;
            case RoomMode.Walls:
                return wallsController.CheckSetupComplete();
                break;
        }
        return true;
    }
    **/

    private void CompleteModeChange() {
        //Set the appropriate controller to active 
        switch (pendingRoomMode) {
            case RoomMode.Spider:
                spiderController.activate();
                textDisplayController.HighLightSpiderText();
                break;
            case RoomMode.Heights:
                heightsController.activate();
                textDisplayController.HighLightHeightsText();
                break;
            case RoomMode.Walls:
                wallsController.activate();
                //textDisplayController.HighLightWallsText();
                break;
        }

        Debug.Log("Mode change complete");
        currentRoomMode = pendingRoomMode;
        changingMode = false;
    }


}
