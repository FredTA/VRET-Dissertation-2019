using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed;
    public float turnSpeed; 
    private Vector3 originalTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Home)) {
            Debug.Log("Debug key active");
            if (Input.GetKey(KeyCode.A)) {
                //Move left
                Debug.Log("DEBUG - moving left");
                transform.Translate(-moveSpeed, 0, 0, Space.World);
            }
            else if (Input.GetKey(KeyCode.D)) {
                //Move right
                Debug.Log("DEBUG - moving right");
                transform.Translate(moveSpeed, 0, 0, Space.World);
            }
            else if (Input.GetKey(KeyCode.W)) {
                //Move backward
                Debug.Log("DEBUG - moving forward");
                transform.Translate(0, 0, moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.S)) {
                //Move forward
                Debug.Log("DEBUG - moving backward");
                transform.Translate(0, 0, -moveSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.R)) {
                //Move up
                Debug.Log("DEBUG - moving Up");
                transform.Translate(0, moveSpeed, 0, Space.World);
            }
            else if (Input.GetKey(KeyCode.F)) {
                //Move down
                Debug.Log("DEBUG - moving Down");
                transform.Translate(0, -moveSpeed, 0, Space.World);
            }
            else if (Input.GetKey(KeyCode.E)) {
                //turn right
                Debug.Log("DEBUG - Turning left");
                transform.Rotate(0, turnSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.Q)) {
                //Move left
                Debug.Log("DEBUG - Turning right");
                transform.Rotate(0, -turnSpeed, 0);
            }
            /**
            else if (Input.GetKey(KeyCode.R)) {
                //Reset
                Debug.Log("DEBUG - reset position");
                transform.position = originalTransform;
            }
            **/
        }
	}
}
