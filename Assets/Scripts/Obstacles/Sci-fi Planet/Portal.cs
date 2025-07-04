using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [HideInInspector] public Transform exitPortal;
    public ParticleSystem teleportEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (ball == null || rb == null) return;

            // Teleport
            if (teleportEffect != null) Instantiate(teleportEffect, transform.position, Quaternion.identity);

            other.transform.position = exitPortal.position;

            // Decidir dirección según quién golpeó
            Vector3 direction;
            if (Game_Controller.Instance.lastHitter == "Player")
                direction = Vector3.forward; // hacia IA (Z negativa)
            else if (Game_Controller.Instance.lastHitter == "Bot")
                direction = Vector3.back; // hacia Player (Z positiva)
            else
                direction = exitPortal.forward; // por defecto

            rb.velocity = direction.normalized * (rb.velocity.magnitude * 0.8f);

            //if (teleportEffect != null) Instantiate(teleportEffect, exitPortal.position, Quaternion.identity);
            StartCoroutine(DisablePortalColliderTemporarily(exitPortal.GetComponent<Collider>(), 0.3f));
        }
    }

    IEnumerator DisablePortalColliderTemporarily(Collider portalCollider, float delay)
    {
        if (portalCollider != null)
        {
            portalCollider.enabled = false;
            yield return new WaitForSeconds(delay);
            if (portalCollider != null)
                portalCollider.enabled = true;
        }
    }
}
