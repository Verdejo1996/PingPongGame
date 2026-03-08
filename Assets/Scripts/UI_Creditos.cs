using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Creditos : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panelInfo;

    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI descripcionText;
    public GameObject panelCreditos;

    private bool mouseEncima = false;


    void Update()
    {
        if (mouseEncima && Input.GetMouseButtonDown(0))
        {
            panelCreditos.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.2f;
        panelInfo.SetActive(true);
        panelInfo.transform.position = transform.position + new Vector3(260f, -60, 0);

        nombreText.text = "Sun";
        descripcionText.text = "Créditos.";

        mouseEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        panelInfo.SetActive(false);
        mouseEncima = false;
    }

    public void ClosePanelCredits()
    {
        panelCreditos.SetActive(false);
    }
}


