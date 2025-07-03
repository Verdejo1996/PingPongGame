using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGeyser : MonoBehaviour
{
    public GameObject warningZone;
    public ParticleSystem lavaParticles;
    public Collider triggerZone;

    public float warningDuration = 2f;
    public float eruptionDuration = 2.5f;
    public float cooldownBeforeDestroy = 2f;

    private GeyserManager manager;
    private Transform spawnPoint;

    public void Init(GeyserManager manager, Transform point)
    {
        this.manager = manager;
        this.spawnPoint = point;

        StartCoroutine(GeyserCycle());
    }

    IEnumerator GeyserCycle()
    {
        // 1. Mostrar advertencia
        warningZone.SetActive(true);
        yield return new WaitForSeconds(warningDuration);
        warningZone.SetActive(false);

        // 2. Activar erupción
        lavaParticles.Play();
        triggerZone.enabled = true;
        yield return new WaitForSeconds(eruptionDuration);

        // 3. Desactivar y destruir
        lavaParticles.Stop();
        triggerZone.enabled = false;

        yield return new WaitForSeconds(cooldownBeforeDestroy);

        manager?.UnregisterPoint(spawnPoint); // Libera el punto
        Destroy(gameObject); // Se elimina completamente
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHit_Controller>(out var player))
                player.ApplySlowEffect(2f);
        }
        else if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
                rb.velocity = dir * rb.velocity.magnitude * 1.2f;
            }
        }
    }
}
