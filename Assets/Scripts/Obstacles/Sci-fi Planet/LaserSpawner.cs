using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform[] laserSpawnPoints;
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

            int pairIndex = Random.Range(0, laserSpawnPoints.Length / 2) * 2;
            Transform start = laserSpawnPoints[pairIndex];
            Transform end = laserSpawnPoints[pairIndex + 1];

            GameObject laserGO = Instantiate(laserPrefab);
            LaserObstacle laser = laserGO.GetComponent<LaserObstacle>();
            laser.Initialize(start, end);
        }
    }
}
