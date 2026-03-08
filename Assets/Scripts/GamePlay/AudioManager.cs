using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Wwise Event Names")]

    [Header("Gameplay Events")]
    public string menuMusic = "Play_MainMenu";
    public string stopMenuMusic = "Stop_MainMenu";
    public string matchMusic = "Play_MatchMusic";
    public string hitBall = "Play_HitBall";
    public string hitBallOnTable = "Play_ping_pong_Ball";
    public string powerUpSound = "Play_PowerUp";

    [Header("Ice Planet Events")]
    public string iceBlizzard = "Play_IcePlanet_windStrong";
    public string slipperySound = "Play_SlipperyZone";
    public string iceSpikeFall = "Play_IceSpike";

    [Header("Vulcan Planet Events")]
    public string earthQueakeSound = "Play_Earthquake";
    public string geiserSound = "Play_Geiser";

    [Header("Rock Planet Events")]
    public string rockCourtBreak = "Play_Rock_Court_break";

    [Header("Sci-fi Planet Events")]
    public string portalLoop = "Play_PortalLoop";
    public string portalLoopStop = "Stop_PortalLoop";
    public string portalJump = "Play_PortalJump";
    public string laserBeam = "Play_LaserBeam";
    public string laserBeamStop = "Stop_LaserBeam";

    [Header("Switch")]
    public string switchGroupPlanet = "Planet";
    public string playPlanetAmbienceEvent = "Play_PlanetAmbience";
    public string stopPlanetAmbienceEvent = "Stop_PlanetAmbience"; // opcional

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlanetSwitch(string planetSwitchName)
    {
        // Setea el switch en ESTE gameObject (el mismo que va a reproducir el ambiente)
        AkUnitySoundEngine.SetSwitch(switchGroupPlanet, planetSwitchName, gameObject);
    }

    public void PlayPlanetAmbience()
    {
        AkUnitySoundEngine.PostEvent(playPlanetAmbienceEvent, gameObject);
    }

    public void StopPlanetAmbience()
    {
        if (!string.IsNullOrEmpty(stopPlanetAmbienceEvent))
            AkUnitySoundEngine.PostEvent(stopPlanetAmbienceEvent, gameObject);
    }

    public void PlayMenuMusic()
    {
        AkUnitySoundEngine.PostEvent(menuMusic, gameObject);
    }

    public void StopMenuMusic()
    {
        AkUnitySoundEngine.PostEvent(stopMenuMusic, gameObject);
    }

    public void PlayMatchMusic()
    {
        AkUnitySoundEngine.PostEvent(matchMusic, gameObject);
    }

    public void PlayHitBall()
    {
        AkUnitySoundEngine.PostEvent(hitBall, gameObject);
    }

    public void PlayHitBallOnTable()
    {
        AkUnitySoundEngine.PostEvent(hitBallOnTable, gameObject);
    }

    public void BreakCourt()
    {
        AkUnitySoundEngine.PostEvent(rockCourtBreak, gameObject);
    }

    public void PlayIceBlizzard()
    {
        AkUnitySoundEngine.PostEvent(iceBlizzard, gameObject);
    }

    public void PlaySlipperyZone()
    {
        AkUnitySoundEngine.PostEvent(slipperySound, gameObject);
    }
    public void PlayiceSpike()
    {
        AkUnitySoundEngine.PostEvent(iceSpikeFall, gameObject);
    }

    public void PlayPortalLoop()
    {
        AkUnitySoundEngine.PostEvent(portalLoop, gameObject);
    }
    public void StopPortalLoop()
    {
        AkUnitySoundEngine.PostEvent(portalLoopStop, gameObject);
    }
    public void PlayPortalJump()
    {
        AkUnitySoundEngine.PostEvent(portalJump, gameObject);
    }
    public void PlayLaserBeam()
    {
        AkUnitySoundEngine.PostEvent(laserBeam, gameObject);
    }
    public void StopLaserBeam()
    {
        AkUnitySoundEngine.PostEvent(laserBeamStop, gameObject);
    }
    public void PlayPowerUp()
    {
        AkUnitySoundEngine.PostEvent(powerUpSound, gameObject);
    }

    public void PlayEarthQuake()
    {
        AkUnitySoundEngine.PostEvent(earthQueakeSound, gameObject);
    }

    public void PlayGeiser()
    {
        AkUnitySoundEngine.PostEvent(geiserSound, gameObject);
    }
}
