using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public TextMeshPro boardTutorial;
    private int step = 0;
    [SerializeField]
    Game_Controller controller;

    private string[] tutorialSteps =
    {
        "Bienvenidos al tutorial.",
        "Para moverte utiliza las flechas.",
        "Presionando la tecla F o G puedes mover el Aim del servicio.",
        "Presionando Shift o Z puedes mover el Aim del golpe.",
        "Para pausar P, reaunudar O",
        "Comienza el juego."
    };

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(TutorialSequence());
    }

    private void Update()
    {
        FinishTutorial();

        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    IEnumerator TutorialSequence()
    {
        while (step < tutorialSteps.Length)
        {
            UpdateTutorialText();
            yield return new WaitForSecondsRealtime(5f); // Espera 5 segundos antes de cambiar el mensaje
            step++;
        }
        Time.timeScale = 1f; 
        Debug.Log("Tutorial completado.");
        boardTutorial.gameObject.SetActive(false);
    }

    void UpdateTutorialText()
    {
        boardTutorial.text = tutorialSteps[step];
    }

    void FinishTutorial()
    {
        if(controller.playerScore > 5)
        {
            boardTutorial.gameObject.SetActive(true);
            boardTutorial.text = "Para terminar el tutorial presiona Enter.";

            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
