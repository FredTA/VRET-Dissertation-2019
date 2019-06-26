using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour {

    private bool activated = false;
    private bool deactivating = false;
    private bool activating = false;

    public GameObject frontWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject backWall;
    public GameObject ceiling;

    public AudioSource wallsMoveInSound;
    public AudioSource wallsMoveOutSound;

    public float shiftSpeedMultiplier;
    public float frontWallSpeed;
    public float sideWallsSpeedMultiplier;
    public float backWallSpeedMultiplier;
    public float ceilingSpeedMultiplier;

    private float originalFrontWallPosition;

    public float endFrontWallPosition;

    public GameObject windowControllerObject;

    private WindowController windowController;
    private HoleControllerMaster holeController; //Attached to this gameobject
    private TextDisplayController textDisplayController;

    //To be used to ensure the show objects and open window are only called once during the deactivation cycle
    private bool wallsResetFlag = false;

	// Use this for initialization
	void Start () {
        windowController = windowControllerObject.GetComponent<WindowController>();
        //Attached to this gameobject
        holeController = gameObject.GetComponent<HoleControllerMaster>();
        textDisplayController = gameObject.GetComponent<TextDisplayController>();

        //Cache positions of wall so we know how far out they should move
        originalFrontWallPosition = frontWall.transform.position.z;
    }

	// Update is called once per frame
	void Update () {
        if (activated) {
            if (deactivating) {
                //If the controller is deactivating, and all objects are stationary, it must be finished deactivating
                if (windowController.isWindowOpen() && holeController.CheckAllObjectsStationary()) {
                    activated = false;
                    deactivating = false;
                    wallsResetFlag = false;
                    Debug.Log("Walls Controller deactivated");
                }
                //If we haven't finished deactivating and If the walls have not returned to their original positions
                else if (frontWall.transform.position.z < originalFrontWallPosition) {
                    MoveWalls(false, true);
                }
                //Else if the walls have been reset (check if audio source is playing so this only executes once)
                else if (!wallsResetFlag) {
                    wallsResetFlag = true;
                    wallsMoveOutSound.Stop();
                    holeController.ShowObjects();
                    windowController.OpenWindow();
                }
            }
            //If the objects are all hidden below and the window is closed 
            else {
                //Moving walls in
                if (Input.GetKey(KeyCode.DownArrow)) {
                    // If wall can move in any further
                    if (frontWall.transform.position.z > endFrontWallPosition) {
                        //Move all walls and ceilings towards the player
                        MoveWalls(true, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
                    }
                    //If wall can't move any further and the sound is playing
                    else if (wallsMoveInSound.isPlaying) {
                        wallsMoveInSound.Stop();
                    }
                }
                //Moving walls back out
                else if (Input.GetKey(KeyCode.UpArrow)) {
                    // If wall can move out any further
                    if (frontWall.transform.position.z < originalFrontWallPosition) {
                        //Move all walls and ceilings away from the player
                        MoveWalls(false, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
                    }
                    //If wall can't move any further and the sound is playing
                    else if (wallsMoveOutSound.isPlaying) {
                        wallsMoveOutSound.Stop();
                    }
                }

                if (Input.GetKeyUp(KeyCode.DownArrow)) {
                    wallsMoveInSound.Stop();
                    Debug.Log("Stopping move in sound");
                }
                if (Input.GetKeyUp(KeyCode.UpArrow)) {
                    wallsMoveOutSound.Stop();
                    Debug.Log("Stopping move out sound");
                }
            }
            
        }
        else if (activating && holeController.CheckAllObjectsStationary()) {
            activated = true;
            activating = false;
            textDisplayController.HighLightWallsText();
        }

    }

    //Moves all walls / ceiling towards player if true, away from player if false
    private void MoveWalls(bool wallsMovingIn, bool shiftBeingHeld) {
        float baseSpeed = 0;

        if (wallsMovingIn) {
            baseSpeed = frontWallSpeed * Time.deltaTime;
            if (!wallsMoveInSound.isPlaying) {
                wallsMoveInSound.Play();
            }
        }
        else {
            baseSpeed = -frontWallSpeed * Time.deltaTime;
            if (!wallsMoveOutSound.isPlaying) {
                wallsMoveOutSound.Play();
            }
        }

        //If the user is holding shift the walls should move twice as fast
        if (shiftBeingHeld) {
            baseSpeed = baseSpeed * shiftSpeedMultiplier;
        }

        //Move gameobjects
        frontWall.transform.Translate(0, 0, -baseSpeed, Space.World);
        leftWall.transform.Translate(baseSpeed * sideWallsSpeedMultiplier, 0, 0, Space.World);
        rightWall.transform.Translate(baseSpeed * -sideWallsSpeedMultiplier, 0, 0, Space.World);
        backWall.transform.Translate(0, 0, baseSpeed * backWallSpeedMultiplier, Space.World);
        ceiling.transform.Translate(0, baseSpeed * -ceilingSpeedMultiplier, 0, Space.World);
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
    }
}
