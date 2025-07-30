using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [Header("Gameplay")]
    private Rigidbody rb;
    public Game_Controller controller;
    public ObjectPool poolPlayer;
    public ObjectPool poolBot;
    //public ParticleSystem effectControlBall;
    [SerializeField] private GameObject lavaAreaPrefab;

    [Header("Banderas")]
    public bool bounceTable = false;
    public bool hasTouchedTable = false;
    [SerializeField] private bool hitNetLast = false;
    private string lastHitterAfterTable = "";
    [SerializeField] bool validServe = false;
    public bool tableAfterNet = false;
    public bool isCurveShotActive = false;
    public bool fireExplosionActive = false;

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer trailBall;
    [SerializeField] private Color colorSoft = Color.blue;
    [SerializeField] private Color colorStrong = Color.red;
    [SerializeField] private Camera_Shake cameraShake;
    public bool isLavaActive;
    private Color fireExplosionColor;
    private bool fireExplosionEnabled = false;
    private Color originalColor;
    private Color explosionColor;
    public ParticleSystem explosionParticles;


    private float duration = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        ResetState();
    }

    public void EnableFireExplosion(Color color)
    {
        fireExplosionEnabled = true;
        explosionColor = color;
        var trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            originalColor = trail.material.color;
            trail.material.color = explosionColor;
        }
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
        Vector3 curve = new(0.17f, 0, 0);
        if(isCurveShotActive)
        {
            rb.AddForce(curve * force, ForceMode.Impulse);
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
                //poolPlayer.ResetTable();
                //poolBot.ResetTable();
            }
        }
        #region
        //RockPlanet
        if (collision.gameObject.CompareTag("RockCourtPlayer") || collision.gameObject.CompareTag("RockCourtBot"))
        {
            hasTouchedTable = true;
        }
        if (collision.gameObject.CompareTag("RockCourtBot") && controller.currentServer == "Player")
        {
            validServe = true;
        }
        if (collision.gameObject.CompareTag("RockCourtPlayer") && controller.currentServer == "Bot")
        {
            validServe = true;
        }
        #endregion
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
            if (collision.gameObject.CompareTag("tableBot") || collision.gameObject.CompareTag("RockCourtBot") && controller.lastHitter == "Player")
            {
                tableAfterNet = true;
            }
            else if(collision.gameObject.CompareTag("tablePlayer") || collision.gameObject.CompareTag("RockCourtPlayer") && controller.lastHitter == "Bot")
            {
                tableAfterNet = true;
            }
        }
        if(isLavaActive && collision.gameObject.CompareTag("tableBot"))
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
        }
        if (fireExplosionEnabled && other.CompareTag("Bot"))
        {
            if (other.TryGetComponent<IA_Controller>(out var ia))
            {
                ia.ApplyDisorientation(2f); // por ejemplo
                fireExplosionEnabled = false;

                // Restaurar visual
                if (TryGetComponent<TrailRenderer>(out var trail))
                    trail.material.color = originalColor;

                if (explosionParticles != null)
                    explosionParticles.Play();
            }
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

    public void ActiveEffectControl()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.Play();
    }
    public void DeactivateEffectControl()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.Stop();
    }
}
