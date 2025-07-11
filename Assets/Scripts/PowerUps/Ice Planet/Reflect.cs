using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Reflect PowerUp")]
public class Reflect : Base_PowerUp
{
    //public float duration = 5f;
    public float addRange = 1f;
    public GameObject reflectPrefab;
    
    public override void Activate(Player_Controller player)
    {
        PlayerHit_Controller hitReflect = player.player_hit_Controller;
        if (hitReflect != null)
        {
            hitReflect.StartCoroutine(ApplyReflect(hitReflect));
        }
    }

    private IEnumerator ApplyReflect(PlayerHit_Controller hitReflect)
    {
        hitReflect.hitRange += addRange;
        if(reflectPrefab != null)
        {
            GameObject reflect = GameObject.Instantiate(reflectPrefab, hitReflect.transform);
            reflect.transform.SetParent(hitReflect.transform);
            GameObject.Destroy(reflect, duration);
        }

        yield return new WaitForSeconds(duration);

        hitReflect.hitRange -= addRange;
    }
}
