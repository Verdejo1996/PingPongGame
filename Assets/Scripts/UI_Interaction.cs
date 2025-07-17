using System;
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
    public GameObject panelProx;

    private bool mouseEncima = false;
    private SceneFader sceneFader;

    private void Start()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }
    void Update()
    {
        if (mouseEncima && Input.GetMouseButtonDown(0) && datos.isAvailable)
        {
            if (sceneFader != null)
                sceneFader.FadeToScene(datos.escenaDestino);
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(datos.escenaDestino);
        }
        else if(mouseEncima && Input.GetMouseButtonDown(0) && !datos.isAvailable)
        {
            StartCoroutine(ShowMessageRoutine());
        }
    }

    private IEnumerator ShowMessageRoutine()
    {
        panelProx.SetActive(true);
        yield return new WaitForSeconds(2f); // Muestra por 2 segundos
        panelProx.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (datos != null)
        {
            nombreText.text = datos.nombre;
            descripcionText.text = datos.descripcion;
        }

        transform.localScale = Vector3.one * 1.2f;

        // Activar el panel
        panelInfo.SetActive(true);
        panelInfo.transform.position = transform.position + new Vector3(-120f,0,0);

        mouseEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        panelInfo.SetActive(false);
        mouseEncima = false;
    }
}
