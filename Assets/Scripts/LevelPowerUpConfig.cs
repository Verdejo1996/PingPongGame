using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelPowerUpConfig", menuName = "PowerUps/Level PowerUp Config")]
public class LevelPowerUpConfig : ScriptableObject
{
    public List<Base_PowerUp> allowedPowerUps;
}
