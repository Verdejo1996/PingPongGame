using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Manager : MonoBehaviour
{
    public static Tutorial_Manager Instance;
    public Ball_Tutorial ball;
    public Tutorial_Paddle paddle;
    public IA_Tutorial ia_Tutorial;

    public Image[] keyImages;

    public Color normalColor = Color.white;
    public Color pressedColor = Color.red;

    private Dictionary<KeyCode, int> keyMap;

    public int playerScore;
    public int botScore;
    public string currentServer;
    private int totalPointsInRound;
    public string lastHitter;

    public bool endTutorial;

    // Start is called before the first frame update
    void Start()
    {
        currentServer = "Player";
        keyMap = new Dictionary<KeyCode, int>
        {
            { KeyCode.LeftArrow, 0 },
            { KeyCode.RightArrow, 1 },
            { KeyCode.UpArrow, 2 },
            { KeyCode.DownArrow, 3 },
            { KeyCode.Z, 4 },
            { KeyCode.X, 5 }
        };
        
        //SetServer();
    }

    // Update is called once per frame
    void Update()
    {
        ArrowsColors();
        //SetServer();
    }

    public void SetServer()
    {
        if(Tutorial.instance.currentPhase == TutorialPhase.ServeIntro || Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            Tutorial.instance.isPaused = false;
            ball.SetServePosition(paddle.transform.position); // Ajusta la posición para el jugador
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
        if (Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            Tutorial.instance.isPaused = false;
            ball.SetServePosition(new Vector3(0, 2f, 7)); // Posición de la IA
            ball.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(WaitBeforeServe());
        }
        if(Tutorial.instance.currentPhase == TutorialPhase.Completed)
        {
            Tutorial.instance.isPaused = false;
            if(currentServer == "Player")
            {
                ball.SetServePosition(paddle.transform.position); // Ajusta la posición para el jugador
                //ball.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                ball.SetServePosition(new Vector3(0, 2f, 7)); // Posición de la IA
                //ball.GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(WaitBeforeServe());
            }
        }
    }

    IEnumerator WaitBeforeServe()
    {
        yield return new WaitForSeconds(5f); // Espera 3 segundos antes de servir

        ia_Tutorial.Serve();
        yield return new WaitForSeconds(5f);
    }

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

    public void CheckScore()
    {
        if (playerScore == 11 || botScore == 11)
        {
            EndTutorial();
            return;
        }
        //Chequeamos si los puntos dan resto 0 para cambiar de servicio.
        if (totalPointsInRound % 2 == 0)
        {
            ChangeServer();
        }
        SetServer();
    }

    public void AddPointToLastHitter()
    {
        if (lastHitter == "Player")
            playerScore++;
        else if (lastHitter == "Bot")
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

    public void UpdateLastHitter(string hitter)
    {
        lastHitter = hitter;
    }

    void ChangeServer()
    {
        currentServer = (currentServer == "Player") ? "Bot" : "Player";
    }

    void EndTutorial()
    {
        endTutorial = true;
    }
}
