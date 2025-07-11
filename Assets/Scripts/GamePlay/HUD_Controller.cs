using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    /*    public Image precisionIcon;
        public Image superHitIcon;
        public Image shieldIcon;*/

    //public List<Image> powerUpsIcons;
    public TextMeshProUGUI powerUpName;
    public Image cooldownBar;

    private void Start()
    {
        powerUpName.gameObject.SetActive(false);
        cooldownBar.gameObject.SetActive(false);
    }

    /*    public void UpdateIcons(bool precisionAvailable, bool superHitAvailable, bool shieldAvailable)
        {
            if (precisionIcon != null) precisionIcon.enabled = precisionAvailable;
            if (superHitIcon != null) superHitIcon.enabled = superHitAvailable;
            if (shieldIcon != null) shieldIcon.enabled = shieldAvailable;
        }*/

    public void UpdateHUD(string name, float duration)
    {
        powerUpName.text = name;
        powerUpName.gameObject.SetActive(true);
        cooldownBar.fillAmount = 1f;
        cooldownBar.gameObject.SetActive(true);

        StartCoroutine(Cooldown(duration));
    }

    IEnumerator Cooldown(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            cooldownBar.fillAmount = 1 - (timer / duration);
            yield return null;
        }

        powerUpName.gameObject.SetActive(false);
        cooldownBar.gameObject.SetActive(false);
    }
}
