using UnityEngine;
using UnityEngine.PlayerLoop;

public class Turret : EnemyController
{
    [SerializeField] private float shootDelay = 0.5f;

    protected override void Move()
    {
        // Turret doesn't move
    }

    protected override void Shoot()
    {
        var bullet = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
        bullet.direction = (player.transform.position - transform.position).normalized;
        bullet.Shoot();
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time > shootDelay)
        {
            Shoot();
            shootDelay = Time.time + shootDelay;
        }
    }
}
