using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard_Controller : MonoBehaviour
{
    public GameObject blizzardOverlay; // Asignar la Image de UI
    public float duration = 5f;
    public float interval = 15f;

    public bool affectBall = true;
    public float windForce = 1f;

    private GameObject ball;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        InvokeRepeating(nameof(StartBlizzard), 10f, interval);
    }

    void StartBlizzard()
    {
        StartCoroutine(BlizzardRoutine());
    }

    IEnumerator BlizzardRoutine()
    {
        if (blizzardOverlay != null)
            blizzardOverlay.SetActive(true);

        float timer = 0f;
        while (timer < duration)
        {
            if (affectBall && ball != null)
            {
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                Vector3 windDir = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.1f, 0.1f));
                rb.AddForce(windDir * windForce, ForceMode.Force);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (blizzardOverlay != null)
            blizzardOverlay.SetActive(false);
    }
}
