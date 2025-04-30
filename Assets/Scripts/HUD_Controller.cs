using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
/*    public Image precisionIcon;
    public Image superHitIcon;
    public Image shieldIcon;*/

    public List<Image> powerUpsIcons;


    /*    public void UpdateIcons(bool precisionAvailable, bool superHitAvailable, bool shieldAvailable)
        {
            if (precisionIcon != null) precisionIcon.enabled = precisionAvailable;
            if (superHitIcon != null) superHitIcon.enabled = superHitAvailable;
            if (shieldIcon != null) shieldIcon.enabled = shieldAvailable;
        }*/

    public void UpdateHUD(List<Base_PowerUp> currentPowerUps)
    {
        Debug.Log($"Icons: {powerUpsIcons.Count}, PowerUps: {currentPowerUps.Count}");
        for (int i = 0; i < powerUpsIcons.Count; i++)
        {
            if(i < currentPowerUps.Count)
            {
                powerUpsIcons[i].sprite = currentPowerUps[i].icon;
                powerUpsIcons[i].enabled = true;
            }
            else
            {
                powerUpsIcons[i].enabled = false;
            }
        }
    }
}
