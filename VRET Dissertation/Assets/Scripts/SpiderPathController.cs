using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPathController : MonoBehaviour {
    public GameObject nodeHolder;
    ArrayList pathNodes = new ArrayList();
    public float speed;
    public float rotateSpeed;
    float timer;
    static Vector3 nodePosition;
    int currentNode;
    Vector3 startPosition;

	// Use this for initialization
	void Start () {
        foreach (Transform child in nodeHolder.transform) {
            pathNodes.Add(child.gameObject);
        }
        //pathNode = nodeHolder.chil
        CheckNode();
        currentNode = 0;
	}

    //Gets the next node's position and saves it
    void CheckNode() {
        timer = 0;
        nodePosition = ((GameObject)pathNodes[currentNode]).transform.position;
        startPosition = transform.position;

   }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * speed;
        if (transform.position != nodePosition) {
            transform.position = Vector3.Lerp(startPosition, nodePosition, timer);
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, (nodePosition - transform.position), (rotateSpeed * Time.deltaTime), 0.0f);
            transform.rotation = Quaternion.LookRotation(newRotation);
        }
        else if (currentNode < pathNodes.Count - 1) {
            currentNode++;
            CheckNode();
        }
        else {
            currentNode = 0;
            CheckNode();
        }
	}
}
