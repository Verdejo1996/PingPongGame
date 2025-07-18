using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ParticleSystem laserParticlesPrefab;
    private ParticleSystem laserParticles;
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 5f;
    public float lifetime = 5f;

    private Vector3 direction;

    public void Initialize(Transform start, Transform end)
    {
        // Configuración básica del LineRenderer
        lineRenderer.startWidth = 0.3f;  // El grosor del láser al principio
        lineRenderer.endWidth = 0.3f;    // El grosor del láser al final

        laserParticles = Instantiate(laserParticlesPrefab, transform.position, Quaternion.identity);
        laserParticles.transform.SetParent(transform);  // Asegura que las partículas sigan el láser
        laserParticles.Play();

        startPoint = start;
        endPoint = end;
        direction = (endPoint.position - startPoint.position).normalized;
        transform.position = startPoint.position;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * 10f);
        }

        Destroy(gameObject, lifetime);      
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        laserParticles.transform.position = transform.position;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * 10f);
        }
    }

    private void OnDestroy()
    {
        if(laserParticles != null)
        {
            laserParticles.Stop();
            Destroy(laserParticles.gameObject);
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
