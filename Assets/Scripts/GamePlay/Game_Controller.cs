using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance {get; private set;}

    [Header("Gameplay")]
    public string lastHitter;
    public string currentServer;
    public int playerScore;
    public int botScore;
    private int totalPointsInRound;
    public Ball ball;
    public Transform ballStartPosition;
    public GameObject pauseUI;
    public GameObject endGamePanel;

    [Header("Player/IA")]
    public IA_Controller iaGameObject;
    public PlayerHit_Controller playerHitController;

    [Header("Banderas")]
    public bool playing;
    public bool endGame;

    [Header("Textos")]
    public TextMeshPro playerTextScore;
    public TextMeshPro botTextScore;
    public TextMeshPro gameText;

    //Agregado para mostrar como se juega
    private Dictionary<KeyCode, int> keyMap;
    public Image[] keyImages;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.red;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        botScore = 0;
        totalPointsInRound = 0;
        currentServer = "Player";
        playing = false;
        endGame = false;

        //Agregado para mostrar como se juega
        keyMap = new Dictionary<KeyCode, int>
        {
            { KeyCode.LeftArrow, 0 },
            { KeyCode.RightArrow, 1 },
            { KeyCode.UpArrow, 2 },
            { KeyCode.DownArrow, 3 },
            { KeyCode.Z, 4 },
            { KeyCode.X, 5 },
            { KeyCode.P, 6 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Agregado para mostrar como se juega
        ArrowsColors();

        PauseGame();
        playerTextScore.text = playerScore.ToString();
        botTextScore.text = botScore.ToString();
    }

    //Agregado para mostrar como se juega
    void ArrowsColors()
    {
        foreach (var key in keyMap.Keys)
        {
            int index = keyMap[key];

            if (Input.GetKeyDown(key))
                keyImages[index].color = pressedColor;
            if (Input.GetKeyUp(key))
                keyImages[index].color = normalColor;
        }
    }
    //

    void CheckScore()
    {
        if (playerScore == 11 || botScore == 11)
        {
            EndGame();
            return;
        }
        //Chequeamos si los puntos dan resto 0 para cambiar de servicio.
        if (totalPointsInRound % 2 == 0)
        {
            ChangeServer();
        }
        SetServer();
    }

    void SetServer()
    {
        if (currentServer == "Player" && !playing)
        {
            ball.SetServePosition(playerHitController.transform.position); // Ajusta la posición para el jugador
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
        else if(currentServer == "Bot" && !playing)
        {
            ball.SetServePosition(new Vector3(0, 2f, 7)); // Posición de la IA
            ball.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(WaitBeforeServe());
        }
    }
    
    //Realizamos esta corutina para que la IA espere al menos 2 segundos para realizar el servicio.
    IEnumerator WaitBeforeServe()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de servir
        iaGameObject.Serve();
    }


    void ChangeServer()
    {
        currentServer = (currentServer == "Player") ? "Bot" : "Player";
    }

    public void AddPointToLastHitter()
    {
        if (lastHitter == "Player")
            playerScore++;
        else if(lastHitter == "Bot")
            botScore++;

        totalPointsInRound++;
        CheckScore();
        Debug.Log("Puntaje - Jugador: " + playerScore + " | Oponente: " + botScore);
    }

    public void AddPointToOpponent()
    {
        if (lastHitter == "Player")
            botScore++;
        else if (lastHitter == "Bot")
            playerScore++;

        totalPointsInRound++;
        CheckScore();
        Debug.Log("Puntaje - Jugador: " + playerScore + " | Oponente: " + botScore);
    }

    void EndGame()
    {
        endGame = true;
        //Debug.Log("Juego terminado: " + (playerScore > botScore ? "¡Ganaste!" : "Perdiste!"));
        gameText.text = playerScore > botScore ? "¡Ganaste!" : "Perdiste!";
        Time.timeScale = 0f;
        if(endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToPlanetMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Planetary Map");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void UpdateLastHitter(string hitter)
    {
        lastHitter = hitter;
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
