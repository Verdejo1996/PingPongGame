using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Manager : MonoBehaviour
{
    public static PowerUp_Manager Instance {  get; private set; }

    private List<GameObject> activeScenePowerUps = new List<GameObject>();
    private PowerUp_Controller powerUpController;
    private Player_Controller player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetController(PowerUp_Controller ctrl)
    {
        powerUpController = ctrl;
        player = ctrl.playerController;
    }

    public void RegisterPowerUp(GameObject powerUp)
    {
        activeScenePowerUps.Add(powerUp);
        CheckIfShouldPause();
    }

    public void UnregisterPowerUp(GameObject powerUp)
    {
        activeScenePowerUps.Remove(powerUp);
        Debug.Log($"PowerUps en escena: {activeScenePowerUps.Count}");
        CheckIfShouldResume();
    }

    public bool CanSpawnPowerUp(Player_Controller player)
    {
        return activeScenePowerUps.Count < 3 && player.ListPowerUps.Count < 3;
    }

    public void NotifyPlayerUsedPowerUp(Player_Controller player)
    {
        CheckIfShouldResume();
    }

    void CheckIfShouldPause()
    {
        if (!CanSpawnPowerUp(player))
        {
            Debug.Log("Pausando spawn.");
            powerUpController?.PauseSpawning();
        }
    }

    void CheckIfShouldResume()
    {
        if (!CanSpawnPowerUp(player))
        {
            Debug.Log("Reanudando spawn.");
            powerUpController?.ResumeSpawning();
        }
    }
}
