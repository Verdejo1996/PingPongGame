using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PowerUpType { Precision, SuperHit, Shield }
public class PickUp_PowerUp : MonoBehaviour
{
    //public PowerUpType type;

    public Base_PowerUp powerUp;

    public GameObject pickUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            if (other.TryGetComponent<Player_Controller>(out var player))
            {
                player.ColectPowerUp(powerUp);
                PowerUp_Manager.Instance.UnregisterPowerUp(this.gameObject);
            }

            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
