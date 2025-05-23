using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Tutorial : MonoBehaviour
{
    private Rigidbody rb;
    public Tutorial_Paddle player;

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
        if (collision.gameObject.CompareTag("Wall") && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            Tutorial.instance.isPaused = true;
            player.ResetServe();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2.5f, -7)); // Ajusta la posición para el jugador
        }
        if (collision.gameObject.CompareTag("Wall") && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            Tutorial.instance.isPaused = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2f, 7));
        }

        //A partir de esta linea es para el tutorial completado
        #region
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && collision.gameObject.CompareTag("tableBot") && Tutorial_Manager.Instance.currentServer == "Player")
        {
            validServe = true;
        }
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && collision.gameObject.CompareTag("tablePlayer") && Tutorial_Manager.Instance.currentServer == "Bot")
        {
            validServe = true;
        }
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && collision.gameObject.CompareTag("Net"))
        {
            hitNetLast = true;

        }
        if (Tutorial.instance.currentPhase == TutorialPhase.Completed && hitNetLast)
        {
            if (collision.gameObject.CompareTag("tableBot") && Tutorial_Manager.Instance.lastHitter == "Player")
            {
                tableAfterNet = true;
            }
            else if (collision.gameObject.CompareTag("tablePlayer") && Tutorial_Manager.Instance.lastHitter == "Bot")
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
            if (!Tutorial_Manager.Instance.endTutorial)
            {
                ScoreValidation();
                ResetState();
            }
        }
    }

    public void RegisterHit(string hitterTag)
    {
        Tutorial_Manager.Instance.UpdateLastHitter(hitterTag);

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
            Tutorial_Manager.Instance.AddPointToOpponent();
        }
        else
        {
            if (Tutorial.instance.currentPhase == TutorialPhase.Completed && !hitNetLast)
            {
                if (hasTouchedTable && lastHitterAfterTable != "")
                {
                    Tutorial_Manager.Instance.AddPointToLastHitter();
                }
                else if (!hasTouchedTable && lastHitterAfterTable != "")
                {
                    Tutorial_Manager.Instance.AddPointToOpponent();
                }
            }
            else if(Tutorial.instance.currentPhase == TutorialPhase.Completed)
            {
                if (lastHitterAfterTable != "" && !tableAfterNet)
                {
                    Tutorial_Manager.Instance.AddPointToOpponent();
                }
                else if (lastHitterAfterTable != "" && tableAfterNet)
                {
                    Tutorial_Manager.Instance.AddPointToLastHitter();
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
