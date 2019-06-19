using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour {

    public GameObject spider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //GameObject can't deactivate itself, needs to be in a seperate script
        if (Input.GetKeyDown(KeyCode.Space)) {
            spider.SetActive(!spider.activeSelf);
        }
    }
}
