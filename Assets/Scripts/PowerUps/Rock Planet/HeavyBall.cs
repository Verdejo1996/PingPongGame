using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HeavyBall")]
public class HeavyBall : Base_PowerUp
{
    public float heavyMass = 5f;
    public Material heavyBallMaterial;
    public ParticleSystem rockTrail;

    public override void Activate(Player_Controller player)
    {
        Ball ball = player.ball;

        if (ball != null) 
        {
            player.StartCoroutine(ApplyHeavyBallEffect(ball));
        }
    }

    private IEnumerator ApplyHeavyBallEffect(Ball ball)
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null) yield break;

        float originalMass = rb.mass;
        Material originalMaterial = ball.GetComponent<MeshRenderer>().material;

        // Cambiar masa y apariencia
        rb.mass = heavyMass;
        ball.GetComponent<MeshRenderer>().material = heavyBallMaterial;

        if (rockTrail != null)
        {
            var trail = GameObject.Instantiate(rockTrail, ball.transform);
            trail.Play();
        }

        ball.isHeavy = true;

        yield return new WaitForSeconds(duration);

        // Revertir
        rb.mass = originalMass;
        ball.GetComponent<MeshRenderer>().material = originalMaterial;
        ball.isHeavy = false;

        // Detener partículas
        ParticleSystem existingTrail = ball.GetComponentInChildren<ParticleSystem>();
        if (existingTrail != null)
        {
            existingTrail.Stop();
            GameObject.Destroy(existingTrail.gameObject, 1f);
        }
    }
}

