using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ControlBall Shot")]
public class ControlBall : Base_PowerUp
{
    public float duration = 1.5f;
    public float controlForce = 3f;
    public override void Activate(Player_Controller player)
    {
        player.StartCoroutine(ControlBallShot(duration, controlForce));
    }

    private IEnumerator ControlBallShot(float duration, float controlForce)
    {
        Ball ball = FindObjectOfType<Ball>();
        if (ball == null) yield break;

        float elapsed = 0f;
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        while (elapsed < duration)
        {
            Vector3 force = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow)) force += -ball.transform.right;
            if (Input.GetKey(KeyCode.RightArrow)) force += ball.transform.right;
            if (Input.GetKey(KeyCode.UpArrow)) force += ball.transform.forward;
            if (Input.GetKey(KeyCode.DownArrow)) force += -ball.transform.forward;

            if (force != Vector3.zero)
            {
                rb.AddForce(force.normalized * controlForce, ForceMode.Acceleration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
