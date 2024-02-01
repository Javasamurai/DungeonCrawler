using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField]
    private HealthBar healthBar;
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateHealth(health);
        var randomValue = Random.Range(0, 2);
        AudioManager.Instance.PlaySFX(randomValue == 0 ? SoundType.PLAYER_HURT_1 : SoundType.PLAYER_HURT_2);
    }
    
    protected override void Die()
    {
        GameManager.Instance.GameOver();
        AudioManager.Instance.PlaySFX(SoundType.PLAYER_DEATH);
    }
}
