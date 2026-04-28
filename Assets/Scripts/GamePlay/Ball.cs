using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private BallRuleValidator ruleValidator;
    private Rigidbody rb;
    public Game_Controller controller;
    [SerializeField] private BallPowerEffects powerEffects;

    private void Awake()
    {
        if (powerEffects == null)
            powerEffects = GetComponent<BallPowerEffects>();
        if (ruleValidator == null)
            ruleValidator = GetComponent<BallRuleValidator>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void SetServePosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero; // Detener la pelota antes del saque
    }

    void OnCollisionEnter(Collision collision)
    {
        ruleValidator.HandleCollision(collision);
        //#region RockPlanet

        //if (collision.gameObject.CompareTag("RockCourtPlayer"))
        //{
        //    hasTouchedTable = true;
        //    lastTableSide = "Player";
        //    playerBounceCount++;
        //    botBounceCount = 0;

        //    if (controller.currentServer == "Bot")
        //        validServe = true;

        //    if (playerBounceCount >= 3)
        //    {
        //        EndPointToLastHitter("Punto para el último golpeador: 3 piques en campo Player");
        //        return;
        //    }
        //}

        //if (collision.gameObject.CompareTag("RockCourtBot"))
        //{
        //    hasTouchedTable = true;
        //    lastTableSide = "Bot";
        //    botBounceCount++;
        //    playerBounceCount = 0;

        //    if (controller.currentServer == "Player")
        //        validServe = true;

        //    if (botBounceCount >= 3)
        //    {
        //        EndPointToLastHitter("Punto para el último golpeador: 3 piques en campo Bot");
        //        return;
        //    }
        //}

        //#endregion

        //if (collision.gameObject.CompareTag("tablePlayer"))
        //{
        //    hasTouchedTable = true;
        //    lastTableSide = "Player";
        //    playerBounceCount++;
        //    botBounceCount = 0;

        //    if (controller.currentServer == "Bot")
        //        validServe = true;

        //    if (playerBounceCount >= 3)
        //    {
        //        EndPointToLastHitter("Punto para el último golpeador: 3 piques en tablePlayer");
        //        return;
        //    }
        //}

        //if (collision.gameObject.CompareTag("tableBot"))
        //{
        //    hasTouchedTable = true;
        //    lastTableSide = "Bot";
        //    botBounceCount++;
        //    playerBounceCount = 0;

        //    if (controller.currentServer == "Player")
        //        validServe = true;

        //    if (botBounceCount >= 3)
        //    {
        //        EndPointToLastHitter("Punto para el último golpeador: 3 piques en tableBot");
        //        return;
        //    }
        //}

        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    controller.playing = false;
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //    Debug.Log(hasTouchedTable);
        //    Debug.Log(hitNetLast);
        //    Debug.Log(validServe);
        //    Debug.Log("Golpe por " + lastHitterAfterTable);

        //    if (!controller.endGame)
        //    {
        //        ScoreValidation();
        //        ResetState();
        //    }
        //}

        //if (collision.gameObject.CompareTag("Net"))
        //{
        //    hitNetLast = true;
        //}

        //if (hitNetLast)
        //{
        //    if ((collision.gameObject.CompareTag("tableBot") || collision.gameObject.CompareTag("RockCourtBot"))
        //        && controller.lastHitter == "Player")
        //    {
        //        tableAfterNet = true;
        //    }
        //    else if ((collision.gameObject.CompareTag("tablePlayer") || collision.gameObject.CompareTag("RockCourtPlayer"))
        //        && controller.lastHitter == "Bot")
        //    {
        //        tableAfterNet = true;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        ruleValidator.HandleTrigger(other);

        powerEffects.TryApplyFireExplosion(other);
    }
}
