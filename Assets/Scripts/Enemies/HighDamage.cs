using UnityEngine;

public class HighDamage : Chaser
{
    [SerializeField] private GameObject explosionPrefab;
    
    private const float EXPLOSION_DELAY = 2.0f;
    private const float EXPLOSION_RANGE = 2.0f;

    protected override void Shoot()
    {
        
    }

    public override void Die()
    {
        base.Die();
        Explode();
    }

    private void Explode()
    {
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, EXPLOSION_DELAY);
        
        // Check if player is in range
        var playerPosition = player.transform.position;
        var distance = Vector2.Distance(playerPosition, transform.position);
        
        if (distance < EXPLOSION_RANGE)
        {
            player.TakeDamage(damageValue);
            player.Knockback(transform.position);
        }
    }
}
