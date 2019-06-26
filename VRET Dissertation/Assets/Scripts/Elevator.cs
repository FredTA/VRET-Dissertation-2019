using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public AudioSource descendingNoise;
    public AudioSource ascendingNoise;
    public float elevationBaseSpeed;
    public float shiftSpeedMultiplier;

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
                    //The floor should move up fast
                    float newElevation = transform.position.y + (elevationBaseSpeed * shiftSpeedMultiplier);
                    transform.position = new Vector3(transform.position.x, newElevation, transform.position.z);

                    if (!ascendingNoise.isPlaying) {
                        ascendingNoise.Play();
                    }
                }
                else {
                    deactivating = false;
                    activated = false;
                    if (ascendingNoise.isPlaying) {
                        ascendingNoise.Stop();
                    }
                }
            }
            //If key down translate GameObject down
            else if (Input.GetKey(KeyCode.DownArrow)) {
                //If the floor can still move down
                if (transform.position.y > minimumHeight) {
                    //If the player holds shift, the floor should move faster
                    float newElevation;
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                        newElevation = transform.position.y - (elevationBaseSpeed * shiftSpeedMultiplier);
                    }
                    else {
                        newElevation = transform.position.y - elevationBaseSpeed;
                    }

                    transform.position = new Vector3(transform.position.x, newElevation, transform.position.z);

                    if (!descendingNoise.isPlaying) {
                        descendingNoise.Play();
                    }
                    Debug.Log("Moving floor down");
                }
                //If the floor can't move down anymore
                else {
                    if (descendingNoise.isPlaying) {
                        descendingNoise.Stop();
                    }
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow)){
                //If the floor can still mvoe up
                if (transform.position.y < initialHeight) {
                    //If the player holds shift, the floor should move faster
                    float newElevation;
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                        newElevation = transform.position.y + (elevationBaseSpeed * shiftSpeedMultiplier);
                    }
                    else {
                        newElevation = transform.position.y + elevationBaseSpeed;
                    }
                    transform.position = new Vector3(transform.position.x, newElevation, transform.position.z);
                    if (!ascendingNoise.isPlaying) {
                        ascendingNoise.Play();
                    }

                    Debug.Log("Moving floor up");
                }
                //If the floor can't move up anymore
                else {
                    if (ascendingNoise.isPlaying) {
                        ascendingNoise.Stop();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow)) {
                descendingNoise.Stop();
            }
            if (Input.GetKeyUp(KeyCode.UpArrow)) {
                ascendingNoise.Stop();
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
