﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpiderBehaviour {
    Stationary, 
    SlowWalk, 
    RandomWalk, 
    GroupWalk, 
    Descend
}

public class SpiderController : MonoBehaviour {

    //TODO fine tune these
    private const float ROTATION_SPEED = 1.35f;
    private const float BASE_SPEED = 0.5f;
    private const float MAXIMUM_SPEED_VARIANCE_MULTIPLIER = 1.9f;
    private const float RANDOM_WALK_SPEED_MULTIPLIER = 1.6f;
    private const float MINIM_WAIT_TIME = 0.3f;
    private const float MAXIMUM_WAIT_TIME = 1.2f;
    private const float MINIMUM_RANDOM_DISTANCE = 0.4f;

    private float randomWalkSpeed;
    private float randomRotateSpeed;
    private float timeOfLastArrival = -1;
    private float waitTime;

    private const float MINIMUM_DISTANCE_TO_TARGET = 0.002f;

    //float timer;
    private int currentNode;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Vector3 initialSpiderPoisition;
    private Quaternion initialSpiderOrientation;

    private SpiderBehaviour currentBehaviour;

    public GameObject squareNodeHolder;
    public GameObject circleNodeHolder;
    public GameObject triangleNodeHolder;

    private ArrayList squareNodes = new ArrayList();
    private ArrayList circleNodes = new ArrayList();
    private ArrayList triangleNodes = new ArrayList();

    public GameObject tableRightBorderGO;
    public GameObject tableLeftBorderGO;
    public GameObject tableUpBorderGO;
    public GameObject tableDownBorderGO;

    private float tableRightBorderX;
    private float tableLeftBorderX;
    private float tableUpBorderZ;
    private float tableDownBorderZ;

    private int spiderPathType = 0;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        initialSpiderPoisition = gameObject.transform.position;
        initialSpiderOrientation = gameObject.transform.rotation;

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

        tableRightBorderX = tableRightBorderGO.transform.position.x;
        tableLeftBorderX = tableLeftBorderGO.transform.position.x;
        tableUpBorderZ = tableUpBorderGO.transform.position.z;
        tableDownBorderZ = tableDownBorderGO.transform.position.z;

        setBeviour(SpiderBehaviour.Stationary);

        currentNode = 0;
        setTargetPositionToNode();
    }

    // Update is called once per frame
    void Update() {
        switch (currentBehaviour) {
            case SpiderBehaviour.Stationary:

                break;
            case SpiderBehaviour.SlowWalk:
                if (Vector3.Distance(transform.position, targetPosition) > MINIMUM_DISTANCE_TO_TARGET) {
                    walk(BASE_SPEED, ROTATION_SPEED);
                } else {
                    //If we're not at the last node
                    if (currentNode < 12 - 1) {
                        currentNode++;
                        setTargetPositionToNode();
                    }
                    else {
                        currentNode = 0;
                        setTargetPositionToNode();
                    }
                }
                break;
            case SpiderBehaviour.RandomWalk:
                if (Vector3.Distance(transform.position, targetPosition) > MINIMUM_DISTANCE_TO_TARGET) {
                    walk(randomWalkSpeed, randomRotateSpeed);
                } else {
                    if (timeOfLastArrival == -1) {
                        timeOfLastArrival = Time.time;
                        waitTime = Random.Range(MINIM_WAIT_TIME, MAXIMUM_WAIT_TIME);
                        animator.enabled = false;
                    } else if (Time.time - timeOfLastArrival > waitTime) {
                        timeOfLastArrival = -1;
                        setRandomPosition();
                    }
                    
                }
                break;
            case SpiderBehaviour.GroupWalk:

                break;
            case SpiderBehaviour.Descend:

                break;

        }
    }

    private void walk(float speed, float rotateSpeed) {
        Vector3 newRotation = Vector3.RotateTowards(transform.forward, (targetPosition - transform.position) / 2, (rotateSpeed * Time.deltaTime), 0.0f);
        transform.position += transform.forward * speed * Time.deltaTime * 0.2f;
        transform.rotation = Quaternion.LookRotation(newRotation);
    }

    //Gets the next node's position and saves it
    private void setTargetPositionToNode() {
        //timer = 0;
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
        //Debug.Log("TARGET POS " + targetPosition.x + "," + targetPosition.z);
        startPosition = transform.position;
    }

    void setRandomPosition() {
        do {
            float x = Random.Range(tableLeftBorderX, tableRightBorderX);
            float z = Random.Range(tableUpBorderZ, tableDownBorderZ);
            startPosition = transform.position;
            targetPosition = new Vector3(x, gameObject.transform.position.y, z);

            Debug.Log("Dis: " + (Vector3.Distance(startPosition, targetPosition)));
            if ((Vector3.Distance(startPosition, targetPosition) < MINIMUM_RANDOM_DISTANCE)) {
                Debug.Log("TOO CLOSE!");
            }
        } while (Vector3.Distance(startPosition, targetPosition) < MINIMUM_RANDOM_DISTANCE);


        float minimumSpeed = BASE_SPEED * RANDOM_WALK_SPEED_MULTIPLIER;
        float multiplier = Random.Range(1, MAXIMUM_SPEED_VARIANCE_MULTIPLIER);
        randomWalkSpeed = minimumSpeed * multiplier;
        randomRotateSpeed = ROTATION_SPEED * multiplier;

        animator.enabled = true;
        animator.speed = multiplier;

        //currentSpeed = Random.Range(baseSpeed + baseSpeed * speedVarianceMultiplier, baseSpeed - baseSpeed * speedVarianceMultiplier);
        //currentSpeed = baseSpeed; //Need to take into account distance between vector3s TODO
    }

    public void changeWalkingMode() {
        spiderPathType++;
    }

    public void setBeviour(SpiderBehaviour behaviour) {
        if (behaviour == SpiderBehaviour.Stationary) {
            animator.enabled = false;
        } else {
            animator.enabled = true;
            if (behaviour == SpiderBehaviour.RandomWalk) {
                setRandomPosition();
            } else {
                animator.speed = 1;
            }
        }
        currentBehaviour = behaviour;
    }

}

