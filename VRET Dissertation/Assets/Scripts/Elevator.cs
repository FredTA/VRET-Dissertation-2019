using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool activated = false;
    private bool deactivating = false;

    public float minimumHeight;
    private float initialHeight;

    // Start is called before the first frame update
    void Start()
    {
        initialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        if (activated) {
            if (deactivating) {
                if (transform.position.y < initialHeight) {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.002f, transform.position.z);
                }
                else {
                    deactivating = false;
                    activated = false;
                    //Set text 
                }
            }
            //If key down translate GameObject down
            else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > minimumHeight) {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.002f, transform.position.z);
                //transform.position = new Vector3(0, -10, 0);
                //gameObject.SetActive(false);
                Debug.Log("Moving floor down");
            }
            else if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < initialHeight) {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.002f, transform.position.z);
                //transform.position = new Vector3(0, -10, 0);
                //gameObject.SetActive(false);
                Debug.Log("Moving floor up");
            }
        }
    }

    public bool controllerIsActive() {
        return activated;
    }

    public void activate() {
        activated = true;
    }

    public void deactivate() {
        deactivating = true;
    }

}
