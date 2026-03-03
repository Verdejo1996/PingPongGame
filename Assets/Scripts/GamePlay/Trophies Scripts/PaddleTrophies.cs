using UnityEngine;

[CreateAssetMenu(menuName = "PingPong/Paddle Definition")]
public class PaddleDefinition : ScriptableObject
{
    public string id;                 // e.g. "PADDLE_ICE"
    public string displayName;        // e.g. "Ice Paddle"
    public GameObject prefab;         // model prefab
    public Sprite icon;               // UI icon
    public string unlockedByPlanetId; // e.g. "ICE"
}
