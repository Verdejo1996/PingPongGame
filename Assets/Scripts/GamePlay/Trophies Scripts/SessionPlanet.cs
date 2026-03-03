using UnityEngine;

public class SessionPlanet : MonoBehaviour
{
    public static SessionPlanet Instance { get; private set; }

    public PlanetData SelectedPlanet { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlanet(PlanetData planet) => SelectedPlanet = planet;
}
