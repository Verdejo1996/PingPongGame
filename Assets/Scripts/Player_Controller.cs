using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Instancias")]
    public List<Base_PowerUp> ListPowerUps = new();
    public PowerUp_Manager powerUp_Manager;
    public IA_Controller iA_Controller;
    public Ball ball;
    public HUD_Controller hud_Controller;

    //public GameObject player;
    //public Paddle paddle;
    [Header("Power Up Activo")]
    public bool precisionActive = false;
    public bool superHitActive = false;
    public bool shieldActive = false;
    public bool hasPrecisionPowerUp = false;
    public bool hasSuperHitPowerUp = false;
    public bool hasShieldPowerUp = false;
    public bool hasFreezePowerUp = false;
    public GameObject prefabShield;
    public float duration = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForActivation();
        if(ListPowerUps.Count > 0)
        {
            Debug.Log($"PowerUp disponible: {ListPowerUps[0].name}");
        }
    }

    public void ColectPowerUp(Base_PowerUp type)
    {
        Debug.Log(this.name);
        if(ListPowerUps.Count >= 3)
        {
            return;
        }
        ListPowerUps.Add(type);
        Debug.Log($"PowerUps: {ListPowerUps.Count}");
        hud_Controller.UpdateHUD(ListPowerUps);
    }

    void CheckForActivation()
    {
        /*        for(int i = 0;  i < ListPowerUps.Count; i++)
                {
                    if(Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
                    {
                        Debug.Log($"Presionaste {KeyCode.Alpha1 + i}, activando powerUp {i}");
                        ActivatePowerUp(i);
                        break;
                    }
                }*/
        Debug.Log(this.name);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"Presionaste {KeyCode.Alpha1}, activando powerUp");
            ActivatePowerUp(0);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ActivatePowerUp(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ActivatePowerUp(2);
    }

    void ActivatePowerUp(int index)
    {
        Debug.Log(this.name);
        Debug.Log($"Intentando activar: {ListPowerUps[index].name}");
        ListPowerUps[index].Activate(this); // Le pasás el contexto
        ListPowerUps.RemoveAt(index);
        powerUp_Manager.NotifyPlayerUsedPowerUp(this);
        hud_Controller.UpdateHUD(ListPowerUps);
/*        if (index < ListPowerUps.Count)
        {
        }*/
        Debug.Log($"PowerUps: {ListPowerUps.Count}");
    }

    void CheckPowerUpActivation()
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
    }
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
