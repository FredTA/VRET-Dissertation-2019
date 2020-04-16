using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpiderBehaviour {
    Inactive,
    Stationary, 
    SlowWalk, 
    RandomWalk, 
    GroupWalk, 
    Descend
}

public class SpiderController : MonoBehaviour {

    private const int NUMBER_OF_SPIDERS = 5;

    public GameObject[] spiders = new GameObject[NUMBER_OF_SPIDERS];
    private SpiderPathController[] pathControllers = new SpiderPathController[NUMBER_OF_SPIDERS];

    private SpiderBehaviour currentBehaviour;

    public GameObject squareNodeHolder;
    public GameObject circleNodeHolder;
    public GameObject triangleNodeHolder;

    private ArrayList squareNodes = new ArrayList();
    private ArrayList circleNodes = new ArrayList();
    private ArrayList triangleNodes = new ArrayList();

    private float tableRightBorderX;
    private float tableLeftBorderX;
    private float tableUpBorderZ;
    private float tableDownBorderZ;

    private const float MINIMUM_RANDOM_DISTANCE = 0.4f;
    private int spiderPathType = 0;

    private Vector3 minimumSpiderScale;
    private Vector3 maximumSpiderScale;
    private const float MAXUMUM_SPIDER_SCALE_MULTIPLIER = 2.9f;
    private const float SPIDER_SCALE_SPEED = 0.004f;

    private bool controllersAssigned = false;

    // Use this for initialization
    void Awake() {
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

        assignControllers();

        tableRightBorderX = GameObject.Find("Border right").transform.position.x;
        tableLeftBorderX = GameObject.Find("Border left").transform.position.x;
        tableUpBorderZ = GameObject.Find("Border up").transform.position.z;
        tableDownBorderZ = GameObject.Find("Border down").transform.position.z;

        minimumSpiderScale = spiders[0].transform.localScale;
        maximumSpiderScale = minimumSpiderScale * MAXUMUM_SPIDER_SCALE_MULTIPLIER;

        //Make sure we're set up for the correct behaviour
        //setBeviour(currentBehaviour);
    }


    public void changeWalkingMode() {
        if (spiderPathType < 2) {
            spiderPathType++;
        }
    }

    public void resetWalkingMode() {
        spiderPathType = 0;
    }

    public int getPathType() {
        return spiderPathType;
    }

    private void assignControllers() {
        for (int i = 0; i < NUMBER_OF_SPIDERS; i++) {
            pathControllers[i] = spiders[i].GetComponent<SpiderPathController>();
        }
        controllersAssigned = true;
    }

    public void setBeviour(SpiderBehaviour behaviour) {
        currentBehaviour = behaviour; //Must do this first so that spiders can get current behaviour when awake

        if (!controllersAssigned) {
            Awake();
        }

        if (behaviour != SpiderBehaviour.Inactive) {
            spiders[0].SetActive(true);
            pathControllers[0].toggleAnimator(behaviour != SpiderBehaviour.Stationary);

            if (behaviour == SpiderBehaviour.SlowWalk) {
                pathControllers[0].setTargetPositionToNode();
            } else if (behaviour == SpiderBehaviour.RandomWalk) {
                pathControllers[0].setRandomTargetPosition();
            } else if (behaviour == SpiderBehaviour.GroupWalk) {
                for (int i = 0; i < NUMBER_OF_SPIDERS; i++) {
                    spiders[i].SetActive(true); //Awake method on spiders is triggered
                    pathControllers[i].toggleAnimator(true);
                    pathControllers[i].setRandomTargetPosition();
                }
                
            }
        } else {
            for (int i = 0; i < NUMBER_OF_SPIDERS; i++) {
                spiders[i].SetActive(false); //Awake method on spiders is triggered
            }
        }
    }

    public void handleSpiderScale(out float scalePercentage) {
        //Don't want to call OVR update here, the UI is already handling that for us
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) > 0.2f) {
            if (spiders[0].transform.localScale.magnitude < maximumSpiderScale.magnitude) {
                scaleSpider(true);
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.2f) {
            if (spiders[0].transform.localScale.magnitude > minimumSpiderScale.magnitude) {
                scaleSpider(false);
            }
        }

        scalePercentage = 100 * ((spiders[0].transform.localScale.magnitude - minimumSpiderScale.magnitude) / (maximumSpiderScale.magnitude - minimumSpiderScale.magnitude));
    }

    public void resetSpiderScale() {
        spiders[0].transform.localScale = minimumSpiderScale;
    }

    private void scaleSpider(bool scaleUp) {
        float scaleToAdd;
        if (scaleUp) {
            scaleToAdd = SPIDER_SCALE_SPEED * Time.deltaTime;
        }
        else {
            scaleToAdd = -SPIDER_SCALE_SPEED * Time.deltaTime;
        }

        Vector3 newScale = new Vector3(spiders[0].transform.localScale.x + scaleToAdd, spiders[0].transform.localScale.y + scaleToAdd, spiders[0].transform.localScale.z + scaleToAdd);
        spiders[0].transform.localScale = newScale;
    }

    public Vector3 getPathNodePosition(int nodeNumber) {
        Vector3 targetPosition = new Vector3();
        switch (spiderPathType) {
            case 0:
                targetPosition = ((GameObject)squareNodes[nodeNumber]).transform.position;
                break;

            case 1:
                targetPosition = ((GameObject)circleNodes[nodeNumber]).transform.position;
                break;

            case 2:
                targetPosition = ((GameObject)triangleNodes[nodeNumber]).transform.position;
                break;
        }
        return targetPosition;
    }

    public Vector3 getRandomPositon(Vector3 startPosition) {
        Vector3 targetPosition = new Vector3();
        do {
            float x = Random.Range(tableLeftBorderX, tableRightBorderX);
            float z = Random.Range(tableUpBorderZ, tableDownBorderZ);
            targetPosition = new Vector3(x, startPosition.y, z);
        } while (Vector3.Distance(startPosition, targetPosition) < MINIMUM_RANDOM_DISTANCE);
        return targetPosition;
    }

    public SpiderBehaviour getCurrentBehaviour() {
        return currentBehaviour;
    }

}

