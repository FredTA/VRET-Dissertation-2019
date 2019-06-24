using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour {

    public GameObject objectToHide;
    private MeshRenderer renderer;
    private HoleControllerMaster holeControllerMaster;

    private const float HOLE_TRANSITION_SPEED = 0.08f;
    private const float OBJECT_ELAVATION_SPEED = 0.4f;
    private const float HOLE_PAUSE_TIME = 0.45f;
    private Vector3 holeTransitionStep = new Vector3(HOLE_TRANSITION_SPEED, 0, 0);
    private Vector3 objectElevationStep = new Vector3(0, OBJECT_ELAVATION_SPEED, 0);
    private Vector3 maxHoleScale;

    private float maxObjectElevation;
    private float minObjectElevation;

    private bool holeOpening = false;
    private bool holeClosing = false;
    private bool objectLowering = false;
    private bool objectRaising = false;

    private bool objectDirectionIsUpward = false;

    private void setObjectToLower() {
        objectLowering = true;
    }

    private void setObjectToRaise() {
        objectRaising = true;
    }

    private void setHoleToClose() {
        holeClosing = true;
    }

    // Use this for initialization
    void Start () {
        renderer = gameObject.GetComponent<MeshRenderer>();

        maxHoleScale = transform.localScale;
        //transform.localScale.Set(0, maxHoleScale.y, maxHoleScale.z);
        transform.localScale = new Vector3(0, maxHoleScale.y, maxHoleScale.z);

        //Cache offsets for the object being hidden
        maxObjectElevation = objectToHide.transform.position.y;
        //float objectHeight = objectToHide.transform.localScale.z;
        //minObjectElevation = maxObjectElevation - objectHeight;

        MeshRenderer objectRenderer = objectToHide.GetComponent<MeshRenderer>();
        float objectHeight = objectRenderer.bounds.size.y;
        minObjectElevation = maxObjectElevation - objectHeight;
    }
	
	// Update is called once per frame
	void Update () {
        if (holeOpening) {
            //Open the hole until it is at it's max size
            if (transform.localScale.x < maxHoleScale.x) {
                Debug.Log("Opening hole");
                transform.localScale += holeTransitionStep * Time.deltaTime;
            }
            else {
                Debug.Log("Hole opened");
                holeOpening = false;

                if (objectDirectionIsUpward) {
                    Invoke("setObjectToRaise", HOLE_PAUSE_TIME);
                }
                else {
                    Invoke("setObjectToLower", HOLE_PAUSE_TIME);
                }
                
            }
        }
        else if (objectLowering) {
            if (objectToHide.transform.position.y > minObjectElevation) {
                Debug.Log("Lowering object");
                objectToHide.transform.Translate(-objectElevationStep * Time.deltaTime);
            }
            else {
                Debug.Log("Object lowered");
                objectLowering = false;
                Invoke("setHoleToClose", HOLE_PAUSE_TIME);
            }
        }
        else if (objectRaising) {
            if (objectToHide.transform.position.y < maxObjectElevation) {
                Debug.Log("Raising object");
                objectToHide.transform.Translate(objectElevationStep * Time.deltaTime);
            }
            else {
                Debug.Log("Object raised");
                objectRaising = false;
                Invoke("setHoleToClose", HOLE_PAUSE_TIME);
            }
        }
        else if (holeClosing) {
            //Close the hole until it is at an x scale of 0
            if (transform.localScale.x > 0) {
                transform.localScale += -holeTransitionStep * Time.deltaTime;
            }
            else {
                holeClosing = false;
                renderer.enabled = false;
                Debug.Log("Hole closed - incrementing counter in master controller");
                holeControllerMaster.IncrementStationaryObjects(); //This object is now stationary, so increment the master's number of hidden objects
            }
        }
	}

    public void giveReferenceToMasterController(HoleControllerMaster holeControllerMaster) {
        this.holeControllerMaster = holeControllerMaster;
    }

    public void hideObject() {
        Debug.Log("Object set to hide");
        holeOpening = true;
        renderer.enabled = true;
        objectDirectionIsUpward = false;
    }

    public void showObject() {
        holeOpening = true;
        renderer.enabled = true;
        objectDirectionIsUpward = true;
    }
}
