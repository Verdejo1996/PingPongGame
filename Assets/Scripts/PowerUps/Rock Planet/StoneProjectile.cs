using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "PowerUps/StoneProjectile")]
public class StoneProjectile : Base_PowerUp
{
    public GameObject rockPrefab;
    public float speed = 12f;
    public float lifetime = 5f;
    public float slowDuration = 2f;

    public override void Activate(Player_Controller player)
    {
        Vector3 spawnPoint = player.transform.position + Vector3.up * 0.5f;

        GameObject rock = GameObject.Instantiate(rockPrefab, spawnPoint, Quaternion.identity);

        RockProjectileBehaviour behaviour = rock.GetComponent<RockProjectileBehaviour>();
        if (behaviour != null)
        {
            behaviour.Init(player.iA_Controller, speed, slowDuration);
        }

        GameObject.Destroy(rock, lifetime);
    }
}
