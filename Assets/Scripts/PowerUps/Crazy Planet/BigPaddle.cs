using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Big Paddle")]
public class BigPaddle : Base_PowerUp
{
    public Vector3 enlargedScale = new(3f, 3f, 3f);
    public float duration = 5f;
    public float addRange = 1f;
    public override void Activate(Player_Controller player)
    {
        PlayerHit_Controller range = player.player_hit_Controller;
        player.StartCoroutine(ApplyBigPaddle(player, range));
    }

    private IEnumerator ApplyBigPaddle(Player_Controller player, PlayerHit_Controller range)
    {
        player.isBigPaddle = true;
        range.hitRange += addRange;
        player.player_Scale += enlargedScale;
        

        yield return new WaitForSeconds(duration);

        player.isBigPaddle = false;
        range.hitRange -= addRange;
    }
}
