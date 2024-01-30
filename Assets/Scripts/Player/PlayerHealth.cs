using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField]
    private HealthBar healthBar;
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateHealth(health);
    }
    
    protected override void Die()
    {
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }
}
