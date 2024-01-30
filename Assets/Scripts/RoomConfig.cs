using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "ScriptableObjects/RoomConfig", order = 1)]
public class RoomConfig : ScriptableObject
{
    public EnemyController[] enemies;
    public GameObject[] obstacles;
    public Powerup powerup;
}

public enum Powerup
{
    BIGG,
    TIME,
    MAGNET,
    DOT,
    MISDIRECT,
    TURNBASED
}