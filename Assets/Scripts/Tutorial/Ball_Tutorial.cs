using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Tutorial : MonoBehaviour
{
    private Rigidbody rb;
    public Tutorial_Paddle player;
    public Tutorial_Manager managerTutorial;

    [SerializeField]
    private bool hitNetLast = false;
    private string lastHitterAfterTable = "";
    [SerializeField]
    bool validServe = false;
    public bool tableAfterNet = false;
    public bool hasTouchedTable = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetServePosition(Vector3 position)
    {
        transform.position = position;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // Detener la pelota antes del saque
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && Tutorial.instance.currentPhase == TutorialPhase.Serving || Tutorial.instance.currentPhase == TutorialPhase.Completed)
        {
            Tutorial.instance.isPaused = true;
            player.ResetServe();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2.5f, -7)); // Ajusta la posición para el jugador
        }
        if (collision.gameObject.CompareTag("Wall") && Tutorial.instance.currentPhase == TutorialPhase.HitPractice || Tutorial.instance.currentPhase == TutorialPhase.Completed)
        {
            Tutorial.instance.isPaused = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2f, 7));
        }

        //A partir de esta linea es para el tutorial completado
        #region

        if (collision.gameObject.CompareTag("tableBot") && managerTutorial.currentServer == "Player")
        {
            validServe = true;
        }
        if (collision.gameObject.CompareTag("tablePlayer") && managerTutorial.currentServer == "Bot")
        {
            validServe = true;
        }
        if (collision.gameObject.CompareTag("Net"))
        {
            hitNetLast = true;

        }
        if (hitNetLast)
        {
            if (collision.gameObject.CompareTag("tableBot") && managerTutorial.lastHitter == "Player")
            {
                tableAfterNet = true;
            }
            else if (collision.gameObject.CompareTag("tablePlayer") && managerTutorial.lastHitter == "Bot")
            {
                tableAfterNet = true;
            }
        }
        
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && other.CompareTag("Table"))
        {
            hasTouchedTable = true;
        }
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && other.CompareTag("Out"))
        {
            Tutorial.instance.isPaused = true;

            Debug.Log(hasTouchedTable);
            Debug.Log(hitNetLast);
            Debug.Log(validServe);
            Debug.Log("Golpe por " + lastHitterAfterTable);
            if (!managerTutorial.endTutorial)
            {
                ScoreValidation();
                ResetState();
            }
        }
    }

    public void RegisterHit(string hitterTag)
    {
        managerTutorial.UpdateLastHitter(hitterTag);

        if (hasTouchedTable || !hasTouchedTable)
        {
            lastHitterAfterTable = hitterTag;
        }

        hitNetLast = false; // Si un jugador la golpea después de la red, ya no cuenta como fallo
    }

    void ScoreValidation()
    {
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && !validServe)
        {
            Debug.Log("Punto para el oponente: Servicio inválido");
            managerTutorial.AddPointToOpponent();
        }
        else
        {
            if (Tutorial.instance.currentPhase == TutorialPhase.Completed && !hitNetLast)
            {
                if (hasTouchedTable && lastHitterAfterTable != "")
                {
                    managerTutorial.AddPointToLastHitter();
                }
                else if (!hasTouchedTable && lastHitterAfterTable != "")
                {
                    managerTutorial.AddPointToOpponent();
                }
            }
            else if(Tutorial.instance.currentPhase == TutorialPhase.Completed)
            {
                if (lastHitterAfterTable != "" && !tableAfterNet)
                {
                    managerTutorial.AddPointToOpponent();
                }
                else if (lastHitterAfterTable != "" && tableAfterNet)
                {
                    managerTutorial.AddPointToLastHitter();
                }
            }
        }
    }

    public void ResetState()
    {
        hasTouchedTable = false;
        hitNetLast = false;
        lastHitterAfterTable = "";
        validServe = false;
        tableAfterNet = false;
    }
}
