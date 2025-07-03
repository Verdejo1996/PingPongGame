using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform[] laserSpawnPoints; // Puntos en ambos lados del campo
    public float interval = 6f;

    void Start()
    {
        StartCoroutine(SpawnLasers());
    }

    IEnumerator SpawnLasers()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            int index = Random.Range(0, laserSpawnPoints.Length);
            Transform spawn = laserSpawnPoints[index];

            Instantiate(laserPrefab, spawn.position, spawn.rotation);
        }
    }
}
