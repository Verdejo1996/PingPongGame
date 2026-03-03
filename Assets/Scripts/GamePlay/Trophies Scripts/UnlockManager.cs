using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance { get; private set; }

    private const string KEY_UNLOCKS = "unlocked_paddles";
    private const string KEY_SELECTED = "selected_paddle";

    private HashSet<string> unlocked = new HashSet<string>();
    public string SelectedPaddleId { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public bool IsUnlocked(string id) => unlocked.Contains(id);

    public void Unlock(string id)
    {
        if (string.IsNullOrEmpty(id)) return;


        if (unlocked.Add(id))
        {
            // auto-seleccionar si no había una elegida
            if (string.IsNullOrEmpty(SelectedPaddleId))
                SelectedPaddleId = id;

            Save();
        }
    }

    public void Select(string id)
    {
        if (!IsUnlocked(id)) return;
        SelectedPaddleId = id;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetString(KEY_UNLOCKS, string.Join(",", unlocked));
        PlayerPrefs.SetString(KEY_SELECTED, SelectedPaddleId ?? "");
        PlayerPrefs.Save();
    }

    private void Load()
    {
        var raw = PlayerPrefs.GetString(KEY_UNLOCKS, "");
        unlocked = raw.Length == 0
            ? new HashSet<string>()
            : raw.Split(',').ToHashSet();

        SelectedPaddleId = PlayerPrefs.GetString(KEY_SELECTED, "");

        if (unlocked.Count == 0)
        {
            unlocked.Add("Classic");
            SelectedPaddleId = "Classic";
            Save();
        }
    }
}

