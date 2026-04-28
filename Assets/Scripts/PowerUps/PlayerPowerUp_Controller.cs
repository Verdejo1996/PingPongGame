using System;
using System.Collections;
using UnityEngine;

public class PlayerPowerUp_Controller : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PowerUp_Manager powerUpManager;
    [SerializeField] private HUD_Controller hudController;
    [SerializeField] private Player_Controller playerController;
    public IA_Controller iA_Controller;
    public BallVisualEffects ball;
    public BallPowerEffects ballPowers;
    public PlayerHit_Controller player_hit_Controller;

    [Header("Shield")]
    [SerializeField] private GameObject prefabShield;

    [Header("Big Paddle")]
    public Vector3 player_Scale;
    public Vector3 original_Scale;

    [Header("Duraciones")]
    [SerializeField] private float defaultDuration = 5f;

    [Header("Power Ups Activos")]
    public bool precisionActive = false;
    public bool superHitActive = false;
    public bool shieldActive = false;
    public bool isBigPaddle = false;

    [Header("Power Ups Disponibles")]
    public bool hasPrecisionPowerUp = false;
    public bool hasSuperHitPowerUp = false;
    public bool hasShieldPowerUp = false;
    public bool hasFreezePowerUp = false;

    private void Update()
    {
        if (isBigPaddle)
        {
            transform.localScale = player_Scale;
        }
        else
        {
            transform.localScale = original_Scale;
        }
    }
    public void CollectPowerUp(Base_PowerUp type)
    {
        type.Activate(playerController);
        hudController.UpdateHUD(type.powerUpName, type.duration);
        powerUpManager.NotifyPlayerUsedPowerUp(playerController);
    }

    //public bool ConsumeShield()
    //{
    //    hasShieldPowerUp = false;
    //    shieldActive = true;

    //    if (shieldActive)
    //    {
    //        Instantiate(prefabShield, new Vector3(0, 0, -6), Quaternion.identity);
    //        StartCoroutine(DeactivateAfterTime(() => shieldActive = false, defaultDuration));
    //        return true;
    //    }

    //    return false;
    //}

    public void SuperHit()
    {
        StartCoroutine(DeactivateAfterTime(() => superHitActive = false, defaultDuration));
    }

    private IEnumerator DeactivateAfterTime(Action onEnd, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onEnd?.Invoke();
    }
}
