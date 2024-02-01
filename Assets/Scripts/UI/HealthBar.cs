using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image[] healthSprites;
    public void UpdateHealth(float health)
    {
        var healthSpritesCount = healthSprites.Length;
        var healthSpritesIndex = Mathf.RoundToInt(health / 100 * healthSpritesCount);
        for (var i = 0; i < healthSpritesCount; i++)
        {
            healthSprites[i].enabled = i < healthSpritesIndex;
        }
    }
}
