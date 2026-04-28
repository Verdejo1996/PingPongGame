using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Big Paddle")]
public class BigPaddle : Base_PowerUp
{
    public Vector3 enlargedScale = new(3f, 3f, 3f);
    //public float duration = 5f;
    public float addRange = 1f;
    public override void Activate(Player_Controller player)
    {
        PlayerHit_Controller range = player.PowerUps.player_hit_Controller;
        player.StartCoroutine(ApplyBigPaddle(player, range));
    }

    private IEnumerator ApplyBigPaddle(Player_Controller player, PlayerHit_Controller range)
    {
        player.PowerUps.isBigPaddle = true;
        range.hitRange += addRange;
        player.PowerUps.player_Scale += enlargedScale;
        

        yield return new WaitForSeconds(duration);

        player.PowerUps.isBigPaddle = false;
        range.hitRange -= addRange;
    }
}
