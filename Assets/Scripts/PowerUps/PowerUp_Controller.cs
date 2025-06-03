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

    // Start is called before the first frame update
    void Start()
    {
        PowerUp_Manager.Instance.SetController(this);
        StartCoroutine(SpawnRoutine());
        //InvokeRepeating(nameof(SpawnRoutine), 5f, spawnInterval);
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            if (canSpawn && powerUpManager.CanSpawnPowerUp(playerController))
            {
                SpawnPowerUp();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0f, 60f * Time.deltaTime, 0f);
    }

    void SpawnPowerUp()
    {
        int index = Random.Range(0, spawnPoints.Length);
        int type = Random.Range(0, powerUpPrefabs.Length);

        GameObject powerUp = Instantiate(powerUpPrefabs[type], spawnPoints[index].position, Quaternion.identity);

        PowerUp_Manager.Instance.RegisterPowerUp(powerUp);
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
