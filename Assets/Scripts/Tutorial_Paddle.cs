using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Paddle : MonoBehaviour
{
    public float speed = 3f;

    public Player_Controller player;

    public float jumpHeight = 2f; // Altura del salto
    public float jumpDuration = 0.5f; // Duración del salto
    private bool isJumping = false;

    Vector3 originalPos;

    public Transform aimTarget;
    public Transform serveTarget;
    public Transform ball;
    public Ball_Tutorial ballGameObject;
    [SerializeField]
    bool hitting;
    [SerializeField]
    bool serving;

    Shot_Controller shot_Controller;
    Shot currentShot;

    void Start()
    {
        originalPos = aimTarget.position;
        shot_Controller = GetComponent<Shot_Controller>();
        //currentShot = shot_Controller.topSpin;
        hitting = false;
        serving = false;
    }

    void Update()
    {

        float h = Input.GetAxisRaw("Horizontal") * speed;
        float v = Input.GetAxisRaw("Vertical") * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            hitting = true;
            currentShot = shot_Controller.topSpin;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            hitting = false;
            aimTarget.position = originalPos;
        }

        if (Input.GetKeyDown(KeyCode.Z) && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            hitting = true;
            currentShot = shot_Controller.flat;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            hitting = false;
            aimTarget.position = originalPos;
        }

        if (Input.GetKeyDown(KeyCode.F) && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            serving = true;
            currentShot = shot_Controller.flatServe;
        }
        else if (Input.GetKeyUp(KeyCode.F) && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            Tutorial.instance.isPaused = false;
            serving = false;
            Serve();
        }

        if (Input.GetKeyDown(KeyCode.G) && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            serving = true;
            currentShot = shot_Controller.kickServe;
        }
        else if (Input.GetKeyUp(KeyCode.G) && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            Serve();

        }

        if (hitting /*&& controller.playing*/)
        {
            //Movemos el target de tiro pero no podemos movernos nosotros.
            if (player.precisionActive)
            {
                Vector3 aimNewPosition = aimTarget.position + speed * 2 * Time.deltaTime * new Vector3(h, 0, v);

                aimNewPosition.x = Mathf.Clamp(aimNewPosition.x, -4f, 4f);
                aimNewPosition.z = Mathf.Clamp(aimNewPosition.z, 1f, 4.2f);

                aimTarget.position = aimNewPosition;
            }
            else
            {
                Vector3 aimNewPosition = aimTarget.position + speed * 2 * Time.deltaTime * new Vector3(h, 0, v);

                aimNewPosition.x = Mathf.Clamp(aimNewPosition.x, -5f, 5f);
                aimNewPosition.z = Mathf.Clamp(aimNewPosition.z, 1f, 5f);

                aimTarget.position = aimNewPosition;
            }
        }   
        else if (serving)
        {
            //Movemos el target de servicio pero no podemos movernos nosotros.
            Vector3 aimNewPosition = serveTarget.position + speed * 2 * Time.deltaTime * new Vector3(h, 0, v);

            aimNewPosition.x = Mathf.Clamp(aimNewPosition.x, -5f, 5f);
            aimNewPosition.z = Mathf.Clamp(aimNewPosition.z, -4f, -2f);

            serveTarget.position = aimNewPosition;
        }
        // Movimiento del jugador si NO está golpeando ni sirviendo
        else if (!hitting && !serving)
        {
            Vector3 newPosition = transform.position + speed * Time.deltaTime * new Vector3(h, 0, v);
            newPosition.x = Mathf.Clamp(newPosition.x, -6f, 6f);
            newPosition.z = Mathf.Clamp(newPosition.z, -8f, -1f);
            newPosition.y = Mathf.Clamp(newPosition.y, -1f, 5f);
            transform.position = newPosition;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // Presiona espacio para saltar
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        isJumping = true;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * jumpHeight;

        // Subir
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = new Vector3(startPosition.x, Mathf.Lerp(startPosition.y, targetPosition.y, (elapsedTime / (jumpDuration / 2))), startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Bajar
        elapsedTime = 0f;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = new Vector3(startPosition.x, Mathf.Lerp(targetPosition.y, startPosition.y, (elapsedTime / (jumpDuration / 2))), startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z); // Asegurar que vuelva exacto
        isJumping = false;
    }

    void Serve()
    {
        serving = false;
        //controller.playing = true;
        ballGameObject.GetComponent<Rigidbody>().useGravity = true;

        //Posicionamos la pelota por encima del jugador, luego le damos la direccion al target y le aplicamos velocidad.
        ball.transform.position = transform.position + new Vector3(0.2f, 1, 0);
        Vector3 dir = serveTarget.position - transform.position;
        ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

        }
        if (other.CompareTag("Ball") && Tutorial.instance.currentPhase == TutorialPhase.Completed)
        {
            Vector3 dir = aimTarget.position - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);

            Ball_Tutorial ball = other.gameObject.GetComponent<Ball_Tutorial>();
            ball.hasTouchedTable = false;
            ball.tableAfterNet = false;
            ball.RegisterHit("Player");
        }
    }
}
