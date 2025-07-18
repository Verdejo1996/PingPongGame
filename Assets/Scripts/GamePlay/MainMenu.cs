using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject btn_Play;
    public GameObject btn_Rules;
    public GameObject btn_Salir;
    public void Play()
    {
        SceneManager.LoadScene("Planetary Map"); // Cambia por el nombre de tu escena
    }

    public void MostrarOpciones()
    {
        optionsPanel.SetActive(true);
        btn_Play.SetActive(false);
        btn_Rules.SetActive(false);
        btn_Salir.SetActive(false);
    }

    public void CerrarOpciones()
    {
        optionsPanel.SetActive(false);
        btn_Play.SetActive(true);
        btn_Rules.SetActive(true);
        btn_Salir.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
        // Para que funcione en el editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
