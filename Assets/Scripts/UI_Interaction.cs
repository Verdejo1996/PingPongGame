using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Interaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlanetData datos;
    public GameObject panelInfo;

    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI descripcionText;

    private bool mouseEncima = false;
    private SceneFader sceneFader;

    private void Start()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }
    void Update()
    {
        if (mouseEncima && Input.GetKeyDown(KeyCode.E))
        {
            if (sceneFader != null)
                sceneFader.FadeToScene(datos.escenaDestino);
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(datos.escenaDestino);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (datos != null)
        {
            nombreText.text = datos.nombre;
            descripcionText.text = datos.descripcion;
        }

        transform.localScale = Vector3.one * 1.2f;
        panelInfo.SetActive(true);
        mouseEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        panelInfo.SetActive(false);
        mouseEncima = false;
    }
}
