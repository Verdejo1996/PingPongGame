using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/Curve PowerUp")]
public class CurvePowerUp : Base_PowerUp
{
    public float curveForce = 5f;

    public override void Activate(Player_Controller player)
    {
        Ball controller = player.ball;
        if (controller != null)
        {
            controller.isCurveShotActive = true;
            controller.StartCoroutine(controller.CurveShot(curveForce));
        }
    }
}
