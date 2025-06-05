using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Interaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string escenaDestino;
    public GameObject panelInfo;

    private bool mouseEncima = false;

    void Update()
    {
        if (mouseEncima && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(escenaDestino);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
