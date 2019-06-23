using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour {

    private bool activated = false;
    private bool deactivating = false;

    public GameObject spider;
    private Vector3 initialSpiderPoisition;
    private Quaternion initialSpiderOrientation;

	// Use this for initialization
	void Start () {
        initialSpiderPoisition = spider.transform.position;
        initialSpiderOrientation = spider.transform.rotation;
		//Set text to spider instructions
	}

    // Update is called once per frame
    void Update() {
        if (activated) {
            if (deactivating) {
                activated = false;
                deactivating = false;
                //Reset spider position
                spider.transform.position = initialSpiderPoisition;
                spider.transform.rotation = initialSpiderOrientation;
                spider.SetActive(false);
                Debug.Log("Spider controller deactivated");
            }
            else if (Input.GetKeyDown(KeyCode.Space)) {
                spider.SetActive(!spider.activeSelf);
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
