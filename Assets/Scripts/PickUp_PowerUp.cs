using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PowerUpType { Precision, SuperHit, Shield }
public class PickUp_PowerUp : MonoBehaviour
{
    //public PowerUpType type;

    public Base_PowerUp powerUp;

    public GameObject pickUpEffect;
    /*    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Paddle>().ColectPowerUp(powerUp);

                if (pickUpEffect != null)
                {
                    Instantiate(pickUpEffect, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player_Controller>();

            if (player != null)
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
