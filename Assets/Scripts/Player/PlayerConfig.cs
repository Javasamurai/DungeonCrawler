
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    public float speed = 10.0f;
    public float shootDelay = 0.5f;
    public float fps = 10.0f;
}
