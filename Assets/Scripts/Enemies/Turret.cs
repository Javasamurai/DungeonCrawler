using UnityEngine;
using UnityEngine.PlayerLoop;

public class Turret : EnemyController
{
    [SerializeField] private float shootDelay = 0.5f;
    private float shootDelayTimer;

    protected override void Move()
    {
        // Turret doesn't move
    }
    
    protected override void Update()
    {
        base.Update();
        shootDelayTimer += Time.deltaTime;
        if (shootDelayTimer > shootDelay)
        {
            Shoot();
            shootDelayTimer = 0.0f;
        }
    }
}
