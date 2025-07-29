using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBreak : MonoBehaviour
{
    public ParticleSystem breakEffect;  // Efecto visual de rotura

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Obtiene el Rigidbody del cubo
        rb.isKinematic = true;  // Inicialmente, no aplicamos físicas
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si la pelota golpea el cubo, lo destruimos y aplicamos físicas
        if (collision.gameObject.CompareTag("Ball"))
        {
            BreakCube();
        }
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BreakCube();
        }
    }*/

    void BreakCube()
    {
        // Activamos el Rigidbody para que las físicas se apliquen
        rb.isKinematic = false;  // Habilitamos las físicas para que el cubo caiga
        rb.AddExplosionForce(5f, transform.position, 1f);  // Agregamos una pequeña fuerza explosiva

        // Instanciamos un efecto de partículas para simular la rotura
        Instantiate(breakEffect, transform.position, Quaternion.identity);

        // Devuelve el cubo al Object Pool después de que haya caído
        ObjectPool pool = FindObjectOfType<ObjectPool>();
        pool.ReturnCube(gameObject);
    }
}
