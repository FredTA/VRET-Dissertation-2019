using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPathController : MonoBehaviour {

    private SpiderController spiderController;

    //TODO fine tune these
    private const float ROTATION_SPEED = 2.5f;
    private const float BASE_SPEED = 0.5f;
    private const float MAXIMUM_SPEED_VARIANCE_MULTIPLIER = 3.3f;
    private const float RANDOM_WALK_SPEED_MULTIPLIER = 1.6f;
    private const float MINIM_WAIT_TIME = 0.3f;
    private const float MAXIMUM_WAIT_TIME = 1.2f;

    private float randomWalkSpeed = BASE_SPEED;
    private float randomRotateSpeed = ROTATION_SPEED;
    private float timeOfLastArrival = -1;
    private float waitTime;

    private const float MINIMUM_DISTANCE_TO_TARGET = 0.03f;

    //float timer;
    private int currentNode;
    private Vector3 startPosition;
    private Vector3 targetPosition = new Vector3(-1,-1,-1);

    private Vector3 initialSpiderPosition;
    private Quaternion initialSpiderOrientation;

    private Animator animator;

    // Use this for initialization
    void Awake() {
        initialSpiderPosition = gameObject.transform.position;
        initialSpiderOrientation = gameObject.transform.rotation;

        spiderController = GameObject.Find("Controller").GetComponent<SpiderController>();
        animator = gameObject.GetComponent<Animator>();

        currentNode = 0;
    }

    //void OnEnable() {
    //    Debug.Log("SPIDER ENABLED " + spiderController.getCurrentBehaviour());
    //    if (spiderController.getCurrentBehaviour() == SpiderBehaviour.GroupWalk || spiderController.getCurrentBehaviour() == SpiderBehaviour.RandomWalk) {
    //        setRandomTargetPosition();
    //    }
    //    else if (spiderController.getCurrentBehaviour() == SpiderBehaviour.SlowWalk) {
    //        setTargetPositionToNode();
    //    }
    //}

    // Update is called once per frame
    void Update() {
        switch (spiderController.getCurrentBehaviour()) {
            case SpiderBehaviour.Stationary:

                break;
            case SpiderBehaviour.SlowWalk:
                if (Vector3.Distance(transform.position, targetPosition) > MINIMUM_DISTANCE_TO_TARGET) {
                    Debug.Log("DIST: " + MINIMUM_DISTANCE_TO_TARGET + ": " + Vector3.Distance(transform.position, targetPosition));
                    walk(BASE_SPEED, ROTATION_SPEED);
                }
                else {
                    //If we're not at the last node
                    if (currentNode < 12 - 1) {
                        Debug.Log("incrementing node");
                        currentNode++;
                        setTargetPositionToNode();
                    }
                    else {
                        currentNode = 0;
                        Debug.Log("Setting node to zero");
                        setTargetPositionToNode();
                    }
                }
                break;
            case SpiderBehaviour.RandomWalk:

                if (Vector3.Distance(transform.position, targetPosition) > MINIMUM_DISTANCE_TO_TARGET) {
                    walk(randomWalkSpeed, randomRotateSpeed);
                }
                else {
                    if (timeOfLastArrival == -1) {
                        timeOfLastArrival = Time.time;
                        waitTime = Random.Range(MINIM_WAIT_TIME, MAXIMUM_WAIT_TIME);
                        animator.enabled = false;
                    }
                    else if (Time.time - timeOfLastArrival > waitTime) {
                        timeOfLastArrival = -1;
                        setRandomTargetPosition();
                    }

                }
                break;
            case SpiderBehaviour.GroupWalk:
           
                if (Vector3.Distance(transform.position, targetPosition) > MINIMUM_DISTANCE_TO_TARGET) {
                    walk(randomWalkSpeed, randomRotateSpeed);
                }
                else {
                    if (timeOfLastArrival == -1) {
                        timeOfLastArrival = Time.time;
                        waitTime = Random.Range(MINIM_WAIT_TIME, MAXIMUM_WAIT_TIME);
                        animator.enabled = false;
                    }
                    else if (Time.time - timeOfLastArrival > waitTime) {
                        timeOfLastArrival = -1;
                        setRandomTargetPosition();
                    }

                }
                break;
            case SpiderBehaviour.Descend:

                break;

        }
    }

    private void walk(float speed, float rotateSpeed) {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, (targetPosition - transform.position) / 2, (rotateSpeed * Time.deltaTime), 0.0f);
        newRotation.y = 0;
        transform.rotation = Quaternion.LookRotation(newRotation);
        transform.position += transform.forward * speed * Time.deltaTime * 0.2f;

    }

    //Gets the next node's position and saves it
    public void setTargetPositionToNode() {
        Debug.Log("Current node " + currentNode);
        targetPosition = spiderController.getPathNodePosition(currentNode);
        //Debug.Log("TARGET POS " + targetPosition.x + "," + targetPosition.z);
        startPosition = transform.position;
    }

    public void setRandomTargetPosition() {
        targetPosition = spiderController.getRandomPositon(transform.position);
        targetPosition.y = initialSpiderPosition.y; //Make sure we don't change elevation

        float minimumSpeed = BASE_SPEED * RANDOM_WALK_SPEED_MULTIPLIER;
        float multiplier = Random.Range(1, MAXIMUM_SPEED_VARIANCE_MULTIPLIER);
        randomWalkSpeed = minimumSpeed * multiplier;
        randomRotateSpeed = ROTATION_SPEED * multiplier;

        animator.enabled = true;
        animator.speed = multiplier;

        Debug.Log("RANDOM POSITON " + targetPosition.x + ", " + targetPosition.z);

        //currentSpeed = Random.Range(baseSpeed + baseSpeed * speedVarianceMultiplier, baseSpeed - baseSpeed * speedVarianceMultiplier);
        //currentSpeed = baseSpeed; //Need to take into account distance between vector3s TODO
    }

    public void toggleAnimator(bool animate) {
        animator.enabled = animate;
    }
}
