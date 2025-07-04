using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public void Play()
    {
        SceneManager.LoadScene("Planetary Map"); // Cambia por el nombre de tu escena
    }

    public void MostrarOpciones()
    {
        optionsPanel.SetActive(true);
    }

    public void CerrarOpciones()
    {
        optionsPanel.SetActive(false);
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
