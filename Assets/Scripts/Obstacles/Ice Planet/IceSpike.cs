using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    public float timeOnTable = 2f;
    public GameObject hitEffect;
    private bool stuck = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (stuck) return;

        if (collision.collider.CompareTag("Table"))
        {
            stuck = true;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = collision.contacts[0].point;
            transform.parent = collision.transform;
            if (hitEffect != null) 
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            StartCoroutine(DestroyAfterDelay());
        }
        else if (collision.collider.CompareTag("Player"))
        {
            PlayerHit_Controller player = collision.collider.GetComponent<PlayerHit_Controller>();
            if (player != null) player.ApplySlowEffect(2f); // ejemplo
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.collider.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 randomDeflect = new Vector3(Random.Range(-2f, 2f), 1f, Random.Range(-2f, 2f));
                ballRb.velocity = randomDeflect.normalized * ballRb.velocity.magnitude;
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject); // Si cae en el suelo o fuera de la mesa
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(timeOnTable);
        Destroy(gameObject);
    }
}
