using System.Collections;
using TMPro;
using UnityEditor;
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

    [Header("Paddle Select UI")]
    public GameObject paddleSelectPanel;
    public PaddleSelectUI paddleSelectUI;

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
            //  Guardar planeta elegido en sesión
            SessionPlanet.Instance.SetPlanet(datos);
            Debug.Log("Planeta elegido = " +  datos.nombre);
            //  Tierra tutorial: no se elige paleta
/*            if (datos.isTutorialPlanet)
            {
                LoadPlanetScene();
                return;
            }*/

            // Abrir panel de selección de paleta
            paddleSelectPanel.SetActive(true);
            paddleSelectUI.OpenForPlanet(datos, onPlay: LoadPlanetScene);
        }
        else if (mouseEncima && Input.GetMouseButtonDown(0) && !datos.isAvailable)
        {
            StartCoroutine(ShowMessageRoutine());
        }
    }

    private string GetPlanetSwitchName()
    {
        // Ajustá esto a tu PlanetData real (id, nombre, enum, etc.)
        // IMPORTANTE: debe coincidir con el nombre del Switch en Wwise.
        if (datos.isTutorialPlanet) return "Earth";

        // Ejemplos típicos:
        // return datos.nombreSwitchWwise;
        // return datos.planetType.ToString();

        return datos.nombreWwise; // solo si datos.nombre es EXACTAMENTE "IcePlanet", "LavaPlanet", etc.
    }

    private void LoadPlanetScene()
    {
        AudioManager.Instance.StopMenuMusic();
        AudioManager.Instance.SetPlanetSwitch(GetPlanetSwitchName());
        // carga usando el fader si existe
        if (sceneFader != null)
            sceneFader.FadeToScene(datos.escenaDestino);
        else
            SceneManager.LoadScene(datos.escenaDestino);
    }

    private IEnumerator ShowMessageRoutine()
    {
        panelProx.SetActive(true);
        yield return new WaitForSeconds(2f);
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
        panelInfo.SetActive(true);
        panelInfo.transform.position = transform.position + new Vector3(-260f, 0, 0);

        mouseEncima = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        panelInfo.SetActive(false);
        mouseEncima = false;
    }
}

