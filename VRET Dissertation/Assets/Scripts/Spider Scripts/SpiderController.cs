using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpiderBehaviour {
    Stationary, 
    SlowWalk, 
    FastWalk, 
    GroupWalk, 
    Descend
}

public class SpiderController : MonoBehaviour {

    private Vector3 minimumScale;
    public float maxScaleMultiplier;
    private Vector3 maximumScale;
    public float scaleSpeed;

    //TODO fine tune these
    private const float ROTATION_SPEED = 0.8f;
    private const float ROTATION_MULTIPLIER = 1f;
    private const float BASE_SPEED = 0.5f;
    private float currentSpeed;

    float timer;
    int currentNode;
    Vector3 startPosition;
    Vector3 targetPosition;

    private Vector3 initialSpiderPoisition;
    private Quaternion initialSpiderOrientation;

    private SpiderBehaviour currentBehaviour;

    public GameObject squareNodeHolder;
    public GameObject circleNodeHolder;
    public GameObject triangleNodeHolder;

    private ArrayList squareNodes = new ArrayList();
    private ArrayList circleNodes = new ArrayList();
    private ArrayList triangleNodes = new ArrayList();

    private int spiderPathType = 0;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        initialSpiderPoisition = gameObject.transform.position;
        initialSpiderOrientation = gameObject.transform.rotation;
        minimumScale = gameObject.transform.localScale;
        maximumScale = minimumScale * maxScaleMultiplier;

        animator = gameObject.GetComponent<Animator>();

        //Assign node gameobjects to the array so we don't have to do it all through the editor 
        foreach (Transform child in squareNodeHolder.transform) {
            squareNodes.Add(child.gameObject);
        }
        foreach (Transform child in circleNodeHolder.transform) {
            circleNodes.Add(child.gameObject);
        }
        foreach (Transform child in triangleNodeHolder.transform) {
            triangleNodes.Add(child.gameObject);
        }

        setBeviour(SpiderBehaviour.Stationary);

        setTargetPositionToNode();
        currentNode = 0;
    }

    // Update is called once per frame
    void Update() {
        switch (currentBehaviour) {
            case SpiderBehaviour.Stationary:

                break;
            case SpiderBehaviour.SlowWalk:
                timer += Time.deltaTime * BASE_SPEED;

                if (transform.position != targetPosition) {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, timer);
                    Vector3 newRotation = Vector3.RotateTowards(transform.forward, (targetPosition - transform.position), (ROTATION_SPEED * Time.deltaTime * ROTATION_MULTIPLIER), 0.0f);
                    transform.rotation = Quaternion.LookRotation(newRotation);
                } else {
                    //If we're not at the last node
                    if (currentNode < 13 - 1) {
                        currentNode++;
                        setTargetPositionToNode();
                    }
                    else {
                        currentNode = 0;
                        setTargetPositionToNode();
                    }
                }

                break;
            case SpiderBehaviour.FastWalk:

                break;
            case SpiderBehaviour.GroupWalk:

                break;
            case SpiderBehaviour.Descend:

                break;

        }
    }

    //Gets the next node's position and saves it
    private void setTargetPositionToNode() {
        timer = 0;
        Debug.Log("Current node " + currentNode);
        switch (spiderPathType) {
            case 0:
                targetPosition = ((GameObject)squareNodes[currentNode]).transform.position;
                break;

            case 1:
                targetPosition = ((GameObject)circleNodes[currentNode]).transform.position;
                break;

            case 2:
                targetPosition = ((GameObject)triangleNodes[currentNode]).transform.position;
                break;
        }
        startPosition = transform.position;
    }

    private void scaleSpider(bool scaleUp) {
        float scaleToAdd;
        if (scaleUp) {
            scaleToAdd = scaleSpeed * Time.deltaTime;
        }
        else {
            scaleToAdd = -scaleSpeed * Time.deltaTime;
        }

        Vector3 newScale = new Vector3(gameObject.transform.localScale.x + scaleToAdd, gameObject.transform.localScale.y + scaleToAdd, gameObject.transform.localScale.z + scaleToAdd);
        gameObject.transform.localScale = newScale;
    }

    public void changeWalkingMode() {
        spiderPathType++;
    }

    public void setBeviour(SpiderBehaviour behaviour) {
        if (behaviour == SpiderBehaviour.Stationary) {
            animator.enabled = false;
        } else {
            animator.enabled = true;
        }
        currentBehaviour = behaviour;
    }

}

//Some old code for changing spider size, might want this later?
//else if (Input.GetKey(KeyCode.UpArrow) && spider.activeSelf) {
//    if (spider.transform.localScale.magnitude<maximumScale.magnitude) {
//        scaleSpider(true);
//    }
//}
//else if (Input.GetKey(KeyCode.DownArrow) && spider.activeSelf) {
//    if (spider.transform.localScale.magnitude > minimumScale.magnitude) {
//        scaleSpider(false);
//    }
//}

//Reset spider position and scale
//spider.transform.position = initialSpiderPoisition;
//spider.transform.rotation = initialSpiderOrientation;
//spider.transform.localScale = minimumScale;

