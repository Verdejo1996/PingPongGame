using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Tutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Tutorial.instance.CollectItem();
            Destroy(gameObject);
        }
    }
}
