using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [Header("Gameplay")]
    private Rigidbody rb;
    public Game_Controller controller;

    [Header("Banderas")]
    public bool bounceTable = false;
    public bool hasTouchedTable = false;
    [SerializeField] private bool hitNetLast = false;
    private string lastHitterAfterTable = "";
    [SerializeField] bool validServe = false;
    public bool tableAfterNet = false;
    public bool isCurveShotActive = false;

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer trailBall;
    [SerializeField] private Color colorSoft = Color.blue;
    [SerializeField] private Color colorStrong = Color.red;
    [SerializeField] private Camera_Shake cameraShake;

    private float duration = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ResetState();
    }

    public void ChangeColorTrail(float force)
    {
        if(force < 10)
        {
            trailBall.material.color = colorSoft;
        }
        else
        {
            trailBall.material.color = colorStrong;
            StartCoroutine(cameraShake.Shake(0.2f, 0.1f));
        }
    }

    public IEnumerator CurveShot(float force)
    {
        if(isCurveShotActive)
        {
            rb.AddForce(Vector3.right * force, ForceMode.Impulse);
            yield return new WaitForSeconds(duration);
            isCurveShotActive = false;
        }
    }

    public void SetServePosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero; // Detener la pelota antes del saque
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            controller.playing = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Debug.Log(hasTouchedTable);
            Debug.Log(hitNetLast);
            Debug.Log(validServe);
            Debug.Log("Golpe por " + lastHitterAfterTable);
            if (!controller.endGame)
            {
                ScoreValidation();
                ResetState();
            }
        }
        if (collision.gameObject.CompareTag("tableBot") && controller.currentServer == "Player")
        {
            validServe = true;
        }
        if (collision.gameObject.CompareTag("tablePlayer") && controller.currentServer == "Bot")
        {
            validServe = true;
        }
        if (collision.gameObject.CompareTag("Net"))
        {
            hitNetLast = true;

        }
        if(hitNetLast)
        {
            if (collision.gameObject.CompareTag("tableBot") && controller.lastHitter == "Player")
            {
                tableAfterNet = true;
            }
            else if(collision.gameObject.CompareTag("tablePlayer") && controller.lastHitter == "Bot")
            {
                tableAfterNet = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            hasTouchedTable = true;
        }
/*        if(other.CompareTag("Out"))
        {
            controller.playing = false;

            Debug.Log(hasTouchedTable);
            Debug.Log(hitNetLast);
            Debug.Log(validServe);
            Debug.Log("Golpe por " + lastHitterAfterTable);
            if (!controller.endGame)
            {
                ScoreValidation();  
                ResetState();
            }
        }*/
    }

    //Metodo para validar las distintas opciones que hay para sumar puntos.
    void ScoreValidation()
    {
        if (!validServe)
        {
            Debug.Log("Punto para el oponente: Servicio inválido");
            controller.AddPointToOpponent();
        }
        else
        {
            if (!hitNetLast)
            {
                if (hasTouchedTable && lastHitterAfterTable != "")
                {
                    controller.AddPointToLastHitter();
                }
                else if (!hasTouchedTable && lastHitterAfterTable != "")
                {
                    controller.AddPointToOpponent();
                }
            }
            else
            {
                if (lastHitterAfterTable != "" && !tableAfterNet)
                {
                    controller.AddPointToOpponent();
                }
                else if(lastHitterAfterTable != "" && tableAfterNet)
                {
                    controller.AddPointToLastHitter();
                }
            }
        }
    }

    //Registramos al ultimo en golpear la pelota.
    public void RegisterHit(string hitterTag)
    {
        controller.UpdateLastHitter(hitterTag);

        if (hasTouchedTable || !hasTouchedTable)
        {
            lastHitterAfterTable = hitterTag;
        }

        hitNetLast = false; // Si un jugador la golpea después de la red, ya no cuenta como fallo
    }

    //Reseteamos el estado de la pelota.
    public void ResetState()
    {
        hasTouchedTable = false;
        hitNetLast = false;
        lastHitterAfterTable = "";
        validServe = false;
        tableAfterNet = false;
    }
}
