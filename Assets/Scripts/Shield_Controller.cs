using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Controller : MonoBehaviour
{
    Shot_Controller shot_Controller;
    Shot currentShot;
    public Transform aimTarget;

    private void Start()
    {
        shot_Controller = GetComponent<Shot_Controller>();
        currentShot = shot_Controller.topSpin;
        GameObject aim = GameObject.Find("aimTarget");
        if(aim != null)
        {
            aimTarget = aim.transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //aimTarget = gameObject.GetComponent<Transform>().Find("aimTarget");
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            Ball ball = other.gameObject.GetComponent<Ball>();
            ball.ChangeColorTrail(currentShot.hitForce);
            ball.hasTouchedTable = false;
            ball.tableAfterNet = false;
            ball.RegisterHit("Player");
            Destroy(gameObject);
        }
    }
}
