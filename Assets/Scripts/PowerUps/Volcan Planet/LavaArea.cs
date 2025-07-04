using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaArea : MonoBehaviour
{
    public float slowAmount = 0.5f;
    public float duration = 2f;

    private void Start()
    {
        Destroy(gameObject, duration); // Se elimina después de un tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            if (other.TryGetComponent<IA_Controller>(out var ia))
            {
                ia.ApplySlow(2f);
            }
        }
    }
}
