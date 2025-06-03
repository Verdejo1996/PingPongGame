using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Freeze PowerUp")]
public class Freeze_PowerUp : Base_PowerUp
{
    public float duration = 3f;
    public float slowFactor = 0.5f;

    public override void Activate(Player_Controller player)
    {
        IA_Controller opponent = player.iA_Controller;
        if (opponent != null)
        {
            opponent.StartCoroutine(ApplyFreeze(opponent));
        }
    }

    private IEnumerator ApplyFreeze(IA_Controller ia)
    {
        float originalSpeed = ia.speed;
        ia.speed *= slowFactor;

        yield return new WaitForSeconds(duration);

        ia.speed = originalSpeed;
    }
}
