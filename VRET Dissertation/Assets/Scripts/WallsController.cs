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

    public float frontWallSpeed;
    public float sideWallsSpeedMultiplier;
    public float backWallSpeedMultiplier;
    public float ceilingSpeedMultiplier;

    private float originalFrontWallPosition;
    //private float originalLeftWallPosition;
    //private float originalRightWallPosition;
    //private float originalBackWallPosition;

    public float endFrontWallPosition;
    //public float endlLeftWallPosition;
    //public float endRightWallPosition;
    //public float endBackWallPosition;

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

        //Cache positions of each wall so we know how far out they should move
        originalFrontWallPosition = frontWall.transform.position.z;
        //originalLeftWallPosition = leftWall.transform.position.x;
        //originalRightWallPosition = rightWall.transform.position.x;
        //originalBackWallPosition = backWall.transform.position.z;
    }

	// Update is called once per frame
	void Update () {
        if (activated) {
            if (deactivating && windowController.isWindowOpen() && holeController.CheckAllObjectsStationary()) {
                //If the controller is deactivating, and all objects are stationary, it must be finished deactivating
                activated = false;
                deactivating = false;
                Debug.Log("Walls Controller deactivated");
            }
            //If the objects are all hidden below and the window is closed 
            else {
                //Moving walls in
                if (Input.GetKey(KeyCode.DownArrow) && frontWall.transform.position.z > endFrontWallPosition) {
                    frontWall.transform.Translate(0, 0, -frontWallSpeed * Time.deltaTime, Space.World);
                    leftWall.transform.Translate(frontWallSpeed * sideWallsSpeedMultiplier * Time.deltaTime, 0, 0, Space.World);
                    rightWall.transform.Translate(frontWallSpeed * -sideWallsSpeedMultiplier * Time.deltaTime, 0, 0, Space.World);
                    backWall.transform.Translate(0, 0, frontWallSpeed * backWallSpeedMultiplier * Time.deltaTime, Space.World);
                    ceiling.transform.Translate(0, frontWallSpeed * -ceilingSpeedMultiplier * Time.deltaTime, 0, Space.World);
                }
                //Moving walls back out
                else if (Input.GetKey(KeyCode.UpArrow) && frontWall.transform.position.z < originalFrontWallPosition) {
                    frontWall.transform.Translate(0, 0, frontWallSpeed * Time.deltaTime, Space.World);
                    leftWall.transform.Translate(frontWallSpeed * -sideWallsSpeedMultiplier * Time.deltaTime, 0, 0, Space.World);
                    rightWall.transform.Translate(frontWallSpeed * sideWallsSpeedMultiplier * Time.deltaTime, 0, 0, Space.World);
                    backWall.transform.Translate(0, 0, frontWallSpeed * -backWallSpeedMultiplier * Time.deltaTime, Space.World);
                    ceiling.transform.Translate(0, frontWallSpeed * ceilingSpeedMultiplier * Time.deltaTime, 0, Space.World);
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
