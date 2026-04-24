using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServeUIFeedback : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider serveChargeBar;
    [SerializeField] private Image idealZoneImage;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Colores")]
    [SerializeField] private Color normalColor = new(0, 1, 0, 0.4f);
    [SerializeField] private Color glowColor = new(1, 1, 0, 0.7f);
    [SerializeField] private Color perfectColor = Color.red;

    [Header("Feedback")]
    [SerializeField] private float feedbackDuration = 1.5f;

    private Image fillImage;
    private Coroutine feedbackCoroutine;

    private void Awake()
    {
        if (serveChargeBar != null && serveChargeBar.fillRect != null)
        {
            fillImage = serveChargeBar.fillRect.GetComponent<Image>();
        }
    }

    public void InitializeIdealZone(float idealChargeMin, float idealChargeMax)
    {
        if (serveChargeBar == null || idealZoneImage == null)
            return;

        RectTransform rt = idealZoneImage.GetComponent<RectTransform>();
        float totalWidth = serveChargeBar.GetComponent<RectTransform>().rect.width;

        float idealStart = idealChargeMin * totalWidth;
        float idealEnd = idealChargeMax * totalWidth;
        float idealWidth = idealEnd - idealStart;

        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 0.5f);
        rt.anchoredPosition = new Vector2(idealStart, 0);
        rt.sizeDelta = new Vector2(idealWidth, 0);
    }

    public void ShowChargeBar()
    {
        if (serveChargeBar != null)
        {
            serveChargeBar.value = 0f;
            serveChargeBar.gameObject.SetActive(true);
        }
    }

    public void HideChargeBar()
    {
        if (serveChargeBar != null)
        {
            serveChargeBar.gameObject.SetActive(false);
        }
    }

    public void UpdateCharge(float chargeValue, float idealChargeMin, float idealChargeMax)
    {
        if (serveChargeBar == null)
            return;

        serveChargeBar.value = chargeValue;

        bool isInIdealRange = chargeValue >= idealChargeMin && chargeValue <= idealChargeMax;

        if (fillImage != null)
        {
            fillImage.color = isInIdealRange ? glowColor : normalColor;
        }

        if (isInIdealRange)
        {
            ShowPerfectFeedback();
        }
    }

    private void ShowPerfectFeedback()
    {
        if (feedbackText == null)
            return;

        feedbackText.text = "¡Max!";
        feedbackText.color = perfectColor;
        feedbackText.gameObject.SetActive(true);

        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }

        feedbackCoroutine = StartCoroutine(HideFeedbackAfterDelay());
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDuration);

        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }
}
