using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerPowerUp_Controller powerUpController;

    public PlayerPowerUp_Controller PowerUps => powerUpController;
}
