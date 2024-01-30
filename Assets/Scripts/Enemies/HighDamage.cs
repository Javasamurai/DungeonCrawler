using UnityEngine;

public class HighDamage : Chaser
{
    [SerializeField] private GameObject explosionPrefab;

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
        Destroy(explosion, 2);
        
        // Check if player is in range
        var playerPosition = player.transform.position;
        var distance = Vector2.Distance(playerPosition, transform.position);
        if (distance < 2)
        {
            player.TakeDamage(damageValue);
            player.Knockback(transform.position);
        }
    }
}
