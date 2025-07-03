using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ExplosiveBall")]
public class ExplosiveBall : Base_PowerUp
{
    public Color poweredColor = Color.red;
    public override void Activate(Player_Controller player)
    {
        if(player.TryGetComponent<PlayerHit_Controller>(out var p))
        {
            p.ActivateFireExplosion(poweredColor);
        }
    }
}
