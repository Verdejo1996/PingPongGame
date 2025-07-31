using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/CourtProtect")]
public class CourtProtect : Base_PowerUp
{
    public GameObject proteccionPrefab;
    public override void Activate(Player_Controller player)
    {
        if (proteccionPrefab != null)
        {
            GameObject shield = GameObject.Instantiate(proteccionPrefab, new Vector3(0.03f, -0.85f, -3f), Quaternion.identity);

            GameObject.Destroy(shield, duration); // Destruye el escudo luego del tiempo
        }
    }
}
