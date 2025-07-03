using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Controller : MonoBehaviour
{
    [Header("Arrays")]
    public GameObject[] powerUpPrefabs;
    public Transform[] spawnPoints;

    [Header("Intervalo")]
    public float spawnInterval = 10f;

    [Header("Instancia")]
    public Player_Controller playerController;
    public PowerUp_Manager powerUpManager;

    private bool canSpawn = true;
    private Dictionary<Transform, bool> occupiedPoints = new Dictionary<Transform, bool>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var point in spawnPoints)
        {
            occupiedPoints.Add(point, false); // todos desocupados al inicio
        }
        PowerUp_Manager.Instance.SetController(this);
        StartCoroutine(SpawnRoutine());
        //InvokeRepeating(nameof(SpawnRoutine), 5f, spawnInterval);
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            if (canSpawn && powerUpManager.CanSpawnPowerUp(playerController) && Game_Controller.Instance.playing)
            {
                SpawnPowerUp();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /*    void SpawnPowerUp()
        {
            int index = Random.Range(0, spawnPoints.Length);
            int type = Random.Range(0, powerUpPrefabs.Length);

            GameObject powerUp = Instantiate(powerUpPrefabs[type], spawnPoints[index].position, Quaternion.identity);

            PowerUp_Manager.Instance.RegisterPowerUp(powerUp);
        }*/

    void SpawnPowerUp()
    {
        List<Transform> freePoints = new List<Transform>();

        foreach (var kvp in occupiedPoints)
        {
            if (!kvp.Value) // si no está ocupado
                freePoints.Add(kvp.Key);
        }

        if (freePoints.Count == 0)
        {
            Debug.Log("No hay puntos libres para spawnear PowerUp.");
            return;
        }

        Transform selectedPoint = freePoints[Random.Range(0, freePoints.Count)];
        int type = Random.Range(0, powerUpPrefabs.Length);

        GameObject powerUp = Instantiate(powerUpPrefabs[type], selectedPoint.position, Quaternion.identity);
        occupiedPoints[selectedPoint] = true; // marcar punto como ocupado

        PowerUp_Manager.Instance.RegisterPowerUp(powerUp);

        // Guardar en el script del PowerUp qué punto lo generó
        var pickup = powerUp.GetComponent<PickUp_PowerUp>();
        if (pickup != null)
        {
            pickup.spawnPoint = selectedPoint; // importante para luego liberar
        }
    }
    public void FreeSpawnPoint(Transform point)
    {
        if (occupiedPoints.ContainsKey(point))
        {
            occupiedPoints[point] = false;
        }
    }

    public void PauseSpawning()
    {
        canSpawn = false;
    }

    public void ResumeSpawning()
    {
        canSpawn = true;
    }
}
