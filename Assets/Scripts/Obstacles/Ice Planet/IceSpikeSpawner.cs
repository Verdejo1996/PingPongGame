using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikeSpawner : MonoBehaviour
{
    public GameObject iceSpikePrefab;
    public Transform[] spawnPoints; // Puntos por encima del campo
    public float spawnInterval = 8f;
    public float activeDuration = 6f;

    private HashSet<int> occupiedSpawns = new HashSet<int>();
    //private bool isActive = false;

    void Start()
    {
        //StartCoroutine(SpikeRoutine());
        InvokeRepeating(nameof(SpawnSpike), 2f, spawnInterval);
    }
    void SpawnSpike()
    {
        List<int> availableIndices = new();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!occupiedSpawns.Contains(i))
                availableIndices.Add(i);
        }

        if (availableIndices.Count == 0)
        {
            Debug.Log("No hay puntos disponibles.");
            return;
        }

        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        Transform spawnPoint = spawnPoints[randomIndex];

        GameObject spike = Instantiate(iceSpikePrefab, spawnPoint.position, Quaternion.identity);
        occupiedSpawns.Add(randomIndex);

        // Pasamos el índice al spike para liberarlo al destruirse
        spike.GetComponent<IceSpike>().SetSpawner(this, randomIndex);
    }

    public void FreeSpawnPoint(int index)
    {
        occupiedSpawns.Remove(index);
    }

    /*    IEnumerator SpikeRoutine()
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
        }*/
}
