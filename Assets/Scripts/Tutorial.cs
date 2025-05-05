using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialPhase
{
    Move,
    ServeIntro,
    Serving,
    HitIntro,
    HitPractice,
    Completed
}
public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    public TutorialPhase currentPhase;
    public bool isPaused = true;
    public TextMeshPro boardTutorial;
    //private int step = 0;
    public Tutorial_Manager manager;

    public GameObject collectableObject;
    public Transform[] spawnPoints;
    public Transform[] spawnPointsServe;
    private float spawnInterval = 5f;
    private int collectedCount = 0;

    public GameObject[] enbaleObjects;

    private int succesfulServesRequired = 3;
    private int succesfulServesCount = 0;

    private int succesfulHitRequired = 3;
    private int succesfulHitCount = 0;

    private string[] tutorialSteps =
    {
        "Bienvenidos al tutorial.",
        "Para moverte utiliza las flechas.",
        "Presionando la tecla F o G puedes mover el Aim del servicio.",
        "Presionando Shift o Z puedes mover el Aim del golpe.",
        "Para pausar P, reaunudar O",
        "Comienza el juego."
    };

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(collectedCount.ToString());
        currentPhase = TutorialPhase.HitIntro;
        StartCoroutine(SpawnRoutine());
    /*        Time.timeScale = 0f;
            StartCoroutine(TutorialSequence());*/
    }

    private void Update()
    {
        ActiveObjects();
        ShowNextInstruccion();
/*        FinishTutorial();

        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Tutorial");
        }*/
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            if(currentPhase == TutorialPhase.Move && collectedCount < 3)
            {
                SpawnItem();
            }
            yield return new WaitForSeconds(spawnInterval);

            if (currentPhase == TutorialPhase.Serving && succesfulServesCount < 3)
            {
                SpawnItemServe();
            }
            yield return new WaitForSeconds(spawnInterval);

            if (currentPhase == TutorialPhase.HitPractice && succesfulHitCount < 3)
            {
                SpawnItemHit();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void CompletePhase()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentPhase++;
            isPaused = true;
            ShowNextInstruccion();
        }
    }

    private void ShowNextInstruccion()
    {
        switch (currentPhase)
        {
            case TutorialPhase.Move:
                // Mostrar instrucciones de movimiento
                //boardTutorial.text = "Bienvenidos al tutorial. Para moverte utiliza las flechas. \nRecolecta todos los objetos.";
                UpdateTutorialText();
                if (collectedCount == 3)
                {
                    boardTutorial.text = "Bien hecho! Si estas listo, presiona ENTER para continuar.";
                }
                Tutorial.instance.CompletePhase();
                break;
            case TutorialPhase.ServeIntro:
                // Mostrar instrucciones de saque
                //boardTutorial.text = "Ahora vamos a aprender el saque.";
                UpdateTutorialText();
                Tutorial.instance.CompletePhase();
                break;
            case TutorialPhase.Serving:
                // Mostrar instrucciones de saque
                UpdateTutorialText();
                if (succesfulServesCount >= succesfulServesRequired)
                {
                    boardTutorial.text = "Bien hecho! Si estas listo, presiona ENTER para continuar.";
                }
                Tutorial.instance.CompletePhase();
                break;
            case TutorialPhase.HitIntro:
                // Mostrar instrucciones de golpe
                UpdateTutorialText();
                Tutorial.instance.CompletePhase();
                break;
            case TutorialPhase.HitPractice:
                // Mostrar instrucciones de golpe
                UpdateTutorialText();
                if (succesfulHitCount >= succesfulHitRequired)
                {
                    boardTutorial.text = "Bien hecho! Si estas listo, presiona ENTER para continuar.";
                }
                break;
            case TutorialPhase.Completed:
                // Fin del tutorial
                break;
        }
    }

    void ActiveObjects()
    {
        if(currentPhase == TutorialPhase.Move)
        {
            enbaleObjects[0].SetActive(false);
            enbaleObjects[1].SetActive(false);
            enbaleObjects[2].SetActive(false);
            enbaleObjects[3].SetActive(false);
        }
        if(currentPhase == TutorialPhase.ServeIntro)
        {
            enbaleObjects[0].SetActive(true);
            enbaleObjects[1].SetActive(true);
            enbaleObjects[2].SetActive(false);
            enbaleObjects[3].SetActive(false);
        }
        if (currentPhase == TutorialPhase.HitIntro)
        {
            enbaleObjects[0].SetActive(true);
            enbaleObjects[1].SetActive(false);
            enbaleObjects[2].SetActive(true);
            enbaleObjects[3].SetActive(true);
        }
    }

/*    IEnumerator TutorialSequence()
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
    }*/

    public void CollectItem()
    {
        collectedCount++;
        Debug.Log(collectedCount.ToString());
    }

    public void CollectItemServe()
    {
        succesfulServesCount++;
        Debug.Log(succesfulServesCount.ToString());
    }

    public void CollectItemHit()
    {
        succesfulHitCount++;
        Debug.Log(succesfulHitCount.ToString());
    }
    void UpdateTutorialText()
    {
        if(currentPhase == TutorialPhase.Move)
        {
            boardTutorial.text = "Bienvenidos al tutorial. Para moverte utiliza las flechas. \nRecolecta todos los objetos.";
        }
        if(currentPhase == TutorialPhase.ServeIntro)
        {
            boardTutorial.text = "Vamos a practicar el servicio. \nApunta a los objetivos. \nPresiona ENTER.";
        }
        if (currentPhase == TutorialPhase.Serving)
        {
            boardTutorial.text = "Manteniendo presionada la tecla F y las flechas podes apuntar. \nCuando sueltes, la pelota saldrá.";
        }
        if(currentPhase == TutorialPhase.HitIntro)
        {
            boardTutorial.text = "Ahora practiquemos el tiro. Golpea la bola siguiendo estos pasos. \nPresiona ENTER.";
        }
        if(currentPhase == TutorialPhase.HitPractice)
        {
            boardTutorial.text = "Presiona Z o Shift + las flechas para apuntar. \nSoltar al momento de hacer contacto con la bola.";
        }
    }

    void SpawnItem()
    {
        int points = UnityEngine.Random.Range(0, spawnPoints.Length);

        Instantiate(collectableObject, spawnPoints[points].position, Quaternion.identity);
    }

    void SpawnItemServe()
    {
        int points = UnityEngine.Random.Range(0, spawnPointsServe.Length);

        Instantiate(collectableObject, spawnPointsServe[points].position, Quaternion.identity);
    }

    void SpawnItemHit()
    {
        int points = UnityEngine.Random.Range(0, spawnPointsServe.Length);

        Instantiate(collectableObject, spawnPointsServe[points].position, Quaternion.identity);
    }
}
