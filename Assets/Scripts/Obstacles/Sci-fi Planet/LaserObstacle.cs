using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 5f;
    public float lifetime = 5f;

    private Vector3 direction;

    void Start()
    {
        direction = (endPoint.position - startPoint.position).normalized;
        transform.position = startPoint.position;

        // L�nea inicial
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * 10f); // Largo del l�ser visual
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * 10f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 newDir = Vector3.Reflect(rb.velocity, direction);
                rb.velocity = newDir;
            }
        }
    }
}
