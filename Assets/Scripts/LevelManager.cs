using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelPowerUpConfig currentLevelConfig;

    void Awake()
    {
        Instance = this;
    }

    public List<Base_PowerUp> GetAvailablePowerUps()
    {
        return currentLevelConfig.allowedPowerUps;
    }

    public Base_PowerUp GetRandomPowerUp()
    {
        List<Base_PowerUp> allowed = GetAvailablePowerUps();
        if (allowed != null && allowed.Count > 0)
        {
            return allowed[Random.Range(0, allowed.Count)];
        }
        return null;
    }
}
