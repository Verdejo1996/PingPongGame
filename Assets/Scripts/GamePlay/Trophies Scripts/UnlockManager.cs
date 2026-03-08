using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance { get; private set; }

    [Header("All Paddles")]
    public PaddleDefinition[] allPaddles;

    private HashSet<string> unlocked = new HashSet<string>();

    public PaddleDefinition SelectedPaddle { get; private set; }

    private const string UNLOCKED_KEY = "UNLOCKED_PADDLES";
    private const string SELECTED_KEY = "SELECTED_PADDLE";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        EnsureDefaultUnlockedAndSelected();
    }

    public bool IsUnlocked(PaddleDefinition paddle)
    {
        if (paddle == null) return false;
        if (paddle.unlockedByDefault) return true;

        return unlocked.Contains(paddle.id);
    }

    public void Unlock(PaddleDefinition paddle)
    {
        if (paddle == null) return;

        unlocked.Add(paddle.id);
        Save();
    }

    public void Select(PaddleDefinition paddle)
    {
        if (paddle == null) return;
        if (!IsUnlocked(paddle)) return;

        SelectedPaddle = paddle;
        Save();
    }

    private void EnsureDefaultUnlockedAndSelected()
    {
        if (SelectedPaddle != null) return;

        foreach (var paddle in allPaddles)
        {
            if (paddle != null && paddle.unlockedByDefault)
            {
                SelectedPaddle = paddle;
                break;
            }
        }

        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetString(UNLOCKED_KEY, string.Join(",", unlocked));

        string selectedId = SelectedPaddle != null ? SelectedPaddle.id : "";
        PlayerPrefs.SetString(SELECTED_KEY, selectedId);

        PlayerPrefs.Save();
    }

    private void Load()
    {
        unlocked.Clear();

        string unlockedData = PlayerPrefs.GetString(UNLOCKED_KEY, "");
        if (!string.IsNullOrEmpty(unlockedData))
        {
            string[] ids = unlockedData.Split(',');
            foreach (var id in ids)
            {
                if (!string.IsNullOrWhiteSpace(id))
                    unlocked.Add(id);
            }
        }

        string selectedId = PlayerPrefs.GetString(SELECTED_KEY, "");

        if (!string.IsNullOrEmpty(selectedId))
        {
            foreach (var paddle in allPaddles)
            {
                if (paddle != null && paddle.id == selectedId)
                {
                    SelectedPaddle = paddle;
                    break;
                }
            }
        }
    }

    private void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

