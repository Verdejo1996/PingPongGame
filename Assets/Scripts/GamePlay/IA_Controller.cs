using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public float slipperyFactor = 3f;


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
    public bool isSlippery;
    private Vector3 velocity = Vector3.zero; // Velocidad interna usada por SmoothDamp
    public float smoothTime = 0.2f; // Tiempo en el que debería alcanzar la posición objetivo

    private bool disoriented = false;
    private Coroutine disorientCoroutine;
    private Vector3 originalPosition;
    private bool isActive = false;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = speed;
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
        // Solo reacciona si la pelota va hacia la IA
        if (controller.lastHitter == "Player" && controller.playing) // pelota viene hacia IA
        {
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

            targetPosition.x = ball.position.x;

            if (isSlippery)
            {
                float currentSmoothTime = smoothTime * slipperyFactor;
                Vector3 target = new(targetPosition.x, transform.position.y, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, currentSmoothTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
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
            if (!ballGameObject.isHeavy)
            {
                other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            }
            else
            {
                other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0) * 0.7f; ;
            }

            Ball ball = other.gameObject.GetComponent<Ball>();
            ball.hasTouchedTable = false;
            ball.tableAfterNet = false;
            ball.RegisterHit("Bot");
        }
    }

    internal void ApplyDisorientation(float v)
    {
        if (disoriented) return;

        disoriented = true;
        originalPosition = transform.position;
        disorientCoroutine = StartCoroutine(Disorient(v));
    }

    private IEnumerator Disorient(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Shake leve en X y Y
            float shakeAmount = 0.1f;
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                0f,
                Random.Range(-shakeAmount, shakeAmount)
            );

            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restaurar posición y velocidad
        transform.position = originalPosition;
        disoriented = false;
    }

    internal void ApplySlow(float v)
    {
        if (isActive)
        {
            return;
        }
        isActive = true;
        speed *= 0.5f;

        StartCoroutine(SlowLavaAreaEffect(v));
    }

    private IEnumerator SlowLavaAreaEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        isActive = false;
        speed = originalSpeed;
    }
}
