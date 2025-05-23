using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Paddle : MonoBehaviour
{
    /*  public float speed = 3f;

      public Player_Controller player;


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

          if (hitting *//*&& controller.playing*//*)
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
      }*/

    [Header("Movimiento del jugador")]
    public float moveSpeed = 5f;

    [Header("Golpe")]
    public Ball_Tutorial ballTutorial;
    public Transform ballTransform;
    public Rigidbody ballRb;
    public Transform racketTransform;
    public float hitRange = 2f;
    public float hitForce = 13f;
    public float upForce = 5f;

    [Header("Servicio")]
    [SerializeField] Transform serveStartPosition;
    [SerializeField] Transform ballHoldPosition;
    [SerializeField] float serveForce = 10f;
    [SerializeField] Slider serveChargeBar;
    [SerializeField] float chargeSpeed = 1.0f; // velocidad de carga visual


    [Header("Paleta")]
    public float racketSpeed = 20f;
    public float racketReturnSpeed = 10f;
    private Vector3 initialRacketLocalPos;
    private bool isHitting = false;
    private bool isServing = true;
    //private bool ballBouncedOnPlayerSide = false;
    private bool serveInProgress = false;
    //private bool ballHeld = true;
    private bool isCharging = false;
    private float chargeValue = 0f;
    private KeyCode currentServeKey;

    private Vector3 posInicial;

    private void Start()
    {
        //shot_Controller = GetComponent<Shot_Controller>();
        initialRacketLocalPos = racketTransform.localPosition;
    }
    void Update()
    {
        Movement();
        Debug.Log(isServing);

        if (isServing && !serveInProgress && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            // Lanzar el servicio
            if (!isCharging && Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
            {
                currentServeKey = Input.GetKeyDown(KeyCode.Z) ? KeyCode.Z : KeyCode.X;
                isCharging = true;
                chargeValue = 0f;
                serveChargeBar.value = 0f;
                serveChargeBar.gameObject.SetActive(true);
            }

            // Cargar barra mientras se mantiene presionada la tecla
            if (isCharging && Input.GetKey(currentServeKey))
            {
                chargeValue += Time.deltaTime * chargeSpeed;
                chargeValue = Mathf.Clamp01(chargeValue);
                serveChargeBar.value = chargeValue;
            }

            // Soltar y servir
            if (isCharging && Input.GetKeyUp(currentServeKey))
            {
                Tutorial.instance.isPaused = false;
                ServeBall(currentServeKey == KeyCode.Z ? "Topspin" : "Slice");
                serveChargeBar.gameObject.SetActive(false);
                isCharging = false;
                isServing = false;
            }
            //ResetServe();
        }
        //isServing = false;
        if (!isHitting && !isServing && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            KeyInput();
        }
    }

    bool CanMove()
    {
        return !isCharging && !serveInProgress;
    }

    void Movement()
    {
        if (CanMove())
        {
            float h = Input.GetAxisRaw("Horizontal"); // Flechas izquierda/derecha
            float v = Input.GetAxisRaw("Vertical");   // Flechas arriba/abajo

            Vector3 move = new Vector3(h, 0f, v).normalized;

            if (move != Vector3.zero)
            {
                Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * move;
                newPosition.x = Mathf.Clamp(newPosition.x, -6f, 6f);
                newPosition.z = Mathf.Clamp(newPosition.z, -8f, -1f);
                newPosition.y = Mathf.Clamp(newPosition.y, -1f, 5f);
                transform.position = newPosition;
            }
        }
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TryHitBall("topspin");
            hitForce = 13f;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            TryHitBall("slice");
            hitForce = 10f;
            Vector3 posBall = ballTransform.position;
            float topEffect = Time.deltaTime * 0.5f;
            posBall.y -= topEffect;
            ballTransform.position = posBall;

        }
    }

    void ServeBall(string type)
    {

        ballRb.useGravity = true;
        Debug.Log(ballRb.useGravity);

        Vector3 direction = GetServeDirection();
        float finalForce = serveForce;

        // Aplicar efecto
        if (type == "Topspin")
        {
            //finalForce += 3f;
            direction += Vector3.down * 0.1f;
        }
        else if (type == "Slice")
        {
            //finalForce *= 0.7f;
            direction += Vector3.up * 0.15f;
        }

        ballRb.velocity = direction.normalized * finalForce;

    }

    public void ResetServe()
    {
        isServing = true;
        serveInProgress = false;
    }

    void TryHitBall(string type)
    {
        float distance = Vector3.Distance(racketTransform.position, ballTransform.position);
        if (distance <= hitRange)
        {
            StartCoroutine(HitAnimation(type));
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

        //if (type == "topspin")


        ballRb.velocity = force;

        // Retornar la paleta a su posición
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * racketReturnSpeed;
            racketTransform.localPosition = Vector3.Lerp(transform.position, initialRacketLocalPos, t);
            yield return null;
        }

        isHitting = false;
        if (!isServing)
        {
            ballTutorial.RegisterHit("Player");

        }
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
        }
    }
}
