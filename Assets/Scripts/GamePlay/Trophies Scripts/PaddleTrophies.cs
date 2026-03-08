using UnityEngine;

[CreateAssetMenu(menuName = "PingPong/Paddle Definition")]
public class PaddleDefinition : ScriptableObject
{
    public string id;                 // e.g. "PADDLE_ICE"
    public string displayName;        // e.g. "Ice Paddle"
    public Sprite icon;               // UI icon

    [Header("Visual")]
    public GameObject visualPrefab;         // model prefab

    [Header("Config")]
    public bool unlockedByDefault;
}
