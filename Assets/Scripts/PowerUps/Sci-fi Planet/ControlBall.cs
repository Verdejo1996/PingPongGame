using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ControlBall Shot")]
public class ControlBall : Base_PowerUp
{
    //public float duration = 2f;
    public float controlForce = 5f;
    public override void Activate(Player_Controller player)
    {
        Ball ball = player.ball;
        player.StartCoroutine(ControlBallShot(duration, controlForce, ball));
    }

    private IEnumerator ControlBallShot(float duration, float controlForce, Ball ball)
    {
        //Ball ball = FindObjectOfType<Ball>();
        if (ball == null) yield break;

        float elapsed = -1f;
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        ball.ActiveEffectControl();

        while (elapsed < duration)
        {
            Vector3 force = Vector3.zero;
            if (Game_Controller.Instance.lastHitter == "Player")
            {
                if (Input.GetKey(KeyCode.LeftArrow)) force += Vector3.left;
                if (Input.GetKey(KeyCode.RightArrow)) force += Vector3.right;
                if (Input.GetKey(KeyCode.UpArrow)) force += Vector3.forward;
                if (Input.GetKey(KeyCode.DownArrow)) force += -Vector3.back;
            }
            if (force != Vector3.zero)
            {
                rb.AddForce(force.normalized * controlForce, ForceMode.Acceleration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        ball.DeactivateEffectControl();
    }
}
