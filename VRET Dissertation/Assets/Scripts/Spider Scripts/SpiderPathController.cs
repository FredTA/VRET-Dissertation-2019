using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPathController : MonoBehaviour {
    public GameObject nodeHolder;

    ArrayList pathNodes = new ArrayList();

    public float rotationMultiplier;
    public float baseSpeed;
    private float currentSpeed;
    public float speedVarianceMultiplier;
    public float rotateSpeed;

    float timer;
    int currentNode;
    Vector3 startPosition;
    Vector3 targetPosition;

    public GameObject tableRightBorderGO;
    public GameObject tableLeftBorderGO;
    public GameObject tableUpBorderGO;
    public GameObject tableDownBorderGO;

    private float tableRightBorderX;
    private float tableLeftBorderX;
    private float tableUpBorderZ;
    private float tableDownBorderZ;

    public bool randomMode;

	// Use this for initialization
	void Start () {

        //Assign node gameobjects to the array so we don't have to do it all through the editor 
        foreach (Transform child in nodeHolder.transform) {
            pathNodes.Add(child.gameObject);
        }

        tableRightBorderX = tableRightBorderGO.transform.position.x;
        tableLeftBorderX = tableLeftBorderGO.transform.position.x;
        tableUpBorderZ = tableUpBorderGO.transform.position.z;
        tableDownBorderZ = tableDownBorderGO.transform.position.z;

        //If we're not in random mode, we want to keep speed consistent, and use nodes!
        if (!randomMode) {
            currentSpeed = baseSpeed;
            setTargetPositionToNode();
            currentNode = 0;
        } else {
            setRandomPosition();
        }
    }

    //Gets the next node's position and saves it
    void setTargetPositionToNode() {
        timer = 0;
        targetPosition = ((GameObject)pathNodes[currentNode]).transform.position;
        startPosition = transform.position;
   }

    void setRandomPosition() {
        timer = 0;

        float x = Random.Range(tableLeftBorderX, tableRightBorderX);
        float z = Random.Range(tableUpBorderZ, tableDownBorderZ);

        startPosition = transform.position;

        //currentSpeed = Random.Range(baseSpeed + baseSpeed * speedVarianceMultiplier, baseSpeed - baseSpeed * speedVarianceMultiplier);
        currentSpeed = baseSpeed; //Need to take into account distance between vector3s TODO

        Debug.Log("Returning new random target position");
        targetPosition = new Vector3(x, gameObject.transform.position.y, z);

        //float distance = 
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime * currentSpeed;

        if (transform.position != targetPosition) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer);
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, (targetPosition - transform.position), (rotateSpeed * Time.deltaTime * rotationMultiplier), 0.0f);
            transform.rotation = Quaternion.LookRotation(newRotation);
        } 
        else if (!randomMode) {
            //If we're not at the last node
            if (currentNode < pathNodes.Count - 1) {
                currentNode++;
                setTargetPositionToNode();
            }
            else {
                currentNode = 0;
                setTargetPositionToNode();
            }
        }
        else {
            setRandomPosition();
        }

	}

    /*(
    // Update is called once per frame
    void Update() {

        Vector3 targetPosition;

        if (randomMode) {
            targetPosition = selectRandomPosition();
        }
        else {
            targetPosition = nodePosition;
        }

        if (!randomMode) {
            timer += Time.deltaTime * speed;
            if (transform.position != nodePosition) {
                transform.position = Vector3.Lerp(startPosition, nodePosition, timer);
                Vector3 newRotation = Vector3.RotateTowards(transform.forward, (nodePosition - transform.position), (rotateSpeed * Time.deltaTime * rotationMultiplier), 0.0f);
                transform.rotation = Quaternion.LookRotation(newRotation);
            }
            else if (currentNode < pathNodes.Count - 1) {
                currentNode++;
                setTargetPositionToNode();
            }
            else {
                currentNode = 0;
                setTargetPositionToNode();
            }
        }
        else {

        }

    } */
}
