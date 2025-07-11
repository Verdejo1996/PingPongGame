using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Lava Trail")]
public class LavaTrail : Base_PowerUp
{
    public Material lavaMaterial; // Para cambiar temporalmente el trail
    //public float duration = 5f;

    public override void Activate(Player_Controller player)
    {
        Debug.Log("Lava Trail activado");
        Ball ball = player.ball;
        if (ball != null)
        {
            player.StartCoroutine(ActivateLavaTrail(ball));
        }
    }

    private IEnumerator ActivateLavaTrail(Ball ball)
    {
        var trail = ball.GetComponent<TrailRenderer>();
        var originalMat = trail.material;

        trail.material = lavaMaterial;

        ball.GetComponent<Ball>().isLavaActive = true;

        yield return new WaitForSeconds(duration);

        trail.material = originalMat;
        ball.GetComponent<Ball>().isLavaActive = false;
    }
}
