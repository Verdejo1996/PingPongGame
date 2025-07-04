using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab;
    public Transform[] spawnPoints;
    public float interval = 15f;
    public float portalLifetime = 10f;

    private GameObject entryPortal;
    private GameObject exitPortal;
    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5f); // espera inicial

        while (true)
        {
            if (canSpawn)
            {
                SpawnPortals();
                yield return new WaitForSeconds(portalLifetime);
                DestroyPortals();
            }

            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnPortals()
    {
        if (spawnPoints.Length < 2) return;

        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++) availableIndexes.Add(i);

        int indexA = availableIndexes[Random.Range(0, availableIndexes.Count)];
        availableIndexes.Remove(indexA);
        int indexB = availableIndexes[Random.Range(0, availableIndexes.Count)];

        entryPortal = Instantiate(portalPrefab, spawnPoints[indexA].position, Quaternion.identity);
        exitPortal = Instantiate(portalPrefab, spawnPoints[indexB].position, Quaternion.identity);

        // Vincular entre ellos
        Portal portalScript = entryPortal.GetComponent<Portal>();
        Portal portalScriptB = exitPortal.GetComponent<Portal>();
        if (portalScript != null) portalScript.exitPortal = exitPortal.transform;
        if (portalScriptB != null) portalScriptB.exitPortal = entryPortal.transform;
    }

    void DestroyPortals()
    {
        if (entryPortal != null) Destroy(entryPortal);
        if (exitPortal != null) Destroy(exitPortal);
    }
}
