using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {
    public Animator spider;
    private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
        spider.SetBool("running", true);
        spider.SetBool("idle", false);
        spider.SetBool("walking", false);
        spider.SetBool("turnright", false);
        spider.SetBool("turnleft", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            spider.SetBool("idle", true);
            spider.SetBool("running", false);
            spider.SetBool("walking", false);
            spider.SetBool("attack", false);
            spider.SetBool("jumping", false);
        }
        if (Input.GetKey("up"))
        {
            spider.SetBool("running", true);
            spider.SetBool("idle", false);
            spider.SetBool("walking", false);
            spider.SetBool("turnright", false);
            spider.SetBool("turnleft", false);
        }
        if (Input.GetKey("down"))
        {
            spider.SetBool("running", false);
            spider.SetBool("walking", true);
            spider.SetBool("idle", false);
            spider.SetBool("turnleft", false);
            spider.SetBool("turnright", false);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            spider.SetBool("attack", true);
            spider.SetBool("walking", false);
            spider.SetBool("idle", false);
            spider.SetBool("running", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            spider.SetBool("attack2", true);
            spider.SetBool("attack", false);
            spider.SetBool("idle", false);
            spider.SetBool("running", false);
            StartCoroutine("idle2");
            idle2();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            spider.SetBool("idle", false);
            spider.SetBool("jumping", true);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            spider.SetBool("idle", false);
            spider.SetBool("hited", true);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            spider.SetBool("idle", false);
            spider.SetBool("died", true);
        }
        if (Input.GetKey("left"))
        {
            spider.SetBool("turnleft", true);
            spider.SetBool("walking", false);
            spider.SetBool("turnright", false);
            spider.SetBool("idle", false);
            spider.SetBool("running", false);
            StartCoroutine("idle2");
            idle2();
        }
        if (Input.GetKey("right"))
        {
            spider.SetBool("turnright", true);
            spider.SetBool("walking", false);
            spider.SetBool("turnleft", false);
            spider.SetBool("idle", false);
            spider.SetBool("running", false);
            StartCoroutine("idle2");
            idle2();
        }
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.35f);
        spider.SetBool("attack", false);
        spider.SetBool("attack2", false);
        spider.SetBool("idle", true);
        spider.SetBool("hited", false);
    }
    IEnumerator idle2()
    {
        yield return new WaitForSeconds(1.0f);
        spider.SetBool("attack", false);
        spider.SetBool("attack2", false);
        spider.SetBool("idle", true);
        spider.SetBool("turnleft", false);
        spider.SetBool("turnright", false);
    }
}
