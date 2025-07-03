using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserManager : MonoBehaviour
{
    public GameObject geyserPrefab;
    public Transform[] spawnPoints;
    public int maxActiveGeysers = 3;
    public float spawnInterval = 8f;

    private List<Transform> usedSpawnPoints = new List<Transform>();

    void Start()
    {
        StartCoroutine(GeyserSpawnRoutine());
    }

    IEnumerator GeyserSpawnRoutine()
    {
        yield return new WaitForSeconds(3f); // Tiempo inicial de espera

        while (true)
        {
            if (usedSpawnPoints.Count < maxActiveGeysers)
            {
                List<Transform> availablePoints = new List<Transform>(spawnPoints);
                availablePoints.RemoveAll(point => usedSpawnPoints.Contains(point));

                if (availablePoints.Count > 0)
                {
                    Transform spawnPoint = availablePoints[Random.Range(0, availablePoints.Count)];
                    GameObject geyser = Instantiate(geyserPrefab, spawnPoint.position, Quaternion.identity);
                    geyser.GetComponent<LavaGeyser>().Init(this, spawnPoint); // Le pasamos referencia
                    usedSpawnPoints.Add(spawnPoint);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void UnregisterPoint(Transform point)
    {
        if (usedSpawnPoints.Contains(point))
            usedSpawnPoints.Remove(point);
    }
}
