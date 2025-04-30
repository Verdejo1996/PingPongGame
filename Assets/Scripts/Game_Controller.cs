using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    public string lastHitter;
    public string currentServer;
    public int playerScore;
    public int botScore;
    private int totalPointsInRound;
    public Ball ball;
    public Transform ballStartPosition;

    public IA_Controller iaGameObject;
    
    public bool playing;
    public bool endGame;

    public TextMeshPro playerTextScore;
    public TextMeshPro botTextScore;
    public TextMeshPro gameText;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        botScore = 0;
        totalPointsInRound = 0;
        currentServer = "Player";
        playing = false;
        endGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        playerTextScore.text = playerScore.ToString();
        botTextScore.text = botScore.ToString();
    }

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
        if (currentServer == "Player")
        {
            ball.SetServePosition(new Vector3(0, 2.5f, -7)); // Ajusta la posición para el jugador
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
        else
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
    }

    public void UpdateLastHitter(string hitter)
    {
        lastHitter = hitter;
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0f;
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 1f;
        }
    }
}
