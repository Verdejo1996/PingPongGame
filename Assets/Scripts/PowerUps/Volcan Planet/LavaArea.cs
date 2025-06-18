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
            IA_Controller ia = other.GetComponent<IA_Controller>();
            if (ia != null)
            {
                ia.StartCoroutine(ApplySlow(ia));
            }
        }
    }

    private IEnumerator ApplySlow(IA_Controller ia)
    {
        float originalSpeed = ia.speed;
        ia.speed *= slowAmount;

        yield return new WaitForSeconds(duration);

        ia.speed = originalSpeed;
    }
}
