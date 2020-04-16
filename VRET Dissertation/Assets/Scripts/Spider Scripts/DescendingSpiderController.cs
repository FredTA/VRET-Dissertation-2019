using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendingSpiderController : MonoBehaviour {

    private Animator animator;
    private const float ANIMATOR_SPEED = 0.1f;
    private float rotationalSpeed;
    private const float MAX_ROTATIONAL_SPEED = 7.5f;
    private const float ROTATIONAL_ACCELERATION = 0.175f;
    private const float ROTATION_TARGET_CHANGE_TIME = 5f;
    private float rotationSpeedTarget = 0;
    private float timer;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = ANIMATOR_SPEED;
        timer = Time.time - ROTATION_TARGET_CHANGE_TIME; //So we start changing immediately
	}
	
	// Update is called once per frame
	void Update () {
        handleDescent();
    }

    private void handleDescent() {
        //Move spider down
        if (Time.time > timer + ROTATION_TARGET_CHANGE_TIME) {
            timer = Time.time;
            rotationSpeedTarget = Random.Range(-MAX_ROTATIONAL_SPEED, MAX_ROTATIONAL_SPEED);
            Debug.Log("CUR: + " + rotationalSpeed + " ROTSPEEDTARGET " + rotationSpeedTarget);
            rotationalSpeed = (rotationalSpeed + rotationSpeedTarget) / 2; //Helps us get back to zero a little faster
            animator.speed = ANIMATOR_SPEED;
        } else {
            animator.speed = 0;
        }

        //Rotate spider a little, as if it's spinning with the wind
        if (rotationalSpeed > -rotationSpeedTarget && rotationalSpeed < rotationSpeedTarget) {
            if (rotationalSpeed < rotationSpeedTarget) {
                rotationalSpeed += ROTATIONAL_ACCELERATION;
            }
            else {
                rotationalSpeed -= ROTATIONAL_ACCELERATION;
            }
        }
        transform.localRotation *= Quaternion.AngleAxis(rotationalSpeed * Time.deltaTime, Vector3.forward);
    }
}
