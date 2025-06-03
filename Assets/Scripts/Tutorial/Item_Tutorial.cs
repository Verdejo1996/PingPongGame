using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Tutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Tutorial.instance.currentPhase == TutorialPhase.Move)
        {
            Tutorial.instance.CollectItem();
            Destroy(gameObject);
        }

        if (other.CompareTag("Ball") && Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            Tutorial.instance.CollectItemServe();
            Destroy(gameObject);
        }

        if (other.CompareTag("Ball") && Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            Tutorial.instance.CollectItemHit();
            Destroy(gameObject);
        }
    }
}
