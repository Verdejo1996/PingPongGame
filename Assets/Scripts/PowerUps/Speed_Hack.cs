using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed Hack")]
public class Speed_Hack : Base_PowerUp
{
    float duration = 5f;
    
    float speedFactor = 3f;
    public override void Activate(Player_Controller player)
    {
        PlayerHit_Controller playerhit = player.player_hit_Controller;
        if(playerhit != null)
        {
            player.StartCoroutine(ApplySpeedHack(playerhit));
        }
    }

    private IEnumerator ApplySpeedHack(PlayerHit_Controller paddleRef)
    {
        float originalSpeed = paddleRef.moveSpeed;
        paddleRef.moveSpeed += speedFactor;

        yield return new WaitForSeconds(duration);

        paddleRef.moveSpeed = originalSpeed;
    }
}
