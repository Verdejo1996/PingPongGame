using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeSpawner : MonoBehaviour
{
    public GameObject iceSpikePrefab;
    public Transform[] spawnPoints; // Puntos por encima del campo
    public float spawnInterval = 8f;
    public float activeDuration = 6f;

    //private bool isActive = false;

    void Start()
    {
        StartCoroutine(SpikeRoutine());
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            //isActive = true;

            float timer = 0f;
            while (timer < activeDuration)
            {
                SpawnSpike();
                timer += 1.5f;
                yield return new WaitForSeconds(1.5f);
            }

            //isActive = false;
        }
    }

    void SpawnSpike()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(iceSpikePrefab, spawnPoints[index].position, Quaternion.identity);
    }
}
