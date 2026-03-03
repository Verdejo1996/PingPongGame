using System;
using UnityEngine;

public class PaddleSelectUI : MonoBehaviour
{
    public PaddleDefinition[] allPaddles; // asignar en inspector
    public Transform gridParent;
    public PaddleButtonUI paddleButtonPrefab;

    private Action onPlayCallback;

    public void OpenForPlanet(PlanetData planet, Action onPlay)
    {
        onPlayCallback = onPlay;

        // limpiar grid
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // crear botones
        foreach (var paddle in allPaddles)
        {
            var btn = Instantiate(paddleButtonPrefab, gridParent);
            bool unlocked = UnlockManager.Instance.IsUnlocked(paddle.id);

            btn.Setup(
                paddle,
                unlocked,
                isSelected: UnlockManager.Instance.SelectedPaddleId == paddle.id,
                onClick: () =>
                {
                    if (!unlocked) return;
                    UnlockManager.Instance.Select(paddle.id);
                    // refrescar selección visual
                    OpenForPlanet(planet, onPlay);
                    OnPlayPressed();
                    Debug.Log("Apretando boton");
                }
            );
        }
    }

    public void OnPlayPressed()
    {
        onPlayCallback?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnClosePressed()
    {
        gameObject.SetActive(false);
    }
}

