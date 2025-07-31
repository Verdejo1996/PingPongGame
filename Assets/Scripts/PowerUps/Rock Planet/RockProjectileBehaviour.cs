using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectileBehaviour : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float slowDuration;
    private Rigidbody rb;

    public void Init(IA_Controller opponent, float moveSpeed, float duration)
    {
        target = opponent.transform;
        speed = moveSpeed;
        slowDuration = duration;

        rb = GetComponent<Rigidbody>();
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            if (other.TryGetComponent<IA_Controller>(out var ai))
            {
                ai.ApplySlow(slowDuration);
            }

            Destroy(gameObject);
        }
    }
}
