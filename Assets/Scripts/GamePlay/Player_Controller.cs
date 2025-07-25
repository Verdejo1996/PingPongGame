using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Instancias")]
    //public List<Base_PowerUp> ListPowerUps = new();
    public PowerUp_Manager powerUp_Manager;
    public IA_Controller iA_Controller;
    public Ball ball;
    public HUD_Controller hud_Controller;
    public PlayerHit_Controller player_hit_Controller;

    [Header("Escalas")]
    public Vector3 player_Scale;
    public Vector3 original_Scale;

    //public GameObject player;
    [Header("Power Up Activo")]
    public bool precisionActive = false;
    public bool superHitActive = false;
    public bool shieldActive = false;
    public bool hasPrecisionPowerUp = false;
    public bool hasSuperHitPowerUp = false;
    public bool hasShieldPowerUp = false;
    public bool hasFreezePowerUp = false;
    public bool isBigPaddle = false;
    public GameObject prefabShield;
    public float duration = 5f;

    private void Start()
    {
        original_Scale = transform.localScale;
        player_Scale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        //CheckForActivation();
        if (isBigPaddle)
        {
            transform.localScale = player_Scale;
        }
        else
        {
            transform.localScale = original_Scale;
        }
    }

    public void ColectPowerUp(Base_PowerUp type)
    {
        type.Activate(this);
        hud_Controller.UpdateHUD(type.powerUpName, type.duration);
        powerUp_Manager.NotifyPlayerUsedPowerUp(this);
    }

/*    void CheckForActivation()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivatePowerUp(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ActivatePowerUp(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ActivatePowerUp(2);
    }*/

/*    void ActivatePowerUp(int index)
    {
        if (index < 0 || index >= ListPowerUps.Count)
            return;
        Debug.Log(this.name);
        Debug.Log($"Intentando activar: {ListPowerUps[index].name}");

        ListPowerUps[index].Activate(this); // Le pas�s el contexto
        ListPowerUps.RemoveAt(index);
        powerUp_Manager.NotifyPlayerUsedPowerUp(this);
        hud_Controller.UpdateHUD(ListPowerUps);

        Debug.Log($"PowerUps: {ListPowerUps.Count}");
    }*/

/*    void CheckPowerUpActivation()
    {
        if (hasPrecisionPowerUp && Input.GetKeyDown(KeyCode.Alpha1))
        {
            PrecisionPower();
        }

        if (hasSuperHitPowerUp && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //SuperHitPower();
        }

        if (hasShieldPowerUp && Input.GetKeyDown(KeyCode.Alpha3))
        {
            ConsumeShield();
        }
    }*/
    void PrecisionPower()
    {
        hasPrecisionPowerUp = false;
        //Reduce el rango de movimiento del Aim
        precisionActive = true;
        StartCoroutine(DeactivateAfterTime(() => precisionActive = false, duration));
    }

/*    void SuperHitPower()
    {
        hasSuperHitPowerUp = false;
        //Aumentar fuerza del golpe
        superHitActive = true;
        StartCoroutine(DeactivateAfterTime(() => superHitActive = false, duration));
    }*/

    public bool ConsumeShield()
    {
        hasShieldPowerUp = false;
        shieldActive = true;
        if (shieldActive)
        {
            Instantiate(prefabShield, new Vector3(0, 0, -6), Quaternion.identity);
            StartCoroutine(DeactivateAfterTime(() => shieldActive = false, 5f));
            return true;
        }
        return false;
    }

    public void SuperHit()
    {
        StartCoroutine(DeactivateAfterTime(() => superHitActive = false, duration));
    }

    IEnumerator DeactivateAfterTime(System.Action onEnd, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onEnd?.Invoke();
    }
}
