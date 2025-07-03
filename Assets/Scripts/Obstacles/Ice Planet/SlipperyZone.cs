using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyZone : MonoBehaviour
{
    public float activeDuration = 5f;
    public float cooldown = 10f;

    public float slipperyVelocity = 5f;

    public ParticleSystem iceParticles;

    private Collider zoneCollider;
    private MeshRenderer mesh;
    public PlayerHit_Controller playerHitController;
    public GameObject visualWarning;

    void Start()
    {
        zoneCollider = GetComponent<Collider>();
        zoneCollider.enabled = false;

        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;

        if (iceParticles != null)
            iceParticles.Stop();

        StartCoroutine(ZoneRoutine());
    }

    IEnumerator ZoneRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            ActivateZone();

            yield return new WaitForSeconds(activeDuration);
            DeactivateZone();
        }
    }

    void ActivateZone()
    {
        zoneCollider.enabled = true;
        mesh.enabled = true;
        if (iceParticles != null)
            iceParticles.Play();
        if (visualWarning != null) 
            visualWarning.SetActive(true);
    }

    void DeactivateZone()
    {
        playerHitController.moveSpeed -= slipperyVelocity;
        zoneCollider.enabled = false;
        mesh.enabled = false;
        if (iceParticles != null)
        {
            iceParticles.Stop();
        }
        if (visualWarning != null) 
            visualWarning.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var rb = other.GetComponent<PlayerHit_Controller>(); // adaptá esto al script de movimiento
            if (rb != null)
            {
                rb.moveSpeed += slipperyVelocity;
            }
        }
    }
}
