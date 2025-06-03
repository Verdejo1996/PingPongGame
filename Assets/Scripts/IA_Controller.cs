using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IA_Controller : MonoBehaviour
{
    [Header("Gameplay")]
    public Game_Controller controller;
    public Transform ball;
    public Rigidbody ballRb;
    public Ball ballGameObject;
    public float speed;

    [Header("Vectores")]
    Vector3 targetPosition;
    Vector3 initialPos;

    [Header("Golpe")]
    public Transform aimTarget;
    public Transform[] targets;
    public Transform[] serveTargets;
    public float anticipationDelay = 0.2f; // tiempo que tarda en reaccionar
    public float reactionTimer = 0f;
    private bool anticipatingShot;

    Shot_Controller shot_controller;

    void Start()
    {
        shot_controller = GetComponent<Shot_Controller>();
        initialPos = transform.position;
        targetPosition = initialPos;
    }

    void Update()
    {
        Move();

    }

    //Realiza el movimiento siguiendo la trayectoria de la pelota.
    void Move()
    {
        /*        if(controller.playing)
                {
                    targetposition.x = ball.position.x;
                    transform.position = Vector3.MoveTowards(transform.position, targetposition, speed * Time.deltaTime);
                }
                else
                {
                    transform.position = inicialPos;    
                }*/

/*        if (controller.playing)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPos, speed * Time.deltaTime);
            anticipatingShot = false;
            float Velocity = ball.GetComponent<Rigidbody>().velocity.z;
            Debug.Log("Z Velocity: " + Velocity);
            return;
        }*/

        float zVelocity = ball.GetComponent<Rigidbody>().velocity.z;

        // Solo reacciona si la pelota va hacia la IA
        if (zVelocity > 0f && controller.playing) // pelota viene hacia IA
        {
            Vector3 target = transform.position;

            if (!anticipatingShot)
            {
                reactionTimer = anticipationDelay;
                anticipatingShot = true;
            }

            if (reactionTimer > 0)
            {
                reactionTimer -= Time.deltaTime;
                return; // esperamos un poco antes de movernos
            }

            if (ballGameObject.hasTouchedTable)
            {
                target.x = ball.position.x;
            }
            else
            {
                /*                // Verificamos si la pelota va a caer dentro del campo
                                float predictedX = ball.position.x;
                                float predictedZ = ball.position.z;
                                if (predictedX < -7f || predictedX > 7f || predictedZ > 5.2)
                                {
                                    // Pelota va fuera del ancho y largo de la mesa
                                    return;
                                }*/
                // Predecimos donde va a caer
                float timeToReach = (7f - ball.position.z) / ballRb.velocity.z;
                //Debug.Log($"PredictedX  {timeToReach}");
                float predictedX = ball.position.x + ballRb.velocity.x * timeToReach;
                //Debug.Log($"PredictedX  {predictedX}");
                // Si va a caer dentro del campo, seguirla
                if (predictedX > -7.5f && predictedX < 7.5f)
                {
                    target.x = predictedX;
                }
                else
                {
                    return; // No la sigue si va fuera y aún no picó
                }

            }


            // Me muevo hacia la posición x de la pelota
            //targetPosition = new Vector3(predictedX, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            // Si la pelota no viene hacia la IA, volver al centro
            transform.position = Vector3.MoveTowards(transform.position, initialPos, speed * Time.deltaTime);
            anticipatingShot = false;
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
