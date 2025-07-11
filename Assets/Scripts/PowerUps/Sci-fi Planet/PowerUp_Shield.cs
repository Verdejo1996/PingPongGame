using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shield PowerUp")]
public class PowerUp_Shield : Base_PowerUp
{
    public GameObject shieldPrefab;
    //public float duration = 5f;
    public override void Activate(Player_Controller player)
    {

        if (shieldPrefab != null)
        {
            GameObject shield = GameObject.Instantiate(shieldPrefab, new Vector3(0, 0, -6), Quaternion.identity);

            GameObject.Destroy(shield, duration); // Destruye el escudo luego del tiempo
        }
    }
}
