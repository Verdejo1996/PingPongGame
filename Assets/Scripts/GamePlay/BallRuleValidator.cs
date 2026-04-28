using UnityEngine;

public class BallRuleValidator : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private Game_Controller controller;
    [SerializeField] private Rigidbody rb;

    [Header("Estado de mesa")]
    public bool bounceTable = false;
    public bool hasTouchedTable = false;
    [SerializeField] private bool hitNetLast = false;
    [SerializeField] private string lastHitterAfterTable = "";
    [SerializeField] private bool validServe = false;
    public bool tableAfterNet = false;

    private int playerBounceCount = 0;
    private int botBounceCount = 0;
    private string lastTableSide = "";

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ResetState();
    }

    public void HandleCollision(Collision collision)
    {
        HandleTableBounce(collision);
        HandleWall(collision);
        HandleNet(collision);
        HandleTableAfterNet(collision);
    }

    private void HandleTableBounce(Collision collision)
    {
        if (collision.gameObject.CompareTag("tablePlayer") ||
            collision.gameObject.CompareTag("RockCourtPlayer"))
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
            }
        }

        if (collision.gameObject.CompareTag("tableBot") ||
            collision.gameObject.CompareTag("RockCourtBot"))
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
            }
        }
    }

    private void HandleWall(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wall"))
            return;

        controller.playing = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

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

    private void HandleNet(Collision collision)
    {
        if (collision.gameObject.CompareTag("Net"))
        {
            hitNetLast = true;
        }
    }

    private void HandleTableAfterNet(Collision collision)
    {
        if (!hitNetLast)
            return;

        if ((collision.gameObject.CompareTag("tableBot") ||
             collision.gameObject.CompareTag("RockCourtBot"))
            && controller.lastHitter == "Player")
        {
            tableAfterNet = true;
        }
        else if ((collision.gameObject.CompareTag("tablePlayer") ||
                  collision.gameObject.CompareTag("RockCourtPlayer"))
                 && controller.lastHitter == "Bot")
        {
            tableAfterNet = true;
        }
    }

    public void HandleTrigger(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            hasTouchedTable = true;
            AudioManager.Instance.PlayHitBallOnTable();
        }
    }

    //Metodo para validar las distintas opciones que hay para sumar puntos.
    private void ScoreValidation()
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

    private void EndPointToLastHitter(string reason)
    {
        controller.playing = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Debug.Log(reason);

        if (!controller.endGame)
        {
            controller.AddPointToLastHitter();
            ResetState();
        }
    }

    public void RegisterHit(string hitterTag)
    {
        if (lastTableSide == "")
        {
            controller.UpdateLastHitter(hitterTag);
            lastHitterAfterTable = hitterTag;
            hitNetLast = false;
            return;
        }

        if (hitterTag == "Player" && controller.lastHitter == "Bot" && lastTableSide != "Player")
        {
            EndPointToLastHitter("Punto para Bot: Player golpeó de volea");
            return;
        }

        if (hitterTag == "Bot" && controller.lastHitter == "Player" && lastTableSide != "Bot")
        {
            EndPointToLastHitter("Punto para Player: Bot golpeó de volea");
            return;
        }

        controller.UpdateLastHitter(hitterTag);
        lastHitterAfterTable = hitterTag;
        hitNetLast = false;
    }

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
