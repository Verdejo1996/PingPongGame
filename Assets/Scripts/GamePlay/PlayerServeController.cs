using UnityEngine;

public class PlayerServeController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Game_Controller controller;
    [SerializeField] private ServeUIFeedback serveUIFeedback;
    [SerializeField] private BallRuleValidator ballGame;
    [SerializeField] private Rigidbody ballRb;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform ballHoldPosition;
    [SerializeField] private Transform serveStartPosition;

    [Header("Servicio")]
    [SerializeField] private float chargeSpeed = 1.0f;
    [SerializeField] private float minServeForce = 7f;
    [SerializeField] private float maxServeForce = 12f;
    [SerializeField] private float idealChargeMin = 0.8f;
    [SerializeField] private float idealChargeMax = 1f;

    [Header("Movimiento visual de pelota")]
    //[SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private float amplitude = 0.1f;

    private float originalY;
    private float serveForce;
    private bool ballHeld = true;
    private bool isCharging;
    private bool isServing;
    private bool serveReleaseRequested;
    private float chargeValue;
    private KeyCode currentServeKey;
    private string pendingServeType = "";
    private float tossProgress = 0f;

    public bool IsCharging => isCharging;
    public bool IsServing => isServing;
    public bool ServeReleaseRequested => serveReleaseRequested;
    public bool BallHeld => ballHeld;

    private void Start()
    {
        originalY = serveStartPosition.position.y;
        serveUIFeedback.InitializeIdealZone(idealChargeMin, idealChargeMax);
    }

    public void HandleServe()
    {
        if (!ballHeld || controller.currentServer != "Player")
            return;

        ServeBall();
    }

    private void ServeBall()
    {
        if (!isCharging && !serveReleaseRequested)
        {
            ballTransform.position = ballHoldPosition.position;
        }

        // 1) Iniciar carga y lanzamiento visual
        if (!isCharging && !serveReleaseRequested &&
            (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)))
        {
            currentServeKey = Input.GetKeyDown(KeyCode.Z) ? KeyCode.Z : KeyCode.X;

            isCharging = true;
            isServing = true;

            chargeValue = 0f;
            tossProgress = 0f;

            serveUIFeedback.ShowChargeBar();
        }

        // 2) Mientras mantengo la tecla: carga potencia + la pelota sube
        if (isCharging)
        {
            if (Input.GetKey(currentServeKey))
            {
                chargeValue += Time.deltaTime * chargeSpeed;
                chargeValue = Mathf.Clamp01(chargeValue);

                // La pelota avanza en su animación de saque
                tossProgress += Time.deltaTime * frequency;
                tossProgress = Mathf.Clamp01(tossProgress);

                float yOffset = Mathf.Sin(tossProgress * Mathf.PI) * amplitude;

                ballTransform.position = new Vector3(
                    ballHoldPosition.position.x,
                    originalY + yOffset,
                    ballHoldPosition.position.z
                );

                serveUIFeedback.UpdateCharge(chargeValue, idealChargeMin, idealChargeMax);
            }

            // 3) Al soltar, NO saco todavía. Espero a que la pelota vuelva.
            if (Input.GetKeyUp(currentServeKey))
            {
                isCharging = false;
                serveReleaseRequested = true;
                pendingServeType = currentServeKey == KeyCode.Z ? "Topspin" : "Slice";
            }
        }

        // 4) Después de soltar: la pelota sigue bajando hasta volver a la paleta
        if (serveReleaseRequested)
        {
            tossProgress += Time.deltaTime * frequency;
            tossProgress = Mathf.Clamp01(tossProgress);

            float yOffset = Mathf.Sin(tossProgress * Mathf.PI) * amplitude;

            ballTransform.position = new Vector3(
                ballHoldPosition.position.x,
                originalY + yOffset,
                ballHoldPosition.position.z
            );

            serveUIFeedback.UpdateCharge(chargeValue, idealChargeMin, idealChargeMax);

            // Cuando termina la curva, la pelota volvió al punto de golpe
            if (tossProgress >= 1f)
            {
                ballTransform.position = ballHoldPosition.position;

                ExecuteServe(pendingServeType);

                serveUIFeedback.HideChargeBar();

                serveReleaseRequested = false;
                isServing = false;
                pendingServeType = "";
                tossProgress = 0f;

                controller.playing = true;

                AudioManager.Instance.PlayHitBall();
            }
        }
    }

    private void ExecuteServe(string type)
    {
        ballHeld = false;
        ballRb.useGravity = true;

        Vector3 direction = GetServeDirection();

        if (type == "Topspin")
        {
            direction += Vector3.down * 0.1f;
        }
        else if (type == "Slice")
        {
            direction += Vector3.up * 0.15f;
        }

        serveForce = Mathf.Lerp(minServeForce, maxServeForce, chargeValue);

        bool perfectTiming = chargeValue >= idealChargeMin && chargeValue <= idealChargeMax;

        if (perfectTiming)
        {
            Debug.Log("Saque perfecto");
        }

        ballRb.velocity = direction.normalized * serveForce;
        ballGame.RegisterHit("Player");
    }

    private Vector3 GetServeDirection()
    {
        // Centro mínimo del campo rival
        float baseZ = -0.40f;

        // Offset para crear variaciones
        float lateralOffset = 2f; // izquierda/derecha
        float forwardOffset = 0.5f; // profundidad
        float dropZ = baseZ; // drop no va más cerca que esto

        // Calculamos el target según teclas
        Vector3 aimPoint = new(0f, 0f, baseZ + forwardOffset);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + forwardOffset);
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + forwardOffset);
            else
                aimPoint = new Vector3(0f, 0f, baseZ + forwardOffset + 0.3f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            aimPoint = new Vector3(0f, 0f, dropZ);

            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint.x = -lateralOffset * 0.5f;
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint.x = lateralOffset * 0.5f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + 0.2f);
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + 0.2f);
        }

        // Dirección desde nuestra posición hacia el punto objetivo
        return (aimPoint - transform.position).normalized;
    }

    public void ResetServe()
    {
        isServing = false;
        isCharging = false;
        serveReleaseRequested = false;

        pendingServeType = "";
        chargeValue = 0f;

        ballHeld = true;

        serveUIFeedback.HideChargeBar();
    }
}
