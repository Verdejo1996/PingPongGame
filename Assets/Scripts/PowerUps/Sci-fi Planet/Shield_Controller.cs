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
            BallRuleValidator ballRules = other.gameObject.GetComponent<BallRuleValidator>();
            BallVisualEffects ball = other.gameObject.GetComponent<BallVisualEffects>();
            ball.ChangeColorTrail(currentShot.hitForce);
            ballRules.hasTouchedTable = false;
            ballRules.tableAfterNet = false;
            ballRules.RegisterHit("Player");
            Destroy(gameObject);
        }
    }
}
