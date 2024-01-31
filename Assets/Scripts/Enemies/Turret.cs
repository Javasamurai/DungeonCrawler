using UnityEngine;
using UnityEngine.PlayerLoop;

public class Turret : EnemyController
{
    [SerializeField] private float shootDelay = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        shootDelay += Time.time;
    }

    protected override void Move()
    {
        // Turret doesn't move
    }

    protected override void Shoot()
    {
        var bullet = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
        bullet.direction = (player.transform.position - transform.position).normalized;
        bullet.Shoot(false);
    }

    protected override void Update()
    {
        base.Update();

        // if (Time.time - shootDelay > 0)
        // {
        //     Shoot();
        //     shootDelay = Time.time ;
        // }
        // Shoot after delay
        
    }
}
