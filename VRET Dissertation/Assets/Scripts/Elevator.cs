using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    private float initialHeight;

    // Start is called before the first frame update
    void Start()
    {
        initialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
                 //If key down translate GameObject down
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.002f, transform.position.z);
            //transform.position = new Vector3(0, -10, 0);
            //gameObject.SetActive(false);
            Debug.Log("Moving floor down");
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < initialHeight) {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.002f, transform.position.z);
            //transform.position = new Vector3(0, -10, 0);
            //gameObject.SetActive(false);
            Debug.Log("Moving floor down");
        }
    }

}
