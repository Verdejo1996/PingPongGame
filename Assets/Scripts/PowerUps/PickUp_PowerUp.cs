using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PowerUpType { Precision, SuperHit, Shield }
public class PickUp_PowerUp : MonoBehaviour
{
    //public PowerUpType type;

    public Base_PowerUp powerUp;
    public Transform spawnPoint;

    public GameObject pickUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            if (other.TryGetComponent<Player_Controller>(out var player))
            {
                player.ColectPowerUp(powerUp);
            }
            if (PowerUp_Manager.Instance != null)
            {
                PowerUp_Manager.Instance.UnregisterPowerUp(gameObject);

                // Liberar punto de spawn
                PowerUp_Controller controller = FindObjectOfType<PowerUp_Controller>();
                if (controller != null && spawnPoint != null)
                {
                    controller.FreeSpawnPoint(spawnPoint);
                }
            }

            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
