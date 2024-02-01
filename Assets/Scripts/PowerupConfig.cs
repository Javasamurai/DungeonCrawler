using UnityEngine;

[CreateAssetMenu(fileName = "PowerupConfig", menuName = "PowerupConfig", order = 0)]
public class PowerupConfig : ScriptableObject
{
    public float bigScale = 3;
    public float bulletScale = 0.5f;
    public float shootDelay = 1f;
    public float timeScale = 1.5f;
}