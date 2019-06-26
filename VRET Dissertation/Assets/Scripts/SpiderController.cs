using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour {

    private bool activated = false;
    private bool deactivating = false;
    private Vector3 minimumScale;
    public float maxScaleMultiplier;
    private Vector3 maximumScale;
    public float scaleSpeed;

    public GameObject spider;
    private Vector3 initialSpiderPoisition;
    private Quaternion initialSpiderOrientation;

	// Use this for initialization
	void Start () {
        initialSpiderPoisition = spider.transform.position;
        initialSpiderOrientation = spider.transform.rotation;
        minimumScale = spider.transform.localScale;
        maximumScale = minimumScale * maxScaleMultiplier;
		//Set text to spider instructions
	}

    // Update is called once per frame
    void Update() {
        if (activated) {
            if (deactivating) {
                activated = false;
                deactivating = false;
                //Reset spider position and scale
                spider.transform.position = initialSpiderPoisition;
                spider.transform.rotation = initialSpiderOrientation;
                spider.transform.localScale = minimumScale;
                spider.SetActive(false);
                Debug.Log("Spider controller deactivated");
            }
            else if (Input.GetKeyDown(KeyCode.Space)) {
                spider.SetActive(!spider.activeSelf);
            }
            else if (Input.GetKey(KeyCode.UpArrow) && spider.activeSelf) {
                if (spider.transform.localScale.magnitude < maximumScale.magnitude) {
                    scaleSpider(true);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) && spider.activeSelf) {
                if (spider.transform.localScale.magnitude > minimumScale.magnitude) {
                    scaleSpider(false);
                }
            }
        }
    }

    private void scaleSpider(bool scaleUp) {
        float scaleToAdd;
        if (scaleUp) {
            scaleToAdd = scaleSpeed * Time.deltaTime;
        }
        else {
            scaleToAdd = -scaleSpeed * Time.deltaTime;
        }

        Vector3 newScale =  new Vector3(spider.transform.localScale.x + scaleToAdd, spider.transform.localScale.y + scaleToAdd, spider.transform.localScale.z + scaleToAdd);
        spider.transform.localScale = newScale;
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
