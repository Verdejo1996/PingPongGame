using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Manager : MonoBehaviour
{
    public Ball_Tutorial ball;
    public Tutorial_Paddle paddle;
    public IA_Tutorial ia_Tutorial;

    public Image[] keyImages;

    public Color normalColor = Color.white;
    public Color pressedColor = Color.red;

    private Dictionary<KeyCode, int> keyMap;

    // Start is called before the first frame update
    void Start()
    {

        keyMap = new Dictionary<KeyCode, int>
        {
            { KeyCode.LeftArrow, 0 },
            { KeyCode.RightArrow, 1 },
            { KeyCode.UpArrow, 2 },
            { KeyCode.DownArrow, 3 },
            { KeyCode.F, 4 },
        };
        
        SetServer();
    }

    // Update is called once per frame
    void Update()
    {
        ArrowsColors();
    }

    void SetServer()
    {
        if(Tutorial.instance.currentPhase == TutorialPhase.ServeIntro || Tutorial.instance.currentPhase == TutorialPhase.Serving)
        {
            ball.SetServePosition(new Vector3(0, 2.5f, -7)); // Ajusta la posición para el jugador
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
        if (Tutorial.instance.currentPhase == TutorialPhase.HitIntro || Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
        {
            StartCoroutine(WaitBeforeServe());
            ball.SetServePosition(new Vector3(0, 2f, 7)); // Posición de la IA
            ball.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    IEnumerator WaitBeforeServe()
    {
        yield return new WaitForSeconds(5f); // Espera 3 segundos antes de servir

        while (true)
        {
            if (Tutorial.instance.currentPhase == TutorialPhase.HitPractice)
            {
                ia_Tutorial.Serve();
                //Tutorial.instance.isPaused = false;
            }
            Tutorial.instance.isPaused = true;
            yield return new WaitForSeconds(5f);
        }
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
}
