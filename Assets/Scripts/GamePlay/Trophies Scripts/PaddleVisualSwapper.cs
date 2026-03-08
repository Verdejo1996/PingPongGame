using UnityEngine;

public class PaddleVisualSwapper : MonoBehaviour
{
    [Header("Visual Root")]
    public Transform visualRoot;

    [Header("Optional fallback visual")]
    public GameObject defaultVisualPrefab;

    private void Start()
    {
        ApplySelectedVisual();
    }

    public void ApplySelectedVisual()
    {
        if (visualRoot == null)
        {
            Debug.LogError("VisualRoot no asignado en PaddleVisualSwapper.");
            return;
        }

        foreach (Transform child in visualRoot)
        {
            Destroy(child.gameObject);
        }

        PaddleDefinition selected = UnlockManager.Instance != null
            ? UnlockManager.Instance.SelectedPaddle
            : null;

        GameObject visualToSpawn = null;

        if (selected != null && selected.visualPrefab != null)
            visualToSpawn = selected.visualPrefab;
        else
            visualToSpawn = defaultVisualPrefab;

        if (visualToSpawn == null)
        {
            Debug.LogWarning("No hay visualPrefab seleccionado ni fallback.");
            return;
        }

        Instantiate(visualToSpawn, visualRoot);
    }
}
