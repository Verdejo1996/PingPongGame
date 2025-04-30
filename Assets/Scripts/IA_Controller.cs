using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Controller : MonoBehaviour
{
    public Transform ball;
    public Transform aimTarget;
    public float speed;
   
    public Game_Controller controller;

    Vector3 targetposition;
    Vector3 inicialPos;

    public Transform[] targets;
    public Transform[] serveTargets;

    Shot_Controller shot_controller;
    public Ball ballGameObject;

    void Start()
    {
        targetposition = transform.position;
        shot_controller = GetComponent<Shot_Controller>();
        inicialPos = transform.position;
    }

    void Update()
    {
        Move();

    }

    //Realiza el movimiento siguiendo la trayectoria de la pelota.
    void Move()
    {
        if(controller.playing)
        {
            targetposition.x = ball.position.x;
            transform.position = Vector3.MoveTowards(transform.position, targetposition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = inicialPos;    
        }
    }

    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    Vector3 PickServeTarget()
    {
        int randomValue = Random.Range(0, serveTargets.Length);
        return serveTargets[randomValue].position;
    }

    Shot PickShot()
    {
        int randomValue = Random.Range(0, 2);
        if(randomValue == 0)
        {          
            return shot_controller.topSpin;
        }
        else
        {

            return shot_controller.flat;
        }
    }

    public void Serve()
    {
        if (controller.currentServer == "Bot" && !controller.playing)
        {
            controller.playing = true;

            Shot currentServe = PickServe();

            Vector3 dir = PickServeTarget() - transform.position;
            ballGameObject.GetComponent<Rigidbody>().useGravity = true;
            ballGameObject.GetComponent<Rigidbody>().velocity = dir.normalized * currentServe.hitForce + new Vector3(0, currentServe.upForce, 0);
            ballGameObject.RegisterHit("Bot");
        }
    }

    Shot PickServe()
    {
        int randonValue = Random.Range(0, 2);
        if(randonValue == 0)
        {
            return shot_controller.flatServe;
        }
        else
        {
            return shot_controller.kickServe;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && controller.playing)
        {
            Shot currentShot = PickShot();

            Vector3 dir = PickTarget() - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            Ball ball = other.gameObject.GetComponent<Ball>();
            ball.hasTouchedTable = false;
            ball.tableAfterNet = false;
            ball.RegisterHit("Bot");
        }
    }
}
