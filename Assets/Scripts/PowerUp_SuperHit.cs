using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SuperHit PowerUp")]
public class PowerUp_SuperHit : Base_PowerUp
{
    float duration = 5f;
    public override void Activate(Player_Controller player)
    {
        player.StartCoroutine(ApplySuperHit(player));
/*        player.hasSuperHitPowerUp = true;
        player.SuperHit();*/
    }

    private IEnumerator ApplySuperHit(Player_Controller player)
    {
        player.superHitActive = true;

        yield return new WaitForSeconds(duration);
    }
}
