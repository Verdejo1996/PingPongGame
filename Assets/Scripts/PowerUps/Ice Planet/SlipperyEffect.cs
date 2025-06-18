using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Slippery PowerUp")]
public class SlipperyEffect : Base_PowerUp
{
    public float duration = 3f;
    public override void Activate(Player_Controller player)
    {
        IA_Controller opponent = player.iA_Controller;
        if (opponent != null)
        {
            opponent.StartCoroutine(ApplySlippery(opponent));
        }
    }

    private IEnumerator ApplySlippery(IA_Controller opponent)
    {
        opponent.isSlippery = true;

        yield return new WaitForSeconds(duration);

        opponent.isSlippery = false;
    }
}
