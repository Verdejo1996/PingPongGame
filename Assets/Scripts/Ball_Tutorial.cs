using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Tutorial : MonoBehaviour
{
    private Rigidbody rb;
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
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2.5f, -7)); // Ajusta la posición para el jugador
        }
        if (collision.gameObject.CompareTag("Wall") && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            Tutorial.instance.isPaused = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            SetServePosition(new Vector3(0, 2f, 7));
        }
    }
}
