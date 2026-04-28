using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [Header("Gameplay")]
    private Rigidbody rb;
    public Game_Controller controller;
    [SerializeField] private BallPowerEffects powerEffects;
    [SerializeField] private GameObject lavaAreaPrefab;
    public bool isLavaActive;

    [Header("Rock Planet PoolTable")]
    public ObjectPool poolPlayer;
    public ObjectPool poolBot;
    
    private int playerBounceCount = 0;
    private int botBounceCount = 0;
    private string lastTableSide = ""; // "Player" o "Bot"

    [Header("Banderas")]
    public bool bounceTable = false;
    public bool hasTouchedTable = false;
    [SerializeField] private bool hitNetLast = false;
    [SerializeField] private string lastHitterAfterTable = "";
    [SerializeField] bool validServe = false;
    public bool tableAfterNet = false;

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer trailBall;
    [SerializeField] private Color colorSoft = Color.blue;
    [SerializeField] private Color colorStrong = Color.red;
    [SerializeField] private Camera_Shake cameraShake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ResetState();
    }

    public void SetServePosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero; // Detener la pelota antes del saque
    }

    void OnCollisionEnter(Collision collision)
    {
        #region RockPlanet

        if (collision.gameObject.CompareTag("RockCourtPlayer"))
        {
            hasTouchedTable = true;
            lastTableSide = "Player";
            playerBounceCount++;
            botBounceCount = 0;

            if (controller.currentServer == "Bot")
                validServe = true;

            if (playerBounceCount >= 3)
            {
                EndPointToLastHitter("Punto para el último golpeador: 3 piques en campo Player");
                return;
            }
        }

        if (collision.gameObject.CompareTag("RockCourtBot"))
        {
            hasTouchedTable = true;
            lastTableSide = "Bot";
            botBounceCount++;
            playerBounceCount = 0;

            if (controller.currentServer == "Player")
                validServe = true;

            if (botBounceCount >= 3)
            {
                EndPointToLastHitter("Punto para el último golpeador: 3 piques en campo Bot");
                return;
            }
        }

        #endregion

        if (collision.gameObject.CompareTag("tablePlayer"))
        {
            hasTouchedTable = true;
            lastTableSide = "Player";
            playerBounceCount++;
            botBounceCount = 0;

            if (controller.currentServer == "Bot")
                validServe = true;

            if (playerBounceCount >= 3)
            {
                EndPointToLastHitter("Punto para el último golpeador: 3 piques en tablePlayer");
                return;
            }
        }

        if (collision.gameObject.CompareTag("tableBot"))
        {
            hasTouchedTable = true;
            lastTableSide = "Bot";
            botBounceCount++;
            playerBounceCount = 0;

            if (controller.currentServer == "Player")
                validServe = true;

            if (botBounceCount >= 3)
            {
                EndPointToLastHitter("Punto para el último golpeador: 3 piques en tableBot");
                return;
            }
        }

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

        if (collision.gameObject.CompareTag("Net"))
        {
            hitNetLast = true;
        }

        if (hitNetLast)
        {
            if ((collision.gameObject.CompareTag("tableBot") || collision.gameObject.CompareTag("RockCourtBot"))
                && controller.lastHitter == "Player")
            {
                tableAfterNet = true;
            }
            else if ((collision.gameObject.CompareTag("tablePlayer") || collision.gameObject.CompareTag("RockCourtPlayer"))
                && controller.lastHitter == "Bot")
            {
                tableAfterNet = true;
            }
        }

        if (isLavaActive && collision.gameObject.CompareTag("tableBot"))
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(lavaAreaPrefab, contact.point + new Vector3(0, 1, 4), Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            hasTouchedTable = true;
            AudioManager.Instance.PlayHitBallOnTable();
        }

        powerEffects.TryApplyFireExplosion(other);
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
                if (tableAfterNet)
                {
                    controller.AddPointToLastHitter();
                }
                else
                {
                    controller.AddPointToOpponent();
                }
            }
        }
    }

    void EndPointToLastHitter(string reason)
    {
        controller.playing = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Debug.Log(reason);

        if (!controller.endGame)
        {
            controller.AddPointToLastHitter();
            ResetState();
        }
    }

    //Registramos al ultimo en golpear la pelota.
    public void RegisterHit(string hitterTag)
    {
        // Si todavía no hubo pique en mesa, no validar volea
        if (lastTableSide == "")
        {
            controller.UpdateLastHitter(hitterTag);
            lastHitterAfterTable = hitterTag;
            hitNetLast = false;
            return;
        }

        // VOLEA DEL PLAYER
        if (hitterTag == "Player" && controller.lastHitter == "Bot" && lastTableSide != "Player")
        {
            EndPointToLastHitter("Punto para Bot: Player golpeó de volea");
            return;
        }

        // VOLEA DEL BOT
        if (hitterTag == "Bot" && controller.lastHitter == "Player" && lastTableSide != "Bot")
        {
            EndPointToLastHitter("Punto para Player: Bot golpeó de volea");
            return;
        }

        controller.UpdateLastHitter(hitterTag);
        lastHitterAfterTable = hitterTag;
        hitNetLast = false;
    }

    //Reseteamos el estado de la pelota.
    public void ResetState()
    {
        hasTouchedTable = false;
        hitNetLast = false;
        lastHitterAfterTable = "";
        validServe = false;
        tableAfterNet = false;

        playerBounceCount = 0;
        botBounceCount = 0;
        lastTableSide = "";

        controller.ResetLastHitter();
    }
}
