using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    public float timeOnTable = 2f;
    public GameObject hitEffect;
    //private bool stuck = false;

    private IceSpikeSpawner spawner;
    private int spawnIndex;

    public void SetSpawner(IceSpikeSpawner s, int index)
    {
        spawner = s;
        spawnIndex = index;
    }

    private void OnDestroy()
    {
        spawner?.FreeSpawnPoint(spawnIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (stuck) return;
        if (collision.gameObject.CompareTag("tablePlayer"))
        {
            //stuck = true;
            GetComponent<Rigidbody>().isKinematic = true;
            //transform.position = collision.contacts[0].point;
            //transform.parent = collision.transform;
            GameObject particles = Instantiate(hitEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if(ps != null)
            {
                float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(particles, totalDuration);
            }
            StartCoroutine(DestroyAfterDelay());
        }
        else if (collision.gameObject.CompareTag("Ball"))
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
            //stuck = true;
            GetComponent<Rigidbody>().isKinematic = true;
            //transform.position = collision.contacts[0].point;
            //transform.parent = collision.transform;
            GameObject particles = Instantiate(hitEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(particles, totalDuration);
            }
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHit_Controller>(out var player))
                player.ApplySlowEffect(2f);

            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(timeOnTable);
        Destroy(gameObject);
    }
}
