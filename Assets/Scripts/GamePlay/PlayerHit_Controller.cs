using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHit_Controller : MonoBehaviour
{
    [Header("Gameplay")]
    public Game_Controller controller;
    public Player_Controller player;

    [Header("Movimiento del jugador")]
    public float moveSpeed = 5f;
    public bool isMoving = false;

    [Header("Golpe")]
    public Ball ballGame;
    public Transform ballTransform;
    public Rigidbody ballRb;
    public Transform racketTransform;
    public float hitRange = 2f;
    public float hitForce;
    public float upForce = 5f;
    [SerializeField] private float hitCooldown = 0.15f;
    private float nextHitTime = 0f;

    [Header("Servicio")]
    [SerializeField] Transform serveStartPosition;
    [SerializeField] Transform ballHoldPosition;
    [SerializeField] float serveForce;
    [SerializeField] Slider serveChargeBar;
    [SerializeField] Image idealZoneImage;
    [SerializeField] float chargeSpeed = 1.0f; // velocidad de carga visual
    [SerializeField] float minServeForce = 7f;
    [SerializeField] float maxServeForce = 12f;
    [SerializeField] float idealChargeMin = 0.8f;
    [SerializeField] float idealChargeMax = 1f;
    [SerializeField] Color normalColor = new(0, 1, 0, 0.4f); // verde suave
    [SerializeField] Color glowColor = new(1, 1, 0, 0.7f);   // amarillo brillante
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 1.5f;
    [SerializeField] private Color perfectColor = Color.red;
    [SerializeField] float maxHeight = 0.5f;  // La altura máxima que alcanzará la pelota
    [SerializeField] float frequency = 2f;    // La frecuencia con la que sube y baja (más alto es más rápido)
    [SerializeField] float amplitude = 0.1f;  // La amplitud del movimiento (cuánto se desplaza en Y)
    private float originalY;        // La posición Y original de la pelota
    private bool serveReleaseRequested = false;
    private string pendingServeType = "";
    private float serveOscillationTime = 0f;
    [SerializeField] private float serveReleaseThreshold = 0.08f;



    [Header("Paleta")]
    public float racketSpeed = 20f;
    public float racketReturnSpeed = 10f;
    private Vector3 initialRacketLocalPos;
    private bool isHitting;
    private bool isServing;
    private bool serveInProgress;
    private bool ballHeld = true;
    private bool isCharging;
    private float chargeValue = 0f;
    private KeyCode currentServeKey;

    private Vector3 posInicial;

    private float anguloActualX = 0f;

    public bool isSlowed = false;
    private float originalSpeed;
    private bool fireExplosionActive = false;
    private Color fireExplosionColor;

    private Coroutine feedbackCoroutine;

    private void Start()
    {
        UpdateIdealZoneIndicator();
        originalSpeed = moveSpeed;
        isCharging = false;
        serveInProgress = false;
        isServing = false;
        isHitting = false;
        initialRacketLocalPos = racketTransform.localPosition;
        originalY = serveStartPosition.position.y;
    }
    void Update()
    {
        if (CanMove())
        {
            Movement();
        }

        if (!serveInProgress && !controller.playing)
        {
            if (ballHeld && controller.currentServer == "Player")
            {
                ServeBall();
            }
        }

        if (!isHitting && !isServing && controller.playing)
        {
            KeyInput();
        }
    }

    bool CanMove()
    {
        return !isCharging && !serveInProgress && !isServing && !serveReleaseRequested; ;
    }

    void Movement()
    {
        if(CanMove())
        {
            float h = Input.GetAxisRaw("Horizontal"); // Flechas izquierda/derecha
            float v = Input.GetAxisRaw("Vertical");   // Flechas arriba/abajo

            Vector3 move = new Vector3(h, 0f, v).normalized;
            float velocidadRotacion = 100f;

            if (move != Vector3.zero)
            {
                isMoving = true;
                Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * move;
                newPosition.x = Mathf.Clamp(newPosition.x, -6f, 6f);
                newPosition.z = Mathf.Clamp(newPosition.z, -8f, -1f);
                newPosition.y = Mathf.Clamp(newPosition.y, -1f, 5f);
                transform.position = newPosition;
            }
            else
            {
                isMoving = false;
            }

            float delta = velocidadRotacion * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow) && anguloActualX < 90)
            {
                // Incrementar la rotación hacia la izquierda (0 a 90 grados)
                float incremento = Mathf.Min(delta, 90 - anguloActualX);
                transform.Rotate(0f, 0f, incremento, Space.Self);
                anguloActualX += incremento;
            }
            else if (Input.GetKey(KeyCode.RightArrow) && anguloActualX > -90)
            {
                // Decrementar la rotación hacia la derecha (-90 a 0 grados)
                float decremento = Mathf.Min(delta, anguloActualX + 90f);
                transform.Rotate(0f, 0f, -decremento, Space.Self);
                anguloActualX -= decremento;
            }
        }
    }

    void ServeBall()
    {
        // Mantener la pelota en la posición base solo si todavía no empezó la carga
        if (!isCharging && !serveReleaseRequested)
        {
            ballTransform.position = ballHoldPosition.position;
        }

        // 1) Iniciar carga
        if (!isCharging && !serveReleaseRequested && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)))
        {
            currentServeKey = Input.GetKeyDown(KeyCode.Z) ? KeyCode.Z : KeyCode.X;

            isCharging = true;
            isServing = true;

            chargeValue = 0f;
            serveOscillationTime = 0f;

            serveChargeBar.value = 0f;
            serveChargeBar.gameObject.SetActive(true);
        }

        // 2) Mientras mantenés la tecla: cargar barra + mover pelota
        if (isCharging)
        {
            if (Input.GetKey(currentServeKey))
            {
                chargeValue += Time.deltaTime * chargeSpeed;
                chargeValue = Mathf.Clamp01(chargeValue);
                serveChargeBar.value = chargeValue;

                serveOscillationTime += Time.deltaTime * chargeSpeed;

                float yOffset = Mathf.Clamp(
                    Mathf.Sin(serveOscillationTime * Mathf.PI * frequency) * amplitude,
                    -maxHeight,
                    maxHeight
                );

                ballTransform.position = new Vector3(
                    ballHoldPosition.position.x,
                    originalY + yOffset,
                    ballHoldPosition.position.z
                );

                UpdateServeBarVisual();
            }

            // 3) Soltaste la tecla: fijar potencia, pero NO sacar todavía
            if (Input.GetKeyUp(currentServeKey))
            {
                isCharging = false;
                serveReleaseRequested = true;
                pendingServeType = currentServeKey == KeyCode.Z ? "Topspin" : "Slice";
            }
        }

        // 4) Después de soltar: seguir movimiento natural hasta volver al punto de golpe
        if (serveReleaseRequested)
        {
            serveOscillationTime += Time.deltaTime * chargeSpeed;

            float yOffset = Mathf.Clamp(
                Mathf.Sin(serveOscillationTime * Mathf.PI * frequency) * amplitude,
                -maxHeight,
                maxHeight
            );

            ballTransform.position = new Vector3(
                ballHoldPosition.position.x,
                originalY + yOffset,
                ballHoldPosition.position.z
            );

            UpdateServeBarVisual();

            float distanceToHold = Vector3.Distance(ballTransform.position, ballHoldPosition.position);

            if (distanceToHold <= serveReleaseThreshold)
            {
                ExecuteServe(pendingServeType);

                serveChargeBar.gameObject.SetActive(false);
                serveReleaseRequested = false;
                isServing = false;
                pendingServeType = "";

                controller.playing = true;

                AudioManager.Instance.PlayHitBall();
            }
        }
    }

    void ExecuteServe(string type)
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

    void UpdateServeBarVisual()
    {
        float currentValue = serveChargeBar.value;
        bool isInIdealRange = currentValue >= idealChargeMin && currentValue <= idealChargeMax;

        Image fillImage = serveChargeBar.fillRect.GetComponent<Image>();
        fillImage.color = isInIdealRange ? glowColor : normalColor;

        if (isInIdealRange)
        {
            ShowPerfectFeedback();
        }
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (player.superHitActive)
            {
                hitForce = 14f;
                player.SuperHit();
            }
            else
            {
                hitForce = 11.5f;
            }

            TryHitBall("topspin");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            hitForce = 9.5f;
            TryHitBall("slice");

            Vector3 posBall = ballTransform.position;
            float topEffect = Time.deltaTime * 0.5f;
            posBall.y -= topEffect;
            ballTransform.position = posBall;
        }
    }

    public void ResetServe()
    {
        isServing = false;
        isCharging = false;
        serveReleaseRequested = false;
        serveInProgress = false;

        pendingServeType = "";
        chargeValue = 0f;
        serveOscillationTime = 0f;

        ballHeld = true;

        serveChargeBar.gameObject.SetActive(false);
    }

    void TryHitBall(string type)
    {
        if (Time.time < nextHitTime)
            return;

        if (isHitting)
            return;

        // Evita pegar dos veces seguidas
        if (controller.lastHitter == "Player")
            return;

        Vector3 toBall = ballTransform.position - racketTransform.position;
        float distance = toBall.magnitude;

        float forwardAngle = Vector3.Angle(racketTransform.forward, toBall.normalized);
        bool isInFront = forwardAngle <= 90f;

        if (distance <= hitRange && isInFront)
        {
            nextHitTime = Time.time + hitCooldown;
            StartCoroutine(HitAnimation(type));
            AudioManager.Instance.PlayHitBall();
        }
        else
        {
            Debug.Log("La pelota está fuera de rango para golpear.");
        }
    }

    IEnumerator HitAnimation(string type)
    {
        isHitting = true;

        posInicial = transform.position;

        // Animar hacia la pelota
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * racketSpeed;
            posInicial = ballTransform.position;
            transform.position = posInicial;
            yield return null;
        }

        // Aplicar fuerza a la pelota
        Vector3 dir = GetDirection();
        Vector3 force = dir * hitForce + Vector3.up * upForce;

        ballRb.velocity = force;

        Debug.Log("Dir: " + dir);
        Debug.Log("Force: " + force);

        // Registrar golpe apenas se aplica
        ballGame.hasTouchedTable = false;
        ballGame.tableAfterNet = false;
        ballGame.RegisterHit("Player");

        if (fireExplosionActive)
        {
            ballGame.EnableFireExplosion(fireExplosionColor);
            fireExplosionActive = false;
        }

        // Retornar la paleta a su posición
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * racketReturnSpeed;
            racketTransform.localPosition = Vector3.Lerp(transform.position, initialRacketLocalPos, t);
            yield return null;
        }

        isHitting = false;
    }

    Vector3 GetDirection()
    {
        #region
        /*        // Posición del centro del campo rival
                Vector3 opponentTarget = new(0f, 0f, 0.26f);

                // Dirección base desde la paleta hacia el campo rival
                Vector3 baseDirection = (opponentTarget - transform.position).normalized;

                // Ajustes según teclas presionadas
                Vector3 adjustment = Vector3.zero;
                if (Input.GetKey(KeyCode.UpArrow)) adjustment += Vector3.forward;
                if (Input.GetKey(KeyCode.DownArrow)) adjustment += Vector3.back;
                if (Input.GetKey(KeyCode.LeftArrow)) adjustment += Vector3.left;
                if (Input.GetKey(KeyCode.RightArrow)) adjustment += Vector3.right;

                // Ajustamos la dirección base
                Vector3 finalDirection = (baseDirection + adjustment * 0.5f).normalized;
                return finalDirection;*/

        /*        // Punto base al que queremos siempre apuntar: lado del oponente
                Vector3 targetPoint = new(0f, 0f, 0.26f); // Centro mínimo del lado rival
                Vector3 aimPoint = targetPoint;

                // Ajustes por flechas (movemos el objetivo ligeramente)
                float horizontalOffset = 2f; // izquierda/derecha
                float depthOffset = 0.4f;      // más cerca o más profundo (drop vs drive)

                if (Input.GetKey(KeyCode.LeftArrow)) aimPoint += Vector3.left * horizontalOffset;
                if (Input.GetKey(KeyCode.RightArrow)) aimPoint += Vector3.right * horizontalOffset;
                if (Input.GetKey(KeyCode.UpArrow)) aimPoint += Vector3.forward * depthOffset;
                if (Input.GetKey(KeyCode.DownArrow)) aimPoint -= Vector3.forward * depthOffset;

                // Asegurar que el Z no sea menor que 1.55f
                if (aimPoint.z < 1.55f)
                    aimPoint.z = 1.55f;

                // Dirección final desde la posición actual hacia el punto objetivo
                Vector3 direction = (aimPoint - transform.position).normalized;

                return direction;*/
        #endregion
        // Centro mínimo del campo rival
        float baseZ = 0.26f;

        // Offset para crear variaciones
        float lateralOffset = 3f; // izquierda/derecha
        float forwardOffset = 1.5f;  // profundidad
        float dropZ = baseZ;         // drop no va más cerca que esto

        // Calculamos el target según teclas
        Vector3 aimPoint = new(0f, 0f, baseZ + forwardOffset); // Centro por defecto

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Golpe profundo hacia la esquina
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + forwardOffset); // cruzado izquierda
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + forwardOffset);  // cruzado derecha
            else
                aimPoint = new Vector3(0f, 0f, baseZ + forwardOffset + 0.3f);       // centro más profundo
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Drop shot (más cerca de la red)
            aimPoint = new Vector3(0f, 0f, dropZ); // drop centro
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint.x = -lateralOffset * 0.5f;
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint.x = lateralOffset * 0.5f;
        }
        else
        {
            // Solo izquierda o derecha (sin Up/Down)
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + 0.2f);
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + 0.2f);
        }

        // Dirección desde nuestra posición hacia el punto objetivo
        Vector3 direction = (aimPoint - transform.position).normalized;

        if (Input.GetKey(KeyCode.X)) // Slice
        {
            direction += Vector3.up * 0.10f;
            direction *= 0.8f;
        }

        return direction;
    }

    Vector3 GetServeDirection()
    {
        // Centro mínimo del campo rival
        float baseZ = -0.40f;

        // Offset para crear variaciones
        float lateralOffset = 2f; // izquierda/derecha
        float forwardOffset = 0.5f;  // profundidad
        float dropZ = baseZ;         // drop no va más cerca que esto

        // Calculamos el target según teclas
        Vector3 aimPoint = new(0f, 0f, baseZ + forwardOffset); // Centro por defecto

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Golpe profundo hacia la esquina
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + forwardOffset); // cruzado izquierda
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + forwardOffset);  // cruzado derecha
            else
                aimPoint = new Vector3(0f, 0f, baseZ + forwardOffset + 0.3f);       // centro más profundo
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Drop shot (más cerca de la red)
            aimPoint = new Vector3(0f, 0f, dropZ); // drop centro
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint.x = -lateralOffset * 0.5f;
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint.x = lateralOffset * 0.5f;
        }
        else
        {
            // Solo izquierda o derecha (sin Up/Down)
            if (Input.GetKey(KeyCode.LeftArrow))
                aimPoint = new Vector3(-lateralOffset, 0f, baseZ + 0.2f);
            else if (Input.GetKey(KeyCode.RightArrow))
                aimPoint = new Vector3(lateralOffset, 0f, baseZ + 0.2f);
        }

        // Dirección desde nuestra posición hacia el punto objetivo
        Vector3 direction = (aimPoint - transform.position).normalized;

        return direction;
    }

    void OnDrawGizmosSelected()
    {
        if (racketTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(racketTransform.position, hitRange);

            // Dirección hacia adelante (racketTransform.forward)
            Gizmos.color = Color.green;
            Gizmos.DrawLine(racketTransform.position, racketTransform.position + racketTransform.up * hitRange);

            // Visualización del cono de golpe
            float angle = 70f; // Grados del cono (30° a cada lado)
            int segments = 10;
            Vector3 forward = racketTransform.forward;

            for (int i = 0; i <= segments; i++)
            {
                float currentAngle = -angle / 2f + angle * i / segments;
                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
                Vector3 direction = rotation * forward;
                Gizmos.DrawLine(racketTransform.position, racketTransform.position + direction * hitRange);
            }
        }
    }

    void UpdateIdealZoneIndicator()
    {
        RectTransform rt = idealZoneImage.GetComponent<RectTransform>();

        float totalWidth = serveChargeBar.GetComponent<RectTransform>().rect.width;

        float idealStart = idealChargeMin * totalWidth;
        float idealEnd = idealChargeMax * totalWidth;
        float idealWidth = idealEnd - idealStart;

        // Ajustar la posición y el tamaño
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 0.5f);
        rt.anchoredPosition = new Vector2(idealStart, 0);
        rt.sizeDelta = new Vector2(idealWidth, 0);
    }

    void ShowPerfectFeedback()
    {
        feedbackText.text = "¡Max!";
        feedbackText.color = perfectColor;
        feedbackText.gameObject.SetActive(true);

        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }

        feedbackCoroutine = StartCoroutine(HideFeedbackAfterDelay());
    }

    IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDuration);
        feedbackText.gameObject.SetActive(false);
    }

    internal void ApplySlowEffect(float v)
    {
        if (isSlowed)
        {
            return;
        }
        isSlowed = true;
        moveSpeed *= 0.4f; // 60% slower

        StartCoroutine(SlowEffectRoutine(v));
    }

    IEnumerator SlowEffectRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSlowed = false;
        moveSpeed = originalSpeed;
    }

    public void ApplyShakeForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 push = new Vector3(Random.Range(-1f, 1f), 0, 0) * 3f;
            rb.AddForce(push, ForceMode.Impulse);
        }
    }
    public void StopMovement()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void ActivateFireExplosion(Color poweredColor)
    {
        fireExplosionActive = true;
        fireExplosionColor = poweredColor;
    }
}
